using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class ProjectilePrefabAuthoring : MonoBehaviour
    {
        public GameObject ProjetilePrefab;

        public class ProjectilePrefabBaker : Baker<ProjectilePrefabAuthoring>
        {
            public override void Bake(ProjectilePrefabAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectilePrefab
                {
                    Value = GetEntity(authoring.ProjetilePrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct ProjectilePrefab : IComponentData
    {
        public Entity Value;
    }
}

