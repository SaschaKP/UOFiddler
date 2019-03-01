/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

namespace FiddlerPlugin
{
    partial class UserControl1
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.text1 = new System.Windows.Forms.TextBox();
            this.text2 = new System.Windows.Forms.TextBox();
            this.button1i = new System.Windows.Forms.Button();
            this.text1i = new System.Windows.Forms.TextBox();
            this.text2i = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.text3 = new System.Windows.Forms.TextBox();
            this.text4 = new System.Windows.Forms.TextBox();
            this.button2i = new System.Windows.Forms.Button();
            this.text3i = new System.Windows.Forms.TextBox();
            this.text4i = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(160, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 25);
            this.button1.TabIndex = 0;
            this.button1.Text = "Esegui Mass Export - Item";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.onClickExportStatic);
            // text1
            text1.Location = new System.Drawing.Point(20, 40);
            text1.Name = "text1";
            text1.Size = new System.Drawing.Size(50, 25);
            text1.Text = "test";
            text1.TabIndex = 1;
            // text2
            text2.Location = new System.Drawing.Point(80, 40);
            text2.Name = "text2";
            text2.Size = new System.Drawing.Size(50, 25);
            text2.Text = "test2";
            text2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1i.Location = new System.Drawing.Point(160, 70);
            this.button1i.Name = "button1";
            this.button1i.Size = new System.Drawing.Size(145, 25);
            this.button1i.TabIndex = 0;
            this.button1i.Text = "Esegui Mass Import - Item";
            this.button1i.UseVisualStyleBackColor = true;
            this.button1i.Click += new System.EventHandler(this.onClickImportStatic);
            // text1i
            text1i.Location = new System.Drawing.Point(20, 70);
            text1i.Name = "text1";
            text1i.Size = new System.Drawing.Size(50, 25);
            text1i.Text = "test";
            text1i.TabIndex = 1;
            // text2
            text2i.Location = new System.Drawing.Point(80, 70);
            text2i.Name = "text2";
            text2i.Size = new System.Drawing.Size(50, 25);
            text2i.Text = "test2";
            text2i.TabIndex = 1;
			// 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(160, 100);
            this.button2.Name = "button1";
            this.button2.Size = new System.Drawing.Size(145, 25);
            this.button2.TabIndex = 0;
            this.button2.Text = "Esegui Mass Export - Land";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.onClickExportLand);
            // text3
            text3.Location = new System.Drawing.Point(20, 100);
            text3.Name = "text3";
            text3.Size = new System.Drawing.Size(50, 25);
            text3.Text = "test3";
            text3.TabIndex = 1;
            // text4
            text4.Location = new System.Drawing.Point(80, 100);
            text4.Name = "text4";
            text4.Size = new System.Drawing.Size(50, 25);
            text4.Text = "test4";
            text4.TabIndex = 1;
            // 
            // button2i
            // 
            this.button2i.Location = new System.Drawing.Point(160, 130);
            this.button2i.Name = "button1";
            this.button2i.Size = new System.Drawing.Size(145, 25);
            this.button2i.TabIndex = 0;
            this.button2i.Text = "Esegui Mass Import - Land";
            this.button2i.UseVisualStyleBackColor = true;
            this.button2i.Click += new System.EventHandler(this.onClickImportLand);
            // text3
            text3i.Location = new System.Drawing.Point(20, 130);
            text3i.Name = "text3";
            text3i.Size = new System.Drawing.Size(50, 25);
            text3i.Text = "test3";
            text3i.TabIndex = 1;
            // text4
            text4i.Location = new System.Drawing.Point(80, 130);
            text4i.Name = "text4";
            text4i.Size = new System.Drawing.Size(50, 25);
            text4i.Text = "test4";
            text4i.TabIndex = 1;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(text1);
            this.Controls.Add(text2);
            this.Controls.Add(this.button1);
            this.Controls.Add(text1i);
            this.Controls.Add(text2i);
            this.Controls.Add(this.button1i);
            this.Controls.Add(text3);
            this.Controls.Add(text4);
            this.Controls.Add(this.button2);
            this.Controls.Add(text3i);
            this.Controls.Add(text4i);
            this.Controls.Add(this.button2i);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(489, 301);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox text1;
        private System.Windows.Forms.TextBox text2;
        private System.Windows.Forms.Button button1i;
        private System.Windows.Forms.TextBox text1i;
        private System.Windows.Forms.TextBox text2i;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox text3;
        private System.Windows.Forms.TextBox text4;
        private System.Windows.Forms.Button button2i;
        private System.Windows.Forms.TextBox text3i;
        private System.Windows.Forms.TextBox text4i;
    }
}
