namespace Desc
{
    partial class GridWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MemberList = new System.Windows.Forms.DataGridView();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsOnlyRead = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FieldType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Elememt = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Checker = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.MemberList)).BeginInit();
            this.SuspendLayout();
            // 
            // MemberList
            // 
            this.MemberList.AllowDrop = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.LightGray;
            this.MemberList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.MemberList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.MemberList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MemberList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MemberList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FieldName,
            this.IsOnlyRead,
            this.FieldType,
            this.Elememt,
            this.Desc,
            this.Checker});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MemberList.DefaultCellStyle = dataGridViewCellStyle6;
            this.MemberList.GridColor = System.Drawing.SystemColors.GrayText;
            this.MemberList.Location = new System.Drawing.Point(3, 13);
            this.MemberList.Name = "MemberList";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MemberList.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.LightGray;
            this.MemberList.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.MemberList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.MemberList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LightGray;
            this.MemberList.RowTemplate.Height = 27;
            this.MemberList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MemberList.Size = new System.Drawing.Size(790, 123);
            this.MemberList.TabIndex = 0;
            this.MemberList.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MemberList_CellMouseMove);
            this.MemberList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.MemberList_RowsAdded);
            this.MemberList.Click += new System.EventHandler(this.MemberList_Click);
            this.MemberList.DragDrop += new System.Windows.Forms.DragEventHandler(this.MemberList_DragDrop);
            this.MemberList.DragEnter += new System.Windows.Forms.DragEventHandler(this.MemberList_DragEnter);
            // 
            // FieldName
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.LightGray;
            this.FieldName.DefaultCellStyle = dataGridViewCellStyle3;
            this.FieldName.HeaderText = "名称";
            this.FieldName.Name = "FieldName";
            this.FieldName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FieldName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // IsOnlyRead
            // 
            this.IsOnlyRead.HeaderText = "只读";
            this.IsOnlyRead.Name = "IsOnlyRead";
            this.IsOnlyRead.ReadOnly = true;
            this.IsOnlyRead.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsOnlyRead.Width = 50;
            // 
            // FieldType
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.FieldType.DefaultCellStyle = dataGridViewCellStyle4;
            this.FieldType.HeaderText = "类型";
            this.FieldType.Items.AddRange(new object[] {
            "Box",
            "Sharp",
            "Bob",
            "Jack"});
            this.FieldType.Name = "FieldType";
            this.FieldType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FieldType.Width = 150;
            // 
            // Elememt
            // 
            this.Elememt.HeaderText = "值/集合类";
            this.Elememt.Name = "Elememt";
            this.Elememt.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Elememt.Text = "";
            this.Elememt.UseColumnTextForButtonValue = true;
            this.Elememt.Width = 150;
            // 
            // Desc
            // 
            this.Desc.HeaderText = "描述";
            this.Desc.Name = "Desc";
            this.Desc.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Desc.Width = 150;
            // 
            // Checker
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.LightGray;
            this.Checker.DefaultCellStyle = dataGridViewCellStyle5;
            this.Checker.HeaderText = "检查条件";
            this.Checker.Name = "Checker";
            this.Checker.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Checker.Width = 150;
            // 
            // GridWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MemberList);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Name = "GridWindow";
            this.Text = "GridWindow";
            ((System.ComponentModel.ISupportInitialize)(this.MemberList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView MemberList;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsOnlyRead;
        private System.Windows.Forms.DataGridViewComboBoxColumn FieldType;
        private System.Windows.Forms.DataGridViewButtonColumn Elememt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.DataGridViewButtonColumn Checker;
    }
}