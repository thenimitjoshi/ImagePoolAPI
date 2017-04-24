using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ImagePool.Models
{
    public class TodaysLineUpDal
    {
        #region Variable
        /// <summary>
        /// variable for Database
        /// </summary>
        Database objDB;
        #endregion

        #region Database Methods
        /// <summary>
        /// This method is created for convert list to data table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datatable"></param>
        /// <returns></returns>
        public List<T> ConvertTo<T>(DataTable datatable) where T : new()
        {
            List<T> Temp = new List<T>();
            try
            {
                List<string> columnsNames = new List<string>();
                foreach (DataColumn DataColumn in datatable.Columns)
                {
                    columnsNames.Add(DataColumn.ColumnName);
                }

                Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                return Temp;
            }
            catch
            {
                return Temp;
            }
        }
        /// <summary>
        /// This method is created to get the list and convert to data row.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columnsName"></param>
        /// <returns></returns>
        public T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties;
                Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                }
                return obj;
            }
            catch
            {
                return obj;
            }
        }
        #endregion

        /// <summary>
        /// This method is created to get today's lineup list.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns>It will return all leagues and games details in todays lineup list.</returns>
        public Dictionary<string, List<TodaysLineUp>> GetTodaysLineUpList(string offset)
        {
            Dictionary<string, List<TodaysLineUp>> TodaysLineUpDictionary = new Dictionary<string, List<TodaysLineUp>>();
            List<TodaysLineUp> objTodaysLineUpList1 = null;
            List<TodaysLineUp> objTodaysLineUpList2 = null;
            objDB = new SqlDatabase(ConfigurationManager.ConnectionStrings["SqlServerConnectionString"].ConnectionString);

            using (DbCommand objCMD = objDB.GetStoredProcCommand("SQ_GetTodaysLineup"))
            {
                try
                {

                    objDB.AddInParameter(objCMD, "@Offset", DbType.String, string.IsNullOrEmpty(offset) ? (object)DBNull.Value : offset);

                    using (DataTable dataTable = objDB.ExecuteDataSet(objCMD).Tables[0])
                    {
                        objTodaysLineUpList1 = ConvertTo<TodaysLineUp>(dataTable);
                    }
                    using (DataTable dataTable = objDB.ExecuteDataSet(objCMD).Tables[1])
                    {
                        objTodaysLineUpList2 = ConvertTo<TodaysLineUp>(dataTable);
                    }
                    if (objTodaysLineUpList1 != null && objTodaysLineUpList1.Count > 0)
                    {
                        TodaysLineUpDictionary.Add("Live", objTodaysLineUpList1);
                    }
                    if (objTodaysLineUpList2 != null && objTodaysLineUpList2.Count > 0)
                    {
                        TodaysLineUpDictionary.Add("Later", objTodaysLineUpList2);
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return TodaysLineUpDictionary;
        }
    }
}