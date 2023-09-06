using Events;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Rendering;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct StatusChangeOnCollisionSystem : ISystem
{

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Status>();
        state.RequireForUpdate<SimulationSingleton>();
    }


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new CollisionEventJob
        {
            Status = SystemAPI.GetComponentLookup<Status>(),
            PhysicsVelocityData = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
            MaterialColor = SystemAPI.GetComponentLookup<MaterialColor>(),
    }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }


    [BurstCompile]
    struct CollisionEventJob : ICollisionEventsJob
    {
        public ComponentLookup<Status> Status;
        public ComponentLookup<PhysicsVelocity> PhysicsVelocityData;
        public ComponentLookup<MaterialColor> MaterialColor;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            // if B è infetto e A no
            Status.GetRefRW(entityA).ValueRW.State = State.Infectious;
            bool isBodyADynamic = PhysicsVelocityData.HasComponent(entityA);
            bool isBodyBDynamic = PhysicsVelocityData.HasComponent(entityB);

            if (isBodyADynamic && isBodyBDynamic)
            {
                RefRW<MaterialColor> volumeMaterialInfoA = MaterialColor.GetRefRW(entityA);
                RefRW<MaterialColor> volumeMaterialInfoB = MaterialColor.GetRefRW(entityB);

                var materialColor = new MaterialColor();
                materialColor.Value = new float4(1f, 0, 0, 1f);

                volumeMaterialInfoA.ValueRW = materialColor;

            }
            
        }
    }

    private static void ChangeCapsuleColor(Entity capsuleEntity, MaterialColor materialMeshInfo)
    {

        // Change the color of the capsule entity
        // Access the RenderMesh or Material component and modify its color
        // Similar to the previous examples

       
        
        
    }
}
