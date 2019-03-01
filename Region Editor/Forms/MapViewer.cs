using System;
using System.Drawing;
using System.Windows.Forms;

namespace Region_Editor
{
    public partial class MapViewer : Form
    {
        public RegionEditor Editor = null;
        private Point ClickLocation;

        public MapViewer()
        {
            InitializeComponent();
        }

        private void renderMapAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.ForceRender(this);
        }

        private void mapImage_MouseDown(object sender, MouseEventArgs e)
        {
            ClickLocation = e.Location;
        }

        private void mapImage_DoubleClick(object sender, EventArgs e)
        {
            int realX = (int)(((double)Parameters.CurrentMap.Width / (double)mapImage.Width) * (double)ClickLocation.X);
            int realY = (int)(((double)Parameters.CurrentMap.Height / (double)mapImage.Height) * (double)ClickLocation.Y);

            Editor.GotoLocation(realX, realY);
        }
    }
}
