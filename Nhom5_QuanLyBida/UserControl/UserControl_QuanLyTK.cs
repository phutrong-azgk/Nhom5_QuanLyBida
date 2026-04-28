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
    public partial class UserControl_QuanLyTK : UserControl
    {
        public UserControl_QuanLyTK()
        {
            InitializeComponent();

            dgvTaiKhoan.AutoGenerateColumns = false;
            TenDangNhap.DataPropertyName = "TenDangNhap";
            MatKhau.DataPropertyName = "MatKhau";
            VaiTro.DataPropertyName = "VaiTro";
        }


        private void UserControl_QuanLyTK_Load(object sender, EventArgs e)
        {
            dgvTaiKhoan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiKhoan.MultiSelect = false;
            dgvTaiKhoan.ReadOnly = true;
            dgvTaiKhoan.AllowUserToAddRows = false;
           

            LoadTaiKhoan();
            LoadVaiTro();

        }
        private void LoadTaiKhoan()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT TenDangNhap,MatKhau, VaiTro FROM TaiKhoan", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTaiKhoan.DataSource = dt;
            }
        }

        private void LoadVaiTro()
        {
            cboLoc.Items.Clear();

            string[] roles = { "ADMIN", "Thu Ngân", "Quản Lý", "Thủ Kho" };

            cboLoc.Items.AddRange(roles);
        }


        private void btnXoaTK_Click_1(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa!");
                return;
            }

            string user = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value.ToString();

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?",
                "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM TaiKhoan WHERE TenDangNhap=@user", conn);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
            }

            LoadTaiKhoan();
        }

        private void btnHienThiDS_Click_1(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            cboLoc.SelectedIndex = -1;
            LoadTaiKhoan();
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TenDangNhap,MatKhau, VaiTro FROM TaiKhoan WHERE TenDangNhap LIKE @search", conn);

                da.SelectCommand.Parameters.AddWithValue("@search", "%" + txtTimKiem.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTaiKhoan.DataSource = dt;
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT TenDangNhap,MatKhau, VaiTro FROM TaiKhoan WHERE VaiTro=@role", conn);
                da.SelectCommand.Parameters.AddWithValue("@role", cboLoc.Text);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvTaiKhoan.DataSource = dt;
            }
        }

        private void btnThemTK_Click_1(object sender, EventArgs e)
        {
            DangKy f = new DangKy();

            if (f.ShowDialog() == DialogResult.OK)
            {
                LoadTaiKhoan();  // Load lại danh sách
            }
            LoadTaiKhoan();
        }

        private void btnSuaTK_Click_1(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản để sửa!");
                return;
            }

            string user = dgvTaiKhoan.CurrentRow.Cells["TenDangNhap"].Value.ToString();
            string pass = dgvTaiKhoan.CurrentRow.Cells["MatKhau"].Value.ToString();
            string role = dgvTaiKhoan.CurrentRow.Cells["VaiTro"].Value.ToString();

            // ====== Tạo form popup ========
            Form popup = new Form()
            {
                Width = 350,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Sửa Tài Khoản",
                StartPosition = FormStartPosition.CenterParent
            };

            // Label + textbox tên đăng nhập
            Label lblUser = new Label() { Text = "Tên Đăng Nhập:", Left = 20, Top = 20, Width = 120 };
            TextBox txtUser = new TextBox() { Left = 150, Top = 20, Width = 150, Text = user, ReadOnly = true };

            // Label + textbox mật khẩu
            Label lblPass = new Label() { Text = "Mật Khẩu:", Left = 20, Top = 60, Width = 120 };
            TextBox txtPass = new TextBox() { Left = 150, Top = 60, Width = 150, Text = pass };

            // Label + combobox vai trò
            Label lblRole = new Label() { Text = "Vai Trò:", Left = 20, Top = 100, Width = 120 };
            ComboBox cboRole = new ComboBox() { Left = 150, Top = 100, Width = 150 };
            cboRole.Items.AddRange(new string[] { "ADMIN", "Thu Ngân", "Quản Lý", "Thủ Kho" });
            cboRole.Text = role;

            Button btnOK = new Button() { Text = "Lưu", Left = 70, Width = 80, Top = 150, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "Hủy", Left = 180, Width = 80, Top = 150, DialogResult = DialogResult.Cancel };

            popup.Controls.Add(lblUser);
            popup.Controls.Add(txtUser);
            popup.Controls.Add(lblPass);
            popup.Controls.Add(txtPass);
            popup.Controls.Add(lblRole);
            popup.Controls.Add(cboRole);
            popup.Controls.Add(btnOK);
            popup.Controls.Add(btnCancel);

            popup.AcceptButton = btnOK;
            popup.CancelButton = btnCancel;

            // ====== Nếu nhấn Lưu ======
            if (popup.ShowDialog() == DialogResult.OK)
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE TaiKhoan SET MatKhau=@pass, VaiTro=@role WHERE TenDangNhap=@user", conn);

                    cmd.Parameters.AddWithValue("@pass", txtPass.Text);
                    cmd.Parameters.AddWithValue("@role", cboRole.Text);
                    cmd.Parameters.AddWithValue("@user", user);

                    cmd.ExecuteNonQuery();
                }

                LoadTaiKhoan(); // load lại bảng
            }
        }

    }
}
