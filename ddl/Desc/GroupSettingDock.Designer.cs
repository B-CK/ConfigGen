namespace Desc
{
    partial class GroupSettingDock
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this._groupGridView = new System.Windows.Forms.DataGridView();
            this.Group = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._cancleButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._groupGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // _groupGridView
            // 
            this._groupGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._groupGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this._groupGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._groupGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Group});
            this._groupGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._groupGridView.Location = new System.Drawing.Point(3, 1);
            this._groupGridView.Name = "_groupGridView";
            this._groupGridView.RowHeadersVisible = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._groupGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this._groupGridView.RowTemplate.Height = 27;
            this._groupGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._groupGridView.Size = new System.Drawing.Size(377, 243);
            this._groupGridView.TabIndex = 5;
            // 
            // Group
            // 
            this.Group.HeaderText = "分组定义";
            this.Group.Name = "Group";
            this.Group.Width = 377;
            // 
            // _cancleButton
            // 
            this._cancleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancleButton.Location = new System.Drawing.Point(254, 257);
            this._cancleButton.Name = "_cancleButton";
            this._cancleButton.Size = new System.Drawing.Size(82, 31);
            this._cancleButton.TabIndex = 6;
            this._cancleButton.Text = "取消";
            this._cancleButton.UseVisualStyleBackColor = true;
            this._cancleButton.Click += new System.EventHandler(this.CancleButton_Click);
            // 
            // _okButton
            // 
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.Location = new System.Drawing.Point(49, 257);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(82, 31);
            this._okButton.TabIndex = 7;
            this._okButton.Text = "确定";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // GroupSettingDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(384, 301);
            this.ControlBox = false;
            this.Controls.Add(this._cancleButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._groupGridView);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GroupSettingDock";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "分组定义";
            ((System.ComponentModel.ISupportInitialize)(this._groupGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView _groupGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Group;
        private System.Windows.Forms.Button _cancleButton;
        private System.Windows.Forms.Button _okButton;
    }
}