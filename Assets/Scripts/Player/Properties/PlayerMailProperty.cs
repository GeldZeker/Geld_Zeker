using GameStudio.GeldZeker.MiniGames.MailOrdering;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    [CreateAssetMenu(menuName = "Player/Properties/Mail")]
    public class PlayerMailProperty : PlayerProperty
    {
        [Header("Property")]
        [SerializeField]
        private List<Mail> mailList = null;

        [Header("Settings")]
        [SerializeField]
        private MailSprite[] sprites = null;

        public List<Mail> MailList
        {
            get { return mailList; }
        }

        public bool HasMailToOrden
        {
            get
            {
                foreach (Mail mail in mailList)
                {
                    if (mail.PickedUp && mail.SortedOut && !mail.Ordened)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasUnPickedUpMail
        {
            get
            {
                if (MailList.Count == 0)
                {
                    return false;
                }

                foreach (Mail mail in mailList)
                {
                    if (mail.PickedUp || mail.Ordened || mail.SortedOut)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool HasMailToSortOut
        {
            get
            {
                foreach (Mail mail in mailList)
                {
                    if (mail.PickedUp && !mail.SortedOut && !mail.Ordened)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateMail(List<Mail> mailList, bool fromSaveFile = false)
        {
            this.mailList = mailList;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        public Sprite GetMailSprite(MailType mailType)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                MailSprite sprite = sprites[i];
                if (sprite.Type == mailType)
                {
                    return sprite.Sprite;
                }
            }

            return null;
        }

        /// <summary>Adds the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void AddMail(MailType mailType)
        {
            mailList.Add(new Mail { Type = mailType });
            SaveToFile();
        }

        /// <summary>Ordens first mail of given type</summary>
        public void OrdenMail(MailType mailType)
        {
            if (mailList.Count == 0)
            {
                Debug.LogWarning("Tried ordening mail while there was none :: this is not intended behaviour");
                return;
            }

            for (int i = 0; i < mailList.Count; i++)
            {
                if (mailList[i].Type == mailType)
                {
                    mailList[i].Ordened = true;
                    break;
                }
            }

            SaveToFile();
        }

        /// <summary>Sets all mail to be picked up</summary>
        public void PickUpMail()
        {
            for (int i = 0; i < mailList.Count; i++)
            {
                mailList[i].PickedUp = true;
            }

            SaveToFile();
        }

        /// <summary>Sorts first mail of given type</summary>
        public void SortMail(MailType mailType)
        {
            for (int i = 0; i < mailList.Count; i++)
            {
                if (mailList[i].Type == mailType)
                {
                    mailList[i].SortedOut = true;
                    break;
                }
            }

            SaveToFile();
        }

        /// <summary>Sets all mail to be ordened</summary>
        public void OrdenAll()
        {
            for (int i = 0; i < mailList.Count; i++)
            {
                mailList[i].Ordened = true;
            }

            SaveToFile();
        }

        /// <summary>Remove the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void RemoveMail(MailType mailType)
        {
            for (int i = mailList.Count - 1; i >= 0; i--)
            {
                if (mailList[i].Type == mailType)
                {
                    mailList.RemoveAt(i);
                    break;
                }
            }

            SaveToFile();
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            mailList.Clear();
            SaveToFile();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerMailProperty)}/{name}";
            GameFileSystem.SaveToFile(path, mailList);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerMailProperty)}/{name}";
            if (GameFileSystem.LoadFromFile(path, out List<Mail> outValue))
            {
                UpdateMail(outValue, true);
            }
        }

        [System.Serializable]
        public class Mail
        {
            public MailType Type;
            public bool PickedUp;
            public bool SortedOut;
            public bool Ordened;
        }

        [System.Serializable]
        private struct MailSprite
        {
#pragma warning disable 0649

            public MailType Type;
            public Sprite Sprite;

#pragma warning restore 0649
        }
    }
}