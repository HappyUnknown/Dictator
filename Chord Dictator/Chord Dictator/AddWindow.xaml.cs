using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chord_Dictator
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        string log;
        string dfp;
        string audiopath = "App Files\\Audio\\";
        string imagepath = "App Files\\Images\\";
        public AddWindow()
        {
            InitializeComponent();
        }
        public AddWindow(string path, string logs)
        {
            InitializeComponent();
            try
            {
                dfp = path;
                log = logs;
                WriteToLog("Editing started.", "AddWindowConstructor");
            }
            catch (Exception ex)
            {
                WriteToLog("Add-constructor issue.", "AddWindowConstructor", ex.Message);
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
        void CreateIfNoDl()
        {
            if (!Directory.Exists("App Files"))
            {
                Directory.CreateDirectory("App Files");
            }
            if (!Directory.Exists("App Files"))
            {
                Directory.CreateDirectory("App Files/Directory");
            }
            if (!File.Exists(dfp))
            {
                File.Create(dfp).Close();
                WriteToLog("Initial dictionary file created.", "CreateIfNoDl");
            }
        }
        private void btnConnImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Picture|*.png;*.jpeg;*.jpg|All|*.*";
                if (ofd.ShowDialog() == true)
                {
                    imgElement.Source = new BitmapImage(new Uri(ofd.FileName));
                    tbElementImgPath.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                WriteToLog("Failed to connect image.", "btnConnImg_Click", ex.Message);
            }
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
                if (path[i] == '/')
                {
                    return path.Substring(0, i) + "/";
                }
            }
            return "NO_PATH";
        }
        private void btnConnSound_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Audio|*.mp3;*.wav|All|*.*";
                if (ofd.ShowDialog() == true)
                {
                    tbSoundPath.Text = ofd.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during add operation (check logs).");
                File.AppendAllText(log, "Failed to connect sound: " + DateTime.Now + "-> " + ex.Message + Environment.NewLine);
            }
        }
        private void btnAddElement_Click(object sender, RoutedEventArgs e)
        {
            FileOk(true);
            AddElement(tbName.Text.Trim(' '), tbSoundPath.Text, tbElementImgPath.Text);
        }
        private void btnAddMoveElement_Click(object sender, RoutedEventArgs e)
        {
            string aupath = "";
            string impath = "";
            try
            {
                if (tbSoundPath.Text.Length > 2) aupath = audiopath + GetFileName(tbSoundPath.Text);
                if (tbElementImgPath.Text.Length > 2) impath = imagepath + GetFileName(tbElementImgPath.Text);

                try
                {
                    if (!File.Exists(aupath) && tbSoundPath.Text.Length > 2) File.Copy(tbSoundPath.Text, aupath); else { WriteToLog("File chosen was located in root folder, so it wasn't copied. [Path: " + tbSoundPath.Text + "]"); }
                    if (!File.Exists(impath) && tbElementImgPath.Text.Length > 2) File.Copy(tbElementImgPath.Text, impath); else { WriteToLog("File chosen was located in root folder, so it wasn't copied. [Path: " + tbElementImgPath.Text + "]"); }
                    AddElement(tbName.Text, aupath, impath);
                }
                catch (Exception ex)
                {
                    WriteToLog("Troubles during moving files to program folder.", "btnAddMoveElement_Click", ex.Message);
                    MessageBox.Show("Error during add operation (check logs).");
                }
                WriteToLog("New image path (copy) : " + aupath, "btnAddMoveElement_Click");
                WriteToLog("New audio path (copy) : " + impath, "btnAddMoveElement_Click");
            }
            catch (Exception ex)
            {
                WriteToLog("Troubles during building new paths.(" + aupath + "," + impath + ")", "btnAddMoveElement_Click", ex.Message);
                MessageBox.Show("Error during add operation (check logs).");
            }

        }

        void AddElement(string name, string soundpath, string imagepath)
        {
            try
            {
                if (name == "")
                {
                    MessageBox.Show("No name signed. Rename please.");
                    return;
                }
                if (soundpath == "")
                {
                    soundpath = "NOSOUND";
                    MessageBox.Show("No sound signed.");
                }
                if (imgElement.Source == null || tbElementImgPath.Text == "")
                {
                    imagepath = "NOIMAGE";
                    MessageBox.Show("No image signed.");
                }

                if (!name.Contains('>') && !imagepath.Contains('>') && !soundpath.Contains('>'))
                {
                    File.AppendAllText(dfp, name + ">" + imagepath + ">" + soundpath + Environment.NewLine);
                    MessageBox.Show("Added \"" + name + "\" element. Path: " + dfp);
                    imgElement.Source = null;
                    tbName.Text = string.Empty;
                    tbSoundPath.Text = string.Empty;
                    tbElementImgPath.Text = string.Empty;
                    WriteToLog("Added new element \"" + name + "\".", "AddElement");
                }
                else
                {
                    MessageBox.Show("\">\" program symbol used. Try removing it.");
                    WriteToLog("Program symbol used in dictionary addition.", "AddElement");
                }
                Close();
            }
            catch (Exception ex)
            {
                WriteToLog("Failed to add Element.", "AddElement", ex.Message, "Try recreating initial dictionary or trying again");
                CreateIfNoDl();
            }
        }
        private void btnRmLast_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (File.ReadAllText(dfp).Length > 0)
                {
                    if (MessageBox.Show("Remove last dictionary element?", "Remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        FileOk(true);

                        if (MessageBox.Show("Do you want to remove last element?", "Removing last", MessageBoxButton.YesNo) == MessageBoxResult.Yes && File.ReadAllLines(dfp).Length > 0)
                        {
                            List<string> content = File.ReadAllLines(dfp).ToList();
                            MessageBox.Show("Last element removed. - " + content[content.Count - 1].Split('>')[0]);
                            content.RemoveAt(content.Count - 1);
                            File.WriteAllLines(dfp, content.ToArray());
                        }
                    }
                }
                else
                {
                    WriteToLog("No elements left to remove.", "btnRmLast_Click");
                    MessageBox.Show("No elements.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't remove.");
                CreateIfNoDl();
                WriteToLog("Failed to remove last from dictionary.", "btnRmLast_Click", ex.Message, "Your dictionary file was recreated, because program did not reach it. Try again.");
            }
        }
        #region NOT USED
        private void btnAddNoMoveElement_Click(object sender, RoutedEventArgs e)
        {
            string aupath = "";
            string impath = "";
            try
            {
                if (tbSoundPath.Text.Length > 2) aupath = tbSoundPath.Text;
                if (tbElementImgPath.Text.Length > 2) impath = tbElementImgPath.Text;

                try
                {
                    AddElement(tbName.Text, aupath, impath);
                }
                catch (Exception ex)
                {
                    WriteToLog("Troubles during moving files to program folder.", "btnAddMoveElement_Click", ex.Message);
                    MessageBox.Show("Error during add operation (check logs).");
                }
                WriteToLog("New image path (copy) : " + aupath, "btnAddMoveElement_Click");
                WriteToLog("New audio path (copy) : " + impath, "btnAddMoveElement_Click");
            }
            catch (Exception ex)
            {
                WriteToLog("Troubles during building new paths.(" + aupath + "," + impath + ")", "btnAddMoveElement_Click", ex.Message);
                MessageBox.Show("Error during add operation (check logs).");
            }
        }
        #endregion
    }
}
