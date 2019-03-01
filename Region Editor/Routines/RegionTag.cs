using System;

namespace Region_Editor
{
    public struct RegionTag
    {
        private Region _Parent;
        public Region Parent { get { return _Parent; } set { _Parent = value; } }

        private int _Index;
        public int Index { get { return _Index; } set { _Index = value; } }

        private string _Reference;
        public string Reference { get { return _Reference; } set { _Reference = value; } }

        private Region _OwnRegion;
        public Region OwnRegion { get { return _OwnRegion; } set { _OwnRegion = value; } }

        public RegionTag(Region parent)
        {
            _Parent = parent;
            _Index = -1;
            _Reference = "";
            _OwnRegion = parent;
        }

        public RegionTag(Region parent, string reference)
        {
            _Parent = parent;
            _Index = -1;
            _Reference = reference;
            _OwnRegion = parent;
        }

        public RegionTag(Region parent, string reference, int index)
        {
            _Parent = parent;
            _Index = index;
            _Reference = reference;
            _OwnRegion = parent;
        }

        public RegionTag(Region parent, string reference, int index, Region ownregion)
        {
            _Parent = parent;
            _Index = index;
            _Reference = reference;
            _OwnRegion = ownregion;
        }
    }
}
