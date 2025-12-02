namespace Nhom5_QuanLyBida
{
    partial class UserControl_NhanVien
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSuaNV = new System.Windows.Forms.Button();
            this.btnThemNV = new System.Windows.Forms.Button();
            this.btnXoaNV = new System.Windows.Forms.Button();
            this.tableLayoutPanelNhanVien = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanelNhanVien = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanelNhanVien.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.05882F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.94118F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 178F));
            this.tableLayoutPanel2.Controls.Add(this.btnSuaNV, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnThemNV, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnXoaNV, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(148, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(537, 70);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnSuaNV
            // 
            this.btnSuaNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSuaNV.Location = new System.Drawing.Point(361, 3);
            this.btnSuaNV.Name = "btnSuaNV";
            this.btnSuaNV.Size = new System.Drawing.Size(173, 64);
            this.btnSuaNV.TabIndex = 4;
            this.btnSuaNV.Text = "Sửa nhân viên";
            this.btnSuaNV.UseVisualStyleBackColor = true;
            this.btnSuaNV.Click += new System.EventHandler(this.btnSuaNV_Click);
            // 
            // btnThemNV
            // 
            this.btnThemNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnThemNV.Location = new System.Drawing.Point(3, 3);
            this.btnThemNV.Name = "btnThemNV";
            this.btnThemNV.Size = new System.Drawing.Size(162, 64);
            this.btnThemNV.TabIndex = 2;
            this.btnThemNV.Text = "Thêm nhân viên";
            this.btnThemNV.UseVisualStyleBackColor = true;
            this.btnThemNV.Click += new System.EventHandler(this.btnThemNV_Click);
            // 
            // btnXoaNV
            // 
            this.btnXoaNV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXoaNV.Location = new System.Drawing.Point(171, 3);
            this.btnXoaNV.Name = "btnXoaNV";
            this.btnXoaNV.Size = new System.Drawing.Size(184, 64);
            this.btnXoaNV.TabIndex = 3;
            this.btnXoaNV.Text = "Xóa nhân viên";
            this.btnXoaNV.UseVisualStyleBackColor = true;
            this.btnXoaNV.Click += new System.EventHandler(this.btnXoaNV_Click);
            // 
            // tableLayoutPanelNhanVien
            // 
            this.tableLayoutPanelNhanVien.ColumnCount = 2;
            this.tableLayoutPanelNhanVien.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.15677F));
            this.tableLayoutPanelNhanVien.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.84322F));
            this.tableLayoutPanelNhanVien.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelNhanVien.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanelNhanVien.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelNhanVien.Name = "tableLayoutPanelNhanVien";
            this.tableLayoutPanelNhanVien.RowCount = 1;
            this.tableLayoutPanelNhanVien.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelNhanVien.Size = new System.Drawing.Size(688, 76);
            this.tableLayoutPanelNhanVien.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 19.8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quản lý nhân viên";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanelNhanVien, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 85);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(691, 316);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // flowLayoutPanelNhanVien
            // 
            this.flowLayoutPanelNhanVien.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelNhanVien.AutoScroll = true;
            this.flowLayoutPanelNhanVien.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelNhanVien.Name = "flowLayoutPanelNhanVien";
            this.flowLayoutPanelNhanVien.Size = new System.Drawing.Size(685, 310);
            this.flowLayoutPanelNhanVien.TabIndex = 0;
            // 
            // UserControl_NhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelNhanVien);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserControl_NhanVien";
            this.Size = new System.Drawing.Size(698, 413);
            this.Load += new System.EventHandler(this.UserControl_NhanVien_Load_1);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanelNhanVien.ResumeLayout(false);
            this.tableLayoutPanelNhanVien.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSuaNV;
        private System.Windows.Forms.Button btnThemNV;
        private System.Windows.Forms.Button btnXoaNV;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNhanVien;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelNhanVien;
    }
}
