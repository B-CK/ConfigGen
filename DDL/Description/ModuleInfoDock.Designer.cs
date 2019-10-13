namespace Description
{
    partial class ModuleInfoDock
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this._nameTextBox = new System.Windows.Forms.TextBox();
            this._groupGridView = new System.Windows.Forms.DataGridView();
            this.Group = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._okButton = new System.Windows.Forms.Button();
            this._cancleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._groupGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.LightGray;
            this.label1.Location = new System.Drawing.Point(4, 17);
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
            this._nameTextBox.Location = new System.Drawing.Point(65, 12);
            this._nameTextBox.Name = "_nameTextBox";
            this._nameTextBox.Size = new System.Drawing.Size(326, 25);
            this._nameTextBox.TabIndex = 3;
            this._nameTextBox.TextChanged += new System.EventHandler(this.NameTextBox_TextChanged);
            // 
            // _groupGridView
            // 
            this._groupGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._groupGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this._groupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._groupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Group});
            this._groupGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._groupGridView.Location = new System.Drawing.Point(12, 44);
            this._groupGridView.Name = "_groupGridView";
            this._groupGridView.RowHeadersVisible = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._groupGridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this._groupGridView.RowTemplate.Height = 27;
            this._groupGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._groupGridView.Size = new System.Drawing.Size(377, 150);
            this._groupGridView.TabIndex = 4;
            // 
            // Group
            // 
            this.Group.HeaderText = "分组定义";
            this.Group.Name = "Group";
            this.Group.Width = 377;
            // 
            // _okButton
            // 
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.Location = new System.Drawing.Point(52, 225);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(82, 31);
            this._okButton.TabIndex = 5;
            this._okButton.Text = "确定";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _cancleButton
            // 
            this._cancleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancleButton.Location = new System.Drawing.Point(257, 225);
            this._cancleButton.Name = "_cancleButton";
            this._cancleButton.Size = new System.Drawing.Size(82, 31);
            this._cancleButton.TabIndex = 5;
            this._cancleButton.Text = "取消";
            this._cancleButton.UseVisualStyleBackColor = true;
            this._cancleButton.Click += new System.EventHandler(this.CancleButton_Click);
            // 
            // ModuleInfoDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(401, 278);
            this.ControlBox = false;
            this.Controls.Add(this._cancleButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._groupGridView);
            this.Controls.Add(this._nameTextBox);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModuleInfoDock";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "模块属性";
            ((System.ComponentModel.ISupportInitialize)(this._groupGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _nameTextBox;
        private System.Windows.Forms.DataGridView _groupGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Group;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancleButton;
    }
}