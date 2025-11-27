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
    public partial class UserControl_BaoCao : UserControl
    {
        public UserControl_BaoCao()
        {
            InitializeComponent();
        }

        private void UserControl_BaoCao_Load(object sender, EventArgs e)
        {
            // Đặt format trước
            NgayBatDau.Format = DateTimePickerFormat.Custom;
            NgayBatDau.CustomFormat = "dd/MM/yyyy";
            NgayKetThuc.Format = DateTimePickerFormat.Custom;
            NgayKetThuc.CustomFormat = "dd/MM/yyyy";

            // Đặt Value trước khi đặt MaxDate để tránh lỗi với Japanese locale
            NgayBatDau.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            NgayKetThuc.Value = DateTime.Now;

            // Sau đó mới đặt MaxDate
            NgayBatDau.MaxDate = DateTime.Now;
            NgayKetThuc.MaxDate = DateTime.Now;

            // Xóa các cột tĩnh nếu có (từ Designer)
            dgvBaoCao.Columns.Clear();
            dgvBaoCao.AutoGenerateColumns = true;

            // Load dữ liệu ban đầu
            LoadBaoCao();
        }

        private void LoadBaoCao()
        {
            try
            {
                // Load dữ liệu hóa đơn
                LoadHoaDon();

                // Load tổng doanh thu tháng
                LoadTongDoanhThu();

                // Load số lượng hóa đơn
                LoadSoHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu báo cáo: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHoaDon()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT MaHD, MaBan, MaNV, MaKhach, 
                                    GioBatDau, GioKetThuc, TongTien, NgayLap
                                    FROM HoaDon
                                    WHERE NgayLap BETWEEN @NgayBatDau AND @NgayKetThuc
                                    ORDER BY MaHD DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NgayBatDau", NgayBatDau.Value.Date);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", NgayKetThuc.Value.Date.AddDays(1).AddSeconds(-1));

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvBaoCao.DataSource = dt;

                        // Debug: Hiển thị số dòng
                        Console.WriteLine($"Số dòng tìm thấy: {dt.Rows.Count}");
                        Console.WriteLine($"Từ ngày: {NgayBatDau.Value.Date}");
                        Console.WriteLine($"Đến ngày: {NgayKetThuc.Value.Date.AddDays(1).AddSeconds(-1)}");

                        // Format hiển thị
                        if (dgvBaoCao.Columns["TongTien"] != null)
                        {
                            dgvBaoCao.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                            dgvBaoCao.Columns["TongTien"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }

                        if (dgvBaoCao.Columns["GioBatDau"] != null)
                        {
                            dgvBaoCao.Columns["GioBatDau"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }

                        if (dgvBaoCao.Columns["GioKetThuc"] != null)
                        {
                            dgvBaoCao.Columns["GioKetThuc"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                        }

                        if (dgvBaoCao.Columns["NgayLap"] != null)
                        {
                            dgvBaoCao.Columns["NgayLap"].DefaultCellStyle.Format = "dd/MM/yyyy";
                        }

                        // Nếu không có dữ liệu, hiển thị thông báo
                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show($"Không có hóa đơn nào trong khoảng thời gian từ {NgayBatDau.Value.ToString("dd/MM/yyyy")} đến {NgayKetThuc.Value.ToString("dd/MM/yyyy")}",
                                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTongDoanhThu()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Lấy tháng và năm từ NgayKetThuc
                    int thang = NgayKetThuc.Value.Month;
                    int nam = NgayKetThuc.Value.Year;

                    using (SqlCommand cmd = new SqlCommand("sp_ThongKeDoanhThuThang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Thang", thang);
                        cmd.Parameters.AddWithValue("@Nam", nam);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(1)) // DoanhThu là cột thứ 2
                                {
                                    decimal doanhThu = reader.GetDecimal(1);
                                    lblTongDoanhThuThang.Text = doanhThu.ToString("N0") + " VNĐ";
                                }
                                else
                                {
                                    lblTongDoanhThuThang.Text = "0 VNĐ";
                                }
                            }
                            else
                            {
                                lblTongDoanhThuThang.Text = "0 VNĐ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải tổng doanh thu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblTongDoanhThuThang.Text = "0 VNĐ";
            }
        }

        private void LoadSoHoaDon()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*) 
                                    FROM HoaDon 
                                    WHERE NgayLap BETWEEN @NgayBatDau AND @NgayKetThuc";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NgayBatDau", NgayBatDau.Value.Date);
                        cmd.Parameters.AddWithValue("@NgayKetThuc", NgayKetThuc.Value.Date.AddDays(1).AddSeconds(-1));

                        int soHoaDon = (int)cmd.ExecuteScalar();
                        lblSoHoaDon.Text = soHoaDon.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đếm số hóa đơn: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblSoHoaDon.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra ngày hợp lệ
            if (NgayBatDau.Value > NgayKetThuc.Value)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Load lại báo cáo
            LoadBaoCao();
        }
    }
}