using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Events
{
    public class StatusAuthoring : MonoBehaviour
    {

        class Baker : Baker<StatusAuthoring>
        {
            
            public override void Bake(StatusAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
               
                AddComponent(entity, new Status()
                {
                    State = State.Susceptible,
                });
            }
        }
    }

    public struct Status : IComponentData
    {
        public State State;
    }

    public enum State
    {
        Susceptible,
        Exposed,
        Infectious,
        Recovered
    }
}