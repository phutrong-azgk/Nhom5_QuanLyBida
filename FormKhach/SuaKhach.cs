using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_QuanLyBida
{
    public partial class SuaKhach : Form
    {
        public string MaKhach { get; set; }
        public string TenKhach { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TongChi { get; set; }
        public SuaKhach()
        {
            InitializeComponent();
        }

        private void SuaKhach_Load(object sender, EventArgs e)
        {
            txtMaKhach.Text = MaKhach;
            txtTenKhach.Text = TenKhach;
            txtSHD.Text = SoHoaDon.ToString();
            txtTongChi.Text = TongChi.ToString();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaKhach.Text) || string.IsNullOrWhiteSpace(txtTenKhach.Text) ||
               string.IsNullOrWhiteSpace(txtSHD.Text) || string.IsNullOrWhiteSpace(txtTongChi.Text))
            {
                MessageBox.Show("Phải điền thông tin đầy đủ");
                return;
            }
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string sql = "UPDATE Khach SET TenKhach=@ten, SoHoaDon=@hd, TongChi=@chi WHERE MaKhach=@ma";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ten", txtTenKhach.Text);
                    cmd.Parameters.AddWithValue("@hd", int.Parse(txtSHD.Text));
                    cmd.Parameters.AddWithValue("@chi", decimal.Parse(txtTongChi.Text));
                    cmd.Parameters.AddWithValue("@ma", txtMaKhach.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Sửa khách thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message);
            }
        }
    }
}
