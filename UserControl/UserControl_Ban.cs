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
    public partial class UserControl_Ban : UserControl
    {
        public static event EventHandler TableStatusChanged;

        DataSet ds;
        private Panel currentlySelectedTable = null;
        private string selectedTableId = null;
        private string currentMaHD = null; // Lưu mã hóa đơn hiện tại

        public UserControl_Ban()
        {
            InitializeComponent();
        }

        private void AddTable(string maBan, string tableName, string status, string price)
        {
            Panel tableBox = new Panel();
            tableBox.Size = new Size(170, 170);
            tableBox.BorderStyle = BorderStyle.FixedSingle;
            tableBox.Margin = new Padding(10);
            tableBox.Name = maBan;
            tableBox.Tag = status;
            tableBox.Cursor = Cursors.Hand;

            Color originalColor = GetColorByStatus(status);
            tableBox.BackColor = originalColor;

            Label lblTableName = new Label();
            lblTableName.Name = "lblTableName";
            lblTableName.Text = tableName;
            lblTableName.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTableName.TextAlign = ContentAlignment.MiddleCenter;
            lblTableName.Location = new Point(10, 30);
            lblTableName.AutoSize = true;

            Label lblStatus = new Label();
            lblStatus.Name = "lblStatus";
            lblStatus.Text = status;
            lblStatus.Font = new Font("Arial", 14);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Location = new Point(10, 70);
            lblStatus.AutoSize = true;

            Label lblPrice = new Label();
            lblPrice.Name = "lblPrice";
            lblPrice.Text = price + " / 1h";
            lblPrice.Font = new Font("Arial", 14);
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;
            lblPrice.Location = new Point(10, 100);
            lblPrice.AutoSize = true;

            tableBox.Controls.Add(lblTableName);
            tableBox.Controls.Add(lblStatus);
            tableBox.Controls.Add(lblPrice);

            tableBox.Click += TableBox_Click;
            lblTableName.Click += (s, e) => TableBox_Click(tableBox, e);
            lblStatus.Click += (s, e) => TableBox_Click(tableBox, e);
            lblPrice.Click += (s, e) => TableBox_Click(tableBox, e);

            flowLayoutPanelBan.Controls.Add(tableBox);
        }

        private Color GetColorByStatus(string status)
        {
            switch (status)
            {
                case "Trống":
                    return Color.LightGreen;
                case "Đang Chơi":
                    return Color.LightCoral;
                case "Đặt Trước":
                    return Color.LightGoldenrodYellow;
                default:
                    return Color.LightGray;
            }
        }

        private void TableBox_Click(object sender, EventArgs e)
        {
            Panel clickedTable = sender as Panel;
            if (clickedTable == null) return;

            // Reset previous selection
            if (currentlySelectedTable != null && currentlySelectedTable != clickedTable)
            {
                string previousStatus = currentlySelectedTable.Tag?.ToString();
                if (previousStatus != null)
                {
                    currentlySelectedTable.BackColor = GetColorByStatus(previousStatus);
                }
            }

            // Set new selection
            currentlySelectedTable = clickedTable;
            selectedTableId = clickedTable.Name;
            clickedTable.BackColor = Color.SlateBlue;

            // Update button states and get current invoice
            UpdateButtonStatesForSelectedTable();
        }

        private void UpdateButtonStatesForSelectedTable()
        {
            if (selectedTableId == null) return;

            // Kiểm tra xem bàn có hóa đơn đang hoạt động không
            currentMaHD = GetActiveInvoiceForTable(selectedTableId);

            if (currentMaHD != null)
            {
                // Có hóa đơn đang hoạt động - bàn đang chơi
                btnBatGio.Enabled = false;
                btnTinhTien.Enabled = true;
            }
            else
            {
                // Không có hóa đơn - bàn trống hoặc đặt trước
                string status = currentlySelectedTable?.Tag?.ToString();
                if (status == "Đang Chơi")
                {
                    // Trường hợp bàn "Đang Chơi" nhưng không có hóa đơn (dữ liệu không đồng bộ)
                    btnBatGio.Enabled = false;
                    btnTinhTien.Enabled = false;
                }
                else
                {
                    btnBatGio.Enabled = true;
                    btnBatGio.BackColor = Color.LightGreen;
                    btnTinhTien.Enabled = false;
                }
            }
        }

        private string GetActiveInvoiceForTable(string maBan)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT TOP 1 MaHD 
                                    FROM HoaDon 
                                    WHERE MaBan = @MaBan 
                                    AND GioKetThuc IS NULL 
                                    ORDER BY GioBatDau DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBan", maBan);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kiểm tra hóa đơn: {ex.Message}");
                return null;
            }
        }

        private void LoadTablesFromDatabase()
        {
            try
            {
                flowLayoutPanelBan.Controls.Clear();

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Ban";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    ds = new DataSet();

                    adapter.Fill(ds, "Ban");

                    foreach (DataRow row in ds.Tables["Ban"].Rows)
                    {
                        string maBan = row["MaBan"].ToString();
                        string tableName = row["TenBan"].ToString();
                        string status = row["TrangThai"].ToString();
                        string price = row["Gia"].ToString();

                        AddTable(maBan, tableName, status, price);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}");
            }
        }

        // ========== BUTTON EVENTS ==========

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            Form addForm = new Form();
            addForm.Text = "Thêm Bàn Mới";
            addForm.Size = new Size(400, 300);
            addForm.StartPosition = FormStartPosition.CenterParent;

            Label lblMaBan = new Label() { Text = "Mã Bàn:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtMaBan = new TextBox() { Location = new Point(120, 20), Width = 200 };

            Label lblTenBan = new Label() { Text = "Tên Bàn:", Location = new Point(20, 60), AutoSize = true };
            TextBox txtTenBan = new TextBox() { Location = new Point(120, 60), Width = 200 };

            Label lblGia = new Label() { Text = "Giá:", Location = new Point(20, 100), AutoSize = true };
            TextBox txtGia = new TextBox() { Location = new Point(120, 100), Width = 200, Text = "100000" };

            Label lblTrangThai = new Label() { Text = "Trạng Thái:", Location = new Point(20, 140), AutoSize = true };
            ComboBox cmbTrangThai = new ComboBox()
            {
                Location = new Point(120, 140),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTrangThai.Items.AddRange(new string[] { "Trống", "Đang Chơi", "Đặt Trước" });
            cmbTrangThai.SelectedIndex = 0;

            Button btnSave = new Button() { Text = "Lưu", Location = new Point(120, 200), Width = 80 };
            Button btnCancel = new Button() { Text = "Hủy", Location = new Point(220, 200), Width = 80 };

            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaBan.Text) || string.IsNullOrWhiteSpace(txtTenBan.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "INSERT INTO Ban (MaBan, TenBan, TrangThai, Gia) VALUES (@MaBan, @TenBan, @TrangThai, @Gia)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaBan", txtMaBan.Text.Trim());
                            cmd.Parameters.AddWithValue("@TenBan", txtTenBan.Text.Trim());
                            cmd.Parameters.AddWithValue("@TrangThai", cmbTrangThai.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Gia", int.Parse(txtGia.Text));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Thêm bàn thành công!");
                            addForm.Close();
                            LoadTablesFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            };

            btnCancel.Click += (s, ev) => addForm.Close();

            addForm.Controls.AddRange(new Control[] { lblMaBan, txtMaBan, lblTenBan, txtTenBan, lblGia, txtGia, lblTrangThai, cmbTrangThai, btnSave, btnCancel });
            addForm.ShowDialog();
        }

        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần sửa!");
                return;
            }

            string maBan = selectedTableId;
            Label lblTableName = currentlySelectedTable.Controls.Find("lblTableName", false)[0] as Label;
            Label lblPrice = currentlySelectedTable.Controls.Find("lblPrice", false)[0] as Label;
            string currentPrice = lblPrice.Text.Replace(" / 1h", "").Trim();

            Form editForm = new Form();
            editForm.Text = "Sửa Bàn";
            editForm.Size = new Size(400, 250);
            editForm.StartPosition = FormStartPosition.CenterParent;

            Label lblTenBan = new Label() { Text = "Tên Bàn:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtTenBan = new TextBox() { Location = new Point(120, 20), Width = 200, Text = lblTableName.Text };

            Label lblGia = new Label() { Text = "Giá:", Location = new Point(20, 60), AutoSize = true };
            TextBox txtGia = new TextBox() { Location = new Point(120, 60), Width = 200, Text = currentPrice };

            Label lblTrangThai = new Label() { Text = "Trạng Thái:", Location = new Point(20, 100), AutoSize = true };
            ComboBox cmbTrangThai = new ComboBox()
            {
                Location = new Point(120, 100),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbTrangThai.Items.AddRange(new string[] { "Trống", "Đang Chơi", "Đặt Trước" });
            cmbTrangThai.SelectedItem = currentlySelectedTable.Tag.ToString();

            Button btnSave = new Button() { Text = "Lưu", Location = new Point(120, 160), Width = 80 };
            Button btnCancel = new Button() { Text = "Hủy", Location = new Point(220, 160), Width = 80 };

            btnSave.Click += (s, ev) =>
            {
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "UPDATE Ban SET TenBan = @TenBan, TrangThai = @TrangThai, Gia = @Gia WHERE MaBan = @MaBan";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaBan", maBan);
                            cmd.Parameters.AddWithValue("@TenBan", txtTenBan.Text.Trim());
                            cmd.Parameters.AddWithValue("@TrangThai", cmbTrangThai.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@Gia", int.Parse(txtGia.Text));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Sửa bàn thành công!");
                            editForm.Close();
                            LoadTablesFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            };

            btnCancel.Click += (s, ev) => editForm.Close();

            editForm.Controls.AddRange(new Control[] { lblTenBan, txtTenBan, lblGia, txtGia, lblTrangThai, cmbTrangThai, btnSave, btnCancel });
            editForm.ShowDialog();
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần xóa!");
                return;
            }

            string maBan = selectedTableId;
            Label lblTableName = currentlySelectedTable.Controls.Find("lblTableName", false)[0] as Label;

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa {lblTableName.Text}?",
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
                        string query = "DELETE FROM Ban WHERE MaBan = @MaBan";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaBan", maBan);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Xóa bàn thành công!");

                            currentlySelectedTable = null;
                            selectedTableId = null;
                            currentMaHD = null;
                            LoadTablesFromDatabase();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }

        private void btnDatTruoc_Click_1(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần đặt trước!");
                return;
            }

            string maBan = selectedTableId;
            string currentStatus = currentlySelectedTable.Tag.ToString();

            if (currentStatus == "Đang Chơi")
            {
                MessageBox.Show("Không thể đặt trước bàn đang được sử dụng!");
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Ban SET TrangThai = N'Đặt Trước' WHERE MaBan = @MaBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBan", maBan);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Đặt trước bàn thành công!");
                        LoadTablesFromDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void UserControl_Ban_Load(object sender, EventArgs e)
        {
            btnTinhTien.Enabled = false;
            lblNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblThoiGian.Text = DateTime.Now.ToShortTimeString();
            LoadTablesFromDatabase();
        }

        private void btnBatGio_EnabledChanged(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;
            if (currentButton.Enabled == false)
            {
                currentButton.BackColor = Color.LightGray;
                currentButton.ForeColor = Color.DarkOrchid;
            }
            else
            {
                currentButton.BackColor = Color.Gold;
                currentButton.ForeColor = Color.Black;
            }
        }

        private void btnBatGio_Click(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!");
                return;
            }

            string maBan = selectedTableId;

            // Hiển thị form chọn khách hàng (tùy chọn)
            string maKhach = ShowCustomerSelectionForm();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Lấy MaNV từ thông tin đăng nhập
                    string maNV = GetEmployeeIdFromUsername();

                    if (string.IsNullOrEmpty(maNV))
                    {
                        MessageBox.Show("Không tìm thấy thông tin nhân viên!");
                        return;
                    }

                    // Sử dụng stored procedure sp_BatGio
                    using (SqlCommand cmd = new SqlCommand("sp_BatGio", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaBan", maBan);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);

                        if (!string.IsNullOrEmpty(maKhach))
                            cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                        else
                            cmd.Parameters.AddWithValue("@MaKhach", DBNull.Value);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Bật giờ thành công!");
                    }
                }

                btnTinhTien.Enabled = true;
                btnBatGio.Enabled = false;

                LoadTablesFromDatabase();
                TableStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private string GetEmployeeIdFromUsername()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT nv.MaNV 
                                    FROM NhanVien nv 
                                    INNER JOIN TaiKhoan tk ON nv.TenDangNhap = tk.TenDangNhap 
                                    WHERE tk.VaiTro = @VaiTro";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@VaiTro", DangNhap.vaitro);
                        object result = cmd.ExecuteScalar();
                        return result?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lấy thông tin nhân viên: {ex.Message}");
                return null;
            }
        }

        private string ShowCustomerSelectionForm()
        {
            Form customerForm = new Form();
            customerForm.Text = "Chọn khách hàng";
            customerForm.Size = new Size(500, 400);
            customerForm.StartPosition = FormStartPosition.CenterParent;

            Label lblTitle = new Label()
            {
                Text = "Chọn khách hàng (tùy chọn)",
                Location = new Point(20, 20),
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true
            };

            ComboBox cmbCustomer = new ComboBox()
            {
                Location = new Point(20, 60),
                Width = 440,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Load danh sách khách hàng
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaKhach, TenKhach FROM Khach ORDER BY TenKhach";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            cmbCustomer.Items.Add(new
                            {
                                MaKhach = reader["MaKhach"].ToString(),
                                TenKhach = reader["TenKhach"].ToString()
                            });
                        }
                    }
                }

                cmbCustomer.DisplayMember = "TenKhach";
                cmbCustomer.ValueMember = "MaKhach";
                cmbCustomer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách khách: {ex.Message}");
            }

            // Button thêm khách hàng mới
            Button btnAddNew = new Button()
            {
                Text = "Thêm khách mới",
                Location = new Point(20, 100),
                Width = 150,
                Height = 35
            };

            btnAddNew.Click += (s, ev) =>
            {
                string newCustomerId = ShowAddCustomerForm();
                if (!string.IsNullOrEmpty(newCustomerId))
                {
                    // Reload customer list
                    cmbCustomer.Items.Clear();
                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            conn.Open();
                            string query = "SELECT MaKhach, TenKhach FROM Khach ORDER BY TenKhach";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                SqlDataReader reader = cmd.ExecuteReader();

                                cmbCustomer.Items.Add(new { MaKhach = "", TenKhach = "-- Không chọn khách --" });

                                while (reader.Read())
                                {
                                    var item = new
                                    {
                                        MaKhach = reader["MaKhach"].ToString(),
                                        TenKhach = reader["TenKhach"].ToString()
                                    };

                                    cmbCustomer.Items.Add(item);

                                    // Select the newly added customer
                                    if (item.MaKhach == newCustomerId)
                                    {
                                        cmbCustomer.SelectedItem = item;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi tải lại danh sách: {ex.Message}");
                    }
                }
            };

            Button btnOK = new Button()
            {
                Text = "Xác nhận",
                Location = new Point(250, 300),
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button()
            {
                Text = "Bỏ qua",
                Location = new Point(360, 300),
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.Cancel
            };

            customerForm.Controls.AddRange(new Control[] { lblTitle, cmbCustomer, btnAddNew, btnOK, btnCancel });
            customerForm.AcceptButton = btnOK;
            customerForm.CancelButton = btnCancel;

            string selectedCustomer = null;
            if (customerForm.ShowDialog() == DialogResult.OK)
            {
                if (cmbCustomer.SelectedItem != null)
                {
                    dynamic selected = cmbCustomer.SelectedItem;
                    selectedCustomer = selected.MaKhach;
                }
            }

            return selectedCustomer;
        }

        private string ShowAddCustomerForm()
        {
            Form addForm = new Form();
            addForm.Text = "Thêm khách hàng mới";
            addForm.Size = new Size(400, 250);
            addForm.StartPosition = FormStartPosition.CenterParent;

            Label lblMaKhach = new Label() { Text = "Mã khách:", Location = new Point(20, 20), AutoSize = true };
            TextBox txtMaKhach = new TextBox() { Location = new Point(120, 20), Width = 240 };

            Label lblTenKhach = new Label() { Text = "Tên khách:", Location = new Point(20, 60), AutoSize = true };
            TextBox txtTenKhach = new TextBox() { Location = new Point(120, 60), Width = 240 };

            Button btnSave = new Button() { Text = "Lưu", Location = new Point(160, 150), Width = 80 };
            Button btnCancel = new Button() { Text = "Hủy", Location = new Point(260, 150), Width = 80 };

            string newCustomerId = null;

            btnSave.Click += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(txtMaKhach.Text) || string.IsNullOrWhiteSpace(txtTenKhach.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                    return;
                }

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        string query = "INSERT INTO Khach (MaKhach, TenKhach, SoHoaDon, TongChi) VALUES (@MaKhach, @TenKhach, 0, 0)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@MaKhach", txtMaKhach.Text.Trim());
                            cmd.Parameters.AddWithValue("@TenKhach", txtTenKhach.Text.Trim());

                            cmd.ExecuteNonQuery();
                            newCustomerId = txtMaKhach.Text.Trim();
                            MessageBox.Show("Thêm khách hàng thành công!");
                            addForm.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            };

            btnCancel.Click += (s, ev) => addForm.Close();

            addForm.Controls.AddRange(new Control[] { lblMaKhach, txtMaKhach, lblTenKhach, txtTenKhach, btnSave, btnCancel });
            addForm.ShowDialog();

            return newCustomerId;
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!");
                return;
            }
            string maBan = selectedTableId;
            // Lấy hóa đơn hiện tại của bàn
            string maHD = GetActiveInvoiceForTable(maBan);
            if (maHD == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn cho bàn này!");
                return;
            }
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // Lấy MaKhach từ HoaDon
                    string maKhach = null;
                    string queryGetMaKhach = "SELECT MaKhach FROM HoaDon WHERE MaHD = @MaHD";
                    using (SqlCommand cmdGetMaKhach = new SqlCommand(queryGetMaKhach, conn))
                    {
                        cmdGetMaKhach.Parameters.AddWithValue("@MaHD", maHD);
                        object result = cmdGetMaKhach.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            maKhach = result.ToString();
                        }
                    }

                    // Kiểm tra nếu không có MaKhach
                    if (string.IsNullOrEmpty(maKhach))
                    {
                        MessageBox.Show("Không tìm thấy thông tin khách hàng cho hóa đơn này!");
                        return;
                    }

                    // Sử dụng stored procedure sp_TinhTienHoaDon
                    using (SqlCommand cmd = new SqlCommand("sp_TinhTienHoaDon", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                        cmd.Parameters.AddWithValue("@MaKhach", maKhach);
                        cmd.ExecuteNonQuery();
                    }

                    // Lấy tổng tiền để hiển thị
                    string queryTotal = "SELECT TongTien FROM HoaDon WHERE MaHD = @MaHD";
                    using (SqlCommand cmdTotal = new SqlCommand(queryTotal, conn))
                    {
                        cmdTotal.Parameters.AddWithValue("@MaHD", maHD);
                        object result = cmdTotal.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            decimal tongTien = Convert.ToDecimal(result);
                            MessageBox.Show($"Tính tiền thành công!\n\nMã hóa đơn: {maHD}\nMã khách: {maKhach}\nTổng tiền: {tongTien:N0} VNĐ",
                                "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Tính tiền thành công!\n\nMã hóa đơn: {maHD}\nMã khách: {maKhach}\nTổng tiền: 0 VNĐ",
                                "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    // Cập nhật trạng thái bàn về Trống
                    string queryUpdateTable = "UPDATE Ban SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                    using (SqlCommand cmdUpdate = new SqlCommand(queryUpdateTable, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@MaBan", maBan);
                        cmdUpdate.ExecuteNonQuery();
                    }
                }

                // Clear selection
                currentMaHD = null;
                currentlySelectedTable = null;
                selectedTableId = null;

                // Reload tables
                LoadTablesFromDatabase();
                TableStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}\n\nStack Trace: {ex.StackTrace}");
            }
        }
    }
}