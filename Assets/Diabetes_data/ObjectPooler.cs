// ObjectPooler.cs
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject objectToPool;
    public int initialPoolSize = 1000;
    private List<GameObject> pooledObjects;

    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPooledObject();
        }
    }

    private GameObject CreateNewPooledObject()
    {
        GameObject obj = Instantiate(objectToPool);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Expand pool if necessary
        Debug.LogWarning("No available pooled objects, expanding pool size.");
        return CreateNewPooledObject();
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}