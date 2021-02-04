using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Introductions
{
    using RelativePosition = IntroArrow.RelativePosition;

    public class IntroducableObject : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField]
        private string intoTagName = string.Empty;

        [SerializeField]
        private IntroductionType type = IntroductionType.Arrow;

        [Header("Arrow Settings")]
        [SerializeField]
        private RelativePosition relativeArrowPosition = RelativePosition.Bottom;

        [SerializeField]
        private float introArrowSpacing = 0.0f;

        [Header("References")]
        [SerializeField]
        private GameObject prefabIntroArrow = null;

        private GameObject activeArrow;
        private ShakeableObject shakeableObject;

        public string IntroTagName
        {
            get { return intoTagName; }
        }

        public const string COMPONENT_TAG_NAME = "Introducable";

        private void Awake()
        {
            if (tag != COMPONENT_TAG_NAME)
            {
                Debug.LogWarning($"Object {gameObject} is an introducable object but has no {COMPONENT_TAG_NAME} tag attached");
            }
        }

        private void OnDestroy()
        {
            if (activeArrow != null)
            {
                EndIntroduction();
            }
        }

        public void StartIntroduction()
        {
            switch (type)
            {
                case IntroductionType.Arrow:
                    activeArrow = Instantiate(prefabIntroArrow, transform);
                    activeArrow.GetComponent<IntroArrow>().PlaceRelative((RectTransform)transform, relativeArrowPosition, introArrowSpacing);
                    break;

                case IntroductionType.Shake:
                    shakeableObject = transform.GetComponent<ShakeableObject>() ?? gameObject.AddComponent<ShakeableObject>();
                    shakeableObject.SetRotation();
                    shakeableObject.Initiate();
                    break;
            }
        }

        public void EndIntroduction()
        {
            switch (type)
            {
                case IntroductionType.Arrow:
                    Destroy(activeArrow);
                    activeArrow = null;
                    break;

                case IntroductionType.Shake:
                    shakeableObject.Stop();
                    break;
            }
        }

        private enum IntroductionType
        {
            Arrow,
            Shake
        }
    }
}