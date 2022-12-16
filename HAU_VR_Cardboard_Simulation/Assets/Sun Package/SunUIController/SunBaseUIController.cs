using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sun_Package
{
    public class SunBaseUIController : SunMonoSingleton<SunBaseUIController>
    {
        #region Variables

        //
        public enum LoadIn { Awake, Start }
        
        //
        [FoldoutGroup("Variables")]
        [FoldoutGroup("Variables/Base info")]
        [SerializeField] private List<SunBaseUI> uiScreens;
        [FoldoutGroup("Variables/Base info")] 
        [SerializeField] private SunBaseUI startingScreen;
        [FoldoutGroup("Variables/Base info")] 
        [SerializeField] private LoadIn loadInAction;
        //
        [FoldoutGroup("Variables/Event System")]
        [SerializeField] private bool usingEventSystem;
        [FoldoutGroup("Variables/Event System"), ShowIf(nameof(usingEventSystem))] 
        [SerializeField] private EventSystem eventSystem;
        
        //
        public Stack<SunBaseUI> UIStack { get; set; } = new Stack<SunBaseUI>();

        #endregion

        #region Functions

        //
        public static void SetEventSystemValue(bool value)
        {
            if (Instance.usingEventSystem)
            {
                Instance.eventSystem.enabled = value;
            }
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

            Debug.LogWarning("Can't find screen. Checking all Screen UI!");
            return null;
        }
        
        //
        public static bool IsScreenInTopStack<T>() where T : SunBaseUI
        {
            return Instance.UIStack.Count > 0 && GetScreen<T>() == Instance.UIStack.Peek();
        }
        
        //
        public static void PushScreen<T>(bool showPushScreen = true, bool hideCurrentScreen = true) where T : SunBaseUI
        {
            if (Instance.UIStack.Count > 0)
            {
                var currentScreen = Instance.UIStack.Peek();
                if (hideCurrentScreen) currentScreen.Hide();
            }
            
            var screen = GetScreen<T>();
            if (showPushScreen) screen.Show();

            Instance.UIStack.Push(screen);
        }
        
        //
        public static void PopScreen(bool hideCurrentScreen = true, bool showNewCurrentScreen = true)
        {
            if (Instance.UIStack.Count > 1)
            {
                var currentScreen = Instance.UIStack.Pop();
                var newCurrentScreen = Instance.UIStack.Peek();
                
                if (hideCurrentScreen) currentScreen.Hide();
                if (showNewCurrentScreen) newCurrentScreen.Show();
            }
        }
        
        //
        public static void PopAllScreen()
        {
            var totalScreenInStack = Instance.UIStack.Count;
            while (totalScreenInStack > 2)
            {
                PopScreen(true, false);
                totalScreenInStack--;
            }
            PopScreen();
        }
        
        //
        public void Initialize()
        {
            foreach (var screenUI in Instance.uiScreens)
            {
                screenUI.gameObject.SetActive(true);
                screenUI.Initialize();
                screenUI.gameObject.SetActive(false);
            }

            if (Instance.startingScreen != null)
            {
                Instance.UIStack.Push(Instance.startingScreen);
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

        #endregion
    }
}