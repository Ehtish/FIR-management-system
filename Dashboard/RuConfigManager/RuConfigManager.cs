using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.ComponentModel;

namespace RuFramework.RuConfig
{
    class RuConfigManager
    {
        #region Init

        // Privates
        private static string pstrFilename;
        private static DataSet dsDataSet;
        private static DataView dvDataView;
        private static string tableName;
        public enum AppDataPath : int { Local = 1, Roaming = 2, Common = 3, ExePath = 4 };

        // Constructor
        /*
        public static bool Open(string UserPath)
        {
            try
            {
                // D:\Users\MyName\AppData\Roaming\AppName\AppName\Version\AppName.config
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                if (r.IsMatch(UserPath))
                {
                    pstrFilename = UserPath;
                    if (initDataSource()) return true;
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
         */
        public static bool Open(AppDataPath appDataPath)
        {
            try
            {
                // C:\users\UserName\AppData\Local\ProductName\ProductName\ProductVersion\ProductName.Config
                if (appDataPath == AppDataPath.Roaming) pstrFilename = Application.UserAppDataPath + "\\" + Application.ProductName + ".config";
                else
                    // C:\users\UserName\AppData\Roaming\ProductName\ProductName\ProductVersion\ProductName.Config
                    if (appDataPath == AppDataPath.Local) pstrFilename = Application.LocalUserAppDataPath + "\\" + Application.ProductName + ".config";
                    else
                        // C:\ProgramData\ProductName\ProductName\ProductVersion\ProductName.Config
                        if (appDataPath == AppDataPath.Common) pstrFilename = Application.CommonAppDataPath + "\\" + Application.ProductName + ".config";
                        else
                            // ProductSaveDirectory\ProductName.Config
                            if (appDataPath == AppDataPath.ExePath) pstrFilename = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Application.ProductName + ".config";

                if (initDataSource()) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        private static bool initDataSource()
        {
            dsDataSet = new DataSet();

            /*
            <Application>
                <Configuration>
                </Configuration>
            </Application>
            */
            dsDataSet.DataSetName = "Application";
            tableName = "Configuration";
            dsDataSet.Tables.Add(createDataTable());

            try
            {
                if (File.Exists(pstrFilename))
                {
                    dsDataSet.ReadXml(pstrFilename);
                }
                saveData();
                dvDataView = dsDataSet.Tables[0].DefaultView;
                dvDataView.AllowEdit = true;
                dvDataView.AllowDelete = true;
                dvDataView.AllowNew = true;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        #endregion
        #region public
        public static bool Exist(string KeyName)
        {
            bool strResult = false;
            dvDataView.RowFilter = "key='" + KeyName + "'";

            if (dvDataView.Count != 0) strResult = true;

            return strResult;
        }
        public static bool RemoveKey(string KeyName)
        {
            DataRowView Row;
            dvDataView.RowFilter = "key='" + KeyName + "'";

            if (dvDataView.Count != 0)
            {
                Row = dvDataView[0];
                Row.Delete();
                saveData();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static dynamic Get(string KeyName, dynamic Default = null)
        {
            dynamic result = Default;
            try
            {
                // If not present, set new
                if (!Exist(KeyName))
                {
                    Set(KeyName, result);
                    return result;
                }

                DataRowView Row;
                dvDataView.RowFilter = "key='" + KeyName + "'";

                if (dvDataView.Count != 0)
                {
                    Row = dvDataView[0];
                    result = Row["value"];
                    if (String.IsNullOrEmpty(result))
                    {
                        result = Default;
                    }
                    TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(Default);

                    if (converter != null)
                    {
                        if (converter.CanConvertFrom(typeof(System.String)))
                        {
                            result = converter.ConvertFrom(result);
                        }
                    }
                    else
                    {
                        result = result.ToString();
                    }
                }
            }
            catch
            {
                return Default;
            }
            return result;
        }
        public static bool Set(string KeyName, dynamic value)
        {
            try
            {
                string StringValue = null;
                TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(value);
                if (converter != null)
                {
                    if (converter.CanConvertTo(typeof(System.String)))
                    {
                        StringValue = converter.ConvertTo(value, typeof(System.String));
                    }
                }
                else
                {
                    StringValue = value.ToString();
                }

                if (Exist(KeyName)) RemoveKey(KeyName);
                // add new Row
                DataRowView Row = dvDataView.AddNew();

                // add values
                Row["key"] = KeyName;
                Row["value"] = StringValue;

                // Save
                Row.EndEdit();
                saveData();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception raised: {0}", e.Message);
                return false;
            }
        }
        #endregion
        #region privat
        private static DataTable createDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.TableName = tableName;
            tbl.Columns.Add("key", typeof(string));
            tbl.Columns.Add("value", typeof(string));
            return (tbl);
        }
        public static void saveData()
        {
            dsDataSet.WriteXml(pstrFilename);
        }
        #endregion
    }
}
