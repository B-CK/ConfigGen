namespace Description
{
    partial class TypeEditorDock
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
            this.MemberGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberSplitContainer = new System.Windows.Forms.SplitContainer();
            this._memberListBox = new System.Windows.Forms.ListBox();
            this._downPictureBox = new System.Windows.Forms.PictureBox();
            this._upPictureBox = new System.Windows.Forms.PictureBox();
            this._minusPictureBox = new System.Windows.Forms.PictureBox();
            this._plusPictureBox = new System.Windows.Forms.PictureBox();
            this._memPictureBox = new System.Windows.Forms.PictureBox();
            this._memFilterBox = new System.Windows.Forms.TextBox();
            this._typeGroupBox = new System.Windows.Forms.GroupBox();
            this.MemberGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).BeginInit();
            this.MemberSplitContainer.Panel1.SuspendLayout();
            this.MemberSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MemberGroupBox
            // 
            this.MemberGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberGroupBox.Controls.Add(this.MemberSplitContainer);
            this.MemberGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MemberGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this.MemberGroupBox.Location = new System.Drawing.Point(3, 215);
            this.MemberGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Name = "MemberGroupBox";
            this.MemberGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.MemberGroupBox.Size = new System.Drawing.Size(815, 378);
            this.MemberGroupBox.TabIndex = 6;
            this.MemberGroupBox.TabStop = false;
            this.MemberGroupBox.Text = "成员列表";
            // 
            // MemberSplitContainer
            // 
            this.MemberSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemberSplitContainer.Location = new System.Drawing.Point(3, 18);
            this.MemberSplitContainer.Name = "MemberSplitContainer";
            // 
            // MemberSplitContainer.Panel1
            // 
            this.MemberSplitContainer.Panel1.Controls.Add(this._memberListBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._downPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._upPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._minusPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._plusPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memPictureBox);
            this.MemberSplitContainer.Panel1.Controls.Add(this._memFilterBox);
            this.MemberSplitContainer.Panel1MinSize = 322;
            this.MemberSplitContainer.Size = new System.Drawing.Size(809, 357);
            this.MemberSplitContainer.SplitterDistance = 322;
            this.MemberSplitContainer.TabIndex = 0;
            // 
            // _memberListBox
            // 
            this._memberListBox.AllowDrop = true;
            this._memberListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._memberListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memberListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memberListBox.ForeColor = System.Drawing.Color.LightGray;
            this._memberListBox.FormattingEnabled = true;
            this._memberListBox.ItemHeight = 17;
            this._memberListBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0",
            "1",
            "1",
            "q",
            "w",
            "e",
            "r",
            "t",
            "y",
            "u",
            "i",
            "o",
            "a",
            "s",
            "d",
            "g",
            "h"});
            this._memberListBox.Location = new System.Drawing.Point(5, 46);
            this._memberListBox.Name = "_memberListBox";
            this._memberListBox.ScrollAlwaysVisible = true;
            this._memberListBox.Size = new System.Drawing.Size(309, 308);
            this._memberListBox.TabIndex = 8;
            // 
            // _downPictureBox
            // 
            this._downPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._downPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._downPictureBox.Image = global::Description.Properties.Resources.TriangleDown;
            this._downPictureBox.Location = new System.Drawing.Point(288, 10);
            this._downPictureBox.Name = "_downPictureBox";
            this._downPictureBox.Size = new System.Drawing.Size(26, 26);
            this._downPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._downPictureBox.TabIndex = 6;
            this._downPictureBox.TabStop = false;
            // 
            // _upPictureBox
            // 
            this._upPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._upPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._upPictureBox.Image = global::Description.Properties.Resources.TriangleUp;
            this._upPictureBox.Location = new System.Drawing.Point(256, 10);
            this._upPictureBox.Name = "_upPictureBox";
            this._upPictureBox.Size = new System.Drawing.Size(26, 26);
            this._upPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._upPictureBox.TabIndex = 6;
            this._upPictureBox.TabStop = false;
            // 
            // _minusPictureBox
            // 
            this._minusPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._minusPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._minusPictureBox.Image = global::Description.Properties.Resources.Minus;
            this._minusPictureBox.Location = new System.Drawing.Point(224, 10);
            this._minusPictureBox.Name = "_minusPictureBox";
            this._minusPictureBox.Size = new System.Drawing.Size(26, 26);
            this._minusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._minusPictureBox.TabIndex = 6;
            this._minusPictureBox.TabStop = false;
            // 
            // _plusPictureBox
            // 
            this._plusPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._plusPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._plusPictureBox.Image = global::Description.Properties.Resources.Plus;
            this._plusPictureBox.Location = new System.Drawing.Point(192, 10);
            this._plusPictureBox.Name = "_plusPictureBox";
            this._plusPictureBox.Size = new System.Drawing.Size(26, 26);
            this._plusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._plusPictureBox.TabIndex = 6;
            this._plusPictureBox.TabStop = false;
            // 
            // _memPictureBox
            // 
            this._memPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memPictureBox.Image = global::Description.Properties.Resources.MagnifyingGlass;
            this._memPictureBox.Location = new System.Drawing.Point(6, 10);
            this._memPictureBox.Name = "_memPictureBox";
            this._memPictureBox.Size = new System.Drawing.Size(26, 26);
            this._memPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._memPictureBox.TabIndex = 6;
            this._memPictureBox.TabStop = false;
            // 
            // _memFilterBox
            // 
            this._memFilterBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
            this._memFilterBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._memFilterBox.ForeColor = System.Drawing.Color.LightGray;
            this._memFilterBox.Location = new System.Drawing.Point(38, 10);
            this._memFilterBox.Name = "_memFilterBox";
            this._memFilterBox.Size = new System.Drawing.Size(144, 27);
            this._memFilterBox.TabIndex = 2;
            // 
            // _typeGroupBox
            // 
            this._typeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeGroupBox.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._typeGroupBox.ForeColor = System.Drawing.Color.LightGray;
            this._typeGroupBox.Location = new System.Drawing.Point(3, 2);
            this._typeGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Name = "_typeGroupBox";
            this._typeGroupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this._typeGroupBox.Size = new System.Drawing.Size(815, 202);
            this._typeGroupBox.TabIndex = 7;
            this._typeGroupBox.TabStop = false;
            this._typeGroupBox.Text = "类型";
            // 
            // TypeEditorDock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(822, 594);
            this.Controls.Add(this.MemberGroupBox);
            this.Controls.Add(this._typeGroupBox);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(838, 641);
            this.Name = "TypeEditorDock";
            this.ShowIcon = false;
            this.Text = "类型编辑";
            this.MemberGroupBox.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.ResumeLayout(false);
            this.MemberSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MemberSplitContainer)).EndInit();
            this.MemberSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._downPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._upPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._minusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._plusPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._memPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox MemberGroupBox;
        private System.Windows.Forms.SplitContainer MemberSplitContainer;
        private System.Windows.Forms.ListBox _memberListBox;
        private System.Windows.Forms.PictureBox _downPictureBox;
        private System.Windows.Forms.PictureBox _upPictureBox;
        private System.Windows.Forms.PictureBox _minusPictureBox;
        private System.Windows.Forms.PictureBox _plusPictureBox;
        private System.Windows.Forms.PictureBox _memPictureBox;
        private System.Windows.Forms.TextBox _memFilterBox;
        private System.Windows.Forms.GroupBox _typeGroupBox;
    }
}