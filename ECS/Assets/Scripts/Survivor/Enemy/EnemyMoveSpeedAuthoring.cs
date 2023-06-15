using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class EnemyMoveSpeedAuthoring : MonoBehaviour
    {
        public float MoveSpeed;
        
        public class EnemyMoveSpeedBaker : Baker<EnemyMoveSpeedAuthoring>
        {
            public override void Bake(EnemyMoveSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyMoveSpeed { Value = authoring.MoveSpeed });
            }
        }
    }

    public struct EnemyProperties : IComponentData
    {
        public Entity TargetCharacter;
        public Entity OriginCharacter;
    }

    public struct EnemyMoveSpeed : IComponentData
    {
        public float Value;
    }
}