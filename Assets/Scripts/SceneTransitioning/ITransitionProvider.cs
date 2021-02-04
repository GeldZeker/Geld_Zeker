using System.Collections;

namespace GameStudio.GeldZeker.SceneTransitioning
{
    /// <summary>Interface to be implemented by Monobehaviour classes that can provide a scene transition for the scene transition system</summary>
    public interface ITransitionProvider
    {
        string TransitionName { get; }

        IEnumerator Outro();

        IEnumerator Intro();

        void OnProgressUpdated(float perc);
    }
}