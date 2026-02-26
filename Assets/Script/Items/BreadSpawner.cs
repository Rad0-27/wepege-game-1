using System.Collections.Generic;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{
    public List<GameObject> breadPrefabs;
    public Transform spawnArea;

    public int spawnAmount = 3;   // berapa jenis order (misal 3 jenis muncul)
    public int minQty = 1;
    public int maxQty = 4;

    private List<GameObject> currentSpawned = new List<GameObject>();


    public List<BreadItem> SpawnRound()
    {
        // ===== CLEAR OLD ROUND =====
        foreach (GameObject g in currentSpawned)
            Destroy(g);

        currentSpawned.Clear();


        List<BreadItem> spawnedItems = new List<BreadItem>();

        Collider2D area = spawnArea.GetComponent<Collider2D>();


        // ===== PICK RANDOM TYPES =====
        for (int t = 0; t < spawnAmount; t++)
        {
            GameObject prefab = breadPrefabs[Random.Range(0, breadPrefabs.Count)];

            int qty = Random.Range(minQty, maxQty + 1);

            Debug.Log(prefab.name + " qty = " + qty);


            // ===== SPAWN VISUAL PER QTY =====
            for (int i = 0; i < qty; i++)
            {
                Vector3 pos = new Vector3(
                    Random.Range(area.bounds.min.x, area.bounds.max.x),
                    Random.Range(area.bounds.min.y, area.bounds.max.y),
                    0f
                );

                GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

                currentSpawned.Add(obj);

                BreadItem item = obj.GetComponent<BreadItem>();

                if (item != null)
                {
                    item.quantity = 1;     // tiap object = 1 roti
                    spawnedItems.Add(item);
                }
            }
        }

        return spawnedItems;
    }
}