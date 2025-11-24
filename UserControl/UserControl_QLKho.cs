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
    public partial class UserControl_QLKho : UserControl
    {
        public UserControl_QLKho()
        {
            InitializeComponent();
        }
        bool isEditing=false;
        private void btn_Them_Click(object sender, EventArgs e)
        {
            txtMaKho.Text = GenerateMaKho();

            // Clear dữ liệu
            txtSLNhap.Text = "0";
            txtSLXuat.Text = "0";
            dtNgayNhap.Value = DateTime.Now;

            // Cho phép chọn MÃ MÓN (vì đang tạo mới)
            cmbMaMon.Enabled = true;

            txtSLXuat.Enabled = false;

            // Cho nhập SL nhập hoặc SL xuất
            txtSLNhap.Enabled = true;
            txtSLXuat.Enabled = true;

            // Bật nút Lưu
            btn_Luu.Enabled = true;

            isEditing = false; // trạng thái thêm mới
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (txtMaKho.Text == "")
            {
                MessageBox.Show("Vui lòng chọn bản ghi cần xóa!");
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa bản ghi này?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm == DialogResult.No)
                return;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Kho WHERE MaKho = @MaKho";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MaKho", txtMaKho.Text);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đã xóa thành công!");
            LoadKho();
        }

        private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (txtMaKho.Text == "")
            {
                MessageBox.Show("Vui lòng chọn bản ghi cần sửa!");
                return;
            }

            // Không cho sửa mã kho + mã món
            txtMaKho.Enabled = false;
            cmbMaMon.Enabled = false;

            // Cho sửa số lượng / ngày nhập
            txtSLNhap.Enabled = true;
            txtSLXuat.Enabled = true;
            dtNgayNhap.Enabled = true;

            btn_Luu.Enabled = true;

            isEditing = true; // trạng thái đang sửa
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            if (cmbMaMon.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn món.");
                return;
            }

            if (!int.TryParse(txtSLNhap.Text, out int slNhap))
            {
                MessageBox.Show("Số lượng nhập phải là số nguyên.");
                return;
            }

            if (!int.TryParse(txtSLXuat.Text, out int slXuat))
            {
                MessageBox.Show("Số lượng xuất phải là số nguyên.");
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Kiểm tra xem mã kho đã tồn tại chưa
                    string checkQuery = "SELECT COUNT(*) FROM Kho WHERE MaKho=@MaKho AND MaMon=@MaMon";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@MaKho", txtMaKho.Text);
                    checkCmd.Parameters.AddWithValue("@MaMon", cmbMaMon.SelectedValue);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0) // Thêm mới
                    {
                        // Nếu chưa có mã kho, tạo mã mới
                        string maKho = string.IsNullOrEmpty(txtMaKho.Text) ? GenerateMaKho() : txtMaKho.Text;

                        string insertQuery = @"
                    INSERT INTO Kho (MaKho, MaMon, SoLuongNhap, SoLuongXuat, NgayNhap)
                    VALUES (@MaKho, @MaMon, @SLNhap, @SLXuat, @NgayNhap)
                ";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@MaKho", maKho);
                        insertCmd.Parameters.AddWithValue("@MaMon", cmbMaMon.SelectedValue);
                        insertCmd.Parameters.AddWithValue("@SLNhap", slNhap);
                        insertCmd.Parameters.AddWithValue("@SLXuat", slXuat);
                        insertCmd.Parameters.AddWithValue("@NgayNhap", dtNgayNhap.Value);

                        insertCmd.ExecuteNonQuery();

                        txtMaKho.Text = maKho; // hiển thị mã kho mới
                    }
                    else // Cập nhật số lượng
                    {
                        string updateQuery = @"
                    UPDATE Kho
                    SET SoLuongNhap=@SLNhap, SoLuongXuat=@SLXuat, NgayNhap=@NgayNhap
                    WHERE MaKho=@MaKho AND MaMon=@MaMon
                ";

                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@MaKho", txtMaKho.Text);
                        updateCmd.Parameters.AddWithValue("@MaMon", cmbMaMon.SelectedValue);
                        updateCmd.Parameters.AddWithValue("@SLNhap", slNhap);
                        updateCmd.Parameters.AddWithValue("@SLXuat", slXuat);
                        updateCmd.Parameters.AddWithValue("@NgayNhap", dtNgayNhap.Value);

                        updateCmd.ExecuteNonQuery();
                    }

                    // Cập nhật tồn kho tổng
                    UpdateTonKho(cmbMaMon.SelectedValue.ToString());

                    // Load lại danh sách kho
                    LoadKho();

                    MessageBox.Show("Đã lưu dữ liệu thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu kho: " + ex.Message);
            }
            txtMaKho.Enabled = cmbMaMon.Enabled = true;
            btn_Luu.Enabled = false;
        }

        private void dgvKho_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtMaKho.Text = dgvKho.Rows[e.RowIndex].Cells["MaKho"].Value.ToString();
                cmbMaMon.SelectedValue = dgvKho.Rows[e.RowIndex].Cells["MaMon"].Value.ToString();
                dtNgayNhap.Value = Convert.ToDateTime(dgvKho.Rows[e.RowIndex].Cells["NgayNhap"].Value);
                txtSLNhap.Text = dgvKho.Rows[e.RowIndex].Cells["SoLuongNhap"].Value.ToString();
                txtSLXuat.Text = dgvKho.Rows[e.RowIndex].Cells["SoLuongXuat"].Value.ToString();
            }
        }

        private void UserControl_QLKho_Load(object sender, EventArgs e)
        {
            LoadKho();
            LoadMenuMon();

            dtNgayNhap.CustomFormat="dd/MM/yyyy";
            dgvKho.ReadOnly = true;
            dgvKho.AllowUserToAddRows = false;

            btn_Luu.Enabled = false;
        }
        private void LoadMenuMon()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaMon, TenMon FROM Mon";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbMaMon.DataSource = dt;
                    cmbMaMon.DisplayMember = "TenMon";
                    cmbMaMon.ValueMember = "MaMon";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load món: " + ex.Message);
            }
        }

        private void LoadKho()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT Kho.MaKho, Kho.MaMon, Mon.TenMon, 
                               Kho.NgayNhap, Kho.SoLuongNhap, Kho.SoLuongXuat
                        FROM Kho 
                        JOIN Mon ON Kho.MaMon = Mon.MaMon
                        ORDER BY MaKho DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvKho.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load kho: " + ex.Message);
            }
        }

        private string GenerateMaKho()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT TOP 1 MaKho FROM Kho ORDER BY MaKho DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();

                if (result == null)
                    return "K001";

                string lastCode = result.ToString();   // K005
                int number = int.Parse(lastCode.Substring(1));  // 5
                number++;

                return "K" + number.ToString("000");
            }
        }


        private void UpdateTonKho(string maMon)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // tính tổng nhập – xuất
                    string query = @"
                        SELECT 
                            SUM(SoLuongNhap) AS TongNhap,
                            SUM(SoLuongXuat) AS TongXuat
                        FROM Kho 
                        WHERE MaMon=@maMon";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maMon", maMon);
                    SqlDataReader reader = cmd.ExecuteReader();

                    int tongNhap = 0, tongXuat = 0;

                    if (reader.Read())
                    {
                        tongNhap = reader["TongNhap"] != DBNull.Value ?
                                   Convert.ToInt32(reader["TongNhap"]) : 0;

                        tongXuat = reader["TongXuat"] != DBNull.Value ?
                                   Convert.ToInt32(reader["TongXuat"]) : 0;
                    }
                    reader.Close();

                    int ton = tongNhap - tongXuat;

                    string update = "UPDATE Mon SET SoLuongTon=@ton WHERE MaMon=@ma";

                    SqlCommand cmd2 = new SqlCommand(update, conn);
                    cmd2.Parameters.AddWithValue("@ton", ton);
                    cmd2.Parameters.AddWithValue("@ma", maMon);
                    cmd2.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật tồn kho: " + ex.Message);
            }
        }

    }
}
