using BWolf.Utilities;
using System.Collections;
using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    public class ShakeableObject : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool startOnAwake = false;

        [Header("Scaling")]
        [SerializeField]
        private ScaleAxis scaleAxis = ScaleAxis.None;

        [SerializeField, Range(0.0f, 1.0f)]
        private float minScale = 1.0f;

        [SerializeField]
        private float scaleSpeed = 0.125f;

        [Header("Rotation")]
        [SerializeField]
        private bool rotate = false;

        [SerializeField, Range(0.0f, 45.0f)]
        private float rotation = DEFAULT_ROTATION;

        [SerializeField]
        private float rotateSpeed = DEFAULT_ROTATESPEED;

        private bool initiated;

        private const float DEFAULT_ROTATION = 1.0f;
        private const float DEFAULT_ROTATESPEED = 10.0f;

        private IEnumerator shakeRoutine;

        private void Awake()
        {
            if (startOnAwake)
            {
                Initiate();
            }
        }

        public void SetScale(ScaleAxis axis, float min, float speed)
        {
            scaleAxis = axis;
            minScale = min;
            scaleSpeed = speed;
        }

        public void SetRotation(float rotation = DEFAULT_ROTATION, float speed = DEFAULT_ROTATESPEED)
        {
            this.rotation = rotation;
            rotateSpeed = speed;
            rotate = true;
        }

        /// <summary>Initiates the shake using set inspector values</summary>
        public void Initiate()
        {
            if (initiated)
            {
                return;
            }

            initiated = true;

            if (scaleAxis != ScaleAxis.None)
            {
                StartCoroutine(Scale());
            }

            if (rotate)
            {
                StartCoroutine(Rotate());
            }
        }

        public void Stop()
        {
            if (initiated)
            {
                StopAllCoroutines();
                initiated = false;
            }
        }

        /// <summary>Returns a routine that shakes the object using scale</summary>
        private IEnumerator Scale()
        {
            PingPongValue scale = new PingPongValue(minScale, transform.localScale.x, int.MaxValue, scaleSpeed);
            while (scale.Continue())
            {
                Vector3 newScale = Vector3.one;
                ConformScaleScaleAxis(ref newScale, scale.value);
                transform.localScale = newScale;
                yield return null;
            }
        }

        /// <summary>makes the original vector3 conform to the set scale axis value</summary>
        private void ConformScaleScaleAxis(ref Vector3 original, float value)
        {
            switch (scaleAxis)
            {
                case ScaleAxis.X:
                    original.x = value;
                    break;

                case ScaleAxis.Y:
                    original.y = value;
                    break;

                case ScaleAxis.XY:
                    original.x = value;
                    original.y = value;
                    break;
            }
        }

        /// <summary>Returns a routine that shakes the object using rotation</summary>
        private IEnumerator Rotate()
        {
            PingPongValue rotate = new PingPongValue(-rotation, rotation, int.MaxValue, rotateSpeed, 0.5f);
            while (rotate.Continue())
            {
                transform.eulerAngles = new Vector3(0, 0, rotate.value);
                yield return null;
            }
        }

        /// <summary>Axis setting on which shake should be applied</summary>
        public enum ScaleAxis
        {
            None,
            X,
            Y,
            XY
        }
    }
}