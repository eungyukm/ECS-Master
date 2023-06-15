using Unity.Entities;

namespace Survivor
{
    // 캐릭터의 공격력
    public struct CharacterAttackStrength : IComponentData
    {
        public int Value;
    }
    // 캐릭터의 경험치
    public struct CharacterExperiencePoints : IComponentData
    {
        public int Value;
    }
    // 캐릭터의 체력
    public struct CharacterHitPoints : IComponentData
    {
        public int Value;
    }
    public struct DamageToCharacter : IComponentData
    {
        public int Value;
        public Entity OriginCharacter;
    }
    
    // 적 태그
    public struct EnemyTag : IComponentData { }
    // 캐릭터 태그
    public struct PlayerTag : IComponentData { }
}

