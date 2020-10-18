using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace Chord_Dictator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Element> elements = new List<Element>();
        public DispatcherTimer timer = new DispatcherTimer();
        string dfp = "App Files/Dictionaries/defDict.txt";
        string log = "App Files/Initial Files/logs.txt";
        string set = "App Files/Initial Files/settings.txt";
        string dictionaryPath = "App Files/Dictionaries/";
        List<int> alreadyShown = new List<int>();
        public MainWindow()
        {
            InitializeComponent();
            CreateIfNo();
            ClearLog();
            InitTimer();
            WriteToLog("Program launched.", "MainWindowConstructor");
            dfp = GetLastDictionary();
        }
        struct Element
        {
            public string name;
            public string imagePath;
            public string soundPath;
            public Element(string n, string i, string s)
            {
                name = n;
                imagePath = i;
                soundPath = s;
            }
        }
        void ClearLog()
        {
            try
            {
                if (File.Exists(log)) File.WriteAllText(log, "");
                else File.Create(log).Close();
            }
            catch (Exception ex)
            {
                WriteToLog("Log clearence error.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
        }
        void InitTimer()
        {
            try
            {
                timer.Tick += StartMoved;
                timer.Interval = TimeSpan.FromSeconds(10);
            }
            catch (Exception ex)
            {
                WriteToLog("Timer setting went wrong.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
        }
        int StrToInt(string str)
        {
            int num;
            if (!int.TryParse(str, out num))
            {
                WriteToLog("Failed to parse string \"" + str + "\" to int.", new StackTrace().GetFrame(0).GetMethod().Name);
                return 5;
            }
            return num;
        }
        bool FileOk(bool writelog = false)
        {
            string[] filelines = File.ReadAllLines(dfp);
            foreach (string line in filelines)
                if (line.Split('>').Length != 3 && line.Length > 0)
                {
                    if (writelog) WriteToLog("Row must contain 2 '>' [Path: " + dfp + "]");
                    return false;
                }
            return true;
        }
        void RemoveEmptyEntries()
        {
            string[] filelines = File.ReadAllLines(dfp);
            for (int i = 0; i < filelines.Length; i++)
            {
                filelines[i] = filelines[i].Replace("  ", " ").Replace("  ", " ");
            }
            File.WriteAllLines(dfp, filelines);
        }
        void CreateIfNo()
        {
            try
            {
                if (!Directory.Exists("App Files"))
                {
                    Directory.CreateDirectory("App Files");
                    if (!Directory.Exists("App Files/Initial Files"))
                    {
                        Directory.CreateDirectory("App Files/Initial Files");
                        if (!File.Exists(dfp))
                        {
                            File.Create(dfp).Close();
                        }
                    }
                    if (!Directory.Exists("App Files/Images"))
                    {
                        Directory.CreateDirectory("App Files/Images");
                    }
                    if (!Directory.Exists("App Files/Dictionaries"))
                    {
                        Directory.CreateDirectory("App Files/Dictionaries");
                    }
                    if (!Directory.Exists("App Files/Audio"))
                    {
                        Directory.CreateDirectory("App Files/Audio");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Program tree folder creation exception.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
        }
        void WriteToLog(string message, string functionName = "", string exmsg = "", string tip = "")
        {
            if (!File.Exists(log)) File.Create(log).Close();
            File.AppendAllText(log, "[" + DateTime.Now + "] ");
            if (functionName.Length > 0) File.AppendAllText(log, functionName + "()");
            File.AppendAllText(log, "-> " + message);
            if (exmsg.Length > 0) File.AppendAllText(log, " (" + exmsg + ") ");
            if (tip.Length > 0) File.AppendAllText(log, "[Tip: " + tip.TrimEnd('.') + "]");
            File.AppendAllText(log, Environment.NewLine);
        }
        string StringBuilder(bool spaces, params string[] elems)
        {
            string str = "";
            if (spaces) foreach (string el in elems) str += el + " ";
            if (!spaces) foreach (string el in elems) str += el;
            return str;
        }
        string GetFileName(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\')
                {
                    return path.Substring(i + 1, path.Length - i - 1);
                }
            }
            return "NO_NAME";
        }
        string GetFileHome(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\')
                {
                    return path.Substring(0, i);
                }
            }
            return "NO_PATH";
        }
        string CropInitialPath(string path)
        {
            for (int i = 0; i < path.Length - 11; i++)
            {
                if (path[i] == '\\' && path[i + 1] == 'A' && path[i + 2] == 'p' && path[i + 3] == 'p' && path[i + 4] == ' ' && path[i + 5] == 'F' && path[i + 6] == 'i' && path[i + 7] == 'l' && path[i + 8] == 'e' && path[i + 9] == 's' && path[i + 10] == '\\')
                {
                    return path.Substring(i + 1);
                }
            }
            return path;
        }
        string GetLastDictionary()
        {
            try
            {
                string[] settings = File.ReadAllLines(set);
                foreach (string setting in settings)
                {
                    string[] settingParts = setting.Split('>');
                    if (settingParts[0] == "LastDictionary" && File.Exists(settingParts[1])) { WriteToLog("LastDictionary setting: " + settingParts[1], new StackTrace().GetFrame(0).GetMethod().Name); return settingParts[1]; }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Can't get last dictionary path from settings.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try changing dictionary again.");
            }
            return dfp;
        }
        string[] GetSettings()
        {
            return File.ReadAllLines(set);
        }
        void AddSetting(string setname, string definition = "-", int addbeforeidx = 0)
        {
            List<string> settings = File.ReadAllLines(set).ToList();
            foreach (string setting in settings)            //Uniqueness check
            {
                if (setting.Split('>')[0] == setname) return;
            }
            settings.Insert(addbeforeidx, setname.Replace('>', ' ') + ">" + definition.Replace('>', ' '));
            WriteToLog(setname + " setting added", new StackTrace().GetFrame(0).GetMethod().Name);
            File.WriteAllLines(set, settings.ToArray());
        }
        bool EditSettings(int rowidx, string definition)
        {
            string[] filecont = File.ReadAllLines(set);
            if (rowidx >= filecont.Length) return false;
            string[] rowCont = filecont[rowidx].Split('>');
            if (rowCont.Length <= 1) WriteToLog("It seems, that setting you are interested in contains an error. [Tip: Try clearing settings file.]", new StackTrace().GetFrame(0).GetMethod().Name);
            WriteToLog(rowCont[0] + " definiton edited from " + rowCont[1] + " to " + definition, new StackTrace().GetFrame(0).GetMethod().Name);
            filecont[rowidx] = rowCont[0] + ">" + definition;
            File.WriteAllLines(set, filecont);
            return true;
        }
        bool EditSettings(string rowName, string definition)
        {
            string[] settings = File.ReadAllLines(set);
            for (int i = 0; i < settings.Length; i++)
            {
                string[] rowCont = settings[i].Split('>');
                if (rowCont[0] == rowName)
                {
                    settings[i] = rowCont[0] + ">" + definition;
                    File.WriteAllLines(set, settings);
                    return true;
                }
            }
            return false;
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (chkImg.IsChecked == true || chkText.IsChecked == true || chkAudio.IsChecked == true)
            {
                try
                {
                    CreateIfNo();
                    FileOk(true);
                    timer.Stop();
                    timer.Interval = TimeSpan.FromSeconds(StrToInt(tbDelay.Text));
                    string[] rawElements = File.ReadAllLines(dfp);
                    string[] currElement;
                    elements.Clear();
                    for (int i = 0; i < rawElements.Length; i++)
                    {
                        currElement = rawElements[i].Split('>');
                        elements.Add(new Element(currElement[0], currElement[1], currElement[2]));
                    }
                    WriteToLog("New session started.", new StackTrace().GetFrame(0).GetMethod().Name);
                    try
                    {
                        Dispatcher.Invoke(() => timer.Start());
                        if (File.ReadAllText(dfp).TrimEnd(' ').Length == 0)
                        {
                            MessageBox.Show(dfp + " is empty");
                            WriteToLog("Dictionary " + dfp + " is empty", new StackTrace().GetFrame(0).GetMethod().Name);
                            timer.Stop();
                            return;
                        }
                        if (alreadyShown.Count > 0) alreadyShown.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Start failed.");
                        WriteToLog("Failed to start timer", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try again");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Start failed.");
                    WriteToLog("Failed to reach dictionary file (" + dfp + ")", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
                    if (!File.Exists(dfp))
                    {
                        if (MessageBox.Show("Do you want to create dictionary file?", "No such file", MessageBoxButton.YesNo) == MessageBoxResult.Yes) File.Create(dfp);
                        else return;
                    }
                    else
                    {
                        MessageBox.Show("Directory exists, but problem occured while starting. Try again.");
                        WriteToLog("Directory exists, but launch terminated.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try again");
                    }
                }
            }
            else { MessageBox.Show("You have chosen nothing to dictate."); WriteToLog("Dictation canceled due to all options unchecked.", new StackTrace().GetFrame(0).GetMethod().Name); }
        }
        bool Unique(int num)
        {
            foreach (var el in alreadyShown)
            {
                if (num == el) return false;
            }
            return true;
        }
        void StartMoved(object sender, EventArgs e)
        {
            int randomIndex = new Random().Next(0, elements.Count);
            if (File.ReadAllLines(dfp).Length <= alreadyShown.Count && alreadyShown.Count != 0)
            {
                Dispatcher.Invoke(() => timer.Stop());
                MessageBox.Show("Dictation finished successfuly.");
                WriteToLog("Dictation finished successfuly.", new StackTrace().GetFrame(0).GetMethod().Name);
                timer.Stop();
                return;
            }
            while (!Unique(randomIndex))
            {
                randomIndex = new Random().Next(0, elements.Count);
            }
            alreadyShown.Add(randomIndex);
            if (chkText.IsChecked == true) tbElementName.Text = elements[randomIndex].name;
            else tbElementName.Text = string.Empty;
            try
            {
                if (chkImg.IsChecked == true) imgElement.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + elements[randomIndex].imagePath, UriKind.Absolute));
                else imgElement.Source = new BitmapImage();
            }
            catch (Exception ex)
            {
                WriteToLog("Failed to load image " + Environment.CurrentDirectory + "\\" + elements[randomIndex].imagePath, new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
            try
            {
                if (chkImg.IsChecked == true)
                {
                    MediaPlayer mp = new MediaPlayer();
                    mp.Open(new Uri(Environment.CurrentDirectory + "\\" + elements[randomIndex].soundPath, UriKind.RelativeOrAbsolute));
                    mp.Play();
                    //SoundPlayer player = new SoundPlayer();
                    //player.Play();
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Failed to load sound.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message + " Path: " + elements[randomIndex].soundPath);
            }
        }

        private void btnGoToAdd_Click(object sender, RoutedEventArgs e)
        {
            AddWindow window = new AddWindow(dfp, log);
            WriteToLog("Opened add/remove window.", new StackTrace().GetFrame(0).GetMethod().Name);
            window.ShowDialog();
        }
        private void btnChangeMoveInit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                if (ofd.FileName.Length > 3)
                {
                    if (ofd.FileName.Substring(ofd.FileName.Length - 4, 4) == ".txt")
                    {
                        dfp = dictionaryPath + GetFileName(ofd.FileName);
                        if (!File.Exists(dfp)) File.Copy(ofd.FileName, dfp); else { WriteToLog("File chosen was located in root folder, so it wasn't copied. [Path: " + ofd.FileName + "]"); }
                        FileOk(true);
                        RemoveEmptyEntries();

                        MessageBox.Show("Dictionary file is now " + dfp);
                        WriteToLog("Dictionary file is now " + dfp, new StackTrace().GetFrame(0).GetMethod().Name);
                        btnGoToAdd.ToolTip = "All added items will be saved to " + dfp;
                        if (!File.Exists(set)) File.Create(set).Close();
                        try
                        {
                            if (EditSettings("LastDictionary", CropInitialPath(ofd.FileName)))
                            {
                                EditSettings("LastDictionary", CropInitialPath(ofd.FileName));
                                WriteToLog("Last dictionary set (" + ofd.FileName + ")", new StackTrace().GetFrame(0).GetMethod().Name);
                            }
                            else
                            {
                                AddSetting("LastDictionary", CropInitialPath(ofd.FileName));
                                WriteToLog("Last dictionary set, but created LastDirectory parameter first (" + ofd.FileName + ")", new StackTrace().GetFrame(0).GetMethod().Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("Initial dictionary wasn't changed", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try choosing txt file.");
                        }
                    }
                    else
                    {
                        WriteToLog("Failed to change dictionary. File type does not match.", new StackTrace().GetFrame(0).GetMethod().Name);
                        MessageBox.Show("File type does not match. Choose txt.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteToLog("Failed to change dictionary.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
        }
        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(CropInitialPath(dfp));
            ListWindow listWindow = new ListWindow(CropInitialPath(dfp), log);
            listWindow.ShowDialog();
        }
        #region NOT USED
        private void btnChangeInitNoMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();
                if (ofd.FileName.Length > 3)
                {
                    if (ofd.FileName.Substring(ofd.FileName.Length - 4, 4) == ".txt")
                    {
                        dfp = dictionaryPath + GetFileName(ofd.FileName);
                        if (!File.Exists(dfp)) File.Copy(ofd.FileName, dfp); else { WriteToLog("File chosen was located in root folder, so it wasn't copied. [Path: " + ofd.FileName + "]"); }
                        FileOk(true);
                        RemoveEmptyEntries();

                        MessageBox.Show("Dictionary file is now " + dfp);
                        WriteToLog("Dictionary file is now " + dfp, new StackTrace().GetFrame(0).GetMethod().Name);
                        btnGoToAdd.ToolTip = "All added items will be saved to " + dfp;
                        if (!File.Exists(set)) File.Create(set).Close();
                        try
                        {
                            if (EditSettings("LastDictionary", dfp))
                            {
                                EditSettings("LastDictionary", dfp);
                                WriteToLog("Last dictionary set (" + dfp + ")", new StackTrace().GetFrame(0).GetMethod().Name);
                            }
                            else
                            {
                                AddSetting("LastDictionary", dfp);
                                WriteToLog("Last dictionary set, but created LastDirectory parameter first (" + dfp + ")", new StackTrace().GetFrame(0).GetMethod().Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("Initial dictionary wasn't changed", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try choosing txt file.");
                        }
                    }
                    else
                    {
                        WriteToLog("Failed to change dictionary. File type does not match.", new StackTrace().GetFrame(0).GetMethod().Name);
                        MessageBox.Show("File type does not match. Choose txt.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteToLog("Failed to change dictionary.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
        }
        void StartNoMove(object sender, EventArgs e)
        {
            int randomIndex = new Random().Next(0, elements.Count);
            if (File.ReadAllLines(dfp).Length <= alreadyShown.Count && alreadyShown.Count != 0)
            {
                Dispatcher.Invoke(() => timer.Stop());
                MessageBox.Show("Dictation finished successfuly.");
                WriteToLog("Dictation finished successfuly.", new StackTrace().GetFrame(0).GetMethod().Name);
                timer.Stop();
                return;
            }
            while (!Unique(randomIndex))
            {
                randomIndex = new Random().Next(0, elements.Count);
            }
            alreadyShown.Add(randomIndex);
            if (chkText.IsChecked == true) tbElementName.Text = elements[randomIndex].name;
            try
            {
                if (chkImg.IsChecked == true) imgElement.Source = new BitmapImage(new Uri(elements[randomIndex].imagePath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                WriteToLog("Failed to load image " + elements[randomIndex].imagePath, new StackTrace().GetFrame(0).GetMethod().Name, ex.Message);
            }
            try
            {
                if (chkImg.IsChecked == true)
                {
                    MediaPlayer mp = new MediaPlayer();
                    mp.Open(new Uri(elements[randomIndex].soundPath, UriKind.RelativeOrAbsolute));
                    mp.Play();
                    //SoundPlayer player = new SoundPlayer();
                    //player.Play();
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Failed to load sound.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message + " Path: " + elements[randomIndex].soundPath);
            }
        }
        string GetLastDictionaryNoMove()
        {
            try
            {
                string[] settings = File.ReadAllLines(set);
                foreach (string setting in settings)
                {
                    string[] settingParts = setting.Split('>');
                    if (settingParts[0] == "LastDictionary") { WriteToLog("LastDictionary setting found.", new StackTrace().GetFrame(0).GetMethod().Name); return settingParts[1]; }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Can't get last dictionary path from settings.", new StackTrace().GetFrame(0).GetMethod().Name, ex.Message, "Try changing dictionary again.");
            }
            return dfp;
        }
        #endregion
    }
}