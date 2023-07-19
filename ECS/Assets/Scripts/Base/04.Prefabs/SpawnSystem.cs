using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

namespace ECSMater.Prefabs
{
    public partial struct SpawnSystem : ISystem
    {
        private uint updateCounter;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawner>();
            state.RequireForUpdate<Execute.Prefabs>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var spinningCubesQuery = SystemAPI.QueryBuilder()
                .WithAll<RotationSpeed>().Build();

            if (spinningCubesQuery.IsEmpty)
            {
                var prefab = SystemAPI.GetSingleton<Spawner>().Prefab;
                var instances = state.EntityManager.Instantiate
                (prefab, 3, Allocator.Temp);
                var random = Random.CreateFromIndex(updateCounter++);

                foreach (var entity in instances)
                {
                    var transform = 
                        SystemAPI.GetComponentRW<LocalTransform>(entity);
                    transform.ValueRW.Position = 
                        (random.NextFloat3() - new float3(0.5f, 0, 0.5f)) * 20;
                }
            }
        }
    }
}

