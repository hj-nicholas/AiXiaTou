using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Json;

namespace UL.AXT
{
    public class JSONHelp
    {
        private static JSONHelp json = null;

        private JSONHelp()
        {
        }

        public static JSONHelp Instance()
        {
            if (json == null)
            {
                json = new JSONHelp();
            }

            return json;
        }

        /// <summary>
        /// 从类对象转化为Json字符串
        /// </summary>
        /// <typeparam name="T">转化的类</typeparam>
        /// <param name="jsonObject">转化的类对象</param>
        /// <returns></returns>
        public string ToJson<T>(T jsonObject)
        {
            string result = String.Empty;
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    serializer.WriteObject(ms, jsonObject);
                    result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception ex) { throw ex; } return result;


        }

        /// <summary>
        /// 普通集合转换Json
        /// </summary>
        /// <param name="array">集合对象</param>
        /// <returns>Json字符串</returns>

        public string ListToJson(IEnumerable array)
        {

            string jsonString = "[";

            foreach (object item in array)
            {
                jsonString += ToJson(item) + ",";
            }
            int t = jsonString.LastIndexOf(',');
            string strTmp = jsonString.Substring(0, t);
            return strTmp + "]";

        }

        /// <summary>
        /// 从Json字符串转化为类对象
        /// </summary>
        /// <typeparam name="T">要转化的类</typeparam>
        /// <param name="json">Json字符串</param>
        /// <returns>转化的类对象</returns>
        public T FromJson<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            T obj = default(T);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
            }
            return obj;
        }

        /// <summary>   
        /// DataTable to json   
        /// </summary>   
        /// <param name="jsonName">返回json的名称</param>   
        /// <param name="dt">转换成json的表</param>   
        /// <returns></returns>   
        public string DataTableToJson(string jsonName, System.Data.DataTable dt, string strTotal = "")
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("[{\"TotalCount\":\"" + strTotal + "\",\"Head\":[");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Json.Append("{\"ColumnHead\":\"" + dt + dt.Columns[i].ColumnName + "\"}");

                if (i < dt.Columns.Count - 1)
                {
                    Json.Append(",");
                }
            }
            Json.Append("],");

            Json.Append("\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}]");
            return Json.ToString();
        }

    }
}
