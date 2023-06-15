using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class EnemyAuthoring : MonoBehaviour
    {
        // 적 프리팹
        public GameObject EnemyPrefab;
        // 몬스터 체력
        public int StartingHitPoints;
        // 몬스터 경험치
        public int ExperiencePointsValue;
        
        public class EnemyBaker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // 적 태그 추가
                AddComponent(entity, new EnemyTag());
                AddComponent(entity, new CharacterHitPoints 
                    { Value = authoring.StartingHitPoints });
                AddComponent(entity, new CharacterExperiencePoints 
                    { Value = authoring.ExperiencePointsValue });
                // 적의 프리팹 ECS Data로 변환
                AddComponent(entity, new EnemyPrefab
                {
                    Value = GetEntity(authoring.EnemyPrefab, 
                        TransformUsageFlags.Dynamic)
                });
            }
        }
    }
    // 적 프리팹을 가지고 있는 데이터
    public struct EnemyPrefab : IComponentData
    {
        public Entity Value;
    }
}