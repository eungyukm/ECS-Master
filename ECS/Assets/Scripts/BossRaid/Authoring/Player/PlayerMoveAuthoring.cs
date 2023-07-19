using Unity.Entities;
using UnityEngine;

namespace BossRaid
{
    public class PlayerMoveAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
    }
    public class PlayerMoveBaker : Baker<PlayerMoveAuthoring>
    {
        public override void Bake(PlayerMoveAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerMoveSpeed
            {
                Value = authoring.MoveSpeed
            });
            AddComponent<NewSpawn>(entity);
        }
    }
    public struct PlayerMoveSpeed : IComponentData
    {
        public float Value;
    }
}

