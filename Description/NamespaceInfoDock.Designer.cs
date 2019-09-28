namespace Description
{
    partial class NamespaceInfoDock
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
            this.label1 = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._descTextBox = new System.Windows.Forms.TextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancleButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称:";
            // 
            // _nameTextBox
            // 
            this._nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(74, 13);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(396, 25);
            this._nameTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "描述:";
            // 
            // _descTextBox
            // 
            this._descTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._descTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._descTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._descTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._descTextBox.Location = new System.Drawing.Point(74, 44);
            this._descTextBox.Name = "_descTextBox";
            this._descTextBox.Size = new System.Drawing.Size(396, 25);
            this._descTextBox.TabIndex = 4;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.ForeColor = System.Drawing.Color.LightGray;
            this._okButton.Location = new System.Drawing.Point(108, 87);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(80, 30);
            this._okButton.TabIndex = 5;
            this._okButton.Text = "保存";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancleButton
            // 
            this._cancleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._cancleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancleButton.ForeColor = System.Drawing.Color.LightGray;
            this._cancleButton.Location = new System.Drawing.Point(306, 87);
            this._cancleButton.Name = "_cancleButton";
            this._cancleButton.Size = new System.Drawing.Size(80, 30);
            this._cancleButton.TabIndex = 6;
            this._cancleButton.Text = "取消";
            this._cancleButton.UseVisualStyleBackColor = true;
            this._cancleButton.Click += new System.EventHandler(this.CancleButton_Click);
            // 
            // NamespaceInfoDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(482, 133);
            this.ControlBox = false;
            this.Controls.Add(this._cancleButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._descTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.LightGray;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 180);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 180);
            this.Name = "NamespaceInfoDock";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "命名空间属性";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _descTextBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancleButton;
    }
}