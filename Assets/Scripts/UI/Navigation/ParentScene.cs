using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.Navigation
{
    /// <summary>Data object regarding a parent scene containg child scenes</summary>
    [System.Serializable]
    public struct ParentScene
    {
#pragma warning disable 0649
        public string name;

        public Sprite icon;

        public ChildScene[] childScenes;
#pragma warning restore 0649

        public bool ContainsChild(string nameOfChildScene)
        {
            for (int i = 0; i < childScenes.Length; i++)
            {
                if (childScenes[i].name == nameOfChildScene)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ChildScene> ChildrenWithout(string nameOfChildScene)
        {
            List<ChildScene> scenes = new List<ChildScene>();

            for (int i = 0; i < childScenes.Length; i++)
            {
                if (childScenes[i].name != nameOfChildScene)
                {
                    scenes.Add(childScenes[i]);
                }
            }

            return scenes;
        }

        public ChildScene GetChild(string nameOfChildScene)
        {
            for (int i = 0; i < childScenes.Length; i++)
            {
                ChildScene scene = childScenes[i];
                if (scene.name == nameOfChildScene)
                {
                    return scene;
                }
            }

            return default;
        }
    }

    /// <summary>data object regarding a child scene</summary>
    [System.Serializable]
    public struct ChildScene
    {
#pragma warning disable 0649
        public string name;

        public Sprite icon;
#pragma warning restore 0649
    }
}