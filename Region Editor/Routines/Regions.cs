using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Region_Editor
{
    public class Regions
    {
        public static Facet[] Facets;

        protected static void InitializeFacets()
        {
            Facets = new Facet[6] { new Facet("Felucca"), new Facet("Trammel"), new Facet("Ilshenar"), new Facet("Malas"), new Facet("Tokuno"), 
                new Facet("TerMur") };
        }

        public static void LoadRegions()
        {
            InitializeFacets();

            if (!System.IO.File.Exists(Parameters.RegionsFile))
                return;

            XmlDocument doc = new XmlDocument();
            doc.Load(Parameters.RegionsFile);

            XmlElement root = doc["ServerRegions"];

            if (root == null)
                return;

            foreach (XmlElement facet in root.SelectNodes("Facet"))
            {
                Facet f = null;

                switch (GetAttribute(facet, "name"))
                {
                    case "Felucca":
                        f = Facets[0];
                        break;
                    case "Trammel":
                        f = Facets[1];
                        break;
                    case "Ilshenar":
                        f = Facets[2];
                        break;
                    case "Malas":
                        f = Facets[3];
                        break;
                    case "Tokuno":
                        f = Facets[4];
                        break;
                    case "TerMur":
                        f = Facets[5];
                        break;
                }

                if (f != null)
                    LoadFacet(f, facet);
            }
        }

        protected static void LoadFacet(Facet f, XmlElement xml)
        {
            foreach (XmlElement xmlReg in xml.SelectNodes("region"))
            {
                f.Regions.Add(LoadRegion(xmlReg));
            }
        }

        protected static Region LoadRegion(XmlElement xml)
        {
            Region region = new Region();

            region.Type = ReadString(xml, "type");
            region.Priority = ReadInt32(xml, "priority");
            region.Name = ReadString(xml, "name");
            region.RuneName = ReadString(xml["rune"], "name");
            region.MusicName = ReadString(xml["music"], "name");
            region.MinZRange = ReadInt32(xml["zrange"], "min");
            region.LogoutDelayActive = ReadBoolean(xml["logoutDelay"], "active", true);
            region.GuardsDisabled = ReadBoolean(xml["guards"], "disabled", false);
            region.SmartNoHousing = ReadBoolean(xml["smartNoHousing"], "active", false);
            region.QuestType = ReadString(xml["quest"], "type");
            region.QuestMin = ReadString(xml["quest"], "min");
            region.QuestComplete = ReadString(xml["quest"], "complete");
            region.QuestMessage = ReadInt32(xml["quest"], "message");

            if (region.QuestMessage == 9999)
                region.QuestMessage = 0;

            int x = ReadInt32(xml["go"], "x");
            int y = ReadInt32(xml["go"], "y");
            int z = ReadInt32(xml["go"], "z");

            if (x != 9999 && y != 9999)
            {
                region.GoLocation = new System.Drawing.Point(x, y);
                region.GoLocationZ = z;
            }

            x = ReadInt32(xml["entrance"], "x");
            y = ReadInt32(xml["entrance"], "y");

            if (x != 9999 && y != 9999)
                region.Entrance = new System.Drawing.Point(x, y);

            XmlElement spawning = xml["spawning"];

            if (spawning != null)
            {
                foreach (XmlElement xmlSpawn in spawning.SelectNodes("object"))
                {
                    int id = ReadInt32(xmlSpawn, "id");
                    string type = ReadString(xmlSpawn, "type");
                    int minSeconds = ReadTimeSpan(xmlSpawn, "minSpawnTime").Seconds;
                    int maxSeconds = ReadTimeSpan(xmlSpawn, "maxSpawnTime").Seconds;
                    int amount = ReadInt32(xmlSpawn, "amount");

                    if (id != 9999 && amount != 9999 && type != null)
                        region.Spawns.Add(new Spawn(id, type, minSeconds, maxSeconds, amount));
                }
            }

            foreach (XmlElement xmlRect in xml.SelectNodes("rect"))
            {
                Rectangle rect = ReadRectangle(xmlRect);
                int minZ = ReadInt32(xmlRect, "zmin");

                if (rect.X != -1)
                {
                    if (minZ != 9999)
                        region.Area.Add(new RegionArea(rect, minZ));
                    else
                        region.Area.Add(new RegionArea(rect));
                }
            }

            foreach (XmlElement xmlReg in xml.SelectNodes("region"))
                region.Regions.Add(LoadRegion(xmlReg));

            return region;
        }

        protected static string GetAttribute(XmlElement xml, string attribute)
        {
            if (xml == null)
                return null;
            else if (xml.HasAttribute(attribute))
                return xml.GetAttribute(attribute);
            else
                return null;
        }

        public static string ReadString(XmlElement xml, string attribute)
        {
            return GetAttribute(xml, attribute);
        }

        public static int ReadInt32(XmlElement xml, string attribute)
        {
            string s = GetAttribute(xml, attribute);

            if (s == null)
                return 9999;

            try
            {
                return XmlConvert.ToInt32(s);
            }
            catch { }

            return 9999;
        }

        public static bool ReadBoolean(XmlElement xml, string attribute, bool defaultValue)
        {
            string s = GetAttribute(xml, attribute);

            if (s == null)
                return defaultValue;

            try
            {
                return XmlConvert.ToBoolean(s);
            }
            catch { }

            return defaultValue;
        }

        public static TimeSpan ReadTimeSpan(XmlElement xml, string attribute)
        {
            string s = GetAttribute(xml, attribute);

            if (s == null)
                return TimeSpan.Zero;

            try
            {
                return XmlConvert.ToTimeSpan(s);
            }
            catch { }

            return TimeSpan.Zero;
        }


        public static Rectangle ReadRectangle(XmlElement xml)
        {
            int x = 0, y = 0, width = 0, height = 0;

            if (xml.HasAttribute("x"))
            {
                x = ReadInt32(xml, "x");
                y = ReadInt32(xml, "y");
                width = ReadInt32(xml, "width");
                height = ReadInt32(xml, "height");

                if (x != -1 && y != -1 && width != -1 && height != -1)
                    return new Rectangle(x, y, width, height);
            }

            return new Rectangle(-1, -1, -1, -1);
        }

        public static void SaveRegions()
        {
            try
            {
                if (System.IO.File.Exists(Parameters.RegionsFile))
                {
                    DateTime n = DateTime.Now;

                    System.IO.File.Move(Parameters.RegionsFile,
                        String.Format("{0}.bck_{1}{2}{3}{4}{5}{6}", Parameters.RegionsFile, n.Millisecond, n.Day, n.Year, n.Hour, n.Minute, n.Second));
                }

                XmlTextWriter writer = new XmlTextWriter(Parameters.RegionsFile, System.Text.Encoding.UTF8);

                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();

                writer.WriteStartElement("ServerRegions");

                foreach (Facet f in Facets)
                    SaveFacet(f, writer);

                writer.WriteEndElement();

                writer.WriteEndDocument();

                writer.Close();
            }
            catch { }
        }

        private static void SaveFacet(Facet facet, XmlTextWriter writer)
        {
            writer.WriteStartElement("Facet");

            writer.WriteStartAttribute("name");
            writer.WriteValue(facet.Name);
            writer.WriteEndAttribute();

            foreach (Region r in facet.Regions)
                SaveRegion(r, writer);

            writer.WriteEndElement();
        }

        private static void SaveRegion(Region region, XmlTextWriter writer)
        {
            writer.WriteStartElement("region");

            if (region.Type != null)
            {
                writer.WriteStartAttribute("type");
                writer.WriteValue(region.Type);
                writer.WriteEndAttribute();
            }

            if (region.Priority != 9999)
            {
                writer.WriteStartAttribute("priority");
                writer.WriteValue(region.Priority);
                writer.WriteEndAttribute();
            }

            if (region.Name != null)
            {
                writer.WriteStartAttribute("name");
                writer.WriteValue(region.Name);
                writer.WriteEndAttribute();
            }

            if (region.RuneName != null)
            {
                writer.WriteStartElement("rune");

                writer.WriteStartAttribute("name");
                writer.WriteValue(region.RuneName);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.Area.Count > 0)
                SaveAreas(region.Area, writer);

            if (region.GoLocation.X != 9999 && region.GoLocation.Y != 9999 && region.GoLocationZ != 9999)
            {
                writer.WriteStartElement("go");

                writer.WriteStartAttribute("x");
                writer.WriteValue(region.GoLocation.X);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("y");
                writer.WriteValue(region.GoLocation.Y);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("z");
                writer.WriteValue(region.GoLocationZ);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.Entrance.X != 9999 && region.Entrance.Y != 9999)
            {
                writer.WriteStartElement("entrance");

                writer.WriteStartAttribute("x");
                writer.WriteValue(region.Entrance.X);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("y");
                writer.WriteValue(region.Entrance.Y);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.MusicName != null)
            {
                writer.WriteStartElement("music");

                writer.WriteStartAttribute("name");
                writer.WriteValue(region.MusicName);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.MinZRange != 9999)
            {
                writer.WriteStartElement("zrange");

                writer.WriteStartAttribute("min");
                writer.WriteValue(region.MinZRange);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (!region.LogoutDelayActive)
            {
                writer.WriteStartElement("logoutDelay");

                writer.WriteStartAttribute("active");
                writer.WriteValue(XmlConvert.ToString(region.LogoutDelayActive));
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.GuardsDisabled)
            {
                writer.WriteStartElement("guards");

                writer.WriteStartAttribute("disabled");
                writer.WriteValue(XmlConvert.ToString(region.GuardsDisabled));
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            if (region.SmartNoHousing)
            {
                writer.WriteStartElement("smartNoHousing");

                writer.WriteStartAttribute("active");
                writer.WriteValue(XmlConvert.ToString(region.SmartNoHousing));
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }
            
            if (region.Type == "QuestOfferRegion" || region.Type == "CancelQuestRegion")
            {
                writer.WriteStartElement("quest");

                if (region.QuestType != null)
                {
                    writer.WriteStartAttribute("type");
                    writer.WriteValue(region.QuestType);
                    writer.WriteEndAttribute();
                }

                writer.WriteEndElement();
            }

            if (region.Type == "QuestNoEntryRegion")
            {
                writer.WriteStartElement("quest");

                if (region.QuestType != null)
                {
                    writer.WriteStartAttribute("type");
                    writer.WriteValue(region.QuestType);
                    writer.WriteEndAttribute();
                }

                if (region.QuestMin != null)
                {
                    writer.WriteStartAttribute("min");
                    writer.WriteValue(region.QuestMin);
                    writer.WriteEndAttribute();
                }

                if (region.QuestMessage != 0)
                {
                    writer.WriteStartAttribute("message");
                    writer.WriteValue(region.QuestMessage);
                    writer.WriteEndAttribute();
                }

                writer.WriteEndElement();
            }

            if (region.Type == "QuestCompleteObjectiveRegion")
            {
                writer.WriteStartElement("quest");

                if (region.QuestType != null)
                {
                    writer.WriteStartAttribute("type");
                    writer.WriteValue(region.QuestType);
                    writer.WriteEndAttribute();
                }

                if (region.QuestComplete != null)
                {
                    writer.WriteStartAttribute("complete");
                    writer.WriteValue(region.QuestComplete);
                    writer.WriteEndAttribute();
                }

                writer.WriteEndElement();
            }

            if (region.Spawns.Count > 0)
                SaveSpawns(region.Spawns, writer);

            foreach(Region r in region.Regions)
                SaveRegion(r, writer);

            writer.WriteEndElement();
        }

        private static void SaveSpawns(List<Spawn> spawns, XmlTextWriter writer)
        {
            writer.WriteStartElement("spawning");

            foreach (Spawn s in spawns)
            {
                writer.WriteStartElement("object");

                writer.WriteStartAttribute("id");
                writer.WriteValue(s.SpawnID);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("type");
                writer.WriteValue(s.SpawnType);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("minSpawnTime");
                writer.WriteValue(String.Format("PT{0}S", s.SpawnMinSeconds));
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("maxSpawnTime");
                writer.WriteValue(String.Format("PT{0}S", s.SpawnMaxSeconds));
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("amount");
                writer.WriteValue(s.SpawnAmount);
                writer.WriteEndAttribute();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void SaveAreas(List<RegionArea> areas, XmlTextWriter writer)
        {
            foreach (RegionArea ra in areas)
            {
                writer.WriteStartElement("rect");

                writer.WriteStartAttribute("x");
                writer.WriteValue(ra.Area.X);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("y");
                writer.WriteValue(ra.Area.Y);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("width");
                writer.WriteValue(ra.Area.Width);
                writer.WriteEndAttribute();

                writer.WriteStartAttribute("height");
                writer.WriteValue(ra.Area.Height);
                writer.WriteEndAttribute();

                if (ra.ZMin != 9999)
                {
                    writer.WriteStartAttribute("zmin");
                    writer.WriteValue(ra.ZMin);
                    writer.WriteEndAttribute();
                }

                writer.WriteEndElement();
            }
        }
    }
}
