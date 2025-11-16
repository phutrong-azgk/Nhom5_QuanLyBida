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
    public partial class UserControl_GoiMon : UserControl
    {
        private string selectedMaBan = null;
        private Dictionary<string, OrderItem> orderedItems = new Dictionary<string, OrderItem>();

        public class OrderItem
        {
            public string MaMon { get; set; }
            public string TenMon { get; set; }
            public int DonGia { get; set; }
            public int SoLuong { get; set; }
            public int ThanhTien => DonGia * SoLuong;
        }

        public UserControl_GoiMon()
        {
            InitializeComponent();
            UserControl_Ban.TableStatusChanged += (s, e) => LoadAvailableTables();
        }

        private void UserControl_GoiMon_Load(object sender, EventArgs e)
        {
            tlpMonDaChon.Visible = false;
            LoadComboBoxCategories();
            LoadAvailableTables();
            LoadMenuItems("Tất cả");
            UpdateTotalMoney();
        }

        // Load categories into combobox
        private void LoadComboBoxCategories()
        {
            cmbMon.Items.Clear();
            cmbMon.Items.AddRange(new string[] { "Tất cả", "Đồ ăn", "Đồ uống" });
            cmbMon.SelectedIndex = 0;
            cmbMon.SelectedIndexChanged += CmbMon_SelectedIndexChanged;
        }

        private void CmbMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = cmbMon.SelectedItem.ToString();
            LoadMenuItems(category);
        }

        // Load active tables into panelChonBan
        private void LoadAvailableTables()
        {
            try
            {
                // Clear previous controls except the label
                for (int i = tlpChonBan.Controls.Count - 1; i >= 0; i--)
                {
                    if (tlpChonBan.Controls[i] != label4)
                    {
                        tlpChonBan.Controls.RemoveAt(i);
                    }
                }

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaBan, TenBan FROM Ban WHERE TrangThai = N'Đang Chơi'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (!reader.HasRows)
                        {
                            // No active tables
                            ShowNoTablesMessage();
                        }
                        else
                        {
                            // Remove "no tables" message if exists
                            tlpChonBan.RowCount = 2;

                            FlowLayoutPanel flowTables = new FlowLayoutPanel();
                            flowTables.Dock = DockStyle.Fill;
                            flowTables.AutoScroll = true;
                            tlpChonBan.Controls.Add(flowTables, 0, 1);

                            while (reader.Read())
                            {
                                string maBan = reader["MaBan"].ToString();
                                string tenBan = reader["TenBan"].ToString();

                                Button btnTable = new Button();
                                btnTable.Text = tenBan;
                                btnTable.Size = new Size(100, 50);
                                btnTable.Margin = new Padding(5);
                                btnTable.Tag = maBan;
                                btnTable.BackColor = Color.LightGreen;
                                btnTable.Click += BtnTable_Click;

                                flowTables.Controls.Add(btnTable);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load bàn: {ex.Message}");
            }
        }

        private void ShowNoTablesMessage()
        {
            Label lblNoTables = new Label();
            lblNoTables.Text = "Không có bàn nào đang chơi";
            lblNoTables.Font = new Font("Arial", 12, FontStyle.Italic);
            lblNoTables.ForeColor = Color.Gray;
            lblNoTables.TextAlign = ContentAlignment.MiddleCenter;
            lblNoTables.Dock = DockStyle.Fill;
            tlpChonBan.Controls.Add(lblNoTables, 0, 1);
        }

        private void BtnTable_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            selectedMaBan = btn.Tag.ToString();

            // Highlight selected table
            foreach (Control ctrl in btn.Parent.Controls)
            {
                if (ctrl is Button)
                {
                    ((Button)ctrl).BackColor = Color.LightGreen;
                }
            }
            btn.BackColor = Color.LightBlue;
        }

        // Load menu items based on category
        private void LoadMenuItems(string category)
        {
            try
            {
                // Clear previous items except header
                for (int i = tlpChonMon.Controls.Count - 1; i >= 0; i--)
                {
                    if (tlpChonMon.Controls[i] != tableLayoutPanel5)
                    {
                        tlpChonMon.Controls.RemoveAt(i);
                    }
                }

                FlowLayoutPanel flowMenu = new FlowLayoutPanel();
                flowMenu.Dock = DockStyle.Fill;
                flowMenu.AutoScroll = true;
                flowMenu.BackColor = Color.FromArgb(255, 255, 192);
                tlpChonMon.Controls.Add(flowMenu, 0, 1);

                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT MaMon, TenMon, DonGia FROM Mon WHERE SoLuongTon > 0";

                    // Filter by category
                    if (category == "Đồ uống")
                        query += " AND MaMon LIKE 'U%'";
                    else if (category == "Đồ ăn")
                        query += " AND MaMon LIKE 'A%'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Panel itemPanel = CreateMenuItem(
                                reader["MaMon"].ToString(),
                                reader["TenMon"].ToString(),
                                Convert.ToInt32(reader["DonGia"])
                            );
                            flowMenu.Controls.Add(itemPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load món: {ex.Message}");
            }
        }

        private Panel CreateMenuItem(string maMon, string tenMon, int donGia)
        {
            Panel panel = new Panel();
            panel.Size = new Size(180, 120);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Margin = new Padding(5);
            panel.BackColor = Color.White;
            panel.Cursor = Cursors.Hand;
            panel.Tag = new { MaMon = maMon, TenMon = tenMon, DonGia = donGia };

            Label lblName = new Label();
            lblName.Text = tenMon;
            lblName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblName.Location = new Point(10, 20);
            lblName.AutoSize = true;

            Label lblPrice = new Label();
            lblPrice.Text = donGia.ToString("N0") + " VNĐ";
            lblPrice.Font = new Font("Arial", 9);
            lblPrice.ForeColor = Color.Green;
            lblPrice.Location = new Point(10, 50);
            lblPrice.AutoSize = true;

            Button btnAdd = new Button();
            btnAdd.Text = "Thêm";
            btnAdd.Size = new Size(160, 30);
            btnAdd.Location = new Point(10, 80);
            btnAdd.BackColor = Color.LightGreen;
            btnAdd.Click += (s, e) =>
            {
                if (selectedMaBan == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn trước!");
                    return;
                }
                AddToOrder(maMon, tenMon, donGia);
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(btnAdd);

            return panel;
        }

        private void AddToOrder(string maMon, string tenMon, int donGia)
        {
            if (orderedItems.ContainsKey(maMon))
            {
                orderedItems[maMon].SoLuong++;
            }
            else
            {
                orderedItems[maMon] = new OrderItem
                {
                    MaMon = maMon,
                    TenMon = tenMon,
                    DonGia = donGia,
                    SoLuong = 1
                };
            }

            tlpMonDaChon.Visible = true;
            UpdateOrderList();
            UpdateTotalMoney();
        }

        private void UpdateOrderList()
        {
            // Clear previous order items except header
            for (int i = tlpMonDaChon.Controls.Count - 1; i >= 0; i--)
            {
                if (tlpMonDaChon.Controls[i] != label5)
                {
                    tlpMonDaChon.Controls.RemoveAt(i);
                }
            }

            if (orderedItems.Count == 0)
            {
                tlpMonDaChon.Visible = false;
                return;
            }

            FlowLayoutPanel flowOrders = new FlowLayoutPanel();
            flowOrders.Dock = DockStyle.Fill;
            flowOrders.AutoScroll = true;
            flowOrders.BackColor = Color.FromArgb(255, 255, 192);
            tlpMonDaChon.Controls.Add(flowOrders, 0, 1);

            foreach (var item in orderedItems.Values)
            {
                Panel orderPanel = CreateOrderItem(item);
                flowOrders.Controls.Add(orderPanel);
            }
        }

        private Panel CreateOrderItem(OrderItem item)
        {
            Panel panel = new Panel();
            panel.Size = new Size(550, 80);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Margin = new Padding(5);
            panel.BackColor = Color.White;

            Label lblName = new Label();
            lblName.Text = item.TenMon;
            lblName.Font = new Font("Arial", 10, FontStyle.Bold);
            lblName.Location = new Point(10, 10);
            lblName.Size = new Size(200, 20);

            Label lblPrice = new Label();
            lblPrice.Text = $"{item.DonGia:N0} VNĐ";
            lblPrice.Font = new Font("Arial", 9);
            lblPrice.Location = new Point(10, 35);
            lblPrice.AutoSize = true;

            Label lblTotal = new Label();
            lblTotal.Text = $"= {item.ThanhTien:N0} VNĐ";
            lblTotal.Font = new Font("Arial", 9, FontStyle.Bold);
            lblTotal.ForeColor = Color.Green;
            lblTotal.Location = new Point(10, 55);
            lblTotal.AutoSize = true;

            // Quantity controls
            Button btnMinus = new Button();
            btnMinus.Text = "-";
            btnMinus.Size = new Size(30, 30);
            btnMinus.Location = new Point(300, 25);
            btnMinus.Click += (s, e) =>
            {
                if (item.SoLuong > 1)
                {
                    item.SoLuong--;
                    UpdateOrderList();
                    UpdateTotalMoney();
                }
            };

            Label lblQuantity = new Label();
            lblQuantity.Text = item.SoLuong.ToString();
            lblQuantity.Font = new Font("Arial", 11, FontStyle.Bold);
            lblQuantity.Location = new Point(340, 30);
            lblQuantity.Size = new Size(40, 20);
            lblQuantity.TextAlign = ContentAlignment.MiddleCenter;

            Button btnPlus = new Button();
            btnPlus.Text = "+";
            btnPlus.Size = new Size(30, 30);
            btnPlus.Location = new Point(385, 25);
            btnPlus.Click += (s, e) =>
            {
                item.SoLuong++;
                UpdateOrderList();
                UpdateTotalMoney();
            };

            Button btnRemove = new Button();
            btnRemove.Text = "Xóa";
            btnRemove.Size = new Size(60, 30);
            btnRemove.Location = new Point(450, 25);
            btnRemove.BackColor = Color.LightCoral;
            btnRemove.Click += (s, e) =>
            {
                orderedItems.Remove(item.MaMon);
                UpdateOrderList();
                UpdateTotalMoney();
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(lblTotal);
            panel.Controls.Add(btnMinus);
            panel.Controls.Add(lblQuantity);
            panel.Controls.Add(btnPlus);
            panel.Controls.Add(btnRemove);

            return panel;
        }

        private void UpdateTotalMoney()
        {
            lblTongTien.ForeColor= Color.Black;
            int total = orderedItems.Values.Sum(item => item.ThanhTien);
            lblTongTien.Text = total.ToString("N0");
        }
    }
}