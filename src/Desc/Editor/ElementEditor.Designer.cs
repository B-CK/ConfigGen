namespace Desc.Editor
{
    partial class ElementEditor
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
            this._cancleButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._label1 = new System.Windows.Forms.Label();
            this._typeComboBox1 = new System.Windows.Forms.ComboBox();
            this._label2 = new System.Windows.Forms.Label();
            this._typeComboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // _cancleButton
            // 
            this._cancleButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cancleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._cancleButton.Location = new System.Drawing.Point(225, 86);
            this._cancleButton.Name = "_cancleButton";
            this._cancleButton.Size = new System.Drawing.Size(75, 31);
            this._cancleButton.TabIndex = 3;
            this._cancleButton.Text = "取消";
            this._cancleButton.UseVisualStyleBackColor = true;
            this._cancleButton.Click += new System.EventHandler(this.CancleButton_Click);
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._okButton.Location = new System.Drawing.Point(93, 85);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 31);
            this._okButton.TabIndex = 4;
            this._okButton.Text = "确定";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _label1
            // 
            this._label1.AutoSize = true;
            this._label1.Location = new System.Drawing.Point(9, 14);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(75, 15);
            this._label1.TabIndex = 5;
            this._label1.Text = "元素类型:";
            // 
            // _typeComboBox1
            // 
            this._typeComboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._typeComboBox1.ForeColor = System.Drawing.Color.LightGray;
            this._typeComboBox1.FormattingEnabled = true;
            this._typeComboBox1.Location = new System.Drawing.Point(95, 10);
            this._typeComboBox1.Name = "_typeComboBox1";
            this._typeComboBox1.Size = new System.Drawing.Size(301, 23);
            this._typeComboBox1.TabIndex = 6;
            // 
            // _label2
            // 
            this._label2.AutoSize = true;
            this._label2.Location = new System.Drawing.Point(9, 43);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(85, 15);
            this._label2.TabIndex = 5;
            this._label2.Text = "Value类型:";
            // 
            // _typeComboBox2
            // 
            this._typeComboBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this._typeComboBox2.ForeColor = System.Drawing.Color.LightGray;
            this._typeComboBox2.FormattingEnabled = true;
            this._typeComboBox2.Location = new System.Drawing.Point(95, 39);
            this._typeComboBox2.Name = "_typeComboBox2";
            this._typeComboBox2.Size = new System.Drawing.Size(301, 23);
            this._typeComboBox2.TabIndex = 6;
            // 
            // ElementEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(406, 136);
            this.ControlBox = false;
            this.Controls.Add(this._typeComboBox2);
            this.Controls.Add(this._label2);
            this.Controls.Add(this._typeComboBox1);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._cancleButton);
            this.Controls.Add(this._okButton);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.LightGray;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(424, 183);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(424, 183);
            this.Name = "ElementEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "复合类型";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _cancleButton;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Label _label1;
        private System.Windows.Forms.ComboBox _typeComboBox1;
        private System.Windows.Forms.Label _label2;
        private System.Windows.Forms.ComboBox _typeComboBox2;
    }
}