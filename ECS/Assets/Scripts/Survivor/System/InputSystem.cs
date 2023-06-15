using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public partial struct InputSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.CreateSingleton<InputState>();
        }
        // 움직임에 대해 지속적으로 갱신
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ref var inputState = ref SystemAPI.GetSingletonRW<InputState>().ValueRW;
            inputState.Horizontal = Input.GetAxisRaw("Horizontal");
            inputState.Vertical = Input.GetAxisRaw("Vertical");
            inputState.Space = Input.GetKeyDown(KeyCode.Space);
        }
    }
    // 움직임 상태
    public struct InputState : IComponentData
    {
        public float Horizontal;
        public float Vertical;
        public bool Space;
    }
}

