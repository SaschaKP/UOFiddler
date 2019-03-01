namespace Region_Editor
{
    partial class Slider
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.slideControl = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.slideControl)).BeginInit();
            this.SuspendLayout();
            // 
            // slideControl
            // 
            this.slideControl.Location = new System.Drawing.Point(10, 1);
            this.slideControl.Name = "slideControl";
            this.slideControl.Size = new System.Drawing.Size(10, 20);
            this.slideControl.TabIndex = 0;
            this.slideControl.TabStop = false;
            this.slideControl.Paint += new System.Windows.Forms.PaintEventHandler(this.slideControl_Paint);
            this.slideControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.slideControl_MouseDown);
            this.slideControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.slideControl_MouseMove);
            this.slideControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.slideControl_MouseUp);
            // 
            // Slider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.slideControl);
            this.MaximumSize = new System.Drawing.Size(1000, 22);
            this.MinimumSize = new System.Drawing.Size(100, 22);
            this.Name = "Slider";
            this.Size = new System.Drawing.Size(100, 22);
            ((System.ComponentModel.ISupportInitialize)(this.slideControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox slideControl;
    }
}
