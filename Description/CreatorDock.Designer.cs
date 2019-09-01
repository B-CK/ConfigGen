namespace Description
{
    partial class CreatorDock
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
            this._2Label = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancleButton = new System.Windows.Forms.Button();
            this._createListBox = new System.Windows.Forms.ListBox();
            this._2ComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _2Label
            // 
            this._2Label.AutoSize = true;
            this._2Label.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._2Label.ForeColor = System.Drawing.Color.LightGray;
            this._2Label.Location = new System.Drawing.Point(7, 280);
            this._2Label.Name = "_2Label";
            this._2Label.Size = new System.Drawing.Size(75, 15);
            this._2Label.TabIndex = 1;
            this._2Label.Text = "命名空间:";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NameLabel.ForeColor = System.Drawing.Color.LightGray;
            this.NameLabel.Location = new System.Drawing.Point(7, 249);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(45, 15);
            this.NameLabel.TabIndex = 1;
            this.NameLabel.Text = "名称:";
            // 
            // _nameTextBox
            // 
            this._nameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._nameTextBox.ForeColor = System.Drawing.Color.LightGray;
            this._nameTextBox.Location = new System.Drawing.Point(105, 243);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(292, 27);
            this._nameTextBox.TabIndex = 2;
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._okButton.ForeColor = System.Drawing.Color.LightGray;
            this._okButton.Location = new System.Drawing.Point(207, 363);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(88, 38);
            this._okButton.TabIndex = 11;
            this._okButton.Text = "确定";
            this._okButton.UseVisualStyleBackColor = false;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancleButton
            // 
            this._cancleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancleButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._cancleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancleButton.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._cancleButton.ForeColor = System.Drawing.Color.LightGray;
            this._cancleButton.Location = new System.Drawing.Point(309, 363);
            this._cancleButton.Name = "_cancleButton";
            this._cancleButton.Size = new System.Drawing.Size(88, 38);
            this._cancleButton.TabIndex = 10;
            this._cancleButton.Text = "取消";
            this._cancleButton.UseVisualStyleBackColor = false;
            this._cancleButton.Click += new System.EventHandler(this.CancleButton_Click);
            // 
            // _createListBox
            // 
            this._createListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._createListBox.Font = new System.Drawing.Font("宋体", 14F);
            this._createListBox.ForeColor = System.Drawing.Color.LightGray;
            this._createListBox.FormattingEnabled = true;
            this._createListBox.ItemHeight = 23;
            this._createListBox.Items.AddRange(new object[] {
            "> Class类型",
            "> Enum类型",
            "> 命名空间"});
            this._createListBox.Location = new System.Drawing.Point(1, 2);
            this._createListBox.Name = "_createListBox";
            this._createListBox.Size = new System.Drawing.Size(404, 234);
            this._createListBox.TabIndex = 12;
            this._createListBox.SelectedIndexChanged += new System.EventHandler(this.CreateListBox_SelectedIndexChanged);
            // 
            // _2ComboBox
            // 
            this._2ComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._2ComboBox.ForeColor = System.Drawing.Color.LightGray;
            this._2ComboBox.FormattingEnabled = true;
            this._2ComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._2ComboBox.Location = new System.Drawing.Point(105, 275);
            this._2ComboBox.Name = "_2ComboBox";
            this._2ComboBox.Size = new System.Drawing.Size(292, 25);
            this._2ComboBox.TabIndex = 13;
            // 
            // CreatorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(407, 413);
            this.Controls.Add(this._2ComboBox);
            this.Controls.Add(this._createListBox);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this._cancleButton);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._2Label);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreatorDock";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "创建内容";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _2Label;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancleButton;
        private System.Windows.Forms.ListBox _createListBox;
        private System.Windows.Forms.ComboBox _2ComboBox;
    }
}