using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

namespace Survivor
{
    public class ProjectileMoveSpeedAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        
        public class ProjectileMoveSpeedBaker : Baker<ProjectileMoveSpeedAuthoring>
        {
            public override void Bake(ProjectileMoveSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ProjectileMoveSpeed { Value = authoring.MoveSpeed });
            }
        }
    }

    public struct ProjectileProperties : IComponentData
    {
        public float3 TargetPosition;
        public Entity TargetCharacter;
        public Entity OriginCharacter;
        public int DamageAmount;
    }

    public struct ProjectileMoveSpeed : IComponentData
    {
        public float Value;
    }
}

