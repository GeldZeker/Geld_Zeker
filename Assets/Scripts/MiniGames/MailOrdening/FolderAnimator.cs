using BWolf.Utilities;
using GameStudio.GeldZeker.MiniGames.Settings;
using System;
using System.Collections;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.MailOrdering
{
    public class FolderAnimator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private MailOrderSetting setting = null;

        [SerializeField]
        private int startSortingOrder = 2;

        [Header("References")]
        [SerializeField]
        private Transform parent = null;

        [SerializeField]
        private GameObject prefabFolderPaper = null;

        [SerializeField]
        private GameObject winEffects = null;

        [Space]
        [SerializeField]
        private FolderTab[] tabs = null;

        private AnimatablePaper[] papers;
        private PaperPlacer placer;

        private bool finished;

        public event Action<PlacementResult> OnFinish;

        public MailOrderSetting Setting
        {
            get { return setting; }
        }

        private void Awake()
        {
            placer = GetComponent<PaperPlacer>();

            int papercount = 0;
            foreach (FolderTab tab in tabs)
            {
                papercount += 1 + tab.Papers.Length;
            }

            papers = new AnimatablePaper[papercount];

            for (int i = 0; i < tabs.Length; i++)
            {
                FolderTab tab = tabs[i];
                foreach (PaperSprite sprite in tab.Papers)
                {
                    papers[--papercount] = CreateTabPaper(sprite, tab.MailType, papercount);
                }

                papers[--papercount] = CreateTabPaper(tab.Tab, tab.MailType, papercount, true);
            }
        }

        private void Start()
        {
            StartCoroutine(AnimateAll());
        }

        /// <summary>Finishes the animator at current paper and returns the time it will take for it to finish animating</summary>
        public float Finish()
        {
            finished = true;
            return GetTimeTillNextAnimation();
        }

        private float GetTimeTillNextAnimation()
        {
            foreach (AnimatablePaper paper in papers)
            {
                if (paper.IsAnimating)
                {
                    return paper.AnimateTimeLeft;
                }
            }

            return 0.0f;
        }

        private AnimatablePaper CreateTabPaper(PaperSprite sprite, MailType mailType, int countAt, bool isTab = false)
        {
            AnimatablePaper paper = Instantiate(prefabFolderPaper, parent).GetComponent<AnimatablePaper>();
            paper.SetDisplay(sprite);
            paper.SetOrderInLayer(startSortingOrder + (papers.Length - countAt));
            paper.SetMailShowing(mailType);
            paper.SetAnimateTime(setting.GetTurnTime());
            paper.SetIsTab(isTab);
            return paper;
        }

        private void StopAnimation()
        {
            StopAllCoroutines();
        }

        private IEnumerator AnimateAll()
        {
            yield return new WaitForSeconds(setting.GetStartDelay()); ;

            while (true)
            {
                //move papers left
                for (int i = 0; i < papers.Length; i++)
                {
                    if (finished)
                    {
                        OnFinish(GetPlacementResult(i != 0 ? i - 1 : i));
                        yield break;
                    }
                    else
                    {
                        yield return MoveLeft(papers[i], i);
                    }
                }

                // move papers right
                for (int i = papers.Length - 1; i >= 0; i--)
                {
                    if (finished)
                    {
                        OnFinish(GetPlacementResult(i));
                        yield break;
                    }
                    else
                    {
                        yield return MoveRight(papers[i], i);
                    }
                }

                yield return new WaitForSeconds(setting.GetReverseDelay());
            }
        }

        private PlacementResult GetPlacementResult(int indexOfPaper)
        {
            AnimatablePaper paper = papers[indexOfPaper];
            bool isStartTab = paper.IsTab && indexOfPaper == 0;

            MailType mailPlacing = placer.MailPlacing;
            MailType mailShowing = paper.MailShowing;

            if (!isStartTab && mailPlacing == mailShowing)
            {
                winEffects.SetActive(true);
                return new PlacementResult { succesfull = true, message = $"Je hebt de post juist geplaatst bij de {mailPlacing}." };
            }
            else
            {
                winEffects.SetActive(false);
                return new PlacementResult { succesfull = false, message = $"Je hebt de post niet bij de {mailPlacing} geplaatst." };
            }
        }

        private IEnumerator MoveRight(AnimatablePaper paper, int index)
        {
            paper.SetIsAnimating(true);

            LerpValue<Vector3> rotateToMiddle = paper.RotateToMiddle;
            while (rotateToMiddle.Continue())
            {
                paper.transform.localEulerAngles = Vector2.Lerp(rotateToMiddle.start, rotateToMiddle.end, rotateToMiddle.perc);
                yield return null;
            }

            Audio.MusicPlayer.Instance.PlaySFXSound(Audio.SFXSound.TurningPage);
            paper.SetOrderInLayer(startSortingOrder + (papers.Length - 1 - index));
            paper.SetIsOnBack(false);

            LerpValue<Vector3> rotateToOutside = paper.RotateToOutside;
            while (rotateToOutside.Continue())
            {
                paper.transform.localEulerAngles = Vector2.Lerp(rotateToOutside.start, rotateToOutside.end, rotateToOutside.perc);
                yield return null;
            }

            paper.SetIsLeft(false);
            paper.SetIsAnimating(false);
        }

        private IEnumerator MoveLeft(AnimatablePaper paper, int index)
        {
            paper.SetIsAnimating(true);

            LerpValue<Vector3> rotateToMiddle = paper.RotateToMiddle;
            while (rotateToMiddle.Continue())
            {
                paper.transform.localEulerAngles = Vector2.Lerp(rotateToMiddle.start, rotateToMiddle.end, rotateToMiddle.perc);
                yield return null;
            }

            Audio.MusicPlayer.Instance.PlaySFXSound(Audio.SFXSound.TurningPage);
            paper.SetOrderInLayer(startSortingOrder + index);
            paper.SetIsOnBack(true);

            LerpValue<Vector3> rotateToOutside = paper.RotateToOutside;
            while (rotateToOutside.Continue())
            {
                paper.transform.localEulerAngles = Vector2.Lerp(rotateToOutside.start, rotateToOutside.end, rotateToOutside.perc);
                yield return null;
            }

            paper.SetIsLeft(true);
            paper.SetIsAnimating(false);
        }
    }
}