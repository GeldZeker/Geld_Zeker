using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.SceneTransitioning
{
    /// <summary>Behaviour to provide a specific scene to return to.</summary>
    class GameHallBackToScene : SingletonBehaviour<GameHallBackToScene>
    {
        [SerializeField]
        public static GameHallBackToScene instance;

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
