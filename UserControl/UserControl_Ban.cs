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

            // Update button states based on saved state
            UpdateButtonStatesForSelectedTable();
        }

        private void UpdateButtonStatesForSelectedTable()
        {
            if (selectedTableId == null) return;

            var state = TableStateManager.GetTableState(selectedTableId);

            if (state != null && state.IsTimerActive)
            {
                // Timer is running
                btnBatGio.Enabled = false;
                btnTinhTien.Enabled = true;
            }
            else
            {
                // Timer not running
                btnBatGio.Enabled = true;
                btnBatGio.BackColor = Color.LightGreen;
                btnTinhTien.Enabled = false;
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

                            // Clear state when deleting table
                            TableStateManager.ClearTableState(maBan);

                            currentlySelectedTable = null;
                            selectedTableId = null;
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

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Ban SET TrangThai = N'Đang Chơi' WHERE MaBan = @MaBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBan", maBan);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Save state
                TableStateManager.SetTableState(maBan, true, DateTime.Now);

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

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!");
                return;
            }

            string maBan = selectedTableId;

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Ban SET TrangThai = N'Trống' WHERE MaBan = @MaBan";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaBan", maBan);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Clear state when payment is done
                TableStateManager.ClearTableState(maBan);

                btnBatGio.Enabled = true;
                btnBatGio.BackColor = Color.LightGreen;
                btnBatGio.Focus();
                btnTinhTien.Enabled = false;

                LoadTablesFromDatabase();
                TableStatusChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}