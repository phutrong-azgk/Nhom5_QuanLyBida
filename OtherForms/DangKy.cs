using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_QuanLyBida.Forms
{
    public partial class DangKy : Form
    {
        public string[] vaitro = { "ADMIN", "Quản Lý", "Thu Ngân", "Thủ Kho" };
        public DangKy()
        {
            InitializeComponent();
        }

        private void DangKy_Load(object sender, EventArgs e)
        {
            txtTenDN.Focus();
            cmbVaiTro.Items.AddRange(vaitro);
            cmbVaiTro.SelectedIndex = 0;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro) VALUES (@tenDN, @matkhau, @vaitro)";
                using(SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    cmd.Parameters.AddWithValue("@tenDN", txtTenDN.Text);
                    cmd.Parameters.AddWithValue("@matkhau", txtMatKhau.Text);
                    cmd.Parameters.AddWithValue("@vaitro", cmbVaiTro.SelectedItem.ToString());
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627) // Mã lỗi trùng khóa chính
                        {
                            MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi đăng ký tài khoản: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
