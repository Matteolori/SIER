using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Burst;
using Unity.Mathematics;
using static UnityEngine.EventSystems.EventTrigger;
using Events;
using Unity.Rendering;
using System.Diagnostics;
using UnityEngine;

[BurstCompile]
public partial struct OptimizedSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);


        // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
        new ProcessSpawnerJob
        {
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            Ecb = ecb,
            SpawnPoint = GetRandomOffset(20f),
        }.ScheduleParallel();

    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
    private float3 GetRandomOffset(float spawnArea)
    {
        float randomX = UnityEngine.Random.Range(-spawnArea, spawnArea);
        float randomY = 1f;
        float randomZ = UnityEngine.Random.Range(-spawnArea, spawnArea);
        return new float3(randomX, randomY, randomZ);
    }
}

[BurstCompile]
public partial struct ProcessSpawnerJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter Ecb;
    public double ElapsedTime;
    //public ComponentLookup<MaterialColor> MaterialColor;
    public float3 SpawnPoint { get; internal set; }

    // IJobEntity generates a component data query based on the parameters of its `Execute` method.
    // This example queries for all Spawner components and uses `ref` to specify that the operation
    // requires read and write access. Unity processes `Execute` for each entity that matches the
    // component data query.
    private void Execute([ChunkIndexInQuery] int chunkIndex, ref Spawner spawner)
    {
        // If the next spawn time has passed.
        if (spawner.NextSpawnTime < ElapsedTime)
        {
            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);
            Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));


            //Add velocity
            Ecb.AddComponent(chunkIndex, newEntity, new Velocity { Value = new float3(1f, 1f, 1f) });

            //Add status
            Ecb.AddComponent(chunkIndex, newEntity, new Status { State = chunkIndex == 0 ? State.Infectious : State.Susceptible });
            if (spawner.SpawnCounter == 0)
            {

                Ecb.AddComponent(chunkIndex, newEntity, new MaterialColor { Value = new float4(1f, 0, 0, 1f) });
            }
            spawner.SpawnCounter++;
            // Resets the next spawn time.
            spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            spawner.SpawnPosition = SpawnPoint;
        }
        if (spawner.NextSpawnTime < ElapsedTime)
        {
            // Spawns a new entity and positions it at the spawner.
            Entity newEntity = Ecb.Instantiate(chunkIndex, spawner.Prefab);
            Ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.SpawnPosition));


            //Add velocity
            Ecb.AddComponent(chunkIndex, newEntity, new Velocity { Value = new float3(1f, 1f, 1f) });

            //Add status
            Ecb.AddComponent(chunkIndex, newEntity, new Status { State = chunkIndex == 0 ? State.Infectious : State.Susceptible });
            if (spawner.SpawnCounter == 0)
            {

                Ecb.AddComponent(chunkIndex, newEntity, new MaterialColor { Value = new float4(1f, 0, 0, 1f) });
            }
            spawner.SpawnCounter++;
            // Resets the next spawn time.
            spawner.NextSpawnTime = (float)ElapsedTime + spawner.SpawnRate;
            spawner.SpawnPosition = SpawnPoint;
        }
    }
}
