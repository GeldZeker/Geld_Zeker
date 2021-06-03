using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.UI
{
    /// <summary> A simple class for closing the application. </summary>
    public class CloseGameButton : MonoBehaviour
    {
        /// <summary> Close the application. </summary>
        public void Exit()
        {
            Application.Quit();
        }
    }
}