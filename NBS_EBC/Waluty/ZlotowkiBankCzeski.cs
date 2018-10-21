using NBS_EBC.Waluty;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows;

namespace NBS_EBC.Waluty
{
    class ZlotowkiBankCzeski : AWaluty{

        public override string Localfile
        {
            get
            {
                return "PL_BC.dat";
            }
        }

        public override string Url
        {
            get
            {
                return string.Format("https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={0}", DateTime.Now.Year.ToString());
            }
        }

        public override void Parse()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(Url, tmpFile);
            }


            string line, kwota;
            StreamReader file = new StreamReader(tmpFile);

            int counter = 0;

            CultureInfo currentCulture = new CultureInfo("en-US");
            while ((line = file.ReadLine()) != null)
            {
                line = line.Trim();

                if (line.Equals(string.Empty)) break;

                if (counter >= 1 )
                {
                    string[] korona = line.Split('|');

                    DateTime dt = DateTime.ParseExact(korona[0].ToUpper(), "dd.MMM yyyy", currentCulture);
                    kwota = korona[24].Replace('.', ',');
                    this.notowania[dt] = float.Parse(kwota);
                    
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
    }
}
