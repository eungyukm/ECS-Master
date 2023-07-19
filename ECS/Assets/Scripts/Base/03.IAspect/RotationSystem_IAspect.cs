using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECSMater.IAspect
{
    public partial struct RotationSystem_IAspect : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Execute.Aspects>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            double elapsedTime = SystemAPI.Time.ElapsedTime;

            foreach (var (transform, speed) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RadianPerSecond * deltaTime);
            }

            foreach (var movement in SystemAPI.Query<VerticalMovementAspect>())
            {
                movement.Move(elapsedTime);
            }
        }
    }
    
    readonly partial struct VerticalMovementAspect : Unity.Entities.IAspect
    {
        private readonly RefRW<LocalTransform> m_Transform;
        private readonly RefRO<RotationSpeed> m_Speed;

        public void Move(double elapsedTime)
        {
            m_Transform.ValueRW.Position.y = 
                (float)math.sin(elapsedTime * m_Speed.ValueRO.RadianPerSecond);
        }
    }
}

