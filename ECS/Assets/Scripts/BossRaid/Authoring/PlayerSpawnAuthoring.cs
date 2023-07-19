using Unity.Entities;
using UnityEngine;

namespace BossRaid
{
    public class PlayerSpawnAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
    }

    public class SpawnBaker : Baker<PlayerSpawnAuthoring>
    {
        public override void Bake(PlayerSpawnAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Spawner
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None)
            });
        }
    }

    public struct Spawner : IComponentData
    {
        public Entity Prefab;
    }
}

