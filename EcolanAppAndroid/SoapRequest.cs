using SoapHttpClient;
using SoapHttpClient.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace EcolanAppAndroid {
    public class SoapRequest {
        private Uri uri;
        private String action;
        public String namespac;
        public Dictionary<String, Object> Parametri = new Dictionary<String, Object>();
        public SoapRequest(String url, String action) {
            uri = new Uri(url);
            namespac = "http://" + uri.Host + "/";
            this.action = action;


        }
        public void AddBody(String Name, Object Valore) {
            Parametri.Add(Name, Valore);
        }

        public class RitornoSoap {
            public String ResponseString;
            public XElement ResponseXml;
            public XDocument XmlOuterBody;
            public object RispostaItem(String Item) {
                var tmp = ResponseXml.Descendants().FirstOrDefault((x) => { return x.Name.LocalName == Item; }).Value;
                return tmp;
            }
        }

        private XElement GeneraContent() {
            XNamespace ns = namespac;
            var El = new XElement(ns + action);
            foreach (var x in Parametri) {
                El.Add(new XElement(ns + x.Key, x.Value));
            }
            return El;
        }


        public RitornoSoap GetResponse() {
            XNamespace ns = namespac;
            var soap = new SoapHttpClient.SoapClient();
            var req = soap.Post(uri,
                SoapVersion.Soap11, body: GeneraContent(),
                action: namespac + "" + action);
            var ritorno = new RitornoSoap();
            var xmlritorno = req.Content.ReadAsStringAsync().Result;
            ritorno.XmlOuterBody = XDocument.Parse(xmlritorno);
            ritorno.ResponseString = ritorno.XmlOuterBody.Descendants().First((x) => { return x.Name.LocalName == action + "Result"; }).Value;
            var tmp = ritorno.XmlOuterBody.Descendants().First((x) => { return x.Name.LocalName == action + "Result"; });
            if (tmp is null) return null;
            ritorno.ResponseXml = tmp;
            return ritorno;
        }





    }

}
