using System.Collections;
using BWolf.Utilities.CharacterDialogue;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player
{
    /// <summary> Provides characterselection functionalities to user. </summary>
    public class SelectCharacterBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string sceneName = string.Empty;

        [SerializeField]
        private Vector3 selectedScale = Vector3.zero;

        [SerializeField]
        private Vector3 unselectedScale = Vector3.zero;

        [SerializeField]
        private float animationDuration = 1.5f;

        [Header("References")]
        [SerializeField]
        private Button maleButton = null;

        [SerializeField]
        private Button femaleButton = null;

        [SerializeField]
        private Image MaleSprite = null;

        [SerializeField]
        private Image FemaleSprite = null;

        [SerializeField]
        private Image maleArrow = null;

        [SerializeField]
        private Image maleSelected = null;

        [SerializeField]
        private Image femaleArrow = null;

        [SerializeField]
        private Image femaleSelected = null;

        [Space]
        [SerializeField]
        private AudableCharacter playerCharacter = null;

        private PlayerGender genderChoice;
        private bool choiceConfirmed;
        private bool isAnimating;

        private void Start()
        {
            maleButton.onClick.AddListener(SelectMale);
            femaleButton.onClick.AddListener(SelectFemale);

            //Do not start selected
            maleSelected.enabled = false;
            femaleSelected.enabled = false;
        }

        /// <summary> Regulates Animation for male character
        private IEnumerator SelectMaleAnimation()
        {
            isAnimating = true;
            Vector3 maleStartScale = MaleSprite.transform.localScale;
            Vector3 femaleStartScale = FemaleSprite.transform.localScale;
            float time = 0;
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                if (time > animationDuration)
                {
                    time = animationDuration;
                }
                float percentage = time / animationDuration;
                MaleSprite.transform.localScale = Vector3.Lerp(maleStartScale, selectedScale, percentage);
                FemaleSprite.transform.localScale = Vector3.Lerp(femaleStartScale, unselectedScale, percentage);
                yield return new WaitForFixedUpdate();
            }
            isAnimating = false;
        }

        /// <summary> Regulates Animation for female character
        private IEnumerator SelectFemaleAnimation()
        {
            isAnimating = true;
            Vector3 maleStartScale = MaleSprite.transform.localScale;
            Vector3 femaleStartScale = FemaleSprite.transform.localScale;
            float time = 0;
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                if (time > animationDuration)
                {
                    time = animationDuration;
                }
                float percentage = time / animationDuration;
                MaleSprite.transform.localScale = Vector3.Lerp(maleStartScale, unselectedScale, percentage);
                FemaleSprite.transform.localScale = Vector3.Lerp(femaleStartScale, selectedScale, percentage);
                yield return new WaitForFixedUpdate();
            }
            isAnimating = false;
        }

        /// <summary> Select male as character </summary>
        private void SelectMale()
        {
            if (isAnimating)
            {
                return;
            }

            if (!choiceConfirmed)
            {
                //If male is already selected, confirm choice
                if (genderChoice == PlayerGender.Male)
                {
                    choiceConfirmed = true;
                    DeactivateButtons();
                    ConfirmChoice(genderChoice);
                }
                // Activate male && Deactivate female
                else
                {
                    {
                        //Set sprite sizes
                        StartCoroutine(SelectMaleAnimation());
                        //Change selection images
                        maleArrow.enabled = false;
                        maleSelected.enabled = true;
                        femaleArrow.enabled = true;
                        femaleSelected.enabled = false;
                        //Change active character
                        genderChoice = PlayerGender.Male;
                    }
                }

                MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
            }
        }

        /// <summary> Select female as character </summary>
        private void SelectFemale()
        {
            if (isAnimating)
            {
                return;
            }

            if (!choiceConfirmed)
            {
                //If female is already selected, confirm choice
                if (genderChoice == PlayerGender.Female)
                {
                    choiceConfirmed = true;
                    DeactivateButtons();
                    ConfirmChoice(genderChoice);
                }
                else  // Activate female && Deactivate male
                {
                    //Set sprite sizes
                    StartCoroutine(SelectFemaleAnimation());
                    //Change selection images
                    maleArrow.enabled = true;
                    maleSelected.enabled = false;
                    femaleArrow.enabled = false;
                    femaleSelected.enabled = true;
                    //Change active character
                    genderChoice = PlayerGender.Female;
                }

                MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
            }
        }

        /// <summary> Makes buttons not interactable. </summary>
        private void DeactivateButtons()
        {
            maleButton.interactable = false;
            femaleButton.interactable = false;
        }

        /// <summary> Sets chosen character gender and continious to next scene. </summary>
        private void ConfirmChoice(PlayerGender gender)
        {
            switch (gender)
            {
                //set player character display sprite for dialogue
                case PlayerGender.Male:
                    playerCharacter.SetDisplaySprite(MaleSprite.sprite);
                    break;

                case PlayerGender.Female:
                    playerCharacter.SetDisplaySprite(FemaleSprite.sprite);
                    break;
            }

            MusicPlayer.Instance.PlaySFXSound(SFXSound.ConfirmChoice);
            //Use PlayerPropertyManager to update the gender value to save it
            PlayerPropertyManager.Instance.GetProperty<PlayerGenderProperty>("Gender").UpdateGender(gender);
            //Send to next scene
            SceneTransitionSystem.Instance.Transition("Fade", sceneName, LoadSceneMode.Additive);
        }
    }
}