using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class RotationSpeedAuthoring : MonoBehaviour
{
    public float DegreesPerSecond = 360.0f;

    class Baker : Baker<RotationSpeedAuthoring>
    {
        public override void Bake(RotationSpeedAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotationSpeed
            {
                RadianPerSecond = math.radians(authoring.DegreesPerSecond)
            });
        }
    }
}

public struct RotationSpeed : IComponentData
{
    public float RadianPerSecond;
}
