using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunFuncGO
    {
        //
        public static void AddEventTrigger(Transform transform, EventTriggerType type, Action action)
        {
            var trigger = transform.GetComponent<EventTrigger>();
            if (trigger == null) trigger = transform.gameObject.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {eventID = type};
            entry.callback.AddListener((eventData) => { action.Invoke(); });
            trigger.triggers.Add(entry);
        }

        //
        public static IEnumerator DelayCoroutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
        
        //
        public static IEnumerator WaitCoroutine(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(condition);
            action.Invoke();
        }
    }
}