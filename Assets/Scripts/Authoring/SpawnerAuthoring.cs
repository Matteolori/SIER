using UnityEngine;
using Unity.Entities;

class SpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float SpawnRate;

}

class SpawnerBaker : Baker<SpawnerAuthoring>
{
    public Vector3 spawnArea = new Vector3(10f, 10f, 10f);
    public override void Bake(SpawnerAuthoring authoring)
    {
        // Generate random offsets within the spawn area
        float randomX = Random.Range(-spawnArea.x, spawnArea.x);
        float randomY = Random.Range(-spawnArea.y, spawnArea.y);
        float randomZ = Random.Range(-spawnArea.z, spawnArea.z);

        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new Spawner
        {

            // By default, each authoring GameObject turns into an Entity.
            // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
            SpawnPosition = authoring.transform.position + new Vector3(randomX, randomY, randomZ),
            NextSpawnTime = 0.0f,
            
            SpawnRate = authoring.SpawnRate
        });
    }
}
