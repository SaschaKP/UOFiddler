using System;
using System.Collections.Generic;

namespace Region_Editor
{
    public class Facet
    {
        private string _Name = "";
        public string Name { get { return _Name; } set { _Name = value; } }

        private List<Region> _Regions = new List<Region>();
        public List<Region> Regions { get { return _Regions; } set { _Regions = value; } }

        public Facet(string name)
        {
            _Name = name;
        }
    }
}
