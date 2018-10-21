using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace NBS_EBC.Waluty
{
    class SlowacjaEuro : AWaluty
    {
        public override string Localfile
        {
            get
            {
                return "ebc.dat";
            }
        }

        public override string Url
        {
            get
            {
                return "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml";
            }
        }

        public override void Parse()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Url, tmpFile);
            }

            XDocument doc = XDocument.Load(tmpFile);

            foreach (XElement xmlRoot in doc.Root.Elements())
            {
                foreach (XElement xmlDaty in xmlRoot.Elements())
                {
                    try
                    {
                        string dtFormat = "yyyy-MM-dd";
                        DateTime dt = DateTime.ParseExact(xmlDaty.Attribute("time").Value, dtFormat, System.Globalization.CultureInfo.InvariantCulture);

                        if (dt.CompareTo(DateTime.ParseExact("2012-12-31", dtFormat, System.Globalization.CultureInfo.InvariantCulture)) > 0)
                        {
                            foreach (XElement xmlNotowania in xmlDaty.Elements())
                            {
                                if (xmlNotowania.Attribute("currency").Value.Equals("PLN"))
                                {
                                    this.notowania[dt] = Single.Parse(xmlNotowania.Attribute("rate").Value.Replace(".", ","));
                                }
                            }
                        }
                    }

                    catch { }
                }
            }

            File.Delete(tmpFile);

            using (BinaryWriter writer = new BinaryWriter(File.Open(this.LocalFile(), FileMode.Create)))
            {
                foreach (KeyValuePair<DateTime, Single> item in this.notowania)
                {
                    writer.Write(item.Key.ToBinary());
                    writer.Write(item.Value);
                }
            }
        }

    }
}
