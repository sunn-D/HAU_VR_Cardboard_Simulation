using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public abstract class ObjectPoolerSO<T> : ScriptableObject where T : Component
    {
        //
        [Serializable]
        public struct PoolData
        {
            [field: SerializeField] public string Tag { get; set; }
            [field: SerializeField] public T Object { get; set; }
            [field: SerializeField] public int AmountToSpawn { get; set; }

        }
        
        //
        [field: SerializeField] public List<PoolData> SerializePools { get; set; } = new List<PoolData>();
        public Dictionary<string, Queue<T>> PoolDictionary { get; } = new Dictionary<string, Queue<T>>();
        public Transform ParentPool { get; private set; }
        
        //
        public virtual void Initialize(Transform parent)
        {
            ParentPool = parent;
            if (SerializePools.Count > 0)
            {
                foreach (var pool in SerializePools)
                {
                    var queuePool = new Queue<T>();
                    for (var i = 0; i < pool.AmountToSpawn; i++)
                    {
                        var newObj = Spawn(pool.Object, ParentPool);
                        newObj.gameObject.name = pool.Tag;
                        newObj.gameObject.SetActive(false);
                        queuePool.Enqueue(newObj);
                    }
                    PoolDictionary.Add(pool.Tag, queuePool);
                }
            }
        }

        //
        public virtual T GetFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (PoolDictionary.ContainsKey(tag))
            {
                // Check if pool not have any element
                if (PoolDictionary[tag].Count == 0)
                {
                    var serializePool = SerializePools.Find(p => p.Tag.Contains(tag));
                    for (var i = 0; i < serializePool.AmountToSpawn; i++)
                    {
                        var newObj = Spawn(serializePool.Object, ParentPool);
                        newObj.gameObject.name = serializePool.Tag;
                        newObj.gameObject.SetActive(false);
                        PoolDictionary[tag].Enqueue(newObj);
                    }
                }
                
                // Get from pool
                var objGet = PoolDictionary[tag].Dequeue();
                objGet.gameObject.SetActive(true);
                objGet.transform.SetPositionAndRotation(position, rotation);
                return objGet;
            }

            Debug.LogWarning("Tag not exist in pool.");
            return null;
        }
        
        //
        public virtual T GetFromPool(string tag, Transform parent)
        {
            if (PoolDictionary.ContainsKey(tag))
            {
                // Check if pool not have any element
                if (PoolDictionary[tag].Count == 0)
                {
                    var serializePool = SerializePools.Find(p => p.Tag.Contains(tag));
                    for (var i = 0; i < serializePool.AmountToSpawn; i++)
                    {
                        var newObj = Spawn(serializePool.Object, ParentPool);
                        newObj.gameObject.name = serializePool.Tag;
                        newObj.gameObject.SetActive(false);
                        PoolDictionary[tag].Enqueue(newObj);
                    }
                }
                
                // Get from pool
                var objGet = PoolDictionary[tag].Dequeue();
                objGet.gameObject.SetActive(true);
                objGet.transform.SetParent(parent);
                return objGet;
            }

            Debug.LogWarning("Tag not exist in pool.");
            return null;
        }
        
        //
        public virtual T GetFromPool(string tag, Transform parent, Vector3 position, Quaternion rotation)
        {
            if (PoolDictionary.ContainsKey(tag))
            {
                // Check if pool not have any element
                if (PoolDictionary[tag].Count == 0)
                {
                    var serializePool = SerializePools.Find(p => p.Tag.Contains(tag));
                    for (var i = 0; i < serializePool.AmountToSpawn; i++)
                    {
                        var newObj = Spawn(serializePool.Object, ParentPool);
                        newObj.gameObject.name = serializePool.Tag;
                        newObj.gameObject.SetActive(false);
                        PoolDictionary[tag].Enqueue(newObj);
                    }
                }
                
                // Get from pool
                var objGet = PoolDictionary[tag].Dequeue();
                objGet.gameObject.SetActive(true);
                objGet.transform.SetParent(parent);
                objGet.transform.SetPositionAndRotation(position, rotation);
                return objGet;
            }

            Debug.LogWarning("Tag not exist in pool.");
            return null;
        }
        
        //
        public virtual void AddToPool(string tag, T objAdd)
        {
            if (PoolDictionary.ContainsKey(tag))
            {
                objAdd.gameObject.SetActive(false);
                objAdd.transform.SetParent(ParentPool);
                PoolDictionary[tag].Enqueue(objAdd);
            }
            else
            {
                Debug.LogWarning("Tag not exist in pool.");
            }
        }
        
        //
        public abstract T Spawn(T obj, Transform parent);
    }
}
