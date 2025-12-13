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
        bool isEditing = false;
        private void btn_Them_Click(object sender, EventArgs e)
        {
            // Tạo mã kho mới tự động nếu textbox mã kho trống
            if (string.IsNullOrEmpty(txtMaKho.Text))
            {
                txtMaKho.Text = GenerateMaKho(); // Hàm tạo mã kho tự động
            }

            // Clear dữ liệ
            txtMaMon.Text = "";
            txtTenMon.Text = "";
            txtSLNhap.Text = "0";
            txtSLXuat.Text = "0";
            dtNgayNhap.Value = DateTime.Now;

            // Cho phép nhập MÃ MÓN
            txtMaMon.Enabled = true;
            txtTenMon.Enabled = true;

            // Cho nhập số lượng nhập hoặc xuất
            txtSLNhap.Enabled = true;
            txtSLXuat.Enabled = false;

            // Bật nút Lưu
            btn_Luu.Enabled = true;

            // Disable txtMaKho khi đang tạo mới
            txtMaKho.Enabled = true; // Không cho phép sửa mã kho khi tạo mới
            isEditing = false; // Trạng thái thêm mới
        }


        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKho.Text))
            {
                MessageBox.Show("Vui lòng nhập mã kho cần xóa!");
                return;
            }

            string selectedMaKho = txtMaKho.Text; // Lấy mã kho từ TextBox

            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm trong kho này?",
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
                cmd.Parameters.AddWithValue("@MaKho", selectedMaKho);
                cmd.ExecuteNonQuery();
            }

            if (!string.IsNullOrEmpty(txtMaMon.Text))
            {
                UpdateTonKho(txtMaMon.Text);
            }

            MessageBox.Show("Đã xóa thành công!");
            LoadKho();

            // Clear các trường
            txtMaKho.Text = "";
            txtMaMon.Text = "";
            txtTenMon.Text = "";
            txtSLNhap.Text = "";
            txtSLXuat.Text = "";
        }


        private void btn_Sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaKho.Text))
            {
                MessageBox.Show("Vui lòng nhập mã kho cần sửa!");
                return;
            }

            string selectedMaKho = txtMaKho.Text; // Lấy mã kho từ TextBox

            // Không cho sửa mã kho + mã món
            txtMaKho.Enabled = true;
            txtMaMon.Enabled = true;
            txtTenMon.Enabled = true;

            // Cho sửa số lượng / ngày nhập
            txtSLNhap.Enabled = true;
            txtSLXuat.Enabled = true;
            dtNgayNhap.Enabled = true;

            btn_Luu.Enabled = true;

            isEditing = true; // Trạng thái sửa
        }

        private void btn_Luu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(txtMaKho.Text))
            {
                // Nếu người dùng không nhập mã kho, hệ thống sẽ tạo mới mã kho
                txtMaKho.Text = GenerateMaKho();
            }

            string selectedMaKho = txtMaKho.Text; // Lấy mã kho từ TextBox

            if (string.IsNullOrEmpty(txtMaMon.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã món.");
                txtMaMon.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtTenMon.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên món.");
                txtTenMon.Focus();
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

                    // Kiểm tra xem mã món đã tồn tại trong bảng Mon chưa
                    string checkMaMonQuery = "SELECT COUNT(*) FROM Mon WHERE MaMon = @MaMon";
                    SqlCommand checkMaMonCmd = new SqlCommand(checkMaMonQuery, conn);
                    checkMaMonCmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text);

                    int maMonCount = (int)checkMaMonCmd.ExecuteScalar();

                    // Nếu mã món chưa tồn tại, thêm vào bảng Mon
                    if (maMonCount == 0)
                    {
                        string insertMonQuery = "INSERT INTO Mon (MaMon, TenMon, DonGia, SoLuongTon) VALUES (@MaMon, @TenMon, @DonGia, @SoLuongTon)";
                        SqlCommand insertMonCmd = new SqlCommand(insertMonQuery, conn);
                        insertMonCmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text);
                        insertMonCmd.Parameters.AddWithValue("@TenMon", txtTenMon.Text);
                        insertMonCmd.Parameters.AddWithValue("@DonGia", decimal.Parse(txtSLNhap.Text));  // Giả sử bạn muốn nhập giá vào textbox SLNhap (có thể điều chỉnh)
                        insertMonCmd.Parameters.AddWithValue("@SoLuongTon", 0);  // Giả sử tồn kho ban đầu là 0
                        insertMonCmd.ExecuteNonQuery();
                    }

                    // Kiểm tra xem cặp MaKho và MaMon đã tồn tại chưa trong bảng Kho
                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM Kho 
                WHERE MaKho = @MaKho AND MaMon = @MaMon";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@MaKho", selectedMaKho);
                    checkCmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text);

                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // Nếu cặp MaKho và MaMon đã tồn tại, cập nhật
                        if (isEditing)
                        {
                            string updateQuery = @"
                        UPDATE Kho
                        SET SoLuongNhap = @SLNhap, SoLuongXuat = @SLXuat, NgayNhap = @NgayNhap
                        WHERE MaKho = @MaKho AND MaMon = @MaMon";

                            SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                            updateCmd.Parameters.AddWithValue("@MaKho", selectedMaKho);
                            updateCmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text);
                            updateCmd.Parameters.AddWithValue("@SLNhap", slNhap);
                            updateCmd.Parameters.AddWithValue("@SLXuat", slXuat);
                            updateCmd.Parameters.AddWithValue("@NgayNhap", dtNgayNhap.Value);

                            updateCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            MessageBox.Show("Cặp mã kho và mã món đã tồn tại trong kho.");
                            return;
                        }
                    }
                    else
                    {
                        // Nếu cặp MaKho và MaMon chưa tồn tại, tiến hành thêm mới vào bảng Kho
                        string insertQuery = @"
                    INSERT INTO Kho (MaKho, MaMon, SoLuongNhap, SoLuongXuat, NgayNhap)
                    VALUES (@MaKho, @MaMon, @SLNhap, @SLXuat, @NgayNhap)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@MaKho", selectedMaKho);
                        insertCmd.Parameters.AddWithValue("@MaMon", txtMaMon.Text);
                        insertCmd.Parameters.AddWithValue("@SLNhap", slNhap);
                        insertCmd.Parameters.AddWithValue("@SLXuat", slXuat);
                        insertCmd.Parameters.AddWithValue("@NgayNhap", dtNgayNhap.Value);

                        insertCmd.ExecuteNonQuery();
                    }

                    // Cập nhật tên món nếu cần thiết
                    string updateNameQuery = "UPDATE Mon SET TenMon = @TenMon WHERE MaMon = @MaMon";
                    SqlCommand cmdName = new SqlCommand(updateNameQuery, conn);
                    cmdName.Parameters.AddWithValue("@TenMon", txtTenMon.Text);
                    cmdName.Parameters.AddWithValue("@MaMon", txtMaMon.Text);
                    cmdName.ExecuteNonQuery();

                    // Cập nhật tồn kho
                    UpdateTonKho(txtMaMon.Text);

                    // Load lại danh sách kho
                    LoadKho();

                    MessageBox.Show("Đã lưu dữ liệu thành công.");
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 547) // Lỗi khóa ngoại (Foreign Key)
                {
                    MessageBox.Show("Mã món này không tồn tại trong danh sách Món! Vui lòng kiểm tra lại.");
                }
                else
                {
                    MessageBox.Show("Lỗi SQL: " + sqlEx.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu kho: " + ex.Message);
            }

            // Reset trạng thái
            btn_Luu.Enabled = false;
            txtMaMon.Enabled = false;
            txtTenMon.Enabled = false;
        }


        private void dgvKho_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Sửa lại gán mã kho từ dgvKho vào txtMaKho.Text
                txtMaKho.Text = dgvKho.Rows[e.RowIndex].Cells["MaKho"].Value.ToString();
                txtMaMon.Text = dgvKho.Rows[e.RowIndex].Cells["MaMon"].Value.ToString();
                txtTenMon.Text = dgvKho.Rows[e.RowIndex].Cells["TenMon"].Value.ToString();
                dtNgayNhap.Value = Convert.ToDateTime(dgvKho.Rows[e.RowIndex].Cells["NgayNhap"].Value);
                txtSLNhap.Text = dgvKho.Rows[e.RowIndex].Cells["SoLuongNhap"].Value.ToString();
                txtSLXuat.Text = dgvKho.Rows[e.RowIndex].Cells["SoLuongXuat"].Value.ToString();
            }
        }


        private void UserControl_QLKho_Load(object sender, EventArgs e)
        {
            LoadKho();

            dtNgayNhap.CustomFormat = "dd/MM/yyyy";
            dgvKho.ReadOnly = true;
            dgvKho.AllowUserToAddRows = false;

            btn_Luu.Enabled = false;

            // Enable txtMaKho khi tạo mới
            txtMaKho.Enabled = false;  // Allow editing txtMaKho when adding new
            txtMaMon.Enabled = false;
            txtTenMon.Enabled = false;
            txtSLNhap.Enabled = false;
            txtSLXuat.Enabled = false;
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

                string lastCode = result.ToString().Trim(); // Trim whitespace

                // Validate the format (should start with 'K' followed by digits)
                if (string.IsNullOrEmpty(lastCode) || lastCode.Length < 2 || !lastCode.StartsWith("K"))
                    return "K001";

                // Extract numeric part
                string numericPart = lastCode.Substring(1);

                // Try to parse the numeric part
                if (!int.TryParse(numericPart, out int number))
                    return "K001"; // Return default if parsing fails

                number++;
                return "K" + number.ToString("000");
            }
        }



        private void LoadKho()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    // Query lấy cả Tên Món để hiển thị
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
