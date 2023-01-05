using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Schwarzer.Windows;
using Arcade.Gameplay;
using Arcade.Aff;
using DG.Tweening;
using Newtonsoft.Json;
using Arcade.Compose.Dialog;
using Arcade.Compose.Editing;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Reflection;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Net.Http;

namespace Arcade.Compose
{
    [Serializable]
    public class ArcadePreference
    {
        public int AgreedUserAgreementVersion;
        public long ReadWhatsNewVersion;
        public int DropRate;
        public bool Auto;
        public Arcade.Gameplay.Chart.ChartSortMode ChartSortMode;
    }

    [Serializable]
    public class I18nBoard
    {
        public long TimeStamp = 0;
        public Dictionary<ILocale, string> LocalizedBoard = new Dictionary<ILocale, string>();
    }

    public class ArcadeComposeManager : MonoBehaviour
    {
        public static ArcadeComposeManager Instance { get; private set; }
        public const float EditorModeGameplayCameraScale = 0.9f;
        public const float ModeSwitchDuration = 0.3f;
        public const float BarSizeRatio = 1.381f;
        public const Ease ToEditorModeEase = Ease.OutCubic;
        public const Ease ToPlayerModeEase = Ease.InCubic;

        public static string ArcadePersistentFolder
        {
            get
            {
                if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Arcade"))
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Arcade");
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Arcade";
            }
        }
        public string PreferencesSavePath
        {
            get
            {
                return ArcadePersistentFolder + "/Preferences.json";
            }
        }

        public bool IsEditorMode { get; set; } = true;
        public float BarHeight
        {
            get
            {
                return IsEditorMode ? TopBar.sizeDelta.y : 0;
            }
        }
        public float BarWidth
        {
            get
            {
                return IsEditorMode ? LeftBar.sizeDelta.x : 0;
            }
        }

        public Camera GameplayCamera, EditorCamera;
        public ArcGameplayManager GameplayManager;
        [Header("Bar")]
        public RectTransform TopBar;
        public RectTransform BottomBar, LeftBar, RightBar, Bars;
        public RectTransform TopBarView, BottomBarView, LeftBarView, RightBarView;
        [Header("Pause")]
        public Button PauseButton;
        public Image PauseButtonImage;
        public Sprite PausePause, PausePlay, PausePausePressed, PausePlayPressed;
        [Header("Info")]
        public CanvasGroup InfoCanvasGroup;
        public Image TimingSliderHandle;
        public Sprite DefaultSliderSprite, GlowSliderSprite;
        [Header("Auto")]
        public Button AutoButton;

        public UnityEvent OnPlay = new UnityEvent();
        public UnityEvent OnPause = new UnityEvent();
        public ArcadePreference ArcadePreference = new ArcadePreference();
        public Text Version;

        private bool switchingMode = false;
        private int playShotTiming = 0;
        private void Awake()
        {
            Instance = this;
            Version.text = ArcadeBuildInfo.BuildString;
            CultureInfo.CurrentCulture = new CultureInfo("zh-Hans");
        }
        private void Start()
        {
            ArcGameplayManager.Instance.OnMusicFinished.AddListener(Pause);
            LoadPreferences();
            Pause();
        }
        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0 && !AdePositionSelector.Instance.Enable && AdeCursorManager.Instance.IsHorizontalHit)
            {
                float timing = GameplayManager.Timingf * 1000;
                int offset = ArcAudioManager.Instance.AudioOffset;
                timing = AdeGridManager.Instance.AttachScroll(timing - offset, Input.mouseScrollDelta.y) + offset;
                if (timing < 0) timing += GameplayManager.Length;
                if (timing > GameplayManager.Length) timing -= GameplayManager.Length;
                if (timing < 0 || timing > GameplayManager.Length) timing = 0;
                GameplayManager.Timingf = timing / 1000;
                GameplayManager.ResetJudge();
            }
            if (EventSystem.current.currentSelectedGameObject == null && IsEditorMode)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameplayManager.Play();
                    playShotTiming = GameplayManager.Timing;
                    //AdeToast.Instance.Show("松开空格暂停并倒回，按下Q仅暂停", "Release 'Space' pause and rollback, Press 'Q' pause only."));
                }
                if (Input.GetKeyUp(KeyCode.Space) && GameplayManager.IsPlaying)
                {
                    GameplayManager.Pause();
                    GameplayManager.Timing = playShotTiming;
                }
            }
            try
            {
                if (Input.GetKeyDown(PlayerPrefs.GetString("HotKeyNoReturn", "q")))
                {
                    if (GameplayManager.IsPlaying) GameplayManager.Pause();
                    else GameplayManager.Play();
                }
            }
            catch (ArgumentException)
            {
                AdeToast.Instance.Show(I.S["hotkeyerr"]);
                PlayerPrefs.SetString("HotKeyNoReturn", "q");
            }
        }
        private void OnEnable()
        {
            Application.logMessageReceived += OnLog;
        }
        private void OnDisable()
        {
            Application.logMessageReceived -= OnLog;
        }
        private void OnLog(string condition, string stackTrace, LogType type)
        {
            if (type != LogType.Exception) return;
            AdeSingleDialog.Instance.Show(condition + "\n" + stackTrace.Substring(0, 500), I.S["exception"], I.S["confirm"]);
        }

        public void Play()
        {
            if (AdeProjectManager.Instance.CurrentProject == null || !ArcGameplayManager.Instance.IsLoaded)
            {
                AdeToast.Instance.Show(I.S["loadchartfirst"]);
                return;
            }

            if (switchingMode) return;
            switchingMode = true;

            GameplayManager.Play();
            float border = (1 - EditorModeGameplayCameraScale) / 2;
            float width = border * 2048 * BarSizeRatio;
            float height = border * 1536 * BarSizeRatio * EditorCamera.pixelHeight / (EditorCamera.pixelWidth * 0.75f);
            TopBar.sizeDelta = new Vector2(0, height);
            BottomBar.sizeDelta = new Vector2(0, height);
            LeftBar.sizeDelta = new Vector2(width, 0);
            RightBar.sizeDelta = new Vector2(width, 0);
            TopBarView.sizeDelta = new Vector2(0, height / BarSizeRatio);
            BottomBarView.sizeDelta = new Vector2(0, height / BarSizeRatio);
            LeftBarView.sizeDelta = new Vector2(width / BarSizeRatio, 0);
            RightBarView.sizeDelta = new Vector2(width / BarSizeRatio, 0);
            TopBar.DOAnchorPosY(height, ModeSwitchDuration).SetEase(ToPlayerModeEase);
            BottomBar.DOAnchorPosY(-height, ModeSwitchDuration).SetEase(ToPlayerModeEase);
            LeftBar.DOAnchorPosX(-width, ModeSwitchDuration).SetEase(ToPlayerModeEase);
            RightBar.DOAnchorPosX(width, ModeSwitchDuration).SetEase(ToPlayerModeEase).OnComplete(() => { Bars.gameObject.SetActive(false); switchingMode = false; });
            GameplayCamera.DORect(new Rect(0, 0, 1, 1), ModeSwitchDuration).SetEase(ToPlayerModeEase);

            PauseButtonImage.sprite = PausePause;
            PauseButton.spriteState = new SpriteState() { pressedSprite = PausePausePressed };
            InfoCanvasGroup.interactable = false;

            TimingSliderHandle.sprite = GlowSliderSprite;

            AdeClickToCreate.Instance.CancelAddLongNote();
            AdeClickToCreate.Instance.Mode = ClickToCreateMode.Idle;

            IsEditorMode = false;
        }
        public void Pause()
        {
            if (switchingMode) return;
            switchingMode = true;

            GameplayManager.Pause();
            float border = (1 - EditorModeGameplayCameraScale) / 2;
            Bars.gameObject.SetActive(true);
            float width = border * 2048 * BarSizeRatio;
            float height = border * 1536 * BarSizeRatio * EditorCamera.pixelHeight / (EditorCamera.pixelWidth * 0.75f);
            TopBar.sizeDelta = new Vector2(0, height);
            BottomBar.sizeDelta = new Vector2(0, height);
            LeftBar.sizeDelta = new Vector2(width, 0);
            RightBar.sizeDelta = new Vector2(width, 0);
            TopBarView.sizeDelta = new Vector2(0, height / BarSizeRatio);
            BottomBarView.sizeDelta = new Vector2(0, height / BarSizeRatio);
            LeftBarView.sizeDelta = new Vector2(width / BarSizeRatio, 0);
            RightBarView.sizeDelta = new Vector2(width / BarSizeRatio, 0);
            TopBar.DOAnchorPosY(0, ModeSwitchDuration).SetEase(ToEditorModeEase);
            BottomBar.DOAnchorPosY(0, ModeSwitchDuration).SetEase(ToEditorModeEase);
            LeftBar.DOAnchorPosX(0, ModeSwitchDuration).SetEase(ToEditorModeEase);
            RightBar.DOAnchorPosX(0, ModeSwitchDuration).SetEase(ToEditorModeEase).OnComplete(() => { switchingMode = false; });
            GameplayCamera.DORect(new Rect(border, border, EditorModeGameplayCameraScale, EditorModeGameplayCameraScale), ModeSwitchDuration).SetEase(ToEditorModeEase);

            PauseButtonImage.sprite = PausePlay;
            PauseButton.spriteState = new SpriteState() { pressedSprite = PausePlayPressed };
            InfoCanvasGroup.interactable = true;

            TimingSliderHandle.sprite = DefaultSliderSprite;

            IsEditorMode = true;
        }

        public void OnPauseClicked()
        {
            ArcGameplayManager.Instance.ResetJudge();
            if (IsEditorMode) Play();
            else Pause();
        }
        public void OnAutoClicked()
        {
            ArcGameplayManager.Instance.Auto = !ArcGameplayManager.Instance.Auto;
            ArcGameplayManager.Instance.ResetJudge();
            AutoButton.image.color = ArcGameplayManager.Instance.Auto ? new Color(0.59f, 0.55f, 0.65f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1);
        }
        public void OnShutdownClicked()
        {
            AdeProjectManager.Instance.SaveProject();
            Application.Quit();
        }

        public void LoadPreferences()
        {
            try
            {
                if (File.Exists(PreferencesSavePath))
                {
                    PlayerPrefs.SetString("ArcadeComposeManagerPreference", File.ReadAllText(PreferencesSavePath));
                    File.Delete(PreferencesSavePath);
                }
                ArcadePreference = JsonConvert.DeserializeObject<ArcadePreference>(PlayerPrefs.GetString("ArcadeComposeManagerPreference", ""));
                if (ArcadePreference == null) ArcadePreference = new ArcadePreference();
            }
            catch (Exception)
            {
                ArcadePreference = new ArcadePreference();
            }
            finally
            {
                ArcTimingManager.Instance.DropRate = PlayerPrefs.GetInt("DropRate", 100);
                ArcGameplayManager.Instance.Auto = ArcadePreference.Auto;
                AutoButton.image.color = ArcGameplayManager.Instance.Auto ? new Color(0.59f, 0.55f, 0.65f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1);
                if (ArcadePreference.AgreedUserAgreementVersion < ArcadeUserAgreement.CurrentUserAgreementVersion)
                {
                    ArcadeUserAgreement.Instance.Show();
                }
                Task.Run(async () => {
                    using (HttpClient client = new HttpClient())
                    {
                        string data = await client.GetStringAsync("https://schwarzer.oss-cn-hangzhou.aliyuncs.com/board_i18n");
                        Dispatcher.Instance.RunInMainThread(() => {
                            try
                            {
                                I18nBoard i18nBoard = JsonConvert.DeserializeObject<I18nBoard>(data);
                                long latest = long.Parse(PlayerPrefs.GetString("LatestWhatsNew", "0"));
                                if(i18nBoard.TimeStamp > latest)
                                {
                                    PlayerPrefs.SetString("LatestWhatsNew", i18nBoard.TimeStamp.ToString());
                                    AdeSingleDialog.Instance.Show(i18nBoard.LocalizedBoard[I.S.currentLocale], I.S["whatsnew"]);
                                }
                            }
                            catch (Exception)
                            {

                            }
                        });
                    }
                });
                //if (ArcadePreference.ReadWhatsNewVersion < ArcadeBuildInfo.Timestamp)
                //{
                //    AdeSingleDialog.Instance.Show(UpdateLog.Latest, "更新内容");
                //    ArcadePreference.ReadWhatsNewVersion = ArcadeBuildInfo.Timestamp;
                //}
            }
        }
        public void SavePreferences()
        {
            ArcadePreference.DropRate = ArcTimingManager.Instance.DropRate;
            ArcadePreference.Auto = ArcGameplayManager.Instance.Auto;
            PlayerPrefs.SetString("ArcadeComposeManagerPreference", JsonConvert.SerializeObject(ArcadePreference));
        }

        private void OnApplicationQuit()
        {
            SavePreferences();
        }
        public void OpenLogFile()
        {
            Schwarzer.Windows.Dialog.OpenExplorer(Application.consoleLogPath);
        }
    }
}