using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneTransitioning
{
    class BackToScene : SingletonBehaviour<BackToScene>
    {
        [SerializeField]
        public static BackToScene instance;

        [Header("Settings")]
        [SerializeField]
        public string backScene = null;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            instance = this;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public string returnBackScene()
        {
            return backScene;
        }
    }
}
