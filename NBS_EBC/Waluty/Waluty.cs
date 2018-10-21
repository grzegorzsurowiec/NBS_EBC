using System;
using System.Collections.Generic;
using System.IO;

namespace NBS_EBC.Waluty
{
    public abstract class AWaluty
    {
        public abstract string Url
        {
            get;
        }

        public abstract string Localfile
        {
            get;
        }

        protected string LocalFile()
        {
            string year = DateTime.Now.Year.ToString();
            if (!Directory.Exists(year)) Directory.CreateDirectory(year);
            return DateTime.Now.Year.ToString() + "\\"+ this.Localfile;
        }

        protected static string tmpFile = "tmp.file";

        public Dictionary<DateTime, Single> notowania = new Dictionary<DateTime, Single>();

        public AWaluty()
        {
            bool download = false;
            if ((this.ExtractDayPart(DateTime.UtcNow) - this.ExtractDayPart(File.GetLastWriteTimeUtc(this.LocalFile()))).TotalDays >= 1) download = true;

            if (File.Exists(this.LocalFile()))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(this.LocalFile(), FileMode.Open)))
                    {
                        this.notowania.Clear();
                        while (reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            this.notowania[DateTime.FromBinary(reader.ReadInt64())] = reader.ReadSingle();
                        }

                    }
                }
                catch { }
            }
            else download = true;

            if (download)
            {
                this.Parse();
            }
        }

        public abstract void Parse();

        private DateTime ExtractDayPart(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
        }
    }

   
}
