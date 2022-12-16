using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sun_Package
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

        #region Variables

        //
        [SerializeField] private List<Pool> pools;
        
        //
        public Dictionary<string, Queue<GameObject>> PoolDictionary { get; set; } = new Dictionary<string, Queue<GameObject>>();

        #endregion

        #region Functions

        //
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

                PoolDictionary.Add(pool.tag, objPool);
            }
        }
        
        //
        public static GameObject SpawnFromPool(string tagValue, Vector3 position, Quaternion rotation)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                if (Instance.PoolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.pools.Find(p => p.tag == tagValue);
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, Instance.transform);
                        obj.SetActive(false);
                        Instance.PoolDictionary[tagValue].Enqueue(obj);
                    }
                }

                var objToSpawn = Instance.PoolDictionary[tagValue].Dequeue();
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
        
        //
        public static GameObject SpawnFromPool(string tagValue, Transform parent)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                if (Instance.PoolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.pools.Find(p => p.tag == tagValue);
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, Instance.transform);
                        obj.SetActive(false);
                        Instance.PoolDictionary[tagValue].Enqueue(obj);
                    }
                }

                var objToSpawn = Instance.PoolDictionary[tagValue].Dequeue();
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
        
        //
        public static void AddToPool(string tagValue, GameObject objToAdd)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                objToAdd.SetActive(false);
                Instance.PoolDictionary[tagValue].Enqueue(objToAdd);
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
            }
        }

        #endregion
    }
}