using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_QuanLyBida
{
    public partial class ThemKhack : Form
    {
        public string MaKhach { get; set; }
        public string TenKhach { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TongChi { get; set; }
        public ThemKhack()
        {
            InitializeComponent();
            SoHoaDon = 0;
            TongChi = 0;
        }

        private void ThemKhack_Load(object sender, EventArgs e)
        {
            txtSHD.Enabled = false;
            txtSHD.Text = "0";
            txtTongChi.Enabled = false;
            txtTongChi.Text = "0";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhach.Text) || string.IsNullOrWhiteSpace(txtTenKhach.Text) ||
                string.IsNullOrWhiteSpace(txtSHD.Text) || string.IsNullOrWhiteSpace(txtTongChi.Text))
            {
                MessageBox.Show("Phải điền thông tin đầy đủ");
                return;
            }
            MaKhach = txtMaKhach.Text.Trim();
            TenKhach = txtTenKhach.Text.Trim();
            SoHoaDon = int.Parse(txtSHD.Text.Trim());
            TongChi = decimal.Parse(txtTongChi.Text.Trim());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
