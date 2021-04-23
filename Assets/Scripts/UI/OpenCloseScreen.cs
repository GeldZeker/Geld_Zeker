using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.UI
{
    /// <summary> A simple class for opening and closing the CloseGameScreen. </summary>
    public class OpenCloseScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject closeGameScreen = null;

        /// <summary> Open the CloseGameScreen. </summary>
        public void OpenCloseGameScreen()
        {
            closeGameScreen.SetActive(true);
        }

        /// <summary> Close the CloseGameScreen. </summary>
        public void CloseGameScreen()
        {
            closeGameScreen.SetActive(false);
        }
    }
}