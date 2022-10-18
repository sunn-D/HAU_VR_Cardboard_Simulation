using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DunnGSunn
{
    public class SunPooler : SunMonoSingleton<SunPooler>
    {
        [Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [FoldoutGroup("Pools")] public List<Pool> pools;

        //
        public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

        private void Start()
        {
            foreach (var pool in pools)
            {
                var objPool = new Queue<GameObject>();
                for (var i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab, transform);
                    obj.SetActive(false);
                    objPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objPool);
            }
        }

        public static GameObject SpawnFromPool(string tagValue, Vector3 position, Quaternion rotation)
        {
            if (Instance.poolDictionary.ContainsKey(tagValue))
            {
                if (Instance.poolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.pools.Find(p => p.tag == tagValue);
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, Instance.transform);
                        obj.SetActive(false);
                        Instance.poolDictionary[tagValue].Enqueue(obj);
                    }
                }

                var objToSpawn = Instance.poolDictionary[tagValue].Dequeue();
                objToSpawn.SetActive(true);
                objToSpawn.transform.SetPositionAndRotation(position, rotation);
                return objToSpawn;
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
                return null;
            }
        }

        public static GameObject SpawnFromPool(string tagValue, Transform parent)
        {
            if (Instance.poolDictionary.ContainsKey(tagValue))
            {
                if (Instance.poolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.pools.Find(p => p.tag == tagValue);
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, Instance.transform);
                        obj.SetActive(false);
                        Instance.poolDictionary[tagValue].Enqueue(obj);
                    }
                }

                var objToSpawn = Instance.poolDictionary[tagValue].Dequeue();
                objToSpawn.SetActive(true);
                objToSpawn.transform.SetParent(parent);
                objToSpawn.transform.SetPositionAndRotation(parent.position, parent.rotation);
                return objToSpawn;
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
                return null;
            }
        }

        public static void AddToPool(string tagValue, GameObject objToAdd)
        {
            if (Instance.poolDictionary.ContainsKey(tagValue))
            {
                objToAdd.SetActive(false);
                Instance.poolDictionary[tagValue].Enqueue(objToAdd);
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
            }
        }
    }
}