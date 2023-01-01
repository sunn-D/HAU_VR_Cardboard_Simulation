using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

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
        
        public enum InitOn
        {
            Awake, Start
        }

        #region Variables

        [FoldoutGroup("Variables")] 
        [SerializeField] private InitOn initOn;
        [FoldoutGroup("Variables")] 
        [SerializeField] private List<Pool> pools;
        
        //
        public Dictionary<string, Queue<GameObject>> PoolDictionary { get; private set; }

        #endregion

        #region Initialize

        //
        private void Initialize()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();
            foreach (var pool in pools)
            {
                var objPool = new Queue<GameObject>();
                
                var parentPool = transform.Find(pool.prefab.name + " Parent");
                if (parentPool == null)
                {
                    parentPool = new GameObject(pool.prefab.name + " Parent").transform;
                    parentPool.SetParent(transform);
                }
                
                for (var i = 0; i < pool.size; i++)
                {
                    var obj = Instantiate(pool.prefab, parentPool);
                    obj.name = pool.prefab.name;
                    obj.SetActive(false);
                    objPool.Enqueue(obj);
                }

                PoolDictionary.Add(pool.tag, objPool);
            }
        }

        //
        protected override void LoadInAwake()
        {
            if (initOn == InitOn.Awake) Initialize();
        }

        //
        protected override void LoadInStart()
        {
            if (initOn == InitOn.Start) Initialize();
        }

        #endregion

        #region Functions

        //
        public static GameObject SpawnFromPool(string tagValue, Vector3 position, Quaternion rotation)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                if (Instance.PoolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.pools.Find(p => p.tag == tagValue);
                    var parentPool = Instance.transform.Find(pool.prefab.name + " Parent");
                    if (parentPool == null)
                    {
                        parentPool = new GameObject(pool.prefab.name + " Parent").transform;
                    }
                
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, parentPool);
                        obj.name = pool.prefab.name;
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
                    var parentPool = Instance.transform.Find(pool.prefab.name + " Parent");
                    if (parentPool == null)
                    {
                        parentPool = new GameObject(pool.prefab.name + " Parent").transform;
                    }
                
                    for (var i = 0; i < pool.size; i++)
                    {
                        var obj = Instantiate(pool.prefab, parentPool);
                        obj.name = pool.prefab.name;
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
                var pool = Instance.pools.Find(p => p.tag == tagValue);
                var parentPool = Instance.transform.Find(pool.prefab.name + " Parent");
                if (parentPool == null)
                {
                    parentPool = new GameObject(pool.prefab.name + " Parent").transform;
                }
                objToAdd.SetActive(false);
                Instance.PoolDictionary[tagValue].Enqueue(objToAdd);
                objToAdd.transform.SetParent(parentPool);
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
            }
        }

        #endregion
    }
}