using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FIASUpdater
{
    public partial class MainForm : Form
    {
        private FIASClassesDataContext mainDB;
        private FIASClassesDataContext tempDB;

        private List<string> filesNamesMasks;
        private Dictionary<string, string> schemes;
        private Dictionary<string, string> versionFiles;
        private DateTime currentFiasVersion;


        public MainForm(FIASClassesDataContext mainDB, FIASClassesDataContext tempDB)
        {
            this.mainDB = mainDB;
            this.tempDB = tempDB;

            versionFiles = new Dictionary<string, string>();

            BindMasks();

            InitializeComponent();
        }

        private void BindMasks()
        {
            filesNamesMasks = new List<string>
            {
                "AS_ACTSTAT",
                "AS_ADDROBJ",
                "AS_CENTERST",
                "AS_CURENTST",
                "AS_ESTSTAT",
                "AS_FLATTYPE",
                "AS_HOUSE",
                "AS_HSTSTAT",
                "AS_INTVSTAT",
                "AS_NDOCTYPE",
                "AS_NORMDOC",
                "AS_OPERSTAT",
                "AS_ROOM", //кофнлиет
                "AS_ROOMTYPE", //
                "AS_SOCRBASE",
                "AS_STEAD",
                "AS_STRSTAT"
            };

            filesNamesMasks = filesNamesMasks.OrderByDescending(s => s.Length).ToList();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            var maxValue = mainDB.UPDATES.Max(x => x.Version);
            currentFiasVersion = mainDB.UPDATES.First(x => x.Version == maxValue).Version;

            lblCurrentVersion.Text = "Текущая версия FIAS: " + currentFiasVersion.ToShortDateString();
        }

        private void btnUpdate_Click(object sender, System.EventArgs e)
        {

        }

        private void btnLoadScheme_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbSchemePath.Clear();
                    tbNewVersionPath.Clear();
                    btnLoadNewVersion.Enabled = false;
                    tbVersionDate.Clear();
                    btnUpdate.Enabled = false;

                    if (!UpdateFileDictionary(out schemes, fbd.SelectedPath, "*.xsd")) return;
                   
                    tbSchemePath.Text = fbd.SelectedPath;
                    btnLoadNewVersion.Enabled = true;
                    if (!string.IsNullOrEmpty(tbSchemePath.Text) && !string.IsNullOrEmpty(tbNewVersionPath.Text)) btnUpdate.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Обновляет словарь файлов.
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="maskPostfix"></param>
        /// <returns>Возвращает true, если словарь был обновлён, иначе false.</returns>
        private bool UpdateFileDictionary(out Dictionary<string,string> dict, string folderPath, string maskPostfix)
        {
            dict = new Dictionary<string, string>();
            foreach (var mask in filesNamesMasks)
            {
                var files = new List<string>();
                files.AddRange(Directory.GetFiles(folderPath, mask +maskPostfix));

                if (files.Count > 1)
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        foreach (var key in dict.Keys)
                        {
                            if (Regex.IsMatch(file, key))
                            {
                                files.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                    }
                }

                if (files.Count != 1)
                {
                    MessageBox.Show("Файл " + mask + " не найден.");
                    return false;
                }

                dict.Add(mask, files[0]);
            }
            return true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                mainDB.Connection.Close();
                tempDB.Connection.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось закрыть подключение к БД");
            }
        }


        private bool CanBeUpdated(DateTime date)
        {
            var maxNew = new TimeSpan(10, 0, 0, 0, 0);

            if (date > currentFiasVersion && date - currentFiasVersion < maxNew) return true;
            return false;
        }


        private void btnLoadNewVersion_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbNewVersionPath.Clear();
                    tbVersionDate.Clear();
                    btnUpdate.Enabled = false;

                    if (!UpdateFileDictionary(out versionFiles, fbd.SelectedPath, "*.xml")) return;

                    var m = Regex.Match(versionFiles[filesNamesMasks[0]], "(?<=_)[0-9]+(?=_)");
                    DateTime newVer;
                    try
                    {
                        newVer = DateTime.ParseExact(m.Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                        tbVersionDate.Text = newVer.ToShortDateString();
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось найти дату версии файлов. Отменено.");
                        return;
                    }

                    if (CanBeUpdated(newVer)) lblReadyToUpdate.Visible = true;
                    else
                    {
                        if(currentFiasVersion.CompareTo(newVer)>0)
                            MessageBox.Show("База данных ФИАСа имеет более новую версию, чем ту, которую Вы пытаетесь загрузить.");
                        else
                            MessageBox.Show("Версия, которую вы пытаетесь загрузить слишком новая для текущий базы данных. Возможно вы пропустили некоторые обновления.");
                        return;
                    }

                    
                    if(!string.IsNullOrEmpty(tbSchemePath.Text) && !string.IsNullOrEmpty(tbNewVersionPath.Text)) btnUpdate.Enabled = true;
                    tbNewVersionPath.Text = fbd.SelectedPath;
                }
            }
        }
    }
}
