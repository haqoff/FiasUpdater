using SQLXMLBULKLOADLib;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
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
        private DateTime newVer;
        private string temp_connStringPart;

        public object BundingFlags { get; private set; }

        public MainForm(FIASClassesDataContext mainDB, FIASClassesDataContext tempDB, string temp_connStringPart)
        {
            this.mainDB = mainDB;
            this.tempDB = tempDB;
            this.temp_connStringPart = temp_connStringPart;

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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate.Enabled = false;
            if (tempDB.DatabaseExists()) tempDB.DeleteDatabase();
            tempDB.CreateDatabase();

            try
            {
                LoadXMLToTempDB();
                UpdateMainDBFromTempDB();

                mainDB.UPDATES.InsertOnSubmit(new UPDATES()
                {
                    Version = newVer,
                    FactUpdateDate = DateTime.Now
                });

                mainDB.SubmitChanges();

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
                lblCurrentVersion.Text = "Текущая версия FIAS: " + currentFiasVersion.ToShortDateString();
                tbVersionDate.Clear();
                lblReadyToUpdate.Visible = false;
                tbNewVersionPath.Clear();
            }
        }

        private void UpdateMainDBFromTempDB()
        {
            //ActualStatus
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[ActualStatus]", "ACTSTATID", new string[]
            {
                "[NAME]"
            });

            //AddressObjectType
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[AddressObjectType]", "KOD_T_ST", new string[]
            {
                "[LEVEL]",
                "[SCNAME]",
                "[SOCRNAME]"
            });
            ///---------------------------------------

            //CenterStatus
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[CenterStatus]", "[CENTERSTID]", new string[]
                {
                    "[NAME]"
                });

            //CurrentStatus
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[CurrentStatus]", "[CURRENTSTID]", new string[]
                {
                    "[NAME]"
                });

            //EstateStatus
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[EstateStatus]", "[ESTSTATID]", new string[]
                {
                    "[NAME]",
                    "[SHORTNAME]"
                });

            //FlatType
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[FlatType]", "[FLTYPEID]", new string[]
                {
                    "[NAME]",
                    "[SHORTNAME]"
                });

            //House
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[House]", "[HOUSEID]", new string[]
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
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[HouseStateStatus]", "[HOUSESTID]", new string[]
                {
                    "[NAME]"
                });

            //IntervalStatus
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[IntervalStatus]", "[INTVSTATID]", new string[]
                {
                    "[NAME]"
                });

            //NormativeDocument
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[NormativeDocument]", "[NORMDOCID]", new string[]
                {
                    "[DOCNAME]",
                    "[DOCDATE]",
                    "[DOCNUM]",
                    "[DOCTYPE]",
                    "[DOCIMGID]"
                });

            //NormativeDocumentType
            Utils.BuildUpdateSqlCommand(mainDB, tempDB, "[NormativeDocumentType]", "[NDTYPEID]", new string[]
                {
                    "[NAME]"
                });

            /////
            /////Object
            /////
            //foreach (var newItem in tempDB.Object)
            //{
            //    Object existItem = mainDB.Object.Where(item => item.AOID == newItem.AOID).FirstOrDefault();

            //    if (existItem != null)
            //    {
            //        existItem.ACTSTATUS = newItem.ACTSTATUS;
            //        existItem.AOGUID = newItem.AOGUID;
            //        existItem.AOLEVEL = newItem.AOLEVEL;
            //        existItem.AREACODE = newItem.AREACODE;
            //        existItem.AUTOCODE = newItem.AUTOCODE;
            //        existItem.CENTSTATUS = newItem.CENTSTATUS;
            //        existItem.CITYCODE = newItem.CITYCODE;
            //        existItem.CODE = newItem.CODE;
            //        existItem.CTARCODE = newItem.CTARCODE;
            //        existItem.CURRSTATUS = newItem.CURRSTATUS;
            //        existItem.DIVTYPE = newItem.DIVTYPE;
            //        existItem.ENDDATE = newItem.ENDDATE;
            //        existItem.EXTRCODE = newItem.EXTRCODE;
            //        existItem.FORMALNAME = newItem.FORMALNAME;
            //        existItem.IFNSFL = newItem.IFNSFL;
            //        existItem.IFNSUL = newItem.IFNSUL;
            //        existItem.LIVESTATUS = newItem.LIVESTATUS;
            //        existItem.NEXTID = newItem.NEXTID;
            //        existItem.NORMDOC = newItem.NORMDOC;
            //        existItem.OFFNAME = newItem.OFFNAME;
            //        existItem.OKATO = newItem.OKATO;
            //        existItem.OKTMO = newItem.OKTMO;
            //        existItem.OPERSTATUS = newItem.OPERSTATUS;
            //        existItem.PARENTGUID = newItem.PARENTGUID;
            //        existItem.PLACECODE = newItem.PLACECODE;
            //        existItem.PLAINCODE = newItem.PLAINCODE;
            //        existItem.PLANCODE = newItem.PLANCODE;
            //        existItem.POSTALCODE = newItem.POSTALCODE;
            //        existItem.PREVID = newItem.PREVID;
            //        existItem.REGIONCODE = newItem.REGIONCODE;
            //        existItem.SEXTCODE = newItem.SEXTCODE;
            //        existItem.SHORTNAME = newItem.SHORTNAME;
            //        existItem.STARTDATE = newItem.STARTDATE;
            //        existItem.STREETCODE = newItem.STREETCODE;
            //        existItem.TERRIFNSFL = newItem.TERRIFNSFL;
            //        existItem.TERRIFNSUL = newItem.TERRIFNSUL;
            //        existItem.UPDATEDATE = newItem.UPDATEDATE;
            //    }
            //    else
            //    {
            //        var aoguids = mainDB.Object.Where(item => item.AOGUID == newItem.AOGUID).ToList();

            //        foreach (var aoguid in aoguids)
            //        {
            //            aoguid.ACTSTATUS = 0;
            //        }

            //        mainDB.Object.InsertOnSubmit(newItem);
            //    }
            //}
            //ctry = 3;
            //for (int i = 0; i < ctry; i++)
            //{
            //    try
            //    {
            //        mainDB.SubmitChanges();
            //        break;
            //    }
            //    catch
            //    {

            //    }
            //}

            /////
            /////OperationStatus
            /////
            //foreach (var newItem in tempDB.OperationStatus)
            //{
            //    OperationStatus existItem = mainDB.OperationStatus.Where(item => item.OPERSTATID == newItem.OPERSTATID).FirstOrDefault();

            //    if (existItem != null)
            //    {
            //        existItem.NAME = newItem.NAME;
            //    }
            //    else mainDB.OperationStatus.InsertOnSubmit(newItem);
            //    mainDB.SubmitChanges();
            //}

            /////
            /////Room
            /////
            //foreach (var newItem in tempDB.Room)
            //{
            //    Room existItem = mainDB.Room.Where(item => item.ROOMID == newItem.ROOMID).FirstOrDefault();

            //    if (existItem != null)
            //    {
            //        existItem.CADNUM = newItem.CADNUM;
            //        existItem.ENDDATE = newItem.ENDDATE;
            //        existItem.FLATNUMBER = newItem.FLATNUMBER;
            //        existItem.FLATTYPE = newItem.FLATTYPE;
            //        existItem.HOUSEGUID = newItem.HOUSEGUID;
            //        existItem.LIVESTATUS = newItem.LIVESTATUS;
            //        existItem.NEXTID = newItem.NEXTID;
            //        existItem.NORMDOC = newItem.NORMDOC;
            //        existItem.OPERSTATUS = newItem.OPERSTATUS;
            //        existItem.POSTALCODE = newItem.POSTALCODE;
            //        existItem.PREVID = newItem.PREVID;
            //        existItem.REGIONCODE = newItem.REGIONCODE;
            //        existItem.ROOMCADNUM = newItem.ROOMCADNUM;
            //        existItem.ROOMGUID = newItem.ROOMGUID;
            //        existItem.ROOMNUMBER = newItem.ROOMNUMBER;
            //        existItem.ROOMTYPE = newItem.ROOMTYPE;
            //        existItem.STARTDATE = newItem.STARTDATE;
            //        existItem.UPDATEDATE = newItem.UPDATEDATE;
            //    }
            //    else
            //    {
            //        mainDB.Room.InsertOnSubmit(newItem);
            //    }
            //}
            //mainDB.SubmitChanges();

            /////
            /////RoomType
            /////
            //foreach (var newItem in tempDB.RoomType)
            //{
            //    RoomType existItem = mainDB.RoomType.Where(item => item.RMTYPEID == newItem.RMTYPEID).FirstOrDefault(); ;

            //    if (existItem != null)
            //    {
            //        existItem.NAME = newItem.NAME;
            //        existItem.SHORTNAME = newItem.SHORTNAME;
            //    }
            //    else
            //    {
            //        mainDB.RoomType.InsertOnSubmit(newItem);
            //    }
            //    mainDB.SubmitChanges();
            //}

            /////
            /////Stead
            /////
            //foreach (var newItem in tempDB.Stead)
            //{
            //    Stead existItem = mainDB.Stead.Where(item => item.STEADID == newItem.STEADID).FirstOrDefault();

            //    if (existItem != null)
            //    {
            //        existItem.CADNUM = newItem.CADNUM;
            //        existItem.DIVTYPE = newItem.DIVTYPE;
            //        existItem.ENDDATE = newItem.ENDDATE;
            //        existItem.IFNSFL = newItem.IFNSFL;
            //        existItem.IFNSUL = newItem.IFNSUL;
            //        existItem.LIVESTATUS = newItem.LIVESTATUS;
            //        existItem.NEXTID = newItem.NEXTID;
            //        existItem.NORMDOC = newItem.NORMDOC;
            //        existItem.NUMBER = newItem.NUMBER;
            //        existItem.OKATO = newItem.OKATO;
            //        existItem.OKTMO = newItem.OKTMO;
            //        existItem.OPERSTATUS = newItem.OPERSTATUS;
            //        existItem.PARENTGUID = newItem.PARENTGUID;
            //        existItem.POSTALCODE = newItem.POSTALCODE;
            //        existItem.PREVID = newItem.PREVID;
            //        existItem.REGIONCODE = newItem.REGIONCODE;
            //        existItem.STARTDATE = newItem.STARTDATE;
            //        existItem.STEADGUID = newItem.STEADGUID;
            //        existItem.TERRIFNSFL = newItem.TERRIFNSFL;
            //        existItem.TERRIFNSUL = newItem.TERRIFNSUL;
            //        existItem.UPDATEDATE = newItem.UPDATEDATE;
            //    }
            //    else
            //    {
            //        mainDB.Stead.InsertOnSubmit(newItem);
            //    }
            //}
            //mainDB.SubmitChanges();

            /////
            /////StructureStatus
            /////
            //foreach (var newItem in tempDB.StructureStatus)
            //{
            //    StructureStatus existItem = mainDB.StructureStatus.Where(item => item.STRSTATID == newItem.STRSTATID).FirstOrDefault();

            //    if (existItem != null)
            //    {
            //        existItem.NAME = newItem.NAME;
            //        existItem.SHORTNAME = newItem.SHORTNAME;
            //    }
            //    else
            //    {
            //        mainDB.StructureStatus.InsertOnSubmit(newItem);
            //    }
            //    mainDB.SubmitChanges();
            //}
        }


        [STAThread]
        private void LoadXMLToTempDB()
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }

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
                if (tempDB.DatabaseExists()) tempDB.DeleteDatabase();
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
                            MessageBox.Show("База данных ФИАСа имеет такую же или более новую версию, чем ту, которую Вы пытаетесь загрузить.");
                        else
                            MessageBox.Show("Версия, которую вы пытаетесь загрузить слишком новая для текущий базы данных. Возможно вы пропустили некоторые обновления.");
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
