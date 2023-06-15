using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Survivor
{
    public partial struct MoveEnemySystem : ISystem, ISystemStartStop
    {
        private Entity _playerEntity;
        
        public void OnStartRunning(ref SystemState state)
        {
            _playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            // 활성화 된 enemy Loop를 도는 코드
            foreach (var (transform, 
                         enemyProperties, enemyMoveSpeed, enemy) in 
                     SystemAPI.Query<RefRW<LocalTransform>, 
                             EnemyProperties, EnemyMoveSpeed>()
                         .WithEntityAccess())
            {
                // 플레이어의 위치를 지속적으로 업데이트
                var transformLookup = 
                    SystemAPI.GetComponentLookup<LocalTransform>();
                var targetPosition = transformLookup[_playerEntity].Position;
                var projectileRotation = transformLookup[_playerEntity].Rotation;
                var projectileScale = transformLookup[_playerEntity].Scale;
                var localTransform = LocalTransform.FromPositionRotationScale(targetPosition,
                    projectileRotation, projectileScale);
                
                // 타겟의 방향 계산
                var distance =  targetPosition - transform.ValueRO.Position;
                var direction = math.normalize(distance);
                
                // 이동 코드
                transform.ValueRW.Position += direction * enemyMoveSpeed.Value * deltaTime;
                transform.ValueRW.Rotation = quaternion.LookRotation(direction, math.up());

                // 타겟과의 거리 측정
                if (math.distance(transform.ValueRO.Position, localTransform.Position) <= 0.25f)
                {
                    // 타겟과 부딧히면 스스로 파괴
                    ecb.DestroyEntity(enemy);
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
        

        public void OnStopRunning(ref SystemState state)
        {
            
        }
    }
}
