using Unity.Entities;
using UnityEngine;

namespace ECSMater.Prefabs
{
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject Prefabs;

        class Baker : Baker<SpawnerAuthoring>
        {
            public override void Bake(SpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Spawner
                {
                    Prefab = GetEntity(authoring.Prefabs, TransformUsageFlags.None)
                });
            }
        }
    }

    struct Spawner : IComponentData
    {
        public Entity Prefab;
    }
}

