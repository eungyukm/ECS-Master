using Unity.Entities;
using UnityEngine;

namespace BossRaid
{
    public class PlayerAuthoring : MonoBehaviour
    {
    }
    
    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            // 플레이어 태그 추가
            AddComponent<PlayerTag>(entity);
        }
    }
}
