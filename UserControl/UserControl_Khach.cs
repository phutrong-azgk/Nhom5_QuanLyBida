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
    public partial class UserControl_Khach : UserControl
    {
        DataSet ds;
        public UserControl_Khach()
        {
            InitializeComponent();
        }
        private void AddTable(string TenKhach, string maKhach, string soHoaDon, string tongChi)
        {
            Panel tableBox = new Panel();
            tableBox.Size = new Size(250, 150);
            tableBox.BorderStyle = BorderStyle.FixedSingle;
            tableBox.BackColor = Color.WhiteSmoke;
            tableBox.Margin = new Padding(10);
            tableBox.Padding = new Padding(10);

            Label lblTenKhach = new Label();
            lblTenKhach.Text = TenKhach;
            lblTenKhach.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTenKhach.AutoSize = true;
            lblTenKhach.Location = new Point(10, 10);

            Label lblMaKhach = new Label();
            lblMaKhach.Text = $"Mã Khách: {maKhach}";
            lblMaKhach.Font = new Font("Segoe UI", 11);
            lblMaKhach.AutoSize = true;
            lblMaKhach.Location = new Point(10, 45);

            Label lblSoHoaDon = new Label();
            lblSoHoaDon.Text = $"Số hóa đơn: {soHoaDon}";
            lblSoHoaDon.Font = new Font("Segoe UI", 11);
            lblSoHoaDon.AutoSize = true;
            lblSoHoaDon.Location = new Point(10, 70);

            Label lblTongChi = new Label();
            lblTongChi.Text = $"Tổng chi: {tongChi}";
            lblTongChi.Font = new Font("Segoe UI", 11);
            lblTongChi.AutoSize = true;
            lblTongChi.Location = new Point(10, 95);

            Label lblDiemThuong = new Label();
            lblDiemThuong.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblDiemThuong.ForeColor = Color.DarkGreen;
            lblDiemThuong.AutoSize = true;
            lblDiemThuong.Location = new Point(10, 120);

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dbo.fn_DiemKhachHang(@MaKhach) AS DiemThuong";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                        object result = cmd.ExecuteScalar();

                        int diemThuong = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                        lblDiemThuong.Text = $"Điểm thưởng: {diemThuong}";
                    }
                }
            }
            catch (Exception ex)
            {
                lblDiemThuong.Text = "Điểm thưởng: N/A";
                MessageBox.Show($"Lỗi khi lấy điểm thưởng: {ex.Message}");
            }

            Button btnEdit = new Button();
            btnEdit.Text = "Sửa";
            btnEdit.Size = new Size(80, 30);
            btnEdit.Location = new Point(150, 60);
            btnEdit.Enabled = false;

            btnEdit.Click += (s, e) =>
            {
                foreach (Control panel in flowLayoutPanelKhach.Controls)
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
                SuaKhach formSuaKhach = new SuaKhach();
                formSuaKhach.MaKhach = maKhach;
                formSuaKhach.TenKhach = TenKhach;
                formSuaKhach.SoHoaDon = int.Parse(soHoaDon);
                formSuaKhach.TongChi = decimal.Parse(tongChi);
                if (formSuaKhach.ShowDialog() == DialogResult.OK)
                {
                    flowLayoutPanelKhach.Controls.Clear();
                    LoadTablesFromDatabase();
                }
                btnEdit.Enabled = false;
            };

            Button btnDelete = new Button();
            btnDelete.Text = "Xóa";
            btnDelete.Enabled = false;
            btnDelete.Size = new Size(80, 30);
            btnDelete.Location = new Point(150, 100);

            btnDelete.Click += (s, e) =>
            {
                foreach (Control panel in flowLayoutPanelKhach.Controls)
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
                if (MessageBox.Show($"Xóa khách '{TenKhach}'?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string sql = "DELETE FROM Khach WHERE MaKhach = @ma";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ma", maKhach);
                        cmd.ExecuteNonQuery();
                    }

                    flowLayoutPanelKhach.Controls.Remove(tableBox);
                    MessageBox.Show("Đã xóa!");
                }
                btnDelete.Enabled = false;
            };

            tableBox.Tag = new List<Button> { btnEdit, btnDelete };

            tableBox.Controls.Add(lblTenKhach);
            tableBox.Controls.Add(lblMaKhach);
            tableBox.Controls.Add(lblSoHoaDon);
            tableBox.Controls.Add(lblTongChi);
            tableBox.Controls.Add(lblDiemThuong);
            tableBox.Controls.Add(btnEdit);
            tableBox.Controls.Add(btnDelete);
            

            flowLayoutPanelKhach.Controls.Add(tableBox);
        }
        private void LoadTablesFromDatabase()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Khach";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    ds = new DataSet();
                    adapter.Fill(ds, "Khach");

                    foreach (DataRow row in ds.Tables["Khach"].Rows)
                    {
                        AddTable(
                            row["TenKhach"].ToString(),
                            row["MaKhach"].ToString(),
                            row["SoHoaDon"].ToString(),
                            row["TongChi"].ToString()
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load khách: " + ex.Message);
            }
        }

        private void UserControl_Khach_Load_1(object sender, EventArgs e)
        {
            LoadTablesFromDatabase();
        }

        private void btnThemKhach_Click(object sender, EventArgs e)
        {
            ThemKhack frmThemKhach = new ThemKhack();
            try
            {
                if (frmThemKhach.ShowDialog() == DialogResult.OK)
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string sql = "INSERT INTO Khach (MaKhach, TenKhach, SoHoaDon, TongChi) VALUES (@ma, @ten, @hd, @chi)";
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        cmd.Parameters.AddWithValue("@ma", frmThemKhach.MaKhach);
                        cmd.Parameters.AddWithValue("@ten", frmThemKhach.TenKhach);
                        cmd.Parameters.AddWithValue("@hd", frmThemKhach.SoHoaDon);
                        cmd.Parameters.AddWithValue("@chi", frmThemKhach.TongChi);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Thêm khách thành công!", "Thông báo", MessageBoxButtons.OK);
                    flowLayoutPanelKhach.Controls.Clear();
                    LoadTablesFromDatabase();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm khách: {ex.Message}");
            }
        }

        private void btnXoaKhach_Click(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanelKhach.Controls)
            {
                if (c is Panel panel && panel.Tag is List<Button> list)
                {
                    list[1].Enabled = true;
                }
            }
        }

        private void btnSuaKhach_Click(object sender, EventArgs e)
        {
            foreach (Control c in flowLayoutPanelKhach.Controls)
            {
                if (c is Panel panel && panel.Tag is List<Button> list)
                {
                    list[0].Enabled = true;
                }
            }
        }
    }
}
