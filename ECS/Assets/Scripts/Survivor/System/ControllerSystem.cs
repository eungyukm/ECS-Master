using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Survivor
{
    [UpdateAfter(typeof(InputSystem))]
    public partial struct ControllerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputState>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // Input 상태에 대해서 얻음
            var input = SystemAPI.GetSingleton<InputState>();
            
            foreach (var (transform, controller) in
                     SystemAPI.Query<RefRW<LocalTransform>, RefRW<Controller>>())
            {
                // float3로 움직임을 생성
                var move = new float3(input.Horizontal, 0, input.Vertical);
                // Speed와 Time.DeltaTime을 곱하여 움직임을 만듬
                move = 
                    move * controller.ValueRO.player_speed * SystemAPI.Time.DeltaTime;
                move = math.mul(transform.ValueRO.Rotation, move);
                // 현재 캐릭터의 위치에서 더해줌
                transform.ValueRW.Position += move;
                
                if (transform.ValueRO.Position.y < 0)
                {
                    transform.ValueRW.Position *= new float3(1, 0, 1);
                }
            }
        }
    }
}