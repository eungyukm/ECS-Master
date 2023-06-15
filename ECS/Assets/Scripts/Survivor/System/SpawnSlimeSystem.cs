using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Survivor
{
    // 슬라임을 만드는 시스템
    [BurstCompile]
    public partial struct SpawnSlimeSystem : ISystem, ISystemStartStop
    {
        private Entity _playerEntity;
        private Entity _enemyEntityReferenceEntity;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemyEntityReferenceTag>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
            _enemyEntityReferenceEntity = 
                SystemAPI.GetSingletonEntity<EnemyEntityReferenceTag>();
        }
        // 하위 별도의 처리 하지 않음
        public void OnStopRunning(ref SystemState state)
        {
            
        }
        // 하위 별도의 처리 하지 않음 
        public void OnDestroy(ref SystemState state)
        {
            
        }
        
        // 4번을 누를 시 적 생성
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                InstantiateSlime(0, ref state);
            }
        }
        
        // 슬라임을 생성하는 함수
        private void InstantiateSlime(int enemyID, ref SystemState state)
        {
            var enemies = 
                SystemAPI.GetBuffer<EnemyEntityReferenceBufferElement>
                    (_enemyEntityReferenceEntity);

            var enemySpawnCount = SystemAPI.GetComponent<EnemySpawnCount>
                (_enemyEntityReferenceEntity);
            var slimePrefab = SystemAPI.GetSingleton<EnemyPrefab>().Value;
            var slimeEntity = state.EntityManager.Instantiate(slimePrefab);
            // 버퍼의 적 저장
            enemies.Add(slimeEntity);
            // 적 Spawn 카운트 증가
            enemySpawnCount.Value++;
            state.EntityManager.SetComponentData
                (_enemyEntityReferenceEntity, enemySpawnCount);
            
            // Spawn 시 위치 설정
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            float3 posFloat3 = new float3(0, 0 , 0);
            var startPosition = posFloat3;
            var targetPosition = transformLookup[_playerEntity].Position;
            var rotation = quaternion.LookRotation
                (math.normalize(targetPosition - startPosition), math.up());
            var localTransform = LocalTransform.FromPositionRotationScale
                (startPosition, rotation, 2.0f);
            state.EntityManager.SetComponentData(slimeEntity, localTransform);
            state.EntityManager.AddComponentData
            (slimeEntity, new EnemyProperties
            {
                TargetCharacter = _playerEntity,
                OriginCharacter = slimeEntity
            });
        }
    }
}