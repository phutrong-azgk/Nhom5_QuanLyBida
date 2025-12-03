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
    public partial class UserControl_KhuyenMai : UserControl
    {
        DataSet ds;
        private Panel currentlySelectedPromotion = null;
        private string selectedPromotionId = null;

        public UserControl_KhuyenMai()
        {
            InitializeComponent();
        }

        private void AddPromotionCard(string maKM, string tenKM, string dieuKien, int phanTramGiam, DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            Panel promoCard = new Panel();
            promoCard.Size = new Size(250, 200);
            promoCard.BorderStyle = BorderStyle.FixedSingle;
            promoCard.Margin = new Padding(15);
            promoCard.Name = maKM;
            promoCard.Cursor = Cursors.Hand;

            // Kiểm tra khuyến mãi còn hiệu lực không
            bool isActive = DateTime.Now >= ngayBatDau && DateTime.Now <= ngayKetThuc;
            Color cardColor = isActive ? Color.LightGreen : Color.LightGray;
            promoCard.BackColor = cardColor;
            promoCard.Tag = isActive ? "Active" : "Inactive";

            // Discount percentage - large and prominent
            Label lblDiscount = new Label();
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Text = phanTramGiam.ToString() + "%";
            lblDiscount.Font = new Font("Arial", 32, FontStyle.Bold);
            lblDiscount.ForeColor = isActive ? Color.DarkGreen : Color.Gray;
            lblDiscount.TextAlign = ContentAlignment.MiddleCenter;
            lblDiscount.Location = new Point(10, 10);
            lblDiscount.Size = new Size(230, 50);

            // Promotion name
            Label lblTenKM = new Label();
            lblTenKM.Name = "lblTenKM";
            lblTenKM.Text = tenKM;
            lblTenKM.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTenKM.TextAlign = ContentAlignment.MiddleCenter;
            lblTenKM.Location = new Point(10, 65);
            lblTenKM.Size = new Size(230, 25);

            // Condition
            Label lblDieuKien = new Label();
            lblDieuKien.Name = "lblDieuKien";
            lblDieuKien.Text = dieuKien;
            lblDieuKien.Font = new Font("Arial", 9);
            lblDieuKien.TextAlign = ContentAlignment.MiddleCenter;
            lblDieuKien.Location = new Point(10, 95);
            lblDieuKien.Size = new Size(230, 40);

            // Date range
            Label lblDateRange = new Label();
            lblDateRange.Name = "lblDateRange";
            lblDateRange.Text = ngayBatDau.ToString("dd/MM/yyyy") + " - " + ngayKetThuc.ToString("dd/MM/yyyy");
            lblDateRange.Font = new Font("Arial", 9);
            lblDateRange.TextAlign = ContentAlignment.MiddleCenter;
            lblDateRange.Location = new Point(10, 140);
            lblDateRange.Size = new Size(230, 20);

            // Status label
            Label lblStatus = new Label();
            lblStatus.Name = "lblStatus";
            lblStatus.Text = isActive ? "Đang hoạt động" : "Hết hạn";
            lblStatus.Font = new Font("Arial", 9, FontStyle.Italic);
            lblStatus.ForeColor = isActive ? Color.DarkGreen : Color.Red;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Location = new Point(10, 165);
            lblStatus.Size = new Size(230, 20);

            promoCard.Controls.Add(lblDiscount);
            promoCard.Controls.Add(lblTenKM);
            promoCard.Controls.Add(lblDieuKien);
            promoCard.Controls.Add(lblDateRange);
            promoCard.Controls.Add(lblStatus);

            // Add click events
            promoCard.Click += PromoCard_Click;
            lblDiscount.Click += (s, e) => PromoCard_Click(promoCard, e);
            lblTenKM.Click += (s, e) => PromoCard_Click(promoCard, e);
            lblDieuKien.Click += (s, e) => PromoCard_Click(promoCard, e);
            lblDateRange.Click += (s, e) => PromoCard_Click(promoCard, e);
            lblStatus.Click += (s, e) => PromoCard_Click(promoCard, e);

            flowLayoutPanelKhuyenMai.Controls.Add(promoCard);
        }

        private void PromoCard_Click(object sender, EventArgs e)
        {
            Panel clickedCard = sender as Panel;
            if (clickedCard == null) return;

            // Reset previous selection
            if (currentlySelectedPromotion != null && currentlySelectedPromotion != clickedCard)
            {
                string previousStatus = currentlySelectedPromotion.Tag?.ToString();
                if (previousStatus == "Active")
                {
                    currentlySelectedPromotion.BackColor = Color.LightGreen;
                }
                else
                {
                    currentlySelectedPromotion.BackColor = Color.LightGray;
                }
            }

            // Set new selection
            currentlySelectedPromotion = clickedCard;
            selectedPromotionId = clickedCard.Name;
            clickedCard.BackColor = Color.LightBlue;
        }

        private void LoadPromotionsFromDatabase()
        {
            try
            {
                flowLayoutPanelKhuyenMai.Controls.Clear();

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM KhuyenMai ORDER BY NgayBatDau DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    ds = new DataSet();

                    adapter.Fill(ds, "KhuyenMai");

                    foreach (DataRow row in ds.Tables["KhuyenMai"].Rows)
                    {
                        string maKM = row["MaKM"].ToString();
                        string tenKM = row["TenKM"].ToString();
                        string dieuKien = row["DieuKien"].ToString();
                        int phanTramGiam = Convert.ToInt32(row["PhanTramGiam"]);
                        DateTime ngayBatDau = Convert.ToDateTime(row["NgayBatDau"]);
                        DateTime ngayKetThuc = Convert.ToDateTime(row["NgayKetThuc"]);

                        AddPromotionCard(maKM, tenKM, dieuKien, phanTramGiam, ngayBatDau, ngayKetThuc);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải khuyến mãi: {ex.Message}");
            }
        }

        private void UserControl_KhuyenMai_Load(object sender, EventArgs e)
        {
            LoadPromotionsFromDatabase();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            Form addForm = new Form();
            addForm.Text = "Thêm Khuyến Mãi Mới";
            addForm.Size = new Size(450, 450);
            addForm.StartPosition = FormStartPosition.CenterParent;

            Label lblMaKM = new Label() { Text = "Mã KM:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtMaKM = new TextBox() { Location = new Point(150, 20), Width = 250 };

            Label lblTenKM = new Label() { Text = "Tên KM:", Location = new Point(20, 60), AutoSize = true };
            TextBox txtTenKM = new TextBox() { Location = new Point(150, 60), Width = 250 };

            Label lblDieuKien = new Label() { Text = "Điều kiện:", Location = new Point(20, 100), AutoSize = true };
            TextBox txtDieuKien = new TextBox() { Location = new Point(150, 100), Width = 250, Height = 60, Multiline = true };

            Label lblPhanTram = new Label() { Text = "Phần trăm giảm:", Location = new Point(20, 170), AutoSize = true };
            NumericUpDown numPhanTram = new NumericUpDown()
            {
                Location = new Point(150, 170),
                Width = 250,
                Minimum = 0,
                Maximum = 100,
                Value = 10
            };

            Label lblNgayBatDau = new Label() { Text = "Ngày bắt đầu:", Location = new Point(20, 210), AutoSize = true };
            DateTimePicker dtpNgayBatDau = new DateTimePicker()
            {
                Location = new Point(150, 210),
                Width = 250,
                Format = DateTimePickerFormat.Short
            };

            Label lblNgayKetThuc = new Label() { Text = "Ngày kết thúc:", Location = new Point(20, 250), AutoSize = true };
            DateTimePicker dtpNgayKetThuc = new DateTimePicker()
            {
                Location = new Point(150, 250),
                Width = 250,
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Now.AddMonths(1)
            };

            Button btnSave = new Button()
            {
                Text = "Lưu",
                Location = new Point(150, 320),
                Width = 100,
                BackColor = Color.LightGreen
            };

            Button btnCancel = new Button()
            {
                Text = "Hủy",
                Location = new Point(270, 320),
                Width = 100,
                BackColor = Color.LightCoral
            };

            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaKM.Text) || string.IsNullOrWhiteSpace(txtTenKM.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                    return;
                }

                if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value)
                {
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string query = @"INSERT INTO KhuyenMai (MaKM, TenKM, DieuKien, PhanTramGiam, NgayBatDau, NgayKetThuc) 
                                        VALUES (@MaKM, @TenKM, @DieuKien, @PhanTramGiam, @NgayBatDau, @NgayKetThuc)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaKM", txtMaKM.Text.Trim());
                            cmd.Parameters.AddWithValue("@TenKM", txtTenKM.Text.Trim());
                            cmd.Parameters.AddWithValue("@DieuKien", txtDieuKien.Text.Trim());
                            cmd.Parameters.AddWithValue("@PhanTramGiam", (int)numPhanTram.Value);
                            cmd.Parameters.AddWithValue("@NgayBatDau", dtpNgayBatDau.Value.Date);
                            cmd.Parameters.AddWithValue("@NgayKetThuc", dtpNgayKetThuc.Value.Date);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thêm khuyến mãi thành công!");
                            addForm.Close();
                            LoadPromotionsFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            };

            btnCancel.Click += (s, ev) => addForm.Close();

            addForm.Controls.AddRange(new Control[]
            {
                lblMaKM, txtMaKM,
                lblTenKM, txtTenKM,
                lblDieuKien, txtDieuKien,
                lblPhanTram, numPhanTram,
                lblNgayBatDau, dtpNgayBatDau,
                lblNgayKetThuc, dtpNgayKetThuc,
                btnSave, btnCancel
            });

            addForm.ShowDialog();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (currentlySelectedPromotion == null)
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần sửa!");
                return;
            }

            string maKM = selectedPromotionId;

            // Get current promotion data
            DataRow currentPromo = null;
            foreach (DataRow row in ds.Tables["KhuyenMai"].Rows)
            {
                if (row["MaKM"].ToString() == maKM)
                {
                    currentPromo = row;
                    break;
                }
            }

            if (currentPromo == null) return;

            Form editForm = new Form();
            editForm.Text = "Sửa Khuyến Mãi";
            editForm.Size = new Size(450, 450);
            editForm.StartPosition = FormStartPosition.CenterParent;

            Label lblTenKM = new Label() { Text = "Tên KM:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtTenKM = new TextBox()
            {
                Location = new Point(150, 20),
                Width = 250,
                Text = currentPromo["TenKM"].ToString()
            };

            Label lblDieuKien = new Label() { Text = "Điều kiện:", Location = new Point(20, 60), AutoSize = true };
            TextBox txtDieuKien = new TextBox()
            {
                Location = new Point(150, 60),
                Width = 250,
                Height = 60,
                Multiline = true,
                Text = currentPromo["DieuKien"].ToString()
            };

            Label lblPhanTram = new Label() { Text = "Phần trăm giảm:", Location = new Point(20, 130), AutoSize = true };
            NumericUpDown numPhanTram = new NumericUpDown()
            {
                Location = new Point(150, 130),
                Width = 250,
                Minimum = 0,
                Maximum = 100,
                Value = Convert.ToInt32(currentPromo["PhanTramGiam"])
            };

            Label lblNgayBatDau = new Label() { Text = "Ngày bắt đầu:", Location = new Point(20, 170), AutoSize = true };
            DateTimePicker dtpNgayBatDau = new DateTimePicker()
            {
                Location = new Point(150, 170),
                Width = 250,
                Format = DateTimePickerFormat.Short,
                Value = Convert.ToDateTime(currentPromo["NgayBatDau"])
            };

            Label lblNgayKetThuc = new Label() { Text = "Ngày kết thúc:", Location = new Point(20, 210), AutoSize = true };
            DateTimePicker dtpNgayKetThuc = new DateTimePicker()
            {
                Location = new Point(150, 210),
                Width = 250,
                Format = DateTimePickerFormat.Short,
                Value = Convert.ToDateTime(currentPromo["NgayKetThuc"])
            };

            Button btnSave = new Button()
            {
                Text = "Lưu",
                Location = new Point(150, 280),
                Width = 100,
                BackColor = Color.LightGreen
            };

            Button btnCancel = new Button()
            {
                Text = "Hủy",
                Location = new Point(270, 280),
                Width = 100,
                BackColor = Color.LightCoral
            };

            btnSave.Click += (s, ev) =>
            {
                if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value)
                {
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string query = @"UPDATE KhuyenMai 
                                        SET TenKM = @TenKM, 
                                            DieuKien = @DieuKien, 
                                            PhanTramGiam = @PhanTramGiam, 
                                            NgayBatDau = @NgayBatDau, 
                                            NgayKetThuc = @NgayKetThuc 
                                        WHERE MaKM = @MaKM";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaKM", maKM);
                            cmd.Parameters.AddWithValue("@TenKM", txtTenKM.Text.Trim());
                            cmd.Parameters.AddWithValue("@DieuKien", txtDieuKien.Text.Trim());
                            cmd.Parameters.AddWithValue("@PhanTramGiam", (int)numPhanTram.Value);
                            cmd.Parameters.AddWithValue("@NgayBatDau", dtpNgayBatDau.Value.Date);
                            cmd.Parameters.AddWithValue("@NgayKetThuc", dtpNgayKetThuc.Value.Date);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Sửa khuyến mãi thành công!");
                            editForm.Close();
                            LoadPromotionsFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            };

            btnCancel.Click += (s, ev) => editForm.Close();

            editForm.Controls.AddRange(new Control[]
            {
                lblTenKM, txtTenKM,
                lblDieuKien, txtDieuKien,
                lblPhanTram, numPhanTram,
                lblNgayBatDau, dtpNgayBatDau,
                lblNgayKetThuc, dtpNgayKetThuc,
                btnSave, btnCancel
            });

            editForm.ShowDialog();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (currentlySelectedPromotion == null)
            {
                MessageBox.Show("Vui lòng chọn khuyến mãi cần xóa!");
                return;
            }

            string maKM = selectedPromotionId;
            Label lblTenKM = currentlySelectedPromotion.Controls.Find("lblTenKM", false)[0] as Label;

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa khuyến mãi '{lblTenKM.Text}'?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();

                        // Delete related records first
                        string deleteRelated = "DELETE FROM HoaDon_KhuyenMai WHERE MaKM = @MaKM";
                        using (SqlCommand cmdRelated = new SqlCommand(deleteRelated, conn))
                        {
                            cmdRelated.Parameters.AddWithValue("@MaKM", maKM);
                            cmdRelated.ExecuteNonQuery();
                        }

                        // Then delete the promotion
                        string query = "DELETE FROM KhuyenMai WHERE MaKM = @MaKM";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaKM", maKM);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa khuyến mãi thành công!");

                            currentlySelectedPromotion = null;
                            selectedPromotionId = null;
                            LoadPromotionsFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}