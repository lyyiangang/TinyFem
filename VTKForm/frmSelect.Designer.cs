namespace TinyFem.VTKForm
{
    partial class frmSelect
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_rbtUnSelect = new System.Windows.Forms.RadioButton();
            this.m_rbtSelect = new System.Windows.Forms.RadioButton();
            this.m_lblObjectCount = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rbtRectPick = new System.Windows.Forms.RadioButton();
            this.m_rbtDotPick = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_rbtUnSelect);
            this.groupBox1.Controls.Add(this.m_rbtSelect);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择类型";
            // 
            // m_rbtUnSelect
            // 
            this.m_rbtUnSelect.AutoSize = true;
            this.m_rbtUnSelect.Location = new System.Drawing.Point(21, 44);
            this.m_rbtUnSelect.Name = "m_rbtUnSelect";
            this.m_rbtUnSelect.Size = new System.Drawing.Size(47, 16);
            this.m_rbtUnSelect.TabIndex = 1;
            this.m_rbtUnSelect.Text = "反选";
            this.m_rbtUnSelect.UseVisualStyleBackColor = true;
            // 
            // m_rbtSelect
            // 
            this.m_rbtSelect.AutoSize = true;
            this.m_rbtSelect.Checked = true;
            this.m_rbtSelect.Location = new System.Drawing.Point(21, 21);
            this.m_rbtSelect.Name = "m_rbtSelect";
            this.m_rbtSelect.Size = new System.Drawing.Size(47, 16);
            this.m_rbtSelect.TabIndex = 0;
            this.m_rbtSelect.TabStop = true;
            this.m_rbtSelect.Text = "选择";
            this.m_rbtSelect.UseVisualStyleBackColor = true;
            // 
            // m_lblObjectCount
            // 
            this.m_lblObjectCount.AutoSize = true;
            this.m_lblObjectCount.Location = new System.Drawing.Point(12, 195);
            this.m_lblObjectCount.Name = "m_lblObjectCount";
            this.m_lblObjectCount.Size = new System.Drawing.Size(77, 12);
            this.m_lblObjectCount.TabIndex = 2;
            this.m_lblObjectCount.Text = "选择对象个数";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.m_rbtRectPick);
            this.groupBox2.Controls.Add(this.m_rbtDotPick);
            this.groupBox2.Location = new System.Drawing.Point(14, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 77);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "选择方式";
            // 
            // m_rbtRectPick
            // 
            this.m_rbtRectPick.AutoSize = true;
            this.m_rbtRectPick.Location = new System.Drawing.Point(21, 44);
            this.m_rbtRectPick.Name = "m_rbtRectPick";
            this.m_rbtRectPick.Size = new System.Drawing.Size(47, 16);
            this.m_rbtRectPick.TabIndex = 1;
            this.m_rbtRectPick.Text = "框选";
            this.m_rbtRectPick.UseVisualStyleBackColor = true;
            // 
            // m_rbtDotPick
            // 
            this.m_rbtDotPick.AutoSize = true;
            this.m_rbtDotPick.Checked = true;
            this.m_rbtDotPick.Location = new System.Drawing.Point(21, 21);
            this.m_rbtDotPick.Name = "m_rbtDotPick";
            this.m_rbtDotPick.Size = new System.Drawing.Size(47, 16);
            this.m_rbtDotPick.TabIndex = 0;
            this.m_rbtDotPick.TabStop = true;
            this.m_rbtDotPick.Text = "点选";
            this.m_rbtDotPick.UseVisualStyleBackColor = true;
            // 
            // frmSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 356);
            this.Controls.Add(this.m_lblObjectCount);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSelect";
            this.Text = "选择";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelect_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton m_rbtUnSelect;
        private System.Windows.Forms.RadioButton m_rbtSelect;
        private System.Windows.Forms.Label m_lblObjectCount;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton m_rbtRectPick;
        private System.Windows.Forms.RadioButton m_rbtDotPick;
    }
}