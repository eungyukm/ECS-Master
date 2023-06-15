using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class ControllerAuthoring : MonoBehaviour
    {
        public float player_speed = 5.0f;

        class Baker : Baker<ControllerAuthoring>
        {
            public override void Bake(ControllerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // 컴포턴트를 추가하고, 캐릭터 속도를 ECS Data로 변환
                AddComponent(entity, new Controller
                {
                    player_speed = authoring.player_speed,
                });
            }
        }
    }
    
    public struct Controller : IComponentData
    {
        // 캐릭터 속도 제어 변수
        public float player_speed;
    }
}
