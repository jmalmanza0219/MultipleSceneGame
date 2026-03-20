using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
   [Header("Pool Settings")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        // Pre-create objects
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    // Get an object from the pool
    public GameObject GetObject()
    {
        // Look for inactive object
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // None available → create new one
        GameObject newObj = Instantiate(prefab, transform);
        pool.Add(newObj);
        return newObj;
    }

    // Return object to pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        // Keep things organized in hierarchy
        obj.transform.SetParent(transform);

        if (!pool.Contains(obj))
        {
            pool.Add(obj);
        }
    }
}
