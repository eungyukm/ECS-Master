using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Survivor
{
    public partial struct MoveProjectileSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, projectileProperties, moveSpeed, projectile) in
                     SystemAPI.Query<RefRW<LocalTransform>, ProjectileProperties, ProjectileMoveSpeed>()
                         .WithEntityAccess())
            {
                var startPosition = transform.ValueRO.Position;
                var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
                var targetPosition = transformLookup[projectileProperties.TargetCharacter].Position;
                
                // 발사체가 몬스터의 방향으로 이동
                transform.ValueRW.Position += transform.ValueRO.Forward() * moveSpeed.Value * deltaTime;

                if (math.distance(transform.ValueRO.Position, targetPosition) <= 0.25f)
                {
                    if (SystemAPI.Exists(projectileProperties.TargetCharacter))
                    {
                        ecb.AddComponent(projectileProperties.TargetCharacter, new DamageToCharacter()
                        {
                            Value = projectileProperties.DamageAmount,
                            OriginCharacter = projectileProperties.OriginCharacter
                        });
                        ecb.DestroyEntity(projectileProperties.TargetCharacter);
                    }
                    
                    // 발사체 파괴
                    ecb.DestroyEntity(projectile);
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}

