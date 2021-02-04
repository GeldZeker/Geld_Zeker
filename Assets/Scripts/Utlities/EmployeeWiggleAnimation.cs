using BWolf.Utilities;
using System.Collections;
using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>
    /// A behaviour used for wiggling employee characters
    /// </summary>
    public class EmployeeWiggleAnimation : MonoBehaviour
    {
        [Header("Settigns")]
        [SerializeField]
        private float speed = 7.0f;

        [SerializeField]
        private float range = 3.0f;

        [SerializeField]
        private float minInterval = 0.25f;

        [SerializeField]
        private float maxInterval = 2.0f;

        private Vector3 anchor;

        private void Awake()
        {
            anchor = transform.position;

            StartCoroutine(WiggleRoutine());
        }

        /// <summary>
        /// Returns a routine that wiggles the character on the x axis
        /// </summary>
        /// <returns></returns>
        private IEnumerator WiggleRoutine()
        {
            while (true)
            {
                PingPongValue wiggle = new PingPongValue(-range, range, 1, speed, 0.5f);
                while (wiggle.Continue())
                {
                    Vector3 position = transform.position;
                    position.x = anchor.x + wiggle.value;
                    transform.position = position;
                    yield return null;
                }

                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            }
        }
    }
}