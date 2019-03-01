using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Region_Editor
{
    public class Region
    {
        private string _Type = null;
        private int _Priority = 9999;
        private string _Name = null;
        private string _RuneName = null;
        private Point _GoLocation = new Point(9999, 9999);
        private int _GoLocationZ = 0;
        private Point _Entrance = new Point(9999, 9999);
        private string _MusicName = null;
        private int _MinZRange = 9999;
        private bool _LogoutDelayActive = true;
        private bool _GuardsDisabled = false;
        private bool _AntiMagic = false;
        private bool _SmartNoHousing = false;
        private string _QuestType = null;
        private string _QuestMin = null;
        private string _QuestComplete = null;
        private int _QuestMessage = 0;

        private List<RegionArea> _Area = new List<RegionArea>();
        private List<Region> _Regions = new List<Region>();
        private List<Spawn> _Spawns = new List<Spawn>();

        public string Type { get { return _Type; } set { _Type = value; } }
        public int Priority { get { return _Priority; } set { _Priority = value; } }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string RuneName { get { return _RuneName; } set { _RuneName = value; } }
        public Point GoLocation { get { return _GoLocation; } set { _GoLocation = value; } }
        public int GoLocationZ { get { return _GoLocationZ; } set { _GoLocationZ = value; } }
        public Point Entrance { get { return _Entrance; } set { _Entrance = value; } }
        public string MusicName { get { return _MusicName; } set { _MusicName = value; } }
        public int MinZRange { get { return _MinZRange; } set { _MinZRange = value; } }
        public bool LogoutDelayActive { get { return _LogoutDelayActive; } set { _LogoutDelayActive = value; } }
        public bool GuardsDisabled { get { return _GuardsDisabled; } set { _GuardsDisabled = value; } }
        public bool AntiMagic { get { return _AntiMagic; } set { _AntiMagic = value; } }
        public bool SmartNoHousing { get { return _SmartNoHousing; } set { _SmartNoHousing = value; } }
        public string QuestType { get { return _QuestType; } set { _QuestType = value; } }
        public string QuestMin { get { return _QuestMin; } set { _QuestMin = value; } }
        public string QuestComplete { get { return _QuestComplete; } set { _QuestComplete = value; } }
        public int QuestMessage { get { return _QuestMessage; } set { _QuestMessage = value; } }

        public List<RegionArea> Area { get { return _Area; } set { _Area = value; } }
        public List<Region> Regions { get { return _Regions; } set { _Regions = value; } }
        public List<Spawn> Spawns { get { return _Spawns; } set { _Spawns = value; } }

        public Region()
        {
        }

        public Region Dupe()
        {
            Region r = new Region();

            r.Type = _Type;
            r.Priority = _Priority;
            r.Name = _Name;
            r.RuneName = _RuneName;
            r.GoLocation = _GoLocation;
            r.GoLocationZ = _GoLocationZ;
            r.Entrance = _Entrance;
            r.MusicName = _MusicName;
            r.MinZRange = _MinZRange;
            r.LogoutDelayActive = _LogoutDelayActive;
            r.GuardsDisabled = _GuardsDisabled;
            r.SmartNoHousing = _SmartNoHousing;
            r.QuestType = _QuestType;
            r.QuestMin = _QuestMin;
            r.QuestComplete = _QuestComplete;
            r.QuestMessage = _QuestMessage;

            return r;
        }

        public void Update(Region r)
        {
            _Type = r.Type;
            _Priority = r.Priority;
            _Name = r.Name;
            _RuneName = r.RuneName;
            _GoLocation = r.GoLocation;
            _GoLocationZ = r.GoLocationZ;
            _Entrance = r.Entrance;
            _MusicName = r.MusicName;
            _MinZRange = r.MinZRange;
            _LogoutDelayActive = r.LogoutDelayActive;
            _GuardsDisabled = r.GuardsDisabled;
            _SmartNoHousing = r.SmartNoHousing;
            _QuestType = r.QuestType;
            _QuestMin = r.QuestMin;
            _QuestComplete = r.QuestComplete;
            _QuestMessage = r.QuestMessage;
        }
    }

    public struct Spawn
    {
        private int _SpawnID;
        private string _SpawnType;
        private int _SpawnMinSeconds;
        private int _SpawnMaxSeconds;
        private int _SpawnAmount;

        public int SpawnID { get { return _SpawnID; } set { _SpawnID = value; } }
        public string SpawnType { get { return _SpawnType; } set { _SpawnType = value; } }
        public int SpawnMinSeconds { get { return _SpawnMinSeconds; } set { _SpawnMinSeconds = value; } }
        public int SpawnMaxSeconds { get { return _SpawnMaxSeconds; } set { _SpawnMaxSeconds = value; } }
        public int SpawnAmount { get { return _SpawnAmount; } set { _SpawnAmount = value; } }

        public Spawn(int id, string type, int minSeconds, int maxSeconds, int amount)
        {
            _SpawnID = id;
            _SpawnType = type;
            _SpawnMinSeconds = minSeconds;
            _SpawnMaxSeconds = maxSeconds;
            _SpawnAmount = amount;
        }
    }

    public struct RegionArea
    {
        private Rectangle _Area;
        private int _ZMin;

        public Rectangle Area { get { return _Area; } set { _Area = value; } }
        public int ZMin { get { return _ZMin; } set { _ZMin = value; } }

        public RegionArea(Rectangle area)
            : this(area, 9999)
        {
        }

        public RegionArea(Rectangle area, int zMin)
        {
            _Area = area;
            _ZMin = zMin;
        }
    }
}
