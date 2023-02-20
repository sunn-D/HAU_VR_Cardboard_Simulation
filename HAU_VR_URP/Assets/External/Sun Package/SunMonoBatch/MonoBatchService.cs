using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Sun_Package
{
    public class MonoBatchService : SunMonoSingleton<MonoBatchService>
    {
        #region Variables

        //
        private struct TypeMethods
        {
            public bool HasUpdate;
            public bool HasFixedUpdate;
        }
        
        //
        private Dictionary<Type, TypeMethods> _methodMapping = new Dictionary<Type, TypeMethods>();
        
        //
        private BatchedMonoBehaviour[] _batchedUpdates = new BatchedMonoBehaviour[1000];
        private BatchedMonoBehaviour[] _batchedFixedUpdates = new BatchedMonoBehaviour[1000];
        
        //
        private int _updateCount;
        private int _fixedUpdateCount;

        #endregion
        
        //
        private void Update()
        {
            OnUpdate();
        }

        //
        private void FixedUpdate()
        {
            OnFixedUpdate();
        }

        //
        public void AddBatchedBehaviour(BatchedMonoBehaviour behaviour)
        {
            var type = behaviour.GetType();
            if (!HasMethodMapping(type)) AddMethodMapping(type);
            var methodMapping = GetMethodMapping(type);
            
            if (methodMapping.HasUpdate)
            {
                _updateCount++;
                if (_updateCount >= _batchedUpdates.Length)
                {
                    Array.Resize(ref _batchedUpdates, _batchedUpdates.Length + 100);
                }
                _batchedUpdates[_updateCount - 1] = behaviour;
            }
            
            if (methodMapping.HasFixedUpdate)
            {
                _fixedUpdateCount++;
                if (_fixedUpdateCount >= _batchedFixedUpdates.Length)
                {
                    Array.Resize(ref _batchedFixedUpdates, _batchedFixedUpdates.Length + 100);
                }
                _batchedFixedUpdates[_fixedUpdateCount - 1] = behaviour;
            }
        }

        //
        private void AddMethodMapping(Type type)
        {
            var hasUpdate = type.GetMethod("BatchedUpdate")?.DeclaringType == type;
            var hasFixedUpdate = type.GetMethod("BatchedFixedUpdate")?.DeclaringType == type;
            _methodMapping.Add(type, new TypeMethods
            {
                HasUpdate = hasUpdate,
                HasFixedUpdate = hasFixedUpdate
            });
        }

        //
        private TypeMethods GetMethodMapping(Type type)
        {
            return _methodMapping[type];
        }

        //
        private bool HasMethodMapping(Type type)
        {
            return _methodMapping.ContainsKey(type);
        }

        //
        public void RemoveBatchedBehaviour(BatchedMonoBehaviour behaviour)
        {
            var type = behaviour.GetType();
            var methodMapping = GetMethodMapping(type);
            if (methodMapping.HasUpdate)
            {
                RemoveElement(ref _batchedUpdates, ref _updateCount, behaviour);
            }
            if (methodMapping.HasFixedUpdate)
            {
                RemoveElement(ref _batchedFixedUpdates, ref _fixedUpdateCount, behaviour);
            }
        }

        //
        private void RemoveElement(ref BatchedMonoBehaviour[] behaviours, ref int elementCount, BatchedMonoBehaviour behaviour)
        {
            for (var i = 0; i < elementCount; i++)
            {
                if (behaviours[i] == behaviour)
                {
                    behaviours[i] = behaviours[elementCount - 1];
                    behaviours[elementCount - 1] = null;
                    elementCount--;
                    return;
                }
            }
        }

        //
        public void OnUpdate()
        {
            var updateCount = _updateCount;
            for (var i = 0; i < updateCount; i++)
            {
                if (_batchedUpdates[i] == null)
                {
                    Debug.LogError("Found Batched Update with null!");
                }
                _batchedUpdates[i].BatchedUpdate();
            }
        }

        public void OnFixedUpdate()
        {
            var fixedUpdateCount = this._fixedUpdateCount;
            for (var i = 0; i < fixedUpdateCount; i++)
            {
                if (_batchedUpdates[i] == null)
                {
                    Debug.LogError("Found Batched FixedUpdate with null!");
                }
                _batchedFixedUpdates[i].BatchedFixedUpdate();
            }
        }

       

        
    }
}