using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Arcade.Compose.Command;
using Arcade.Compose.Dialog;
using Arcade.Compose.Feature;
using Arcade.Compose.UI;
using Arcade.Gameplay;
using Arcade.Gameplay.Chart;
using Newtonsoft.Json;
using Schwarzer.Mp3Converter;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Arcade.Compose
{
    [Serializable]
    public class AdeChartDifficulty
    {
        public string Rating;
        public string ChartPath;
        public string Designer;
    }
    [Serializable]
    public class ArcadeProject
    {
        public string Title;
        public string Artist; 
        public string AudioPath;
        public AdeChartDifficulty[] Difficulties = new AdeChartDifficulty[3];
        public Dictionary<int, AdeChartDifficulty> Difficulties2 = new Dictionary<int, AdeChartDifficulty>();

        public int LastWorkingDifficulty = 2;
        public int LastWorkingTiming;

        [JsonIgnore]
        public AudioClip AudioClip;
        [JsonIgnore]
        public Texture2D Cover;
        [JsonIgnore]
        public Sprite CoverSprite;
    }

    public class AdeProjectManager : MonoBehaviour
    {
        public static AdeProjectManager Instance { get; private set; }

        public string CurrentProjectFolder { get; set; }
        public ArcadeProject CurrentProject { get; set; }
        public int CurrentDifficulty { get; set; } = 2;

        public Sprite DefaultCover;
        public Image CoverImage;
        public Image[] DifficultyImages;
        public InputField Name, Composer, Diff, AudioOffset;
        public Text OpenLabel;
        public Text SaveMode;

        public Color EnableColor, DisableColor;
        public Image FileWatchEnableImage;

        private FileSystemWatcher watcher = new FileSystemWatcher();
        private bool shouldReload = false;

        public string ProjectFilePath
        {
            get
            {
                return $"{CurrentProjectFolder}/Arcade/Project.arcade";
            }
        }

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnWatcherChanged;

            watcher.EnableRaisingEvents = true;

            SaveMode.text = ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode == ChartSortMode.Timing ? I.S["sortbytime"] : I.S["sortbytype"];
            I.S.OnLocaleChanged.AddListener(OnLocaleChanged);

            StartCoroutine(AutosaveCoroutine());
        }
        private void Update()
        {
            if (shouldReload)
            {
                ReloadChart(CurrentDifficulty);
                ArcGameplayManager.Instance.Timing = 0;
                shouldReload = false;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SaveProject();
                }
            }
        }
        private void OnDestroy()
        {
            I.S.OnLocaleChanged.RemoveListener(OnLocaleChanged);
        }
        private void OnLocaleChanged()
        {
            SaveMode.text = ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode == ChartSortMode.Timing ? I.S["sortbytime"] : I.S["sortbytype"];
        }

        private void InitializeProject(string folder)
        {
            ArcadeProject p = new ArcadeProject();
            File.WriteAllText(ProjectFilePath, JsonConvert.SerializeObject(p));
        }
        private void CreateDirectories(string folder)
        {
            string[] directories = new string[] { $"{folder}/Arcade", $"{folder}/Arcade/Autosave", $"{folder}/Arcade/Converting", $"{folder}/Arcade/Backup" };
            foreach (var s in directories) if (!Directory.Exists(s)) Directory.CreateDirectory(s);
        }

        public void CleanProject()
        {
            if (CurrentProject == null) return;
            if (CurrentProject.AudioClip != null)
            {
                Destroy(CurrentProject.AudioClip);
                CurrentProject.AudioClip = null;
            }
            if (CurrentProject.Cover != null)
            {
                Destroy(CurrentProject.Cover);
                CurrentProject.Cover = null;
            }
            if (CurrentProject.CoverSprite != null)
            {
                Destroy(CurrentProject.CoverSprite);
                CurrentProject.CoverSprite = null;
            }
            foreach (Image i in DifficultyImages) i.color = new Color(1f, 1f, 1f, 0.6f);
            CoverImage.sprite = DefaultCover;
            Name.text = "";
            Composer.text = "";
            Diff.text = "";
            Name.interactable = false;
            Composer.interactable = false;
            Diff.interactable = false;
            AdeTimingSlider.Instance.Enable = false;
            OpenLabel.color = new Color(1, 1, 1, 1); 
            AudioOffset.interactable = false;
            watcher.EnableRaisingEvents = false;
            FileWatchEnableImage.color = DisableColor;
            ArcGameplayManager.Instance.Clean(); 
        } 
        public void OpenProject()
        {
            try
            {
                string folder = Schwarzer.Windows.Dialog.OpenFolderDialog(I.S["openfoldercaption"]);
                if (folder == null) return;
                CleanProject();
                CreateDirectories(folder);
                CurrentProjectFolder = folder;
                if (!File.Exists(ProjectFilePath)) InitializeProject(folder);
                try
                {
                    CurrentProject = JsonConvert.DeserializeObject<ArcadeProject>(File.ReadAllText(ProjectFilePath));
                }
                catch (Exception Ex)
                {
                    AdeSingleDialog.Instance.Show(Ex.Message, I.S["loaderror"]);
                    CurrentProject = new ArcadeProject();
                }

                StartCoroutine(LoadingCoroutine());
            }
            catch (Exception Ex)
            {
                AdeSingleDialog.Instance.Show(Ex.Message, I.S["loaderror"]);
                CurrentProject = null;
                CurrentProjectFolder = null;
            }
        }
        public void SaveProject()
        {
            if (CurrentProject == null || CurrentProjectFolder == null) return;
            if (!ArcGameplayManager.Instance.IsLoaded) return;
            CurrentProject.LastWorkingDifficulty = CurrentDifficulty;
            CurrentProject.LastWorkingTiming = ArcGameplayManager.Instance.Timing;
            File.WriteAllText(ProjectFilePath, JsonConvert.SerializeObject(CurrentProject));
            string path = CurrentProjectFolder + $"/{CurrentDifficulty}.aff";
            string backupPath = CurrentProjectFolder + $"/Arcade/Backup/{CurrentDifficulty}_{DateTimeOffset.Now:yyyy-MM-dd hh-mm-ss}.aff";
            File.Copy(path, backupPath);
            FileStream fs = new FileStream(path, FileMode.Create);
            try
            {
                ArcGameplayManager.Instance.SerializeChart(fs, ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode);
            }
            catch (Exception Ex)
            {
                AdeSingleDialog.Instance.Show(Ex.ToString() + "\n" + Ex.ToString(), I.S["saveerror"]);
            }
            AdeToast.Instance.Show(string.Format(I.S["saveprojectnotify"], path, backupPath));
            fs.Close();
        }

        private IEnumerator AutosaveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(30f);
                if (CurrentProject == null || CurrentProjectFolder == null) continue; 
                string backupPath = CurrentProjectFolder + $"/Arcade/Autosave/{CurrentDifficulty}_{DateTimeOffset.Now.ToString("yyyy-MM-dd hh-mm-ss")}.aff";
                FileStream fs = new FileStream(backupPath, FileMode.Create);
                try
                {
                    ArcGameplayManager.Instance.SerializeChart(fs, ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode);
                }
                catch (Exception Ex)
                {
                    AdeSingleDialog.Instance.Show(Ex.Message + "\n" + Ex.ToString(), I.S["autosaveerror"]);
                }
                fs.Close();
            }
        }

        private IEnumerator LoadChartCoroutine(int index, bool shutter)
        {
            if (CurrentProject == null || CurrentProjectFolder == null || CurrentProject.AudioClip == null)
            {
                yield break;
            }

            if (!File.Exists($"{CurrentProjectFolder}/{index}.aff"))
            {
                File.WriteAllText($"{CurrentProjectFolder}/{index}.aff", "AudioOffset:0\n-\ntiming(0,100.00,4.00);");
            }

            if (shutter) yield return AdeShutterManager.Instance.CloseCoroutine();
            ArcadeComposeManager.Instance.Pause();
            AdeObsManager.Instance.ForceClose();
            CommandManager.Instance.FreeSilent();

            Aff.ArcaeaAffReader reader = null;
            try
            {
                reader = new Aff.ArcaeaAffReader($"{CurrentProjectFolder}/{index}.aff");
            }
            catch (Aff.ArcaeaAffFormatException Ex)
            {
                AdeSingleDialog.Instance.Show(Ex.ToString(), I.S["charterror"]);
                reader = null;
            }
            catch (Exception Ex)
            {
                AdeSingleDialog.Instance.Show(Ex.ToString(), I.S["charterror"]);
                reader = null;
            }
            if (reader == null)
            {
                if (shutter) yield return AdeShutterManager.Instance.OpenCoroutine();
                yield break;
            } 
            ArcGameplayManager.Instance.Load(new ArcChart(reader), CurrentProject.AudioClip);
            CurrentDifficulty = index;

            try
            {
                Diff.text = CurrentProject.Difficulties2[CurrentDifficulty] == null ? "" : CurrentProject.Difficulties2[CurrentDifficulty].Rating;
            }
            catch (Exception)
            {
                try
                {
                    Diff.text = CurrentProject.Difficulties[CurrentDifficulty] == null ? "" : CurrentProject.Difficulties[CurrentDifficulty].Rating;
                }
                catch (Exception)
                {

                }
            }
            foreach (Image i in DifficultyImages) i.color = new Color(1f, 1f, 1f, 0.6f);
            DifficultyImages[index].color = new Color(1, 1, 1, 1);

            AudioOffset.interactable = true;
            AudioOffset.text = ArcAudioManager.Instance.AudioOffset.ToString();

            watcher.Path = CurrentProjectFolder;
            watcher.Filter = $"{index}.aff";

            yield return null;
             
            ArcGameplayManager.Instance.Timing = CurrentProject.LastWorkingTiming;
            ArcTimingManager.Instance.DropRate = PlayerPrefs.GetInt("DropRate", 100);
            ArcArcManager.Instance.Rebuild();

            if (shutter) yield return AdeShutterManager.Instance.OpenCoroutine();
        }
        private IEnumerator LoadCoverCoroutine()
        {
            if (!File.Exists($"{CurrentProjectFolder}/base.jpg"))
            {
                CoverImage.sprite = DefaultCover;
                yield break;
            }
            string path = $"{CurrentProjectFolder}/base.jpg";
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(Uri.EscapeUriString("file:///" + path.Replace("\\", "/"))))
            {
                yield return req.SendWebRequest();
                if (!string.IsNullOrWhiteSpace(req.error))
                {
                    CoverImage.sprite = DefaultCover;
                    yield break;
                }
                CurrentProject.Cover = DownloadHandlerTexture.GetContent(req);
                CurrentProject.CoverSprite = Sprite.Create(CurrentProject.Cover, new Rect(0, 0, CurrentProject.Cover.width, CurrentProject.Cover.height), new Vector2(0.5f, 0.5f));
                CoverImage.sprite = CurrentProject.CoverSprite;
            }
        }
        private IEnumerator LoadMusicCoroutine()
        {
            string[] searchPaths = new string[] { $"{CurrentProjectFolder}/Arcade/Converting/base.wav", $"{CurrentProjectFolder}/base.wav", $"{CurrentProjectFolder}/base.ogg" };
            string path = null;
            foreach (var s in searchPaths)
            {
                if (File.Exists(s)) path = s;
            }
            if (path == null)
            {
                if (File.Exists($"{CurrentProjectFolder}/base.mp3"))
                {
                    Task converting = Task.Run(() => Mp3Converter.Mp3ToWav($"{CurrentProjectFolder}/base.mp3", $"{CurrentProjectFolder}/Arcade/Converting/base.wav"));
                    while (!converting.IsCompleted) yield return null;
                    if (converting.Status == TaskStatus.RanToCompletion)
                    {
                        path = $"{CurrentProjectFolder}/Arcade/Converting/base.wav";
                    }
                }
            }
            if (path == null)
            {
                AdeSingleDialog.Instance.Show(I.S["charterrormusic"], I.S["charterror"]);
                yield break;
            }
            using (UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip(Uri.EscapeUriString("file:///" + path.Replace("\\", "/")), path.EndsWith("wav") ? AudioType.WAV : AudioType.OGGVORBIS))
            {
                yield return req.SendWebRequest();
                if (!string.IsNullOrWhiteSpace(req.error))
                {
                    yield break;
                }
                CurrentProject.AudioClip = DownloadHandlerAudioClip.GetContent(req);
                AdeTimingSlider.Instance.Enable = true;
                AdeTimingSlider.Instance.Length = (int)(CurrentProject.AudioClip.length * 1000);
            }
        }
        private IEnumerator LoadingCoroutine()
        {
            yield return AdeShutterManager.Instance.CloseCoroutine();

            Name.text = CurrentProject.Title;
            Composer.text = CurrentProject.Artist;
            Diff.text = "";
            Name.interactable = true;
            Composer.interactable = true;
            Diff.interactable = true;
            OpenLabel.color = new Color(0, 0, 0, 0); 

            watcher.EnableRaisingEvents = false;
            FileWatchEnableImage.color = DisableColor;

            yield return LoadCoverCoroutine();
            yield return LoadMusicCoroutine();
            yield return LoadChartCoroutine(CurrentProject.LastWorkingDifficulty, false);

            yield return AdeShutterManager.Instance.OpenCoroutine();
        }
        public void ReloadChart(int index)
        {
            StartCoroutine(LoadChartCoroutine(index, true));
        }

        public void OnComposerEdited()
        {
            if (CurrentProject == null) return;
            CurrentProject.Artist = Composer.text;
        }
        public void OnNameEdited()
        {
            if (CurrentProject == null) return;
            CurrentProject.Title = Name.text;
        }
        public void OnDiffEdited()
        {
            if (CurrentProject == null) return;
            if (CurrentDifficulty < 0 || CurrentDifficulty > 3) return;
            if (!CurrentProject.Difficulties2.ContainsKey(CurrentDifficulty))
                CurrentProject.Difficulties2[CurrentDifficulty] = new AdeChartDifficulty();
            CurrentProject.Difficulties2[CurrentDifficulty].Rating = Diff.text;
        }
        public void OnAudioOffsetEdited()
        {
            int value;
            bool result = int.TryParse(AudioOffset.text, out value);
            if (result)
            {
                ArcAudioManager.Instance.AudioOffset = value;
                AudioOffset.text = value.ToString();
            }
        }
        public void OnFileWatchClicked()
        {
            if (CurrentProject != null && CurrentDifficulty != -1)
            {
                watcher.EnableRaisingEvents = !watcher.EnableRaisingEvents;
                FileWatchEnableImage.color = watcher.EnableRaisingEvents ? EnableColor : DisableColor;
            }
        }

        private void OnWatcherChanged(object sender, FileSystemEventArgs e)
        {
            DateTime begin = DateTime.Now;
            FileStream fs = null;

            retry:
            try
            {
                fs = File.Open(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                if (DateTime.Now - begin < TimeSpan.FromSeconds(3))
                    goto retry;
                else return;
            }

            fs?.Close();

            shouldReload = true;
        }

        public void OnSaveModeClicked()
        {
            ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode = ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode == ChartSortMode.Timing ? ChartSortMode.Type : ChartSortMode.Timing;
            SaveMode.text = ArcadeComposeManager.Instance.ArcadePreference.ChartSortMode == ChartSortMode.Timing ? I.S["sortbytime"] : I.S["sortbytype"];
        }
        public void OnOpenFolder()
        {
            if (CurrentProject == null || string.IsNullOrWhiteSpace(CurrentProjectFolder)) return;
            Schwarzer.Windows.Dialog.OpenExplorer(CurrentProjectFolder);
        }

        public void OnApplicationQuit()
        {
            SaveProject();
        }
    }
}