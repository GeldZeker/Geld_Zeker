using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>A property describing the gender of the character being played</summary>
    [CreateAssetMenu(menuName = "Player/Properties/BankAppointment")]
    public class BankAppointmentProperty : PlayerProperty
    {
        [SerializeField]
        private BankAppointmentType appointment = BankAppointmentType.None;

        public BankAppointmentType Value
        {
            get { return appointment; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateAppointment(BankAppointmentType newAppointment, bool fromSaveFile = false)
        {
            if (appointment == newAppointment)
            {
                return;
            }

            appointment = newAppointment;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateAppointment(BankAppointmentType.None);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BankAppointmentType)}/{name}";
            GameFileSystem.SaveToFile(path, appointment);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BankAppointmentType)}/{name}";

            if (GameFileSystem.LoadFromFile(path, out BankAppointmentType outValue))
            {
                UpdateAppointment(outValue, true);
            }
        }
    }

    public enum BankAppointmentType
    {
        None,
        CreateAccount
    }
}