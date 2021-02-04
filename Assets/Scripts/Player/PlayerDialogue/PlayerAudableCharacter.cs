using BWolf.Utilities.CharacterDialogue;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.PlayerDialogue
{
    [CreateAssetMenu(menuName = "CharacterDialogue/PlayerAudableCharacter")]
    public class PlayerAudableCharacter : AudableCharacter
    {
        [Space]
        [SerializeField]
        private string nameOfGenderProperty = "Gender";

        [SerializeField]
        private Sprite maleSprite = null;

        [SerializeField]
        private Sprite femaleSprite = null;

        public void InitFromLocalStorage()
        {
            string path = $"{PlayerProperty.FOLDER_NAME}/{nameof(PlayerGender)}/{nameOfGenderProperty}";

            if (GameFileSystem.LoadFromFile(path, out PlayerGender outValue))
            {
                switch (outValue)
                {
                    case PlayerGender.NotSelected:
                        Debug.LogWarning("Loaded PlayerGender as not selected :: this is not intended behaviour!");
                        break;

                    case PlayerGender.Male:
                        SetDisplaySprite(maleSprite);
                        break;

                    case PlayerGender.Female:
                        SetDisplaySprite(femaleSprite);
                        break;
                }
            }
        }
    }
}