using BWolf.Utilities;
using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.UI.Navigation
{
    /// <summary>System for managing the navigation between scenes</summary>
    public class NavigationSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float toggleTime = 0.125f;

        [SerializeField]
        private string nameOfLivingRoomScene = "Living Room";

        [SerializeField]
        private string nameOfHomeScreen = "HomeScreen";

        [SerializeField]
        private string nameOfGameHall = "GameHall";

        [Space]
        [SerializeField]
        private ParentScene[] parentScenes = null;

        [Header("References")]
        [SerializeField]
        private ChildSceneDisplay childSceneDisplay = null;

        private RectTransform rectTransform;
        private bool isToggling;
        private bool hasFocus;

        public const string SCENE_SAVE_FILE_PATH = "ProgressSaves/LastScene";

        private LoadSceneButton[] navButtons;

        public static string NameOfHomeScreen { get; private set; }
        public static string NameOfLivingRoom { get; private set; }
        public static string NameOfGameHall { get; private set; }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            navButtons = GetComponentsInChildren<LoadSceneButton>();

            NameOfHomeScreen = nameOfHomeScreen;
            NameOfLivingRoom = nameOfLivingRoomScene;
            NameOfGameHall = nameOfGameHall;

            foreach (LoadSceneButton navButton in navButtons)
            {
                navButton.Clicked += OnButtonClick;
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>Returns a parent scene object based on given name</summary>
        public ParentScene GetParentScene(string nameOfParent)
        {
            for (int i = 0; i < parentScenes.Length; i++)
            {
                ParentScene parentScene = parentScenes[i];
                if (parentScene.name == nameOfParent)
                {
                    return parentScene;
                }
            }
            return default;
        }

        /// <summary>Called when a scene has been loaded to set the active state of navigation buttons based on the scene loaded</summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string nameOfScene = scene.name;
            if (nameOfScene != nameOfHomeScreen && nameOfScene != nameOfGameHall && !MinigameSystem.Instance.IsGameScene(nameOfScene))
            {
                GameFileSystem.SaveToFile(SCENE_SAVE_FILE_PATH, nameOfScene);
            }

            if (MainCanvasManager.Instance.IsAnActiveScene(nameOfScene))
            {
                InitActiveSceneDisplays(scene);
            }
        }

        private void InitActiveSceneDisplays(Scene scene)
        {
            //set navigation button display active states based on scene loaded
            for (int i = 0; i < navButtons.Length; i++)
            {
                LoadSceneButton navbutton = navButtons[i];
                navbutton.gameObject.SetActive(scene.name != navbutton.NameOfSceneLoading);
            }

            //populate sub scene displays based on scene loaded
            if (GetSubSceneDisplayContent(scene.name, out ParentScene parentObject))
            {
                childSceneDisplay.SetActive(true);
                childSceneDisplay.Populate(scene.name, parentObject);
            }
            else
            {
                childSceneDisplay.SetActive(false);
            }
        }

        /// <summary>Outputs a parentscene object with given scene name. Returns whether it was succesfull</summary>
        private bool GetSubSceneDisplayContent(string sceneName, out ParentScene parentObject)
        {
            for (int i = 0; i < parentScenes.Length; i++)
            {
                ParentScene parentScene = parentScenes[i];
                if (parentScene.name == sceneName || parentScene.ContainsChild(sceneName))
                {
                    parentObject = parentScene;
                    return true;
                }
            }

            parentObject = default;
            return false;
        }

        /// <summary>Called when the navigation button has been clicked to toggle the active state of the navigation screen</summary>
        private void OnButtonClick()
        {
            ToggleNavigation();
        }

        private void Update()
        {
            if (hasFocus && (RectTransformExtensions.PressOutsideTransform(rectTransform) || RectTransformExtensions.TouchOutsideTransform(rectTransform)))
            {
                //if the navigation has focus and there is a touch outside the navigation, close it
                ToggleNavigation();
            }

#if UNITY_ANDROID
            if (!IntroductionManager.Instance.IsActive && Input.GetKeyDown(KeyCode.Escape))
            {
                //on an android system use the escape key (native back button on mobile) to navigate between scenes or close the application
                string nameOfScene = SceneManager.GetActiveScene().name;

                if (nameOfScene == nameOfHomeScreen)
                {
                    Application.Quit();
                }
                else if (nameOfScene == nameOfGameHall || nameOfScene == NameOfLivingRoom || GetParentScene(NameOfLivingRoom).ContainsChild(nameOfScene))
                {
                    SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, nameOfHomeScreen, LoadSceneMode.Additive);
                }
            }
#endif
        }

        /// <summary>Toggles the display state of the navigation screen if only one touch is recorded</summary>
        public void OnOpenNavigationButtonClick()
        {
            if (Application.platform == RuntimePlatform.Android && Input.touchCount != 1)
            {
                return;
            }

            ToggleNavigation();
        }

        /// <summary>Toggles the display state of the navigation screen</summary>
        public void ToggleNavigation()
        {
            if (!isToggling)
            {
                StartCoroutine(ToggleEnumerator());
            }
        }

        /// <summary>Returns an enumerator that either shows or hides the navigation screen baed on its current state</summary>
        private IEnumerator ToggleEnumerator()
        {
            isToggling = true;

            hasFocus = !hasFocus;
            SetNavButtonInteractability(hasFocus);

            Vector3 position = rectTransform.anchoredPosition;
            float newXPosition = position.x * -1f;
            LerpValue<float> moveXAxis = new LerpValue<float>(position.x, newXPosition, toggleTime);

            while (moveXAxis.Continue())
            {
                Vector2 newPosition = new Vector2(Mathf.Lerp(moveXAxis.start, moveXAxis.end, moveXAxis.perc), position.y);
                rectTransform.anchoredPosition = newPosition;
                yield return null;
            }

            isToggling = false;
        }

        /// <summary>Sets the interactability of the navigation buttons</summary>
        private void SetNavButtonInteractability(bool value)
        {
            for (int i = 0; i < navButtons.Length; i++)
            {
                navButtons[i].SetInteractable(value);
            }
        }
    }
}