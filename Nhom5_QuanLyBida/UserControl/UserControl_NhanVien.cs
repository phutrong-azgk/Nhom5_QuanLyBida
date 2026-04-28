using Nhom5_QuanLyBida.FormNhanVien;
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
    public partial class UserControl_NhanVien : UserControl
    {
        DataSet ds;
        public UserControl_NhanVien()
        {
            InitializeComponent();
        }
        private void AddTable(string hoTen, string maMV, string ngaySinh, string gioiTinh, string soDienThoai, string diaChi, string tenDangNhap)
        {
            Panel tableBox = new Panel();
            tableBox.Size = new Size(250, 300);
            tableBox.BorderStyle = BorderStyle.FixedSingle;
            tableBox.BackColor = Color.WhiteSmoke;
            tableBox.Margin = new Padding(10);
            tableBox.Padding = new Padding(10);

            Label lblHoTen = new Label();
            lblHoTen.Text = hoTen;
            lblHoTen.Font = new Font("Arial", 16, FontStyle.Bold);
            lblHoTen.Location = new Point(10, 10);
            lblHoTen.AutoSize = true;

            Label lblMaMV = new Label();
            lblMaMV.Text = $"Mã NV: {maMV}";
            lblMaMV.Font = new Font("Arial", 14);
            lblMaMV.Location = new Point(10, 40);
            lblMaMV.AutoSize = true;

            Label lblNgaySinh = new Label();
            lblNgaySinh.Text = $"Ngày sinh: {ngaySinh}";
            lblNgaySinh.Font = new Font("Arial", 14);
            lblNgaySinh.Location = new Point(10, 80);
            lblNgaySinh.AutoSize = true;

            Label lblGioiTinh = new Label();
            lblGioiTinh.Text = $"Giới tính: {gioiTinh}";
            lblGioiTinh.Font = new Font("Arial", 14);
            lblGioiTinh.Location = new Point(10, 120);
            lblGioiTinh.AutoSize = true;

            Label lblSoDienThoai = new Label();
            lblSoDienThoai.Text = $"SĐT: {soDienThoai}";
            lblSoDienThoai.Font = new Font("Arial", 14);
            lblSoDienThoai.Location = new Point(10, 160);
            lblSoDienThoai.AutoSize = true;

            Label lblDiaChi = new Label();
            lblDiaChi.Text = $"Địa chỉ: {diaChi}";
            lblDiaChi.Font = new Font("Arial", 14);
            lblDiaChi.Location = new Point(10, 200);
            lblDiaChi.MaximumSize = new Size(280, 0); // Giới hạn chiều rộng để chữ tự động xuống dòng
            lblDiaChi.AutoSize = true;

            Label lblTenDangNhap = new Label();
            lblDiaChi.Text = $"Tên đăng nhập: {tenDangNhap}";
            lblDiaChi.Font = new Font("Arial", 14);
            lblDiaChi.Location = new Point(10, 200);
            lblDiaChi.MaximumSize = new Size(280, 0); // Giới hạn chiều rộng để chữ tự động xuống dòng
            lblDiaChi.AutoSize = true;

            Button btnDelete = new Button();
            btnDelete.Text = "Xóa";
            btnDelete.Enabled = false;
            btnDelete.Size = new Size(80, 30);
            btnDelete.Location = new Point(10, 250);

            btnDelete.Click += (s, e) =>
            {
                // Vô hiệu hóa các nút xóa khác trong giao diện để tránh thao tác trùng lặp
                foreach (Control panel in flowLayoutPanelNhanVien.Controls)
                {
                    if (panel is Panel pnl)
                    {
                        foreach (Control c in pnl.Controls)
                        {
                            if (c is Button btn && btn.Text == "Xóa")
                            {
                                btn.Enabled = false;
                            }
                        }
                    }
                }

                // Xác nhận trước khi xóa
                if (MessageBox.Show($"Xóa nhân viên '{hoTen}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            conn.Open();
                            // Gọi stored procedure sp_XoaNhanVien
                            using (SqlCommand cmd = new SqlCommand("sp_XoaNhanVien", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                // Truyền tham số MaNV cho procedure
                                cmd.Parameters.AddWithValue("@MaNV", maMV);

                                cmd.ExecuteNonQuery();
                            }
                        }

                        // Cập nhật giao diện sau khi xóa thành công
                        flowLayoutPanelNhanVien.Controls.Remove(tableBox);
                        MessageBox.Show("Đã xóa nhân viên thành công!");
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị lỗi từ SQL Server (ví dụ: Nhân viên không tồn tại)
                        MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            };

            Button btnEdit = new Button();
            btnEdit.Text = "Sửa";
            btnEdit.Size = new Size(80, 30);
            btnEdit.Location = new Point(120, 250);
            btnEdit.Enabled = false;

            btnEdit.Click += (s, e) =>
            {
                foreach (Control panel in flowLayoutPanelNhanVien.Controls)
                {
                    if (panel is Panel pnl)
                    {
                        foreach (Control c in pnl.Controls)
                        {
                            if (c is Button btn && btn.Text == "Sửa")
                            {
                                btn.Enabled = false;
                            }
                        }
                    }
                }
                SuaNhanVien formSuaNV = new SuaNhanVien();
                formSuaNV.MaNV = maMV;
                formSuaNV.TenNV = hoTen;
                DateTime ns;
                if (!DateTime.TryParseExact(ngaySinh, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out ns))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ");
                    return;
                }
                formSuaNV.NgaySinh = ns;
                formSuaNV.GioiTinh = gioiTinh;
                formSuaNV.SDT = soDienThoai;
                formSuaNV.DiaChi = diaChi;
                if (formSuaNV.ShowDialog() == DialogResult.OK)
                {
                    flowLayoutPanelNhanVien.Controls.Clear();
                    LoadTablesFromDatabase();
                }
                btnEdit.Enabled = false;
            };

            tableBox.Tag = new List<Button> { btnDelete, btnEdit };

            tableBox.Controls.Add(lblHoTen);
            tableBox.Controls.Add(lblMaMV);
            tableBox.Controls.Add(lblNgaySinh);
            tableBox.Controls.Add(lblGioiTinh);
            tableBox.Controls.Add(lblSoDienThoai);
            tableBox.Controls.Add(lblDiaChi);
            tableBox.Controls.Add(btnDelete);
            tableBox.Controls.Add(btnEdit);

            flowLayoutPanelNhanVien.Controls.Add(tableBox);
        }
        private void LoadTablesFromDatabase()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string sql = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, VaiTro) VALUES (@tenDN, @matkhau, @vaitro)";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        string query = "SELECT * FROM NhanVien";
                        SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                        ds = new DataSet();

                        adapter.Fill(ds, "NhanVien");

                        // Loop through DataSet
                        foreach (DataRow row in ds.Tables["NhanVien"].Rows)
                        {
                            string tableName = row["HoTen"].ToString();
                            string costumerID = row["MaNV"].ToString();
                            string ngaySinhFormatted = "";
                            DateTime ngaySinh;
                            if (DateTime.TryParse(row["NgaySinh"].ToString(), out ngaySinh))
                            {
                                ngaySinhFormatted = ngaySinh.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                ngaySinhFormatted = "";
                            }
                            string price = row["GioiTinh"].ToString();
                            string phoneNumber = row["SoDienThoai"].ToString();
                            string place = row["DiaChi"].ToString();
                            string login = row["TenDangNhap"].ToString();
                            AddTable(tableName, costumerID, ngaySinhFormatted, price, phoneNumber, place,login);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}");
            }
        }
        private void UserControl_NhanVien_Load_1(object sender, EventArgs e)
        {
            LoadTablesFromDatabase();
        }
        private void btnThemNV_Click_1(object sender, EventArgs e)
        {
            ThemNhanVien frmThemNV = new ThemNhanVien();
            if (frmThemNV.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        // Cập nhật câu truy vấn thêm trường TenDangNhap
                        string query = "INSERT INTO NhanVien (MaNV, HoTen, NgaySinh, GioiTinh, SoDienThoai, DiaChi, TenDangNhap) " +
                                       "VALUES (@ma, @ten, @ns, @gt, @sdt, @dc, @tdn)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ma", frmThemNV.MaNV);
                            cmd.Parameters.AddWithValue("@ten", frmThemNV.TenNV);
                            cmd.Parameters.AddWithValue("@ns", frmThemNV.NgaySinh);
                            cmd.Parameters.AddWithValue("@gt", frmThemNV.GioiTinh);
                            cmd.Parameters.AddWithValue("@sdt", frmThemNV.SDT);
                            cmd.Parameters.AddWithValue("@dc", frmThemNV.DiaChi);
                            cmd.Parameters.AddWithValue("@tdn", frmThemNV.TenDangNhap); // Thêm tham số này

                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK);
                        flowLayoutPanelNhanVien.Controls.Clear();
                        LoadTablesFromDatabase();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi thêm nhân viên: {ex.Message}");
                }
            }
        }

        private void btnXoaNV_Click_1(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanelNhanVien.Controls)
            {
                if (c is Panel panel && panel.Tag is List<Button> list)
                {
                    list[0].Enabled = true;
                }
            }
        }

        private void btnSuaNV_Click_1(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanelNhanVien.Controls)
            {
                if (c is Panel panel && panel.Tag is List<Button> list)
                {
                    list[1].Enabled = true;
                }
            }
        }
    }
}
