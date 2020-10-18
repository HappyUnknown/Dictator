using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Логика взаимодействия для ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        class Element
        {
            public string Name;
            public string ImgPath;
            public string AuPath;
            public Element(string n, string i, string a)
            {
                Name = n;
                ImgPath = i;
                AuPath = a;
            }
        }
        string dfp;
        string log;
        public ListWindow(string dictPath, string logPath)
        {
            InitializeComponent();
            dfp = dictPath;
            log = logPath;
            DownloadTable();
            TblFromList(File.ReadAllLines(dfp).ToList());
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
        List<string> ElemsToStrs(List<Element> elems)
        {
            List<string> strs = new List<string>();
            foreach (Element item in elems)
            {
                strs.Add(item.Name + ">" + item.ImgPath + ">" + item.AuPath);
            }
            return strs;
        }
        void TblFromFile()
        {
            try
            {
                cmbElements.Items.Clear();
                string[] filelines = File.ReadAllLines(dfp);
                foreach (string line in filelines)
                {
                    cmbElements.Items.Add(line/*.Split('>')[0]*/);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        List<string> TblFromList(List<string> elements)
        {
            try
            {
                cmbElements.Items.Clear();
                string[] filelines = File.ReadAllLines(dfp);
                foreach (string line in elements)
                {
                    cmbElements.Items.Add(line/*.Split('>')[0]*/);
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Converting table content to list issues.", "TblFromList", ex.Message);
            }
            return elements;
        }

        List<Element> ListFromTbl()
        {
            List<Element> elems = new List<Element>();
            try
            {
                for (int i = 0; i < cmbElements.Items.Count; i++)
                {
                    elems.Add(new Element(cmbElements.Items[i].ToString().Split('>')[0], cmbElements.Items[i].ToString().Split('>')[1], cmbElements.Items[i].ToString().Split('>')[2]));
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Converting list to table content issues.", "ListFromTbl", ex.Message);
            }
            return elems;
        }
        void DownloadTable()
        {
            try
            {
                cmbElements.Items.Clear();
                string[] filelines = File.ReadAllLines(dfp);
                foreach (string line in filelines)
                {
                    cmbElements.Items.Add(new Element(line.Split('>')[0], line.Split('>')[1], line.Split('>')[2]));
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Table downloading from file issue. [PATH:" + dfp + "]", "DownloadTable", ex.Message);
            }
        }
        List<Element> FileToList()
        {
            List<Element> lines = new List<Element>();
            try
            {
                string[] filelines = File.ReadAllLines(dfp);
                foreach (string str in filelines)
                {
                    lines.Add(new Element(str.Split('>')[0], str.Split('>')[1], str.Split('>')[2]));
                }
            }
            catch (Exception ex)
            {
                WriteToLog("File to list convertation error.", "FileToList", ex.Message);
            }
            return lines;
        }
        void UploadTable()
        {
            try
            {
                for (int i = 0; i < cmbElements.Items.Count; i++)
                {
                    File.WriteAllText(dfp, cmbElements.Items[i].ToString().Split('>')[0] + ">" + cmbElements.Items[i].ToString().Split('>')[1] + ">" + cmbElements.Items[i].ToString().Split('>')[1] + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Uploading program table to file error.", "UploadTable", ex.Message);
            }
        }
        private void btnChangeName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbElements.SelectedIndex != -1)
                {
                    tbName.IsReadOnly = false;
                    btnChangeName.Background = Brushes.LimeGreen;
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Unlocking tbName issue.", "btnChangeName_Click", ex.Message);
            }
        }
        private void btnChangeImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Images|*.jpg;*.png|All|*.*";
                if (fileDialog.ShowDialog() == true)
                {
                    if (fileDialog.FileName.Length > 0)
                    {
                        tbImgPath.Text = fileDialog.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Changing attaching image issue.", "btnChangeImg_Click", ex.Message);
            }
        }

        private void btnChangeAu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Audio|*.mp3;*.wav|All|*.*";
                if (fileDialog.ShowDialog() == true)
                {
                    if (fileDialog.FileName.Length > 0)
                    {
                        tbImgPath.Text = fileDialog.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Changing attaching audio issue.", "btnChangeAu_Click", ex.Message);
            }

        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbName.IsReadOnly = true;
                btnChangeName.Background = Brushes.Crimson;
                List<Element> tempels = ListFromTbl();
                tempels[cmbElements.SelectedIndex].Name = tbName.Text;
                tempels[cmbElements.SelectedIndex].ImgPath = Environment.CurrentDirectory + "\\" + tbImgPath.Text;
                tempels[cmbElements.SelectedIndex].AuPath = Environment.CurrentDirectory + "\\" + tbAuPath.Text;
                File.WriteAllLines(dfp, TblFromList(ElemsToStrs(tempels)));
            }
            catch (Exception ex)
            {
                WriteToLog("Saving changes error. (Index:" + cmbElements.SelectedIndex + " )", "btnApply_Click", ex.Message);
            }
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DownloadTable();
                UploadTable();
            }
            catch (Exception ex)
            {
                WriteToLog("Issue during syncronization.", "btnSync_Click", ex.Message);
            }
        }

        private void cmbElements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                tbName.IsReadOnly = true;
                btnChangeName.Background = Brushes.Crimson;

                tbName.Text = ListFromTbl()[cmbElements.SelectedIndex].Name;
                tbImgPath.Text = Environment.CurrentDirectory + "\\" + ListFromTbl()[cmbElements.SelectedIndex].ImgPath;
                if (ListFromTbl()[cmbElements.SelectedIndex].AuPath != "NOIMAGE")
                    imgElement.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\" + ListFromTbl()[cmbElements.SelectedIndex].ImgPath));
                if (ListFromTbl()[cmbElements.SelectedIndex].AuPath != "NOAUDIO")
                    tbAuPath.Text = Environment.CurrentDirectory + "\\" + ListFromTbl()[cmbElements.SelectedIndex].AuPath;
            }
            catch (Exception ex)
            {
                WriteToLog("Issue during list element revealing. (Index:" + cmbElements.SelectedIndex + " )", "cmbElements_SelectionChanged", ex.Message);
            }
        }
    }
}
