using System;
using System.IO;
using System.Xml;
using Ultima;

namespace Region_Editor
{
    public class Parameters
    {
        private static string _MulPath = "";
        public static string MulPath { get { return _MulPath; } set { _MulPath = value; SetMulPath(); } }

        private static string _RegionsFile = "";
        public static string RegionsFile { get { return _RegionsFile; } set { _RegionsFile = value; } }

        public static Map MyFelucca = new Map(0, 0, 7168, 4096);
        public static Map MyTrammel = new Map(1, 1, 7168, 4096);
        public static Map CurrentMap = MyTrammel;

        private static bool _Compatibility = false;
        public static bool Compatibility { get { return _Compatibility; } set { _Compatibility = value; ResetMaps(); } }

        private static void SetMulPath()
        {
            Ultima.Files.SetMulPath(_MulPath);
            Ultima.Files.ReLoadDirectory();
            Ultima.Art.Reload();

            ResetMaps();
        }

        private static void ResetMaps()
        {
            if (Compatibility)
            {
                MyFelucca = Map.Felucca;
                MyTrammel = Map.Trammel;
            }
            else
            {
                MyFelucca = new Map(0, 0, 7168, 4096);
                MyTrammel = new Map(1, 1, 7168, 4096);
            }

            Ultima.Map.Reload();
        }

        public static void LoadParameters()
        {
            if (!System.IO.File.Exists("params.xml"))
                return;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("params.xml");

                XmlElement root = doc["RegionEditor"];

                if (root == null)
                    return;

                MulPath = Regions.ReadString(root["mulpath"], "path");
                RegionsFile = Regions.ReadString(root["script"], "file");
                Compatibility = Regions.ReadBoolean(root["client"], "compatibilitymode", false);
            }
            catch { }
        }

        public static void SaveParameters()
        {
            try
            {
                XmlTextWriter writer = new XmlTextWriter("params.xml", System.Text.Encoding.UTF8);

                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("RegionEditor");

                if (MulPath != "")
                {
                    writer.WriteStartElement("mulpath");

                    writer.WriteStartAttribute("path");
                    writer.WriteValue(_MulPath);
                    writer.WriteEndAttribute();

                    writer.WriteEndElement();
                }

                if (RegionsFile != "")
                {
                    writer.WriteStartElement("script");

                    writer.WriteStartAttribute("file");
                    writer.WriteValue(_RegionsFile);
                    writer.WriteEndAttribute();

                    writer.WriteEndElement();
                }

                if (Compatibility)
                {
                    writer.WriteStartElement("client");

                    writer.WriteStartAttribute("compatibilitymode");
                    writer.WriteValue(Compatibility);
                    writer.WriteEndAttribute();

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteEndDocument();

                writer.Close();
            }
            catch { }
        }
    }
}
