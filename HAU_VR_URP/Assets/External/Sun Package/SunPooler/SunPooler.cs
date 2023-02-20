using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    [Serializable] 
    public class Pool
    {
        [field: SerializeField] public string Tag { get; set; }
        [field: SerializeField] public GameObject Prefab { get; set; }
        [field: SerializeField] public int Size { get; set; }

        //
        public Pool(string tag, GameObject prefab, int size)
        {
            Tag = tag;
            Prefab = prefab;
            Size = size;
        }
    }
    
    //
    public class SunPooler : SunMonoSingleton<SunPooler>
    {
        #region Variables
        
        //
        public enum InitOn { Awake, Start }
        
        //
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public InitOn InitializeOn { get; set; }
        [field: FoldoutGroup("Variables")] 
        [field: SerializeField] public List<Pool> SerializePools { get; set; }
        
        //
        public Dictionary<string, Queue<GameObject>> PoolDictionary { get; private set; }

        #endregion

        #region Initialize

        //
        protected override void LoadInAwake()
        {
            if (InitializeOn == InitOn.Awake) Initialize();
        }

        //
        protected override void LoadInStart()
        {
            if (InitializeOn == InitOn.Start) Initialize();
        }

        //
        private void Initialize()
        {
            PoolDictionary = new Dictionary<string, Queue<GameObject>>();
            if (SerializePools.Count > 0)
            {
                foreach (var pool in SerializePools)
                {
                    var newPool = new Queue<GameObject>();
                
                    var parentPool = transform.Find(pool.Prefab.name + " Parent");
                    if (parentPool == null)
                    {
                        parentPool = new GameObject(pool.Prefab.name + " Parent").transform;
                        parentPool.SetParent(transform);
                    }
                
                    for (var i = 0; i < pool.Size; i++)
                    {
                        var obj = Instantiate(pool.Prefab, parentPool);
                        obj.name = pool.Prefab.name;
                        obj.SetActive(false);
                        newPool.Enqueue(obj);
                    }

                    PoolDictionary.Add(pool.Tag, newPool);
                }
            }
        }

        #endregion

        #region Functions
        
        //
        public static void CreatePool(string tagValue, GameObject prefabValue, int sizeValue)
        {
            if (Instance.SerializePools.Find(o => o.Tag == tagValue) == null)
            {
                var newSerializePool = new Pool(tagValue, prefabValue, sizeValue);
                Instance.SerializePools.Add(newSerializePool);
            }
            
            if (!Instance.PoolDictionary.ContainsKey(tagValue))
            {
                var newPool = new Queue<GameObject>();
                
                var parentPool = Instance.transform.Find(prefabValue.name + " Parent");
                if (parentPool == null)
                {
                    parentPool = new GameObject(prefabValue.name + " Parent").transform;
                    parentPool.SetParent(Instance.transform);
                }
                
                for (var i = 0; i < sizeValue; i++)
                {
                    var obj = Instantiate(prefabValue, parentPool);
                    obj.name = prefabValue.name;
                    obj.SetActive(false);
                    newPool.Enqueue(obj);
                }

                Instance.PoolDictionary.Add(tagValue, newPool);
            }
        }

        //
        public static GameObject SpawnFromPool(string tagValue, Vector3 position, Quaternion rotation)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                if (Instance.PoolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.SerializePools.Find(p => p.Tag == tagValue);
                    var parentPool = Instance.transform.Find(pool.Prefab.name + " Parent");
                    if (parentPool == null)
                    {
                        parentPool = new GameObject(pool.Prefab.name + " Parent").transform;
                        parentPool.SetParent(Instance.transform);
                    }
                
                    for (var i = 0; i < pool.Size; i++)
                    {
                        var obj = Instantiate(pool.Prefab, parentPool);
                        obj.name = pool.Prefab.name;
                        obj.SetActive(false);
                        Instance.PoolDictionary[tagValue].Enqueue(obj);
                    }
                }

                var objToSpawn = Instance.PoolDictionary[tagValue].Dequeue();
                objToSpawn.SetActive(true);
                objToSpawn.transform.SetPositionAndRotation(position, rotation);
                return objToSpawn;
            }

            Debug.LogWarning("Tag not exist in pool.");
            return null;
        }
        
        //
        public static GameObject SpawnFromPool(string tagValue, Transform parent)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                if (Instance.PoolDictionary[tagValue].Count == 0)
                {
                    var pool = Instance.SerializePools.Find(p => p.Tag == tagValue);
                    var parentPool = Instance.transform.Find(pool.Prefab.name + " Parent");
                    if (parentPool == null)
                    {
                        parentPool = new GameObject(pool.Prefab.name + " Parent").transform;
                        parentPool.SetParent(Instance.transform);
                    }
                
                    for (var i = 0; i < pool.Size; i++)
                    {
                        var obj = Instantiate(pool.Prefab, parentPool);
                        obj.name = pool.Prefab.name;
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

            Debug.LogWarning("Tag not exist in pool.");
            return null;
        }
        
        //
        public static void AddToPool(string tagValue, GameObject objToAdd)
        {
            if (Instance.PoolDictionary.ContainsKey(tagValue))
            {
                var pool = Instance.SerializePools.Find(p => p.Tag == tagValue);
                var parentPool = Instance.transform.Find(pool.Prefab.name + " Parent");
                if (parentPool == null)
                {
                    parentPool = new GameObject(pool.Prefab.name + " Parent").transform;
                    parentPool.SetParent(Instance.transform);
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