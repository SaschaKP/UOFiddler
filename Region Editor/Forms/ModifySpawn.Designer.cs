namespace Region_Editor
{
    partial class ModifySpawn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModifySpawn));
            this.label1 = new System.Windows.Forms.Label();
            this.id = new Region_Editor.FilteredTextBox();
            this.type = new Region_Editor.FilteredTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.min = new Region_Editor.FilteredTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.max = new Region_Editor.FilteredTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.amount = new Region_Editor.FilteredTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.setButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // id
            // 
            this.id.AllowedChars = "1234567890";
            this.id.Location = new System.Drawing.Point(162, 9);
            this.id.MaxLength = 8;
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(44, 20);
            this.id.TabIndex = 1;
            // 
            // type
            // 
            this.type.AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";
            this.type.Location = new System.Drawing.Point(162, 35);
            this.type.MaxLength = 255;
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(264, 20);
            this.type.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Type";
            // 
            // min
            // 
            this.min.AllowedChars = "1234567890";
            this.min.Location = new System.Drawing.Point(162, 61);
            this.min.MaxLength = 5;
            this.min.Name = "min";
            this.min.Size = new System.Drawing.Size(57, 20);
            this.min.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Minimum Seconds to Spawn";
            // 
            // max
            // 
            this.max.AllowedChars = "1234567890";
            this.max.Location = new System.Drawing.Point(162, 87);
            this.max.MaxLength = 5;
            this.max.Name = "max";
            this.max.Size = new System.Drawing.Size(57, 20);
            this.max.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mazimum Seconds to Spawn";
            // 
            // amount
            // 
            this.amount.AllowedChars = "1234567890";
            this.amount.Location = new System.Drawing.Point(162, 113);
            this.amount.MaxLength = 3;
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(57, 20);
            this.amount.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Amount";
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(131, 146);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 10;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(226, 146);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ModifySpawn
            // 
            this.AcceptButton = this.setButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(433, 173);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.amount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.max);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.min);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.type);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.id);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ModifySpawn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Modify Spawn";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private FilteredTextBox id;
        private FilteredTextBox type;
        private System.Windows.Forms.Label label2;
        private FilteredTextBox min;
        private System.Windows.Forms.Label label3;
        private FilteredTextBox max;
        private System.Windows.Forms.Label label4;
        private FilteredTextBox amount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.Button cancelButton;
    }
}