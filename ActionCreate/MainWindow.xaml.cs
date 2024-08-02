using System.Windows;
using System.Windows.Threading;
using System.IO;
using System.Windows.Controls;
using System.Text.Json;
using System.Collections.ObjectModel;
using ActionCreate.classes;
using System;


namespace ActionCreate
{
    public partial class MainWindow : Window
    {
        bool PlayFlag = false;
        public string PythFolderVideo { get; set; } = "not open folder";
        public string PythFolderVideoSaveJson { get; set; } = "not open save folder";
        public string PythSelectedVideo { get; set; }
        public string NameOpenVideo { get; set; }
        public ObservableCollection<string>? collectionSaveJson { get; set; } = null;
        public double StartCrash;
        public double EndCrash;
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            lbFolder.Content = PythFolderVideo;
            ListBoxSaveFolder.ItemsSource = collectionSaveJson;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (mePlayer.Source != null)
            {
                if (mePlayer.NaturalDuration.HasTimeSpan)
                    lblStatus.Content = String.Format("{0} / {1}", mePlayer.Position.ToString(@"mm\:ss"), mePlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                lblStatus.Content = "No file selected...";
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (!PlayFlag)
            {
                mePlayer.Play();
                PlayFlag = true;
            }
            else
            {
                mePlayer.Pause();
                PlayFlag = false;
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Stop();
        }
        private void open_video_file(object sender, RoutedEventArgs e)
        {
            if(PythSelectedVideo != null && PythSelectedVideo != "")
            {
                mePlayer.Source = new Uri(PythSelectedVideo);
                mePlayer.Play();
                mePlayer.Pause();
                btnPlay.Focus();
            }
        }
        private void Open_folder(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select a folder";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string fullPathToFolder = dialog.FolderName;
                string folderNameOnly = dialog.SafeFolderName;
                PythFolderVideo = dialog.FolderName;
                lbFolder.Content = PythFolderVideo;
            }
            if (Directory.Exists(PythFolderVideo))
            {
                var ListFiles = Directory.GetFiles(PythFolderVideo);
                //todo out only mp4 format
                listbFolder.ItemsSource = toNameFiles(SortFileVIdeo(ListFiles.ToList(),".mp4"));
            }
        }

        private void selected_video(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            PythSelectedVideo = lb.SelectedItem.ToString();
            var arr = lb.SelectedItem.ToString().Split('\\');
            NameOpenVideo = arr[arr.Length-1];
        }

        private void Start_crash(object sender, RoutedEventArgs e)
        {
            lbStartCrash.Content = mePlayer.Position.ToString();
            StartCrash = Convert.ToDouble(mePlayer.Position.ToString(@"ss\,fff"));
        }

        private void End_crash(object sender, RoutedEventArgs e)
        {
            lbEndCrash.Content = mePlayer.Position.ToString();
            EndCrash = Convert.ToDouble(mePlayer.Position.ToString(@"ss\,fff"));
        }

        private void save_json_file(object sender, RoutedEventArgs e)
        {
            if(PythFolderVideoSaveJson == "not open save folder")
            {
                MessageBox.Show("opne folder for save json");
                return;
            }
            data data = new data( 
                new List<Annotation>() { 
                    new Annotation(new List<double>() { StartCrash, EndCrash }) 
                }, 
                EndCrash - StartCrash, 
                "train", 
                PythSelectedVideo);

            using (FileStream fs = new FileStream($"{PythFolderVideoSaveJson}\\{NameOpenVideo}.json", FileMode.OpenOrCreate))
            {
                
                JsonSerializer.Serialize<data>(fs, data);
                Console.WriteLine("Data has been saved to file");
            }

            if (Directory.Exists(PythFolderVideoSaveJson))
            {
                var ListFiles = Directory.GetFiles(PythFolderVideoSaveJson);
                collectionSaveJson = new ObservableCollection<string>(toNameFiles(SortFileVIdeo(ListFiles.ToList(), ".json")));

                ListBoxSaveFolder.ItemsSource = collectionSaveJson;
            }
        }

        private void Grid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Space)
            {
                if (!PlayFlag)
                {
                    mePlayer.Play();
                    PlayFlag = true;
                }
                else
                {
                    mePlayer.Pause();
                    PlayFlag = false;
                }
            }
        }

        private List<string> SortFileVIdeo(List<string> list, string format)
        {
            return list.FindAll(x => x.Contains(format));
        }

        private void listbFolder_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBox lb = sender as ListBox;
            PythSelectedVideo = lb.SelectedItem.ToString();

            if (PythSelectedVideo != null && PythSelectedVideo != "")
            {
                mePlayer.Source = new Uri(PythSelectedVideo);
                mePlayer.Play();
                mePlayer.Pause();
                btnPlay.Focus();
            }
        }

        private void select_folder_save(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFolderDialog dialog = new();

            dialog.Multiselect = false;
            dialog.Title = "Select a folder";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string fullPathToFolderSave = dialog.FolderName;
                string folderNameOnlySave = dialog.SafeFolderName;

                lbFolderSAve.Content = fullPathToFolderSave;
                PythFolderVideoSaveJson = dialog.FolderName;
                lbFolder.Content = PythFolderVideo;
            }
            if (Directory.Exists(PythFolderVideoSaveJson))
            {
                var ListFiles = Directory.GetFiles(PythFolderVideoSaveJson);
                collectionSaveJson = new ObservableCollection<string>(toNameFiles(SortFileVIdeo(ListFiles.ToList(), ".json")));

                ListBoxSaveFolder.ItemsSource =  collectionSaveJson;
            }
        }

        private List<string> toNameFiles(List<string> list)
        {
            List<string> result = new List<string>();
            foreach (string file in list)
            {
                var arr = file.Split('\\');
                result.Add(arr[arr.Length - 1]);
            }
            return result;
        }

        private void convert_to_json_dataset(object sender, RoutedEventArgs e)
        {
            DataSet dataSet = new DataSet(
                new Dictionary<string, data>(),
                new List<Taxanomy>() 
                    { 
                        new Taxanomy(),
                        new Taxanomy(){ NodeId = "1", NodeName = "Crash", ParentId = "0", ParetName = "Root" }
                    }
                );

            var ListFiles = Directory.GetFiles(PythFolderVideoSaveJson);

            foreach (var file in ListFiles)
            {
                var arr = file.Split('\\');
                string name = arr[arr.Length - 1];

                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
                {
                    data data = JsonSerializer.Deserialize<data>(fs);
                    dataSet.Database.Add(name, data);
                }
            }

            using (FileStream fs = new FileStream($"{PythFolderVideoSaveJson}\\HACS_segments_v1.0.json", FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize<DataSet>(fs, dataSet);
            }

            MessageBox.Show("Convet to vin");
        }
    }
}
