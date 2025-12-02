namespace Nhom5_QuanLyBida
{
    partial class UserControl_Khach
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
            this.btnSuaKhach = new System.Windows.Forms.Button();
            this.btnThemKhach = new System.Windows.Forms.Button();
            this.btnXoaKhach = new System.Windows.Forms.Button();
            this.tableLayoutPanelNhanVien = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanelKhach = new System.Windows.Forms.FlowLayoutPanel();
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
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.tableLayoutPanel2.Controls.Add(this.btnSuaKhach, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnThemKhach, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnXoaKhach, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(142, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(516, 74);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnSuaKhach
            // 
            this.btnSuaKhach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSuaKhach.Location = new System.Drawing.Point(335, 3);
            this.btnSuaKhach.Name = "btnSuaKhach";
            this.btnSuaKhach.Size = new System.Drawing.Size(178, 68);
            this.btnSuaKhach.TabIndex = 4;
            this.btnSuaKhach.Text = "Sửa khách";
            this.btnSuaKhach.UseVisualStyleBackColor = true;
            this.btnSuaKhach.Click += new System.EventHandler(this.btnSuaKhach_Click_1);
            // 
            // btnThemKhach
            // 
            this.btnThemKhach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnThemKhach.Location = new System.Drawing.Point(3, 3);
            this.btnThemKhach.Name = "btnThemKhach";
            this.btnThemKhach.Size = new System.Drawing.Size(150, 68);
            this.btnThemKhach.TabIndex = 2;
            this.btnThemKhach.Text = "Thêm khách";
            this.btnThemKhach.UseVisualStyleBackColor = true;
            this.btnThemKhach.Click += new System.EventHandler(this.btnThemKhach_Click_1);
            // 
            // btnXoaKhach
            // 
            this.btnXoaKhach.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnXoaKhach.Location = new System.Drawing.Point(159, 3);
            this.btnXoaKhach.Name = "btnXoaKhach";
            this.btnXoaKhach.Size = new System.Drawing.Size(170, 68);
            this.btnXoaKhach.TabIndex = 3;
            this.btnXoaKhach.Text = "Xóa khách";
            this.btnXoaKhach.UseVisualStyleBackColor = true;
            this.btnXoaKhach.Click += new System.EventHandler(this.btnXoaKhach_Click_1);
            // 
            // tableLayoutPanelNhanVien
            // 
            this.tableLayoutPanelNhanVien.ColumnCount = 2;
            this.tableLayoutPanelNhanVien.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.15677F));
            this.tableLayoutPanelNhanVien.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.84322F));
            this.tableLayoutPanelNhanVien.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanelNhanVien.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelNhanVien.Location = new System.Drawing.Point(3, 6);
            this.tableLayoutPanelNhanVien.Name = "tableLayoutPanelNhanVien";
            this.tableLayoutPanelNhanVien.RowCount = 1;
            this.tableLayoutPanelNhanVien.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelNhanVien.Size = new System.Drawing.Size(661, 80);
            this.tableLayoutPanelNhanVien.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift", 19.8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 66);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quản lý khách";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanelKhach, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 89);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(662, 305);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanelKhach
            // 
            this.flowLayoutPanelKhach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelKhach.AutoScroll = true;
            this.flowLayoutPanelKhach.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelKhach.Name = "flowLayoutPanelKhach";
            this.flowLayoutPanelKhach.Size = new System.Drawing.Size(656, 299);
            this.flowLayoutPanelKhach.TabIndex = 0;
            // 
            // UserControl_Khach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelNhanVien);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserControl_Khach";
            this.Size = new System.Drawing.Size(669, 397);
            this.Load += new System.EventHandler(this.UserControl_Khach_Load_1);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanelNhanVien.ResumeLayout(false);
            this.tableLayoutPanelNhanVien.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSuaKhach;
        private System.Windows.Forms.Button btnThemKhach;
        private System.Windows.Forms.Button btnXoaKhach;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelNhanVien;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelKhach;
    }
}
