using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
// ReSharper disable CheckNamespace

namespace Sun_Package
{
    public class SunBaseUIController : SunMonoSingleton<SunBaseUIController>
    {
        #region Variables

        //
        public enum LoadIn { Awake, Start }
        
        //
        [FoldoutGroup("Variables")]
        [SerializeField] private List<SunBaseUI> uiScreens;
        [FoldoutGroup("Variables")]
        [SerializeField] private SunBaseUI startingScreen;
        [FoldoutGroup("Variables")]
        [SerializeField] private LoadIn loadInAction;
        [FoldoutGroup("Variables")] 
        [SerializeField] private bool logging;
        [FoldoutGroup("Variables")] 
        [SerializeField] private GameObject startCanvas;
        [FoldoutGroup("Variables")] 
        [SerializeField] private GameObject outCanvas;
        
        //
        public List<SunBaseUI> UIScreens => uiScreens;
        public SunBaseUI StartingScreen => startingScreen;
        public LoadIn LoadInAction => loadInAction;

        //
        public Stack<SunBaseUI> UIStack { get; private set; }

        #endregion

        #region Initialize

        //
        public void Initialize()
        {
            //
            UIStack = new Stack<SunBaseUI>();
            
            //
            foreach (var screenUI in Instance.UIScreens)
            {
                screenUI.gameObject.SetActive(true);
                screenUI.Initialize();
                screenUI.gameObject.SetActive(false);
            }

            //
            if (Instance.StartingScreen != null)
            {
                Instance.UIStack.Push(Instance.StartingScreen);
                Instance.StartingScreen.Show();
            }
            else
            {
                if (logging) Debug.LogWarning("No reference to starting screen. Check missing in UI Controller.");
            }
        }
        
        //
        protected override void LoadInAwake()
        {
            if (LoadInAction == LoadIn.Awake)
            {
                Initialize();
                if (logging) Debug.Log("Initialized on Awake.");
            }
        }

        //
        protected override void LoadInStart()
        {
            if (LoadInAction == LoadIn.Start)
            {
                Initialize();
                if (logging) Debug.Log("Initialized on Start.");
            }
        }

        #endregion

        #region Functions

        //
        public static T GetScreen<T>() where T : SunBaseUI
        {
            foreach (var screenUI in Instance.UIScreens)
            {
                if (screenUI is T tScreenUI)
                {
                    return tScreenUI;
                }
            }

            if (Instance.logging) Debug.LogWarning($"No {typeof(T)} in UIScreen.");
            return null;
        }
        
        //
        public static bool IsScreenInTopStack<T>() where T : SunBaseUI
        {
            var result = Instance.UIStack.Count > 0 && GetScreen<T>() == Instance.UIStack.Peek();
            if (Instance.logging) Debug.Log($"{typeof(T)} is on top of UIStack: {result}");
            return result;
        }
        
        //
        public static bool IsScreenInStack<T>() where T : SunBaseUI
        {
            var result = false;
            var screenCheck = GetScreen<T>();
            if (screenCheck != null && Instance.UIStack.Count > 0)
            {
                foreach (var ui in Instance.UIStack)
                {
                    if (ui == screenCheck) result = true;
                }
            }
            if (Instance.logging) Debug.Log($"{typeof(T)} is in UIStack: {result}");
            return result;
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
            if (Instance.logging) Debug.Log($"Push {typeof(T)} success.");
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
                if (Instance.logging) Debug.Log($"Pop {currentScreen.GetType()} and show {newCurrentScreen.GetType()} success.");
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
            if (Instance.logging) Debug.Log($"Pop all screen success.");
        }
        
        //
        public static void ShowOutCanvas()
        {
            Instance.startCanvas.SetActive(false);
            Instance.outCanvas.SetActive(true);
        }

        #endregion

        #region Button Inspector

        //
        [FoldoutGroup("Button"), Button(ButtonSizes.Large)]
        public void GetAllScreenInChildren()
        {
            var allSunBaseUIs = transform.GetComponentsInChildren<SunBaseUI>();
            if (uiScreens == null) uiScreens = new List<SunBaseUI>();
            uiScreens.Clear();
            foreach (var ui in allSunBaseUIs)
            {
                uiScreens.Add(ui);
            }
        }

        #endregion
    }
}