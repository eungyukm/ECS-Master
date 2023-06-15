using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


namespace ECSMater.IJobEntity
{
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.IJobEntity>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new RotationJob { deltaTime = SystemAPI.Time.DeltaTime };
            // job.Schedule();
        }
    }
    
    [BurstCompile]
    partial struct RotationJob : Unity.Entities.IJobEntity
    {
        public float deltaTime;

        void Execute(ref LocalTransform transform, in RotationSpeed speed)
        {
            transform = transform.RotateY(speed.RadianPerSecond * deltaTime);
        }
    }
}