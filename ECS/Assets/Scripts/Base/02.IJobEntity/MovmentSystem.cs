using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class MovmentSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // new MoveJob{DeltaTime = Time.DeltaTime}.Run();
    }
}
// [BurstCompile]
// public partial struct MoveJob : IJobEntity
// {
//     public float DeltaTime;
//     public float MoveMod;
//
//     void Execute(ref Translate translation, in MoveSpeed )
//     {
//             
//     }
// }