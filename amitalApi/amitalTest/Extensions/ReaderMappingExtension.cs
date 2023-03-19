using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Microsoft.Data.SqlClient;

namespace amitalTest.Extensions
{
    public static class ReaderMappingExtension
    {
        /// <Summary>
        /// Map data from DataReader to list of objects
        /// </Summary>
        /// <typeparam name="T">Object</typeparam>
        /// <param name="dr">Data Reader</param>
        /// <returns>List of objects having data from data reader</returns>
        public static List<T> MapToList<T>(this SqlDataReader dr) where T : new()
        {
            List<T> RetVal = null;

            try
            {
                if (dr != null && dr.HasRows)
                {
                    RetVal = new List<T>();

                    var Entity = typeof(T);
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    var PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (dr.Read())
                    {
                        T newObject = new T();
                        for (int Index = 0; Index < dr.FieldCount; Index++)
                        {
                            if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                            {
                                var Info = PropDict[dr.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = dr.GetValue(Index);
                                    Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                }
                            }
                        }
                        RetVal.Add(newObject);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RetVal;
        }
    }
}
