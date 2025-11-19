using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_QuanLyBida.FormNhanVien
{
    public partial class ThemNhanVien : Form
    {
        public string MaNV{ get; set; }
        public string TenNV { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public ThemNhanVien()
        {
            InitializeComponent();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtTenNV.Text) ||
                string.IsNullOrWhiteSpace(txtNgaySinh.Text) || string.IsNullOrWhiteSpace(txtSDT.Text) || 
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Phải điền thông tin đầy đủ");
                return;
            }
            MaNV = txtMaNV.Text.Trim();
            TenNV = txtTenNV.Text.Trim();
            NgaySinh = DateTime.Parse(txtNgaySinh.Text.Trim());
            if (rdoNam.Checked)
                GioiTinh = "Nam";
            else
                GioiTinh = "Nữ";
            SDT = txtSDT.Text.Trim();
            DiaChi = txtDiaChi.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
