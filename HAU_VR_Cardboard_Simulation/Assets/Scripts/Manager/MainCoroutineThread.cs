using System;
using System.Collections.Generic;
using DunnGSunn;

namespace Manager
{
    public class MainCoroutineThread : SunMonoSingleton<MainCoroutineThread>
    {
        #region Variables

        public static List<Action> ActionOneTime = new List<Action>();
        public static List<Action> ActionUpdates = new List<Action>();

        #endregion
        
        //
        private void Update()
        {
            if (ActionOneTime.Count > 0)
            {
                var arrayActions = ActionOneTime.ToArray();
                ActionOneTime.Clear();
                foreach (var action in arrayActions)
                {
                    action.Invoke();
                }
            }

            foreach (var update in ActionUpdates)
            {
                update.Invoke();
            }
        }
    }
}