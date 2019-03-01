namespace Region_Editor
{
    partial class MapViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapViewer));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.renderMapAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapImage = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renderMapAgainToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // renderMapAgainToolStripMenuItem
            // 
            this.renderMapAgainToolStripMenuItem.Name = "renderMapAgainToolStripMenuItem";
            this.renderMapAgainToolStripMenuItem.Size = new System.Drawing.Size(117, 20);
            this.renderMapAgainToolStripMenuItem.Text = "Render Map Again";
            this.renderMapAgainToolStripMenuItem.Click += new System.EventHandler(this.renderMapAgainToolStripMenuItem_Click);
            // 
            // mapImage
            // 
            this.mapImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mapImage.Location = new System.Drawing.Point(0, 27);
            this.mapImage.Name = "mapImage";
            this.mapImage.Size = new System.Drawing.Size(284, 235);
            this.mapImage.TabIndex = 1;
            this.mapImage.TabStop = false;
            this.mapImage.DoubleClick += new System.EventHandler(this.mapImage_DoubleClick);
            this.mapImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapImage_MouseDown);
            // 
            // MapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.mapImage);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(50, 50);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Map Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem renderMapAgainToolStripMenuItem;
        public System.Windows.Forms.PictureBox mapImage;
    }
}