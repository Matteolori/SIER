using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UIElements.Experimental;

public readonly partial struct SubjectMovementAspect : IAspect
{
    public readonly Entity Entity;
    private readonly RefRW<LocalTransform> _transform;
    // Start is called before the first frame update
    public void Move(float deltaTime)
    {
        //WalkTimer += deltaTime;
        //_transform.ValueRW.Position += _transform.ValueRO.Forward() * WalkSpeed * deltaTime;

        //var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
        //_transform.ValueRW.Rotation = quaternion.Euler(0, Heading, swayAngle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
