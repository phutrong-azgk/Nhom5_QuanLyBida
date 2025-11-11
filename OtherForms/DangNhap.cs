using Nhom5_QuanLyBida.Forms;
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
    public partial class DangNhap : Form
    {
        public static string vaitro;
        public DangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            lblThongBao.ForeColor = Color.Black;
            if (txtMatKhau.Text=="" || txtTenDN.Text=="")
            {
                lblThongBao.Location = new Point(240, 190);
                lblThongBao.Text = "Vui lòng nhập đầy đủ thông tin!";
                return;
            }
            using (SqlConnection conn = DatabaseHelper.GetConnection())//mở kết nối
            {
                conn.Open();
                string sql = "SELECT VaiTro FROM TaiKhoan WHERE TenDangNhap=@tenDN AND MatKhau=@matkhau";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tenDN", txtTenDN.Text);
                    cmd.Parameters.AddWithValue("@matkhau", txtMatKhau.Text);
                    vaitro = cmd.ExecuteReader().Cast<IDataRecord>().Select(r => r.GetString(0)).FirstOrDefault();
                }
            }
            if (ktDangNhap(txtTenDN.Text, txtMatKhau.Text))
            {
                this.Hide();
                MainForm mf = new MainForm();
                mf.FormClosing += (s, args) => this.Close();
                mf.FormClosed += (s, args) => {
                    if (mf.IsLoggingOut)
                    {
                        // User logged out - show login form again
                        this.Show();
                        txtMatKhau.Clear();  
                        txtTenDN.Clear();    
                        lblThongBao.ForeColor = Color.FromArgb(192, 255, 255);
                    }
                    else
                    {
                        // User closed the app - close login form too
                        this.Close();
                    }
                    
                };
                
                mf.ShowDialog();
                
            }
            else
            {
                lblThongBao.Location = new Point(180, 190);
                lblThongBao.Text = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
        }
        public bool ktDangNhap(string tenDN,string matkhau)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())//mở kết nối
                {
                    conn.Open();
                    string sql = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap=@tenDN AND MatKhau=@matkhau";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@tenDN", tenDN);
                        cmd.Parameters.AddWithValue("@matkhau", matkhau);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Lỗi: " + ex);
                return false;
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            DangKy dk = new DangKy();
            dk.ShowDialog();
        }
    }
}
