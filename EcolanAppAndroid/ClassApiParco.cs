using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using TinyJson;
using ZXing;

namespace EcolanAppAndroid {
    public class ClassApiParco {
        const String BaseUrl = "http://www.ecocontrolgsm.it/webservice.asmx/";


        //public List<KeyValuePair<String, Object>> Param = new List<KeyValuePair<String, Object>>();
        public ClassApiParco() {

        }

        public DataTable EseguiQuery(String Query) {
            String URLSqlQuery = BaseUrl + "EcoparcoEseguiQuery?query=" + System.Web.HttpUtility.UrlPathEncode(Query) + "&staging=false";
            var req = (HttpWebRequest)WebRequest.Create(URLSqlQuery);
            req.Method = "GET";
            req.Accept = "application/json";
            var st = req.GetResponse().GetResponseStream();
            var stRead = new System.IO.StreamReader(st);
            var rit = stRead.ReadToEnd();
            var Table = Serializza.XmlDeserialize<DataTable>(rit);
            return Table;
        }
        public Object EseguiCommand(String Query) {
            String URLSqlQuery = BaseUrl + "EcoparcoEseguiCommand?Command=" + System.Web.HttpUtility.UrlPathEncode(Query) + "&staging=false";
            var req = (HttpWebRequest)WebRequest.Create(URLSqlQuery);
            req.Method = "GET";
            req.Accept = "application/json";
            var st = req.GetResponse().GetResponseStream();
            var stRead = new System.IO.StreamReader(st);
            var rit = stRead.ReadToEnd();
            var RitObj = Serializza.XmlDeserialize<Object>(rit);
            return RitObj;
        }

        public class Parametri {
            public List<KeyValuePair<String, Object>> Param = new List<KeyValuePair<String, Object>>();
            public Parametri() { }
            public void AddParameterString(string Parametro, string Valore) {
                Param.Add(new KeyValuePair<string, object>(Parametro, Valore));
            }

            public void AddParameterInteger(String Parametro, int Valore) {
                Param.Add(new KeyValuePair<string, object>(Parametro, Valore));
            }
            public void AddParameterDecimal(String Parametro, Decimal Valore) {
                Param.Add(new KeyValuePair<string, object>(Parametro, Valore));
            }
        }

        public Parametri GetParam() {
            return new Parametri();
        }




        private string SqlParameterGenerator(Parametri Param) {
            String Output = "";
            foreach (KeyValuePair<string, object> x in Param.Param) {
                if (x.Value is String) {
                    Output += "SET @" + x.Key + "='" + x.Value.ToString().Replace("'", "\'") + "';\n\n";
                }
                if (x.Value is int) {
                    Output += "SET @" + x.Key + "=" + x.Value.ToString() + ";\n";
                }
            }
            return Output;
        }

        public object EseguiInsert(string Tabella, Parametri Parameters) {
            string Sql = "INSERT INTO " + Tabella + " (";
            foreach (KeyValuePair<String, Object> x in Parameters.Param)
                Sql += x.Key + ",";
            Sql = Sql.TrimEnd(',');
            Sql += ") VALUES(";
            foreach (KeyValuePair<String, Object> x in Parameters.Param)
                // Sql &= "?,"
                Sql += "@" + x.Key + ",";
            Sql = Sql.TrimEnd(',');
            Sql += ");";
            Sql += "SELECT @@IDENTITY";
            var commandtotale = SqlParameterGenerator(Parameters) + Sql;
            var rit = EseguiCommand(commandtotale);
            return rit;
        }
        public object EseguiUpdate(string Tabella, long Id, Parametri Parameters) {
            string Sql = "UPDATE " + Tabella + " SET ";
            foreach (KeyValuePair<String, Object> x in Parameters.Param)
                Sql += x.Key + "=@" + x.Key + ",";
            Sql = Sql.TrimEnd(',');
            Sql += " WHERE Id=" + Id;
            var commandtotale = SqlParameterGenerator(Parameters) + Sql + ";SELECT ROW_COUNT();";
            var rit = EseguiCommand(commandtotale);
            return rit;
        }
        public object EseguiUpdateWhere(string Tabella, string WhereUpdate, Parametri Parameters) {
            string Sql = "UPDATE " + Tabella + " SET ";
            foreach (KeyValuePair<String, Object> x in Parameters.Param)
                Sql += x.Key + "=@" + x.Key + ",";
            Sql = Sql.TrimEnd(',');
            Sql += " WHERE " + WhereUpdate;
            var commandtotale = SqlParameterGenerator(Parameters) + Sql + ";SELECT ROW_COUNT();";
            var rit = EseguiCommand(commandtotale);
            return rit;
        }

        public object EseguiDelete(string Tabella, long Id) {
            string Sql = "DELETE FROM " + Tabella + " Where Id=" + Id.ToString() + ";SELECT ROW_COUNT();";
            return EseguiCommand(Sql);
        }
        public object EseguiDeleteWhere(string Tabella, string Where) {
            string Sql = "DELETE FROM " + Tabella + " Where " + Where + ";SELECT ROW_COUNT();";
            return EseguiCommand(Sql);
        }










    }

    public static class Serializza {
        public static T XmlDeserialize<T>(this string toDeserialize) {
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(T), "");

            using (StringReader textReader = new StringReader(toDeserialize)) {
                return (T)xmlSerialize.Deserialize(textReader);
            }
        }

        public static string XmlSerialize<T>(this T toSerialize) {
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(T));

            using (StringWriter textWriter = new StringWriter()) {
                xmlSerialize.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
