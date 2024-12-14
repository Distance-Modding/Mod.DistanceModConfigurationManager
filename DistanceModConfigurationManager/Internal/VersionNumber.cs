using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DistanceModConfigurationManager.Internal
{
    internal class VersionNumber : MonoBehaviour
    {
        internal const float MaximumOpacity = 0.7f;

        internal static VersionNumber Instance { get; set; } = null;
        internal static bool creatingInstance = false;

        internal UILabel label;
        internal UIWidget widget;
        internal UIPanel panel;
        internal bool Visible
        {
            get => Visible;
            set
            {
                if (value != _visible)
                {
                    if (_transitionCoroutine != null)
                    {
                        StopCoroutine(_transitionCoroutine);
                    }

                    _transitionCoroutine = StartCoroutine(Transition(value));
                }

                _visible = value;
            }
        }
        internal bool CanDisplay
        {
            get
            {
                bool flag = true;
                flag &= string.Equals(SceneManager.GetActiveScene().name, "mainmenu", StringComparison.InvariantCultureIgnoreCase);
                flag &= G.Sys.MenuPanelManager_.panelStack_.Count == 2;
                flag &= Mod.ShowVersionInfo.Value;
                flag &= G.Sys.GameManager_.IsLevelLoaded_;
                flag &= G.Sys.GameManager_.BlackFade_.currentState_ == BlackFadeLogic.FadeState.Idle;
                return flag;
            }
        }

        private bool _visible = true;
        private Coroutine _transitionCoroutine = null;

        internal static void Create(GameObject speedrunTimerLogic = null)
        {
            if (!Instance && !creatingInstance)
            {
                creatingInstance = true;
                GameObject alphaVersionAnchorBlueprint = null;

                if (speedrunTimerLogic)
                {
                    alphaVersionAnchorBlueprint = speedrunTimerLogic.Parent();
                }
                else
                {
                    alphaVersionAnchorBlueprint = GameObject.Find("Anchor : Speedrun Timer");
                }

                if (alphaVersionAnchorBlueprint)
                {
                    GameObject modManagerInfoAnchor = Instantiate(alphaVersionAnchorBlueprint, alphaVersionAnchorBlueprint.transform.parent);

                    modManagerInfoAnchor.SetActive(true);
                    modManagerInfoAnchor.name = "Anchor : Mod Manager Info";

                    modManagerInfoAnchor.ForEachChildObjectDepthFirstRecursive((obj =>
                    {
                        obj.SetActive(true);
                        obj.RemoveComponent<SpeedrunTimerLogic>();
                    }));

                    UILabel label = modManagerInfoAnchor.GetComponentInChildren<UILabel>();

                    Instance = label.gameObject.AddComponent<VersionNumber>();
                }

                creatingInstance = false;
            }
        }

        internal void Start()
        {
            GameObject anchorObject = gameObject.Parent();
            GameObject panelObject = anchorObject.Parent();

            label = GetComponent<UILabel>();
            widget = anchorObject.GetComponent<UIWidget>();
            panel = panelObject.GetComponent<UIPanel>();

            widget.alpha = 0;

            AdjustPosition();
        }

        internal void Update()
        {
            label.text = $"DISTANCE MOD CONFIGURATION {Mod.modVersion} - {Mod.Instance.modsLoaded} MOD(S) LOADED";

            Visible = CanDisplay;
        }

        internal void AdjustPosition()
        {
            label.SetAnchor(panel.gameObject, 21, 19, -19, -17);
            label.pivot = UIWidget.Pivot.TopLeft;
        }

        internal IEnumerator Transition(bool visible, float duration = 0.2f)
        {
            float target = visible ? MaximumOpacity : 0.0f;
            float current = widget.alpha;

            for (float time = 0.0f; time < duration; time += Timex.deltaTime_)
            {
                if (!G.Sys.OptionsManager_.General_.menuAnimations_)
                {
                    break;
                }

                float value = MathUtil.Map(time, 0, duration, current, target);
                widget.alpha = value;

                yield return null;
            }

            widget.alpha = target;
        }

        private static class MathUtil
        {
            public static float Map(float s, float a1, float a2, float b1, float b2)
            {
                return b1 + ((s - a1) * (b2 - b1) / (a2 - a1));
            }
        }


    }
}
