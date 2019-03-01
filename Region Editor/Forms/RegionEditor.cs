using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ultima;

namespace Region_Editor
{
    public partial class RegionEditor : Form
    {
        private Bitmap[] RenderedMaps = new Bitmap[6] { null, null, null, null, null, null };
        private Facet facet = null;
        private bool ChangesMade = false;

        public RegionEditor()
        {
            InitializeComponent();

            // Load Paramters and Regions
            Parameters.LoadParameters();
            Regions.LoadRegions();

            mapDisplay.MapScale = Scaling.Scale30;
            mapDisplay.Refresh();

            // Load Prerendered Maps
            for (int map = 0; map < 6; map++)
                if (File.Exists(String.Format("Map{0}.bmp", map)))
                    RenderedMaps[map] = (Bitmap)Bitmap.FromFile(String.Format("Map{0}.bmp", map));

            // Initialize the Facet
            facetList.SelectedIndex = 0;

            Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            Text = String.Format("Region Editor {0}.{1}", v.Major, v.Minor);
        }

        private void RegionEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Parameters.SaveParameters();

            if (ChangesMade)
            {
                DialogResult result = MessageBox.Show("Changes were made to the regions.  Would you like to save the changes before closing?", "Save Changes", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    Regions.SaveRegions();
            }
        }

        #region Menu Handlers

        private void mULFilePathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Parameters.MulPath;
            dialog.ShowDialog(this);

            Parameters.MulPath = dialog.SelectedPath;
            InitializeDisplay(); // Reset the map and regions
        }

        private void regionsxmlLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = Parameters.RegionsFile;
            dialog.ShowDialog(this);

            Parameters.RegionsFile = dialog.FileName;
            Regions.LoadRegions();
            InitializeDisplay(); // Reset the map and regions
        }

        private void reloadRegionsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regions.LoadRegions();
            ChangesMade = false;
            InitializeDisplay(); // Reset the map and regions
        }

        private void saveRegionsToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regions.SaveRegions();
            ChangesMade = false;
            MessageBox.Show("Save complete!");
        }

        #endregion

        #region Facet and Map Handlers

        private void compatible_CheckedChanged(object sender, EventArgs e)
        {
            Parameters.Compatibility = compatible.Checked;
            InitializeDisplay();
        }

        private void facetList_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeDisplay(); // Reset the map and regions
        }

        private void mapDisplay_HoverCoordinatedChanged(object sender, EventArgs e)
        {
            locationLabel.Text = String.Format("{0}, {1}", mapDisplay.Hover_Coordinate.X, mapDisplay.Hover_Coordinate.Y);
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            locationLabel.Text = String.Format("{0}, {1}", xSlider.Value, ySlider.Value);
        }

        private void Slider_SliderMoveComplete(object sender, EventArgs e)
        {
            int x = xSlider.Value;
            int y = ySlider.Value;

            if (x > xSlider.Maximum - mapDisplay.TileWidth)
                x = xSlider.Maximum - mapDisplay.TileWidth;

            if (y > ySlider.Maximum - mapDisplay.TileWidth)
                y = ySlider.Maximum - mapDisplay.TileWidth;

            mapDisplay.Upper_Coordinate = new Point(x, y);
            locationLabel.Text = String.Format("{0}, {1}", x, y);
        }

        private void scaleSlider_SliderMoveComplete(object sender, EventArgs e)
        {
            mapDisplay.MapScale = (Scaling)scaleSlider.Value;
            xSlider.SmallChange = mapDisplay.TileWidth / 4;
            ySlider.SmallChange = mapDisplay.TileWidth / 4;
        }

        private void ShowMapButton_Click(object sender, EventArgs e)
        {
            try { Application.OpenForms["MapViewer"].Close(); }
            catch { }

            MapViewer DWM = new MapViewer();

            if (RenderedMaps[facetList.SelectedIndex] == null)
            {
                TileMatrix matrix = Parameters.CurrentMap.Tiles;

                Bitmap bmp = new Bitmap(Parameters.CurrentMap.Width, Parameters.CurrentMap.Height);

                renderProgress.Visible = true;
                renderProgress.Value = 0;
                renderProgress.Maximum = bmp.Width;

                for (int x = 0; x < Parameters.CurrentMap.Width; x++)
                {
                    for (int y = 0; y < Parameters.CurrentMap.Height; y++)
                    {
                        bmp.SetPixel(x, y, Cache.GetColor(matrix.GetLandTile(x, y).ID));
                    }

                    renderProgress.Increment(1);
                }

                renderProgress.Visible = false;

                bmp.Save(String.Format("Map{0}.bmp", facetList.SelectedIndex));
                RenderedMaps[facetList.SelectedIndex] = bmp;
            }

            if (RenderedMaps[facetList.SelectedIndex] != null)
            {
                DWM.Editor = this;

                System.Drawing.Size size = new System.Drawing.Size(RenderedMaps[facetList.SelectedIndex].Width, RenderedMaps[facetList.SelectedIndex].Height);

                int reductionPercentage = 100;

                while ((size.Width > Screen.GetWorkingArea(DWM).Width - 100 || size.Height > Screen.GetWorkingArea(DWM).Height - 100) && reductionPercentage > 0)
                {
                    double reduction = (double)reductionPercentage * 0.01;

                    size.Width = (int)((double)RenderedMaps[facetList.SelectedIndex].Width * reduction);
                    size.Height = (int)((double)RenderedMaps[facetList.SelectedIndex].Height * reduction);

                    reductionPercentage -= 1;
                }

                size.Width += 6;
                size.Height += 51;

                DWM.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - size.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - size.Height) / 2);
                DWM.Size = size;

                Bitmap newBMP = new Bitmap(DWM.mapImage.Width, DWM.mapImage.Height);
                Graphics g = Graphics.FromImage(newBMP);
                g.DrawImage(RenderedMaps[facetList.SelectedIndex], 0, 0, DWM.mapImage.Width, DWM.mapImage.Height);
                g.Dispose();

                DWM.mapImage.Image = newBMP;
                DWM.Show();
                DWM.Focus();
                DWM.Select();
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            int x = -1;
            int y = -1;

            if (goX.Text == "" || goY.Text == "")
                return;

            try { x = Convert.ToInt32(goX.Text); }
            catch { }

            try { y = Convert.ToInt32(goY.Text); }
            catch { }

            if (x < 0 || x > xSlider.Maximum)
            {
                MessageBox.Show(String.Format("X value is out of range.  Please enter an X value in the range of 0 to {0}.", xSlider.Maximum));
                return;
            }

            if (y < 0 || y > ySlider.Maximum)
            {
                MessageBox.Show(String.Format("Y value is out of range.  Please enter an Y value in the range of 0 to {0}.", ySlider.Maximum));
                return;
            }

            xSlider.Value = x;
            xSlider.InvokeSliderMoveComplete(this, new EventArgs());

            ySlider.Value = y;
            ySlider.InvokeSliderMoveComplete(this, new EventArgs());

            goX.Text = "";
            goY.Text = "";
        }

        #endregion

        #region Region Control Handlers

        private void facetView_DoubleClick(object sender, EventArgs e)
        {
            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;

                if (tag.Reference == "golocation")
                {
                    mapDisplay.HighlightedArea = new Rectangle(r.GoLocation.X, r.GoLocation.Y, 1, 1);
                    GotoLocation(r.GoLocation.X, r.GoLocation.Y);
                }
                else if (tag.Reference == "entrance")
                {
                    mapDisplay.HighlightedArea = new Rectangle(r.Entrance.X, r.Entrance.Y, 1, 1);
                    GotoLocation(r.Entrance.X, r.Entrance.Y);
                }
                else if (tag.Reference == "regionarea")
                {
                    mapDisplay.HighlightedArea = r.Area[tag.Index].Area;
                    GotoLocation(r.Area[tag.Index].Area.X, r.Area[tag.Index].Area.Y, false);
                }
            }
        }

        private void facetView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;

                addSubRegion.Enabled = true;
                modifyRegion.Enabled = true;
                removeRegion.Enabled = true;
                addSpawn.Enabled = true;
                addArea.Enabled = true;

                if (tag.Reference == "spawndef")
                {
                    modifySpawn.Enabled = true;
                    removeSpawn.Enabled = true;
                }
                else
                {
                    modifySpawn.Enabled = false;
                    removeSpawn.Enabled = false;
                }

                if (tag.Reference == "regionarea")
                {
                    removeArea.Enabled = true;
                    areaModify.Enabled = true;
                }
                else
                {
                    removeArea.Enabled = false;
                    areaModify.Enabled = false;
                }
            }
        }

        #endregion

        #region Button Handlers

        private void newRegion_Click(object sender, EventArgs e)
        {
            ModifyRegion mr = new ModifyRegion();
            mr.ShowDialog();

            if (mr.Canceled)
                return;

            ChangesMade = true;

            Region r = mr.ModdedRegion.Dupe();

            facet.Regions.Add(r);
            RebuildRegionList();
        }

        private void addSubRegion_Click(object sender, EventArgs e)
        {
            Region r = null;
            TreeNode node = null;

            FindRegionNode(ref r, ref node, false);

            if (node == null || r == null)
                return;

            ModifyRegion mr = new ModifyRegion();
            mr.ShowDialog();

            if (mr.Canceled)
                return;

            ChangesMade = true;

            Region region = mr.ModdedRegion.Dupe();

            r.Regions.Add(region);
            RebuildRegionList();
        }

        private void modifyRegion_Click(object sender, EventArgs e)
        {
            Region r = null;
            TreeNode node = null;

            FindRegionNode(ref r, ref node, false);

            if (node == null || r == null)
                return;

            ModifyRegion mr = new ModifyRegion();
            mr.Initialize(r.Dupe());
            mr.ShowDialog();

            if (mr.Canceled)
                return;

            ChangesMade = true;

            r.Update(mr.ModdedRegion);
            RebuildRegionList();
        }

        private void removeRegion_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete region?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result != System.Windows.Forms.DialogResult.Yes)
                return;

            Region r = null;
            TreeNode node = null;
            RegionTag tag = new RegionTag();

            FindRegionNode(ref r, ref node, true, true, ref tag);

            if (node == null)
                return;

            ChangesMade = true;

            TreeNode parent = node.Parent;

            if (tag.Parent == null)
            {
                facet.Regions.RemoveAt(tag.Index);
                facetView.Nodes.Remove(node);

                if (facet.Regions.Count > 0)
                {
                    for (int x = 0; x < facetView.Nodes.Count; x++)
                    {
                        tag = (RegionTag)facetView.Nodes[x].Tag;
                        facetView.Nodes[x].Tag = new RegionTag(tag.Parent, tag.Reference, x, tag.OwnRegion);
                    }
                }
            }
            else
            {
                r.Regions.RemoveAt(tag.Index);

                if (r.Regions.Count > 0)
                {
                    parent.Nodes.Remove(facetView.SelectedNode);

                    for (int x = 0; x < parent.Nodes.Count; x++)
                    {
                        tag = (RegionTag)parent.Nodes[x].Tag;
                        parent.Nodes[x].Tag = new RegionTag(tag.Parent, tag.Reference, x, tag.OwnRegion);
                    }
                }
                else
                {
                    try { parent.Parent.Nodes.Remove(parent); }
                    catch { }
                }
            }
        }

        private void addSpawn_Click(object sender, EventArgs e)
        {
            Region r = null;
            TreeNode node = null;

            FindRegionNode(ref r, ref node, false);

            if (node == null || r == null)
                return;

            ModifySpawn ms = new ModifySpawn();
            ms.ShowDialog(this);

            if (ms.Canceled)
                return;

            ChangesMade = true;

            Spawn s = ms.Spawn;

            r.Spawns.Add(s);

            bool spawnNodeFound = false;

            foreach (TreeNode child in node.Nodes)
            {
                RegionTag tag = (RegionTag)child.Tag;

                if (tag.Reference == "spawns")
                {
                    spawnNodeFound = true;

                    TreeNode spawnNode = new TreeNode(String.Format("ID={0}, Type={1}, MinSeconds={2}, MaxSeconds={3}, Amount={4}",
                        s.SpawnID.ToString(), s.SpawnType, s.SpawnMinSeconds.ToString(), s.SpawnMaxSeconds.ToString(), s.SpawnAmount.ToString()));

                    spawnNode.Tag = new RegionTag(r, "spawndef", r.Spawns.Count - 1);
                    child.Nodes.Add(spawnNode);
                }
            }

            if (!spawnNodeFound)
            {
                TreeNode childNode = new TreeNode("Spawns");
                childNode.Tag = new RegionTag(r, "spawns");

                TreeNode spawnNode = new TreeNode(String.Format("ID={0}, Type={1}, MinSeconds={2}, MaxSeconds={3}, Amount={4}",
                    s.SpawnID.ToString(), s.SpawnType, s.SpawnMinSeconds.ToString(), s.SpawnMaxSeconds.ToString(), s.SpawnAmount.ToString()));

                spawnNode.Tag = new RegionTag(r, "spawndef", r.Spawns.Count - 1);
                childNode.Nodes.Add(spawnNode);

                node.Nodes.Add(childNode);
            }
        }

        private void modifySpawn_Click(object sender, EventArgs e)
        {
            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;

                Spawn s = r.Spawns[tag.Index];

                ModifySpawn ms = new ModifySpawn();
                ms.Initialize(s);
                ms.ShowDialog(this);

                if (ms.Canceled)
                    return;

                ChangesMade = true;

                s = ms.Spawn;

                facetView.SelectedNode.Text = String.Format("ID={0}, Type={1}, MinSeconds={2}, MaxSeconds={3}, Amount={4}",
                    s.SpawnID.ToString(), s.SpawnType, s.SpawnMinSeconds.ToString(), s.SpawnMaxSeconds.ToString(), s.SpawnAmount.ToString());

                r.Spawns[tag.Index] = s;
            }
        }

        private void removeSpawn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete spawn?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result != System.Windows.Forms.DialogResult.Yes)
                return;

            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;
                TreeNode parent = facetView.SelectedNode.Parent;

                ChangesMade = true;

                r.Spawns.RemoveAt(tag.Index);

                if (r.Spawns.Count > 0)
                {
                    parent.Nodes.Remove(facetView.SelectedNode);

                    for (int x = 0; x < parent.Nodes.Count; x++)
                    {
                        tag = (RegionTag)parent.Nodes[x].Tag;
                        parent.Nodes[x].Tag = new RegionTag(tag.Parent, tag.Reference, x);
                    }
                }
                else
                {
                    try { parent.Parent.Nodes.Remove(parent); }
                    catch { }
                }
            }
        }

        private void addArea_Click(object sender, EventArgs e)
        {
            Rectangle rect = mapDisplay.HighlightedArea;
            int zMin = 9999;

            if (rect.Width <= 0)
            {
                SpecifyArea sa = new SpecifyArea();
                sa.Initialize(xSlider.Maximum, ySlider.Maximum);
                sa.ShowDialog();

                if (sa.Canceled)
                    return;

                rect = sa.Area;
                zMin = sa.zMin;
            }

            Region r = null;
            TreeNode node = null;

            FindRegionNode(ref r, ref node, false);

            if (node == null || r == null)
                return;

            ChangesMade = true;

            foreach (RegionArea area in r.Area)
            {
                if (area.Area.X == rect.X &&
                    area.Area.Y == rect.Y &&
                    area.Area.Width == rect.Width &&
                    area.Area.Height == rect.Height)
                    return;
            }

            RegionArea newArea = new RegionArea(rect, zMin);
            r.Area.Add(newArea);

            bool areaNodeFound = false;

            foreach (TreeNode child in node.Nodes)
            {
                RegionTag tag = (RegionTag)child.Tag;

                if (tag.Reference == "area")
                {
                    areaNodeFound = true;

                    TreeNode areaNode = new TreeNode(String.Format("x={0}, y={1}, width={2}, height={3}{4}", newArea.Area.X, newArea.Area.Y,
                        newArea.Area.Width, newArea.Area.Height, (newArea.ZMin != 9999 ? String.Format(", zmin={0}", newArea.ZMin) : "")));
                    areaNode.Tag = new RegionTag(r, "regionarea", r.Area.Count - 1);
                    child.Nodes.Add(areaNode);
                }
            }

            if (!areaNodeFound)
            {
                TreeNode childNode = new TreeNode("Area");
                childNode.Tag = new RegionTag(r, "area");

                TreeNode areaNode = new TreeNode(String.Format("x={0}, y={1}, width={2}, height={3}{4}", newArea.Area.X, newArea.Area.Y,
                    newArea.Area.Width, newArea.Area.Height, (newArea.ZMin != 9999 ? String.Format(", zmin={0}", newArea.ZMin) : "")));
                areaNode.Tag = new RegionTag(r, "regionarea", r.Area.Count - 1);
                childNode.Nodes.Add(areaNode);

                node.Nodes.Add(childNode);
            }
        }

        private void removeArea_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete area?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result != System.Windows.Forms.DialogResult.Yes)
                return;

            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;
                TreeNode parent = facetView.SelectedNode.Parent;

                ChangesMade = true;

                r.Area.RemoveAt(tag.Index);

                if (r.Area.Count > 0)
                {
                    parent.Nodes.Remove(facetView.SelectedNode);

                    for (int x = 0; x < parent.Nodes.Count; x++)
                    {
                        tag = (RegionTag)parent.Nodes[x].Tag;
                        parent.Nodes[x].Tag = new RegionTag(tag.Parent, tag.Reference, x);
                    }
                }
                else
                {
                    try { parent.Parent.Nodes.Remove(parent); }
                    catch { }
                }
            }
        }

        private void areaModify_Click(object sender, EventArgs e)
        {
            if (facetView.SelectedNode != null)
            {
                RegionTag tag = (RegionTag)facetView.SelectedNode.Tag;
                Region r = tag.Parent;

                RegionArea ra = r.Area[tag.Index];
                
                SpecifyArea sa = new SpecifyArea();
                sa.Initialize(xSlider.Maximum, ySlider.Maximum, ra.Area, ra.ZMin);
                sa.ShowDialog(this);

                if (sa.Canceled)
                    return;

                Rectangle area = sa.Area;
                int zmin = sa.zMin;

                ChangesMade = true;

                facetView.SelectedNode.Text = String.Format("x={0}, y={1}, width={2}, height={3}{4}", area.X, area.Y, area.Width, area.Height,
                    (zmin != 9999 ? String.Format(", zmin={0}", zmin) : ""));

                r.Area[tag.Index] = new RegionArea(area, zmin);
            }
        }

        private void clearFacet_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("This option will remove every region from the current facet.  Are you sure you want to perform this action?", "Remove Facet Regions", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
                return;

            ChangesMade = true;

            facet.Regions = new List<Region>();
            RebuildRegionList();
        }

        private void FindRegionNode(ref Region region, ref TreeNode node)
        {
            RegionTag tag = new RegionTag();

            FindRegionNode(ref region, ref node, true, false, ref tag);
        }

        private void FindRegionNode(ref Region region, ref TreeNode node, bool parent)
        {
            RegionTag tag = new RegionTag();

            FindRegionNode(ref region, ref node, parent, false, ref tag);
        }

        private void FindRegionNode(ref Region region, ref TreeNode node, bool parent, bool allowNullRegion, ref RegionTag tag)
        {
            if (facetView.SelectedNode == null)
                return;

            region = null;
            node = facetView.SelectedNode;

            bool done = false;

            while (node != null && region == null && !done)
            {
                tag = (RegionTag)node.Tag;

                if (tag.Reference == "regionnode")
                {
                    if (parent)
                        region = tag.Parent;
                    else
                        region = tag.OwnRegion;

                    if (allowNullRegion)
                        done = true;
                }
                else
                    node = node.Parent;
            }
        }

        #endregion

        #region Private Routines

        private void InitializeDisplay()
        {
            switch (facetList.SelectedIndex)
            {
                case 0:
                    Parameters.CurrentMap = Parameters.MyFelucca;
                    break;
                case 1:
                    Parameters.CurrentMap = Parameters.MyTrammel;
                    break;
                case 2:
                    Parameters.CurrentMap = Map.Ilshenar;
                    break;
                case 3:
                    Parameters.CurrentMap = Map.Malas;
                    break;
                case 4:
                    Parameters.CurrentMap = Map.Tokuno;
                    break;
                case 5:
                    Parameters.CurrentMap = Map.TerMur;
                    break;
            }

            mapDisplay.Upper_Coordinate = new Point(0, 0);

            xSlider.Value = 0;
            ySlider.Value = 0;
            xSlider.Maximum = Parameters.CurrentMap.Width;
            ySlider.Maximum = Parameters.CurrentMap.Height;

            facet = Regions.Facets[facetList.SelectedIndex];
            RebuildRegionList();
        }

        private void ResetButtons()
        {
            addSubRegion.Enabled = false;
            modifyRegion.Enabled = false;
            removeRegion.Enabled = false;
            addSpawn.Enabled = false;
            modifySpawn.Enabled = false;
            removeSpawn.Enabled = false;
            addArea.Enabled = false;
            removeArea.Enabled = false;
            areaModify.Enabled = false;
        }

        #endregion

        #region Public Routines

        public void ForceRender(MapViewer viewer)
        {
            Select();
            viewer.Close();
            RenderedMaps[facetList.SelectedIndex] = null;
            ShowMapButton_Click(this, new EventArgs());
        }

        public void GotoLocation(int x, int y)
        {
            GotoLocation(x, y, true);
        }

        public void GotoLocation(int x, int y, bool center)
        {
            int displayX = x - (center ? (mapDisplay.TileWidth / 2) : 0);
            int displayY = y - (center ? (mapDisplay.TileWidth / 2) : 0);

            if (displayX < 0)
                displayX = 0;

            if (displayY < 0)
                displayY = 0;

            Select();

            mapDisplay.Upper_Coordinate = new Point(displayX, displayY);

            xSlider.Value = displayX;
            ySlider.Value = displayY;
        }

        #endregion

        #region Region Display

        private void RebuildRegionList()
        {
            facetView.Nodes.Clear();

            for (int x = 0; x < facet.Regions.Count; x++)
            {
                Region r = facet.Regions[x];

                TreeNode parentNode = new TreeNode(String.Format("{0}{1}{2}", r.Name, (r.Priority != 9999 ? String.Format(", Priority : {0}", r.Priority) : ""),
                    (r.Type != null ? String.Format(", [{0}]", r.Type) : "")));
                parentNode.Tag = new RegionTag(null, "regionnode", x, r);

                BuildRegionNode(r, ref parentNode);

                facetView.Nodes.Add(parentNode);
            }
        }

        private void BuildRegionNode(Region r, ref TreeNode parentNode)
        {
            TreeNode childNode = null;

            if (r.RuneName != null)
            {
                childNode = new TreeNode(String.Format("Rune Name = \"{0}\"", r.RuneName));
                childNode.Tag = new RegionTag(r, "runename");
                parentNode.Nodes.Add(childNode);
            }

            if (r.GoLocation.X != 9999)
            {
                childNode = new TreeNode(String.Format("Go Location : ({0}, {1}, {2})", r.GoLocation.X, r.GoLocation.Y, r.GoLocationZ));
                childNode.Tag = new RegionTag(r, "golocation");
                parentNode.Nodes.Add(childNode);
            }

            if (r.Entrance.X != 9999)
            {
                childNode = new TreeNode(String.Format("Entrance : ({0}, {1})", r.Entrance.X, r.Entrance.Y));
                childNode.Tag = new RegionTag(r, "entrance");
                parentNode.Nodes.Add(childNode);
            }

            if (r.MusicName != null)
            {
                childNode = new TreeNode(String.Format("Music : {0}", r.MusicName));
                childNode.Tag = new RegionTag(r, "music");
                parentNode.Nodes.Add(childNode);
            }

            if (r.MinZRange != 9999)
            {
                childNode = new TreeNode(String.Format("Minimum Z Range : {0}", r.MinZRange));
                childNode.Tag = new RegionTag(r, "minzrange");
                parentNode.Nodes.Add(childNode);
            }

            if (!r.LogoutDelayActive)
            {
                childNode = new TreeNode(String.Format("Logout Delay Not Active"));
                childNode.Tag = new RegionTag(r, "logoutdelay");
                parentNode.Nodes.Add(childNode);
            }

            if (r.GuardsDisabled)
            {
                childNode = new TreeNode(String.Format("Guards Disabled"));
                childNode.Tag = new RegionTag(r, "guards");
                parentNode.Nodes.Add(childNode);
            }

            if (r.SmartNoHousing)
            {
                childNode = new TreeNode(String.Format("Smart NoHousing Active"));
                childNode.Tag = new RegionTag(r, "smartnohousing");
                parentNode.Nodes.Add(childNode);
            }

            if (r.Type == "QuestOfferRegion" || r.Type == "CancelQuestRegion")
            {
                childNode = new TreeNode(String.Format("Quest Type : \"{0}\"", r.QuestType));
                childNode.Tag = new RegionTag(r, "questtype");
                parentNode.Nodes.Add(childNode);
            }

            if (r.Type == "QuestNoEntryRegion")
            {
                childNode = new TreeNode(String.Format("Quest Type : \"{0}\", Objective = \"{1}\", Message = {2}", r.QuestType, r.QuestMin, r.QuestMessage));
                childNode.Tag = new RegionTag(r, "questnoentry");
                parentNode.Nodes.Add(childNode);
            }

            if (r.Type == "QuestCompleteObjectiveRegion")
            {
                childNode = new TreeNode(String.Format("Quest Type : \"{0}\", Objective = \"{1}\"", r.QuestType, r.QuestComplete));
                childNode.Tag = new RegionTag(r, "questcomplete");
                parentNode.Nodes.Add(childNode);
            }

            childNode = new TreeNode("Area");
            childNode.Tag = new RegionTag(r, "area");

            for (int x = 0; x < r.Area.Count; x++)
            {
                RegionArea ra = r.Area[x];

                TreeNode areaNode = new TreeNode(String.Format("x={0}, y={1}, width={2}, height={3}{4}", ra.Area.X, ra.Area.Y, ra.Area.Width,
                    ra.Area.Height, (ra.ZMin != 9999 ? String.Format(", zmin={0}", ra.ZMin) : "")));
                areaNode.Tag = new RegionTag(r, "regionarea", x);
                childNode.Nodes.Add(areaNode);
            }

            if (childNode.Nodes.Count > 0)
                parentNode.Nodes.Add(childNode);

            childNode = new TreeNode("Spawns");
            childNode.Tag = new RegionTag(r, "spawns");

            for (int x = 0; x < r.Spawns.Count; x++)
            {
                Spawn s = r.Spawns[x];

                TreeNode spawnNode = new TreeNode(String.Format("ID={0}, Type={1}, MinSeconds={2}, MaxSeconds={3}, Amount={4}",
                    s.SpawnID.ToString(), s.SpawnType, s.SpawnMinSeconds.ToString(), s.SpawnMaxSeconds.ToString(), s.SpawnAmount.ToString()));
                spawnNode.Tag = new RegionTag(r, "spawndef", x);
                childNode.Nodes.Add(spawnNode);
            }

            if (childNode.Nodes.Count > 0)
                parentNode.Nodes.Add(childNode);

            childNode = new TreeNode("Regions");
            childNode.Tag = new RegionTag(r, "regions");

            for (int x = 0; x < r.Regions.Count; x++)
            {
                Region region = r.Regions[x];

                TreeNode regionNode = new TreeNode(String.Format("{0}{1}{2}", (region.Name != null ? region.Name : "(Unnamed Region)"),
                    (region.Priority != 9999 ? String.Format(", Priority : {0}", region.Priority) : ""),
                    (region.Type != null ? String.Format(", [{0}]", region.Type) : "")));
                regionNode.Tag = new RegionTag(r, "regionnode", x, region);

                BuildRegionNode(region, ref regionNode);

                childNode.Nodes.Add(regionNode);
            }

            if (childNode.Nodes.Count > 0)
                parentNode.Nodes.Add(childNode);
        }

        #endregion
    }
}