using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace Survivor
{
    public partial struct PlayerAttackSystem : ISystem, ISystemStartStop
    {
        private Entity _playerEntity;
        private Entity _enemyEntityReferenceEntity;
        private int _enemyID;
        private int _enemySpawnCount;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerTag>();
            state.RequireForUpdate<ProjectilePrefab>();
            state.RequireForUpdate<EnemyEntityReferenceTag>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            _enemyEntityReferenceEntity = SystemAPI.GetSingletonEntity<EnemyEntityReferenceTag>();
        }
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AttackEnemy(0, ref state);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AttackEnemy(1, ref state);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AttackEnemy(2, ref state);
            }
        }

        private void AttackEnemy(int enemyID, ref SystemState state)
        {
            Debug.Log("enemyID : " + enemyID);
            // 공격할 적 찾기
            var enemies = 
                SystemAPI.GetBuffer<EnemyEntityReferenceBufferElement>(_enemyEntityReferenceEntity);

            _enemyID = SystemAPI.GetComponent<CharacterExperiencePoints>(_playerEntity).Value;
            Debug.Log("_enemyID : " + _enemyID);
            _enemySpawnCount = SystemAPI.GetComponent<EnemySpawnCount>(_enemyEntityReferenceEntity).Value;
            Debug.Log("_enemySpawnCount : " + _enemySpawnCount);
            if(_enemyID >= _enemySpawnCount) return;
            
            var enemyEntity = enemies[_enemyID+1].Value;

            if (!SystemAPI.Exists(enemyEntity))
            {
                Debug.Log("enemyEntity is not exist");
                _enemyID = SystemAPI.GetComponent<CharacterExperiencePoints>(_enemyEntityReferenceEntity).Value;  
                return;
            }
            
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            var startPosition = transformLookup[_playerEntity].Position;
            var targetPosition = transformLookup[enemyEntity].Position;
            
            var projectilePrefab = SystemAPI.GetSingleton<ProjectilePrefab>().Value;
            var newProjectile = state.EntityManager.Instantiate(projectilePrefab);
            Debug.Log("projectilePrefab : " + projectilePrefab.ToString());
            Debug.Log("newProjectile : " + newProjectile.ToString());
            
            var projectileRotation = quaternion.LookRotation(math.normalize(targetPosition - startPosition), math.up());
            var projectileTransform = LocalTransform.FromPositionRotationScale(startPosition, projectileRotation, 0.5f);
            state.EntityManager.SetComponentData(newProjectile, projectileTransform);
            state.EntityManager.AddComponentData(newProjectile, new ProjectileProperties
            {
                TargetPosition = targetPosition,
                TargetCharacter = enemyEntity,
                DamageAmount = SystemAPI.GetComponent<CharacterAttackStrength>(_playerEntity).Value,
                OriginCharacter = _playerEntity
            });
            
        }
        

        public void OnStopRunning(ref SystemState state)
        {
            
        }
    }
}

