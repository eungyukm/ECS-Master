using Unity.Entities;
using UnityEngine;

namespace Survivor
{
    public class PlayerAuthoring : MonoBehaviour
    {
        // 경험치
        public int ExperiencePoints;
        // 공격력
        public int AttackStrength;
    
        public class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                // 캐릭터 태그 추가
                AddComponent<PlayerTag>(entity);
                // 경험치 추가
                AddComponent(entity, 
                    new CharacterExperiencePoints 
                        { Value = authoring.ExperiencePoints });
                // 공격력 추가
                AddComponent(entity, 
                    new CharacterAttackStrength 
                        { Value = authoring.AttackStrength });
            }
        }
    }
}