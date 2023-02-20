using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Events;

// ReSharper disable CheckNamespace
namespace Sun_Package
{
    public static class SunEventManager
    {
        #region Fields

        //
        private static Dictionary<string, UnityEvent<object>> _eventDictionary = new Dictionary<string, UnityEvent<object>>();

        #endregion

        #region Listening functions

        //
        public static void StartListening(string eventName, UnityAction<object> callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.AddListener(callBack);
            }
            else
            {
                thisEvent = new UnityEvent<object>();
                thisEvent.AddListener(callBack);
                _eventDictionary.Add(eventName, thisEvent);
            }
        }
        
        //
        public static void StopListening(string eventName, UnityAction<object> callBack)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.RemoveListener(callBack);
            }
        }

        #endregion

        #region Event functions

        //
        public static void EmitEvent(string eventName)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke(null);
            }
        }
        
        //
        public static void EmitEvent(string eventName, object sender)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                thisEvent.Invoke(sender);
            }
        }
        
        //
        public static void EmitEvent(string eventName, float delay)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                if (delay <= 0)
                {
                    thisEvent.Invoke(null);
                }
                else
                {
                    var delayTime = (int) (delay * 1000);
                    DelayedInvoke(thisEvent, null, delayTime);
                }
            }
        }

        //
        public static void EmitEvent(string eventName, object sender, float delay)
        {
            if (_eventDictionary.TryGetValue(eventName, out var thisEvent))
            {
                if (delay <= 0)
                {
                    thisEvent.Invoke(sender);
                }
                else
                {
                    var d = (int) (delay * 1000);
                    DelayedInvoke(thisEvent, sender, d);
                }
            }
        }
        
        //
        private static async void DelayedInvoke(UnityEvent<object> thisEvent, object sender, int delay)
        {
            await Task.Delay(delay);
            thisEvent.Invoke(sender);
        }

        #endregion
    }
}