using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace BossRaid
{
    public partial struct SpawnSystem : ISystem
    {
        private uint seedOffset;
        private float spawnTimer;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            const int count = 20;
            const float spawnWait = 1.0f;

            spawnTimer -= SystemAPI.Time.DeltaTime;

            if (spawnTimer > 0)
            {
                return;
            }

            spawnTimer = spawnWait;
            
            // Player 생성 쿼리
            var newPlayerQuery = SystemAPI.QueryBuilder().WithAll<NewSpawn>().Build();
            state.EntityManager.RemoveComponent<NewSpawn>(newPlayerQuery);

            var prefab = SystemAPI.GetSingleton<Spawner>().Prefab;
            state.EntityManager.Instantiate(prefab, count, Allocator.Temp);

            seedOffset += count;

            new RandomPositonJob
            {
                SeedOffset = seedOffset
            }.ScheduleParallel();
        }
    }
    
    // Random Positon을 생성하는 Job
    [WithAll(typeof(NewSpawn))]
    [BurstCompile]
    partial struct RandomPositonJob : IJobEntity
    {
        public uint SeedOffset;

        public void Execute([EntityIndexInQuery] int index, ref LocalTransform transform)
        {
            var random = Random.CreateFromIndex(SeedOffset + (uint)index);
            var xz = random.NextFloat2Direction() * 50;
            transform.Position = new float3(xz[0], 1, xz[1]);
        }
    }

}

