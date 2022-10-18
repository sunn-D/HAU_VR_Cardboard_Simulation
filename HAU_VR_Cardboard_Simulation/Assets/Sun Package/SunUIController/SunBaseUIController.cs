using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

namespace DunnGSunn
{
    public class SunBaseUIController : SunMonoSingleton<SunBaseUIController>
    {
        #region Variables

        //
        public enum LoadIn { Awake, Start }
        
        //
        [FoldoutGroup("Variables")]
        //
        [FoldoutGroup("Variables/Base info")]
        public List<SunBaseUI> uiScreens;
        [FoldoutGroup("Variables/Base info")] 
        public SunBaseUI startingScreen;
        [FoldoutGroup("Variables/Base info")] 
        public LoadIn loadInAction;
        //
        [FoldoutGroup("Variables/Stack UI")]
        public Stack<SunBaseUI> uiStack = new Stack<SunBaseUI>();
        //
        [FoldoutGroup("Variables/Event System")] 
        public EventSystem eventSystem;

        #endregion
        
        //
        public static void SetEventSystemValue(bool value)
        {
            Instance.eventSystem.enabled = value;
        }
        
        //
        public static T GetScreen<T>() where T : SunBaseUI
        {
            foreach (var screenUI in Instance.uiScreens)
            {
                if (screenUI is T tScreenUI)
                {
                    return tScreenUI;
                }
            }
            return null;
        }
        
        //
        public static bool IsScreenInTopStack<T>() where T : SunBaseUI
        {
            return Instance.uiStack.Count > 0 && GetScreen<T>() == Instance.uiStack.Peek();
        }
        
        //
        public static void PushScreen<T>(bool showPushScreen = true, bool hideCurrentScreen = true) where T : SunBaseUI
        {
            if (Instance.uiStack.Count > 0)
            {
                var currentScreen = Instance.uiStack.Peek();
                if (hideCurrentScreen) currentScreen.Hide();
            }
            
            var screen = GetScreen<T>();
            if (showPushScreen) screen.Show();

            Instance.uiStack.Push(screen);
        }
        
        //
        public static void PopScreen(bool hideCurrentScreen = true, bool showNewCurrentScreen = true)
        {
            if (Instance.uiStack.Count > 1)
            {
                var currentScreen = Instance.uiStack.Pop();
                var newCurrentScreen = Instance.uiStack.Peek();
                
                if (hideCurrentScreen) currentScreen.Hide();
                if (showNewCurrentScreen) newCurrentScreen.Show();
            }
        }
        
        //
        public static void PopAllScreen()
        {
            var totalScreenInStack = Instance.uiStack.Count;
            while (totalScreenInStack > 2)
            {
                PopScreen(true, false);
                totalScreenInStack--;
            }
            PopScreen();
        }
        
        //
        public virtual void Initialize()
        {
            foreach (var screenUI in Instance.uiScreens)
            {
                screenUI.gameObject.SetActive(true);
                screenUI.Initialize();
                screenUI.gameObject.SetActive(false);
            }

            if (Instance.startingScreen != null)
            {
                Instance.uiStack.Push(Instance.startingScreen);
                Instance.startingScreen.Show();
            }
        }
        
        // Unity callback function: Awake
        protected override void LoadInAwake()
        {
            if (loadInAction == LoadIn.Awake) Initialize();
        }

        // Unity callback function: Start
        private void Start()
        {
            if (loadInAction == LoadIn.Start) Initialize();
        }
        
        //
        [FoldoutGroup("Button"), Button]
        public void GetAllScreenInChildren()
        {
            var uis = transform.GetComponentsInChildren<SunBaseUI>();
            if (uiScreens == null) uiScreens = new List<SunBaseUI>();
            uiScreens.Clear();
            foreach (var ui in uis)
            {
                uiScreens.Add(ui);
            }
        }
    }
}