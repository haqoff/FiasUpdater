using SQLXMLBULKLOADLib;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FIASUpdater
{
    public partial class MainForm : Form
    {
        private FIASClassesDataContext mainDB;
        private FIASClassesDataContext tempDB;

        private TimeSpan timeLeft;

        private List<string> filesNamesMasks;
        private Dictionary<string, string> schemes;
        private Dictionary<string, string> versionFiles;
        private DateTime currentFiasVersion;
        private DateTime newVer;
        private string temp_connStringPart;

        public object BundingFlags { get; private set; }

        public MainForm(FIASClassesDataContext mainDB, FIASClassesDataContext tempDB, string temp_connStringPart)
        {
            this.mainDB = mainDB;
            this.tempDB = tempDB;
            this.temp_connStringPart = temp_connStringPart;

            timeLeft = new TimeSpan();

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            var maxValue = mainDB.UPDATES.Max(x => x.Version);
            currentFiasVersion = mainDB.UPDATES.First(x => x.Version == maxValue).Version;

            lblCurrentVersion.Text = "Текущая версия FIAS: " + currentFiasVersion.ToShortDateString();
        }

        private void SetProgressInfo(string info)
        {
            lblReadyToUpdate.Invoke(new Action(() => lblReadyToUpdate.Text = info)); 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var th = new Thread(LoadAndUpdate);
            th.Start();
        }

        private void LoadAndUpdate()
        {
            btnUpdate.Invoke(new Action(() => btnUpdate.Enabled = false));
            btnCheckAllUpdates.Invoke(new Action(() => btnCheckAllUpdates.Enabled = false));

            SetProgressInfo("Создание временной пустой БД.");
            if (tempDB.DatabaseExists()) tempDB.DeleteDatabase();
            tempDB.CreateDatabase();

            try
            {
                timeLeft = TimeSpan.Zero;

                SetProgressInfo("Загрузка XML-файлов во временную БД.");
                LoadXmlToTempDb();
                UpdateMainDbFromTempDb();

                mainDB.UPDATES.InsertOnSubmit(new UPDATES()
                {
                    Version = newVer,
                    FactUpdateDate = DateTime.Now
                });

                mainDB.SubmitChanges();

                SetProgressInfo("Удаление временной БД для загрузки.");
                tempDB.DeleteDatabase();
                tempDB.SubmitChanges();

                MessageBox.Show(String.Format("FIAS успешно обновлён с версии '{0}' до '{1}'.", currentFiasVersion.ToShortDateString(), newVer.ToShortDateString()));
                currentFiasVersion = newVer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (tempDB.DatabaseExists())
                {
                    tempDB.DeleteDatabase();
                    tempDB.SubmitChanges();
                }

                lblCurrentVersion.Invoke(new Action(() => lblCurrentVersion.Text = "Текущая версия FIAS: " + currentFiasVersion.ToShortDateString()));
                tbVersionDate.Invoke(new Action(() => tbVersionDate.Clear()));
                btnUpdate.Invoke(new Action(() => btnUpdate.Enabled = true));
                btnCheckAllUpdates.Invoke(new Action(() => btnCheckAllUpdates.Visible = true));
                lblReadyToUpdate.Invoke(new Action(() => lblReadyToUpdate.Visible = false));
                tbNewVersionPath.Invoke(new Action(() => tbNewVersionPath.Clear()));
            }
        }

        private void UpdateMainDbFromTempDb()
        {
            //ActualStatus
            SetProgressInfo("Обновление таблицы ACTUALSTATUS");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[ActualStatus]", "ACTSTATID", new[]
            {
                "[NAME]"
            });

            //AddressObjectType
            SetProgressInfo("Обновление таблицы AddressObjectType");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[AddressObjectType]", "KOD_T_ST", new[]
            {
                "[LEVEL]",
                "[SCNAME]",
                "[SOCRNAME]"
            });

            //-----

            //CenterStatus
            SetProgressInfo("Обновление таблицы CenterStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[CenterStatus]", "[CENTERSTID]", new[]
                {
                    "[NAME]"
                });

            //CurrentStatus
            SetProgressInfo("Обновление таблицы CurrentStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[CurrentStatus]", "[CURENTSTID]", new[]
                {
                    "[NAME]"
                });

            //EstateStatus
            SetProgressInfo("Обновление таблицы EstateStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[EstateStatus]", "[ESTSTATID]", new[]
                {
                    "[NAME]",
                    "[SHORTNAME]"
                });

            //FlatType
            SetProgressInfo("Обновление таблицы FlatType");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[FlatType]", "[FLTYPEID]", new[]
                {
                    "[NAME]",
                    "[SHORTNAME]"
                });

            //House
            SetProgressInfo("Обновление таблицы House");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[House]", "[HOUSEID]", new[]
               {
                "[AOGUID]",
                "[BUILDNUM]",
                "[CADNUM]",
                "[COUNTER]",
                "[DIVTYPE]",
                "[ENDDATE]",
                "[ESTSTATUS]",
                "[HOUSEGUID]",
                "[HOUSENUM]",
                "[IFNSFL]",
                "[IFNSUL]",
                "[NORMDOC]",
                "[OKATO]",
                "[OKTMO]",
                "[POSTALCODE]",
                "[REGIONCODE]",
                "[STARTDATE]",
                "[STATSTATUS]",
                "[STRSTATUS]",
                "[STRUCNUM]",
                "[TERRIFNSFL]",
                "[TERRIFNSUL]",
                "[UPDATEDATE]"
            });

            //HouseStateStatus
            SetProgressInfo("Обновление таблицы HouseStateStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[HouseStateStatus]", "[HOUSESTID]", new[]
                {
                    "[NAME]"
                });

            //IntervalStatus
            SetProgressInfo("Обновление таблицы IntervalStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[IntervalStatus]", "[INTVSTATID]", new[]
                {
                    "[NAME]"
                });

            //NormativeDocument
            SetProgressInfo("Обновление таблицы NormativeDocument");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[NormativeDocument]", "[NORMDOCID]", new[]
                {
                    "[DOCNAME]",
                    "[DOCDATE]",
                    "[DOCNUM]",
                    "[DOCTYPE]",
                    "[DOCIMGID]"
                });

            //NormativeDocumentType
            SetProgressInfo("Обновление таблицы NormativeDocumentType");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[NormativeDocumentType]", "[NDTYPEID]", new[]
                {
                    "[NAME]"
                });

            //Object
            SetProgressInfo("Обновление таблицы Object");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[Object]", "[AOID]", new[]
                {
                    "[ACTSTATUS]",
                    "[AOGUID]",
                    "[AOLEVEL]",
                    "[AREACODE]",
                    "[AUTOCODE]",
                    "[CENTSTATUS]",
                    "[CITYCODE]",
                    "[CODE]",
                    "[CTARCODE]",
                    "[CURRSTATUS]",
                    "[DIVTYPE]",
                    "[ENDDATE]",
                    "[EXTRCODE]",
                    "[FORMALNAME]",
                    "[IFNSFL]",
                    "[IFNSUL]",
                    "[LIVESTATUS]",
                    "[NEXTID]",
                    "[NORMDOC]",
                    "[OFFNAME]",
                    "[OKATO]",
                    "[OKTMO]",
                    "[OPERSTATUS]",
                    "[PARENTGUID]",
                    "[PLACECODE]",
                    "[PLAINCODE]",
                    "[PLANCODE]",
                    "[POSTALCODE]",
                    "[PREVID]",
                    "[REGIONCODE]",
                    "[SEXTCODE]",
                    "[SHORTNAME]",
                    "[STARTDATE]",
                    "[STREETCODE]",
                    "[TERRIFNSFL]",
                    "[TERRIFNSUL]",
                    "[UPDATEDATE]"
                });

            //OperationStatus
            SetProgressInfo("Обновление таблицы OperationStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[OperationStatus]", "[OPERSTATID]", new[]
            {
                "[NAME]"
            });

            //Room
            SetProgressInfo("Обновление таблицы Room");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[Room]", "[ROOMID]", new[]
            {
                "[ROOMGUID]",
                "[FLATNUMBER]",
                "[FLATTYPE]",
                "[ROOMNUMBER]",
                "[ROOMTYPE]",
                "[REGIONCODE]",
                "[POSTALCODE]",
                "[UPDATEDATE]",
                "[HOUSEGUID]",
                "[PREVID]",
                "[NEXTID]",
                "[STARTDATE]",
                "[ENDDATE]",
                "[LIVESTATUS]",
                "[NORMDOC]",
                "[OPERSTATUS]",
                "[CADNUM]",
                "[ROOMCADNUM]"
            });

            //RoomType
            SetProgressInfo("Обновление таблицы RoomType");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[RoomType]", "[RMTYPEID]", new[]
            {
                "[NAME]",
                "[SHORTNAME]"
            });

            //Stead
            SetProgressInfo("Обновление таблицы Stead");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[Stead]", "[STEADID]", new[]
            {
                "[STEADGUID]",
                "[NUMBER]",
                "[REGIONCODE]",
                "[POSTALCODE]",
                "[IFNSFL]",
                "[TERRIFNSFL]",
                "[TERRIFNSUL]",
                "[OKATO]",
                "[OKTMO]",
                "[UPDATEDATE]",
                "[PARENTGUID]",
                "[PREVID]",
                "[NEXTID]",
                "[OPERSTATUS]",
                "[STARTDATE]",
                "[ENDDATE]",
                "[NORMDOC]",
                "[LIVESTATUS]",
                "[CADNUM]",
                "[DIVTYPE]"
            });

            //StructureStatus
            SetProgressInfo("Обновление таблицы StructureStatus");
            Utils.BuildUpdateSqlCommand(mainDB.Connection, tempDB.Connection, "[StructureStatus]", "[STRSTATID]", new[]
            {
                "[NAME]",
                "[SHORTNAME]"
            });
            SetProgressInfo("Готово к загрузке.");
        }


        [STAThread]
        private void LoadXmlToTempDb()
        {
                var objBL = new SQLXMLBulkLoad4
                {
                    ConnectionString = temp_connStringPart,
                    ErrorLogFile = "error.xml",
                    SchemaGen = false,
                    SGDropTables = false,
                };


                foreach (var key in filesNamesMasks)
                {
                    var schemePath = schemes[key];
                    var xmlPath = versionFiles[key];

                    objBL.Execute(schemePath, xmlPath);
                }
        }

        private void btnLoadScheme_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

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
        /// <param name="folderPath"></param>
        /// <param name="maskPostfix"></param>
        /// <returns>Возвращает true, если словарь был обновлён, иначе false.</returns>
        private bool UpdateFileDictionary(out Dictionary<string, string> dict, string folderPath, string maskPostfix)
        {
            dict = new Dictionary<string, string>();
            foreach (var mask in filesNamesMasks)
            {
                var files = new List<string>();
                files.AddRange(Directory.GetFiles(folderPath, mask + maskPostfix));

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

            return date > currentFiasVersion && date - currentFiasVersion < maxNew;
        }


        private void btnLoadNewVersion_Click(object sender, System.EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    tbNewVersionPath.Clear();
                    tbVersionDate.Clear();
                    btnUpdate.Enabled = false;

                    if (!UpdateFileDictionary(out versionFiles, fbd.SelectedPath, "*.xml")) return;

                    var m = Regex.Match(versionFiles[filesNamesMasks[0]], "(?<=_)[0-9]+(?=_)");
                    try
                    {
                        newVer = DateTime.ParseExact(m.Value, "yyyyMMdd", CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        MessageBox.Show("Не удалось найти дату версии файлов. Отменено.");
                        return;
                    }

                    if (CanBeUpdated(newVer)) lblReadyToUpdate.Visible = true;
                    else
                    {
                        if (currentFiasVersion.CompareTo(newVer) >= 0)
                            MessageBox.Show(
                                "База данных ФИАСа имеет такую же или более новую версию, чем ту, которую Вы пытаетесь загрузить.");
                        else
                            MessageBox.Show(
                                "Версия, которую вы пытаетесь загрузить слишком новая для текущий базы данных. Возможно вы пропустили некоторые обновления.");
                        return;
                    }

                    tbVersionDate.Text = newVer.ToShortDateString();
                    tbNewVersionPath.Text = fbd.SelectedPath;
                    if (!string.IsNullOrEmpty(tbSchemePath.Text) && !string.IsNullOrEmpty(tbNewVersionPath.Text)) btnUpdate.Enabled = true;
                }
            }
        }

        private void btnCheckAllUpdates_Click(object sender, EventArgs e)
        {
            var updForm = new UpdatesForm(mainDB);

            updForm.ShowDialog();
        }
    }
}
