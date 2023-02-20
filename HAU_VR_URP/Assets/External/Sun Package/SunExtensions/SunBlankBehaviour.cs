using System;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public class SunBlankBehaviour : BatchedMonoBehaviour
    {
        //
        public Events events = new Events();

        //
        private void Awake()
        {
            events.onAwake.Invoke();
        }

        //
        protected override void Start()
        {
            base.Start();
            events.onStart.Invoke();
        }

        //
        public override void BatchedUpdate()
        {
            events.onUpdate.Invoke();
        }

        //
        public override void BatchedFixedUpdate()
        {
            base.BatchedFixedUpdate();
            events.onFixedUpdate.Invoke();
        }

        //
        private void OnEnable()
        {
            events.onEnable.Invoke();
        }

        //
        private void OnDisable()
        {
            events.onDisable.Invoke();
        }

        //
        protected override void OnDestroy()
        {
            base.OnDestroy();
            events.onDestroy.Invoke();
        }

        //
        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }

        //
        [Serializable]
        public class Events
        {
            public UnityEvent onAwake = new UnityEvent();
            public UnityEvent onStart = new UnityEvent();
            public UnityEvent onEnable = new UnityEvent();
            public UnityEvent onUpdate = new UnityEvent();
            public UnityEvent onFixedUpdate = new UnityEvent();
            public UnityEvent onDisable = new UnityEvent();
            public UnityEvent onDestroy = new UnityEvent();
        }
    }
}