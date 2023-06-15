using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Survivor
{
    public partial class DealDamageSystem : SystemBase
    {
        public Action<int, float3> OnDealDamage;
        public Action<int, float3> OnGrantExperience;

        protected override void OnUpdate()
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (hitPoints, damageToCharacter, experiencePoints, transform, entity) in
                     SystemAPI.Query<RefRW<CharacterHitPoints>, DamageToCharacter, CharacterExperiencePoints,
                     LocalTransform>().WithEntityAccess())
            {
                // 적 체력 감소
                hitPoints.ValueRW.Value -= damageToCharacter.Value;
                
                // DealDagme 이벤트 호출
                OnDealDamage?.Invoke(damageToCharacter.Value, transform.Position);
                
                ecb.RemoveComponent<DamageToCharacter>(entity);

                if (hitPoints.ValueRO.Value <= 0)
                {
                    ecb.DestroyEntity(entity);
                    var originCharacterExperience =
                        SystemAPI.GetComponent<CharacterExperiencePoints>(damageToCharacter.OriginCharacter);
                    originCharacterExperience.Value += experiencePoints.Value;
                    SystemAPI.SetComponent(damageToCharacter.OriginCharacter, originCharacterExperience);

                    var originCharacterPosition =
                        SystemAPI.GetComponent<LocalTransform>(damageToCharacter.OriginCharacter).Position;
                    OnGrantExperience?.Invoke(experiencePoints.Value, originCharacterPosition);
                }
            }
            
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }
    }
}

