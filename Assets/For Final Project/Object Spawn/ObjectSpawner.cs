using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public string name;
        public GameObject prefab;

        [Range(0, 100)]
        public float spawnChance = 50f;
        public bool canRespawn;
        public float respawnTime = 5f;

        [Header("Limit Settings")]
        public bool limitSpawnCount;
        public int maxSpawnCount = 1;

        [HideInInspector]
        public int currentSpawnCount;
    }

    [Header("Settings")]
    [SerializeField] private SpawnableItem[] itemsToSpawn;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Amount Settings")]
    [SerializeField] private int minObjectsToSpawn = 3; // [ใหม่] จำนวนต่ำสุด
    [SerializeField] private int maxObjectsToSpawn = 5; // [ใหม่] จำนวนสูงสุด

    private List<Transform> occupiedPoints = new List<Transform>();

    private void Start()
    {
        occupiedPoints.Clear();

        if (itemsToSpawn != null)
        {
            foreach (var item in itemsToSpawn)
            {
                item.currentSpawnCount = 0;
            }
        }

        SpawnObjects();
    }

    private void SpawnObjects()
    {
        if (itemsToSpawn == null || itemsToSpawn.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
            return;

        // [ใหม่] สุ่มจำนวนที่จะเกิดในรอบนี้ (บวก 1 เพื่อให้รวมเลขตัวท้ายด้วย)
        int targetSpawnCount = Random.Range(minObjectsToSpawn, maxObjectsToSpawn + 1);

        // เช็คความปลอดภัย: อย่าให้จำนวนที่จะเกิด มากกว่าจุดที่มีจริง
        if (targetSpawnCount > spawnPoints.Length)
        {
            targetSpawnCount = spawnPoints.Length;
        }

        int currentSpawnCountInScene = 0;

        // วนลูปจนกว่าจะได้ตามจำนวนที่สุ่มมา
        while (currentSpawnCountInScene < targetSpawnCount)
        {
            Transform freePoint = GetRandomFreeSpawnPoint();
            if (freePoint == null)
            {
                Debug.LogWarning("ที่เต็ม Spawnไม่ได้");
                break;
            }

            SpawnableItem selectedItem = GetRandomItemBasedOnWeight();

            if (selectedItem != null)
            {
                CreateObject(selectedItem, freePoint);
                currentSpawnCountInScene++;
            }
            else
            {
                Debug.Log("ไม่มีไอเทมเหลือให้สปอว์นแล้ว (ครบโควตาหมดแล้ว)");
                break;
            }
        }
    }

    private Transform GetRandomFreeSpawnPoint()
    {
        List<Transform> availablePoints = new List<Transform>();
        foreach (Transform point in spawnPoints)
        {
            if (!occupiedPoints.Contains(point))
            {
                availablePoints.Add(point);
            }
        }
        if (availablePoints.Count == 0)
        {
            return null;
        }

        return availablePoints[Random.Range(0, availablePoints.Count)];
    }

    private SpawnableItem GetRandomItemBasedOnWeight()
    {
        float totalWeight = 0f;
        List<SpawnableItem> validItems = new List<SpawnableItem>();

        foreach (var item in itemsToSpawn)
        {
            if (item.limitSpawnCount && item.currentSpawnCount >= item.maxSpawnCount)
            {
                continue;
            }

            validItems.Add(item);
            totalWeight += item.spawnChance;
        }

        if (validItems.Count == 0)
        {
            return null;
        }

        float randomValue = Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var item in validItems)
        {
            currentWeight += item.spawnChance;
            if (randomValue <= currentWeight)
            {
                return item;
            }
        }
        return validItems[0];
    }

    private void CreateObject(SpawnableItem itemData, Transform spawnLocation)
    {
        itemData.currentSpawnCount++;
        occupiedPoints.Add(spawnLocation);

        GameObject newObj = Instantiate(itemData.prefab, spawnLocation.position, spawnLocation.rotation);

        if (itemData.canRespawn)
        {
            StartCoroutine(RespawnRoutine(newObj, itemData, spawnLocation));
        }
    }

    IEnumerator RespawnRoutine(GameObject obj, SpawnableItem oldItemData, Transform oldLocation)
    {
        yield return new WaitUntil(() => obj == null);

        occupiedPoints.Remove(oldLocation);

        yield return new WaitForSeconds(oldItemData.respawnTime);

        Transform newSpot = GetRandomFreeSpawnPoint();

        if (newSpot != null)
        {
            SpawnableItem newItem = GetRandomItemBasedOnWeight();

            if (newItem != null)
            {
                CreateObject(newItem, newSpot);
            }
            else
            {
                Debug.Log("ของครบโควตาหมดแล้ว ไม่เกิดเพิ่ม");
            }
        }
        else
        {
            Debug.Log("ที่เต็มแล้ว รอไปก่อน");
        }
    }
}