using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class EnemyEntityReferenceAuthoring : MonoBehaviour
    {
        public int EnemySpawnCount;
    }
    
    public class EnemyEntityReferenceBaker : Baker<EnemyEntityReferenceAuthoring>
    {
        public override void Bake(EnemyEntityReferenceAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<EnemyEntityReferenceTag>(entity);
            // 스폰하고 있는 적들의 개수
            AddComponent(entity, new EnemySpawnCount
            {
                Value = authoring.EnemySpawnCount
            });
            // 생성한 적들을 가지고 있는 Buffer를 생성
            AddBuffer<EnemyEntityReferenceBufferElement>(entity);
        }
    }
    public struct EnemyEntityReferenceTag : IComponentData {}
    // 적들을 가지고 있는 Buffer Data
    [InternalBufferCapacity(8)]
    public struct EnemyEntityReferenceBufferElement : IBufferElementData
    {
        public Entity Value;

        public static implicit operator EnemyEntityReferenceBufferElement
            (Entity value)
        {
            return new EnemyEntityReferenceBufferElement 
                { Value = value };
        }
    }
    // 몇개의 적을 Spawn하였는지 측정하는 Data
    public struct EnemySpawnCount : IComponentData
    {
        public int Value;
    }
}

