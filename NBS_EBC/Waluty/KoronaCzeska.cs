using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;

namespace NBS_EBC.Waluty
{
    class KoronaCzeska : AWaluty{

        public override string Localfile
        {
            get
            {
                return "CZK.dat";
            }
        }

        public override string Url
        {
            get
            {
                return string.Format("http://www.nbp.pl/kursy/Archiwum/archiwum_tab_a_{0}.csv", DateTime.Now.Year.ToString());
            }
        }

        public override void Parse()
        {
            try {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(Url, tmpFile);
                }

                string line;
                StreamReader file = new StreamReader(tmpFile);

                int counter = 0;
                while ((line = file.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Equals(string.Empty)) break;

                    if (counter >= 2)
                    {
                        string[] korona = line.Split(';');
                        this.notowania[DateTime.ParseExact(korona[0], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture)] = float.Parse(korona[14]);
                    }
                    counter++;
                }

                file.Close();


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
            catch 
            {
                MessageBox.Show("Nie udało się pobrać kursów Korony Czeskiej.");
            }
        }
    }
}
