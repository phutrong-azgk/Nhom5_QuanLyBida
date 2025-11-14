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
        DataSet ds;
        private Panel currentlySelectedTable = null; // Track selected table
        private Color previousColor; // Store original color

        public UserControl_Ban()
        {
            InitializeComponent();
        }

        private void AddTable(string tableName, string status, string price)
        {
            Panel tableBox = new Panel();
            tableBox.Size = new Size(170, 170);
            tableBox.BorderStyle = BorderStyle.FixedSingle;
            tableBox.Margin = new Padding(10);
            tableBox.Tag = new { Name = tableName, Status = status }; // Store data
            tableBox.Cursor = Cursors.Hand;

            // Set color based on status
            Color originalColor = GetColorByStatus(status);
            tableBox.BackColor = originalColor;

            Label lblTableName = new Label();
            lblTableName.Text = tableName;
            lblTableName.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTableName.TextAlign = ContentAlignment.MiddleCenter;
            lblTableName.Location = new Point(10, 30);
            lblTableName.AutoSize = true;

            Label lblStatus = new Label();
            lblStatus.Text = status;
            lblStatus.Font = new Font("Arial", 14);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Location = new Point(10, 70);
            lblStatus.AutoSize = true;

            Label lblPrice = new Label();
            lblPrice.Text = price + " / 1h";
            lblPrice.Font = new Font("Arial", 14);
            lblPrice.TextAlign = ContentAlignment.MiddleCenter;
            lblPrice.Location = new Point(10, 100);
            lblPrice.AutoSize = true;

            tableBox.Controls.Add(lblTableName);
            tableBox.Controls.Add(lblStatus);
            tableBox.Controls.Add(lblPrice);

            // Updated click event
            tableBox.Click += TableBox_Click;

            // Make labels clickable too
            lblTableName.Click += (s, e) => TableBox_Click(tableBox, e);
            lblStatus.Click += (s, e) => TableBox_Click(tableBox, e);
            lblPrice.Click += (s, e) => TableBox_Click(tableBox, e);

            flowLayoutPanelBan.Controls.Add(tableBox);
        }

        private Color GetColorByStatus(string status)
        {
            if (status == "Trống")
            {
                return Color.LightGreen;
            }
            else if (status == "Đang Chơi")
            {
                return Color.LightCoral;
            }
            else
            {
                return Color.LightGoldenrodYellow;
            }
        }

        private void TableBox_Click(object sender, EventArgs e)
        {
            Panel clickedTable = sender as Panel;
            if (clickedTable == null) return;

            // Reset previous selection
            if (currentlySelectedTable != null && currentlySelectedTable != clickedTable)
            {
                // Restore original color based on status
                var data = currentlySelectedTable.Tag as dynamic;
                if (data != null)
                {
                    currentlySelectedTable.BackColor = GetColorByStatus(data.Status);
                }
            }

            // Set new selection
            currentlySelectedTable = clickedTable;
            clickedTable.BackColor = Color.SlateBlue;
        }

        private void LoadTablesFromDatabase()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Ban";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    ds = new DataSet();

                    adapter.Fill(ds, "Ban");

                    // Loop through DataSet
                    foreach (DataRow row in ds.Tables["Ban"].Rows)
                    {
                        string tableName = row["TenBan"].ToString();
                        string status = row["TrangThai"].ToString();
                        string price = row["Gia"].ToString();

                        AddTable(tableName, status, price);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}");
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

            btnTinhTien.Enabled = true;
            btnBatGio.Enabled = false;
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            if (currentlySelectedTable == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!");
                return;
            }

            btnBatGio.Enabled = true;
            btnBatGio.BackColor = Color.LightGreen;
            btnBatGio.Focus();
            btnTinhTien.Enabled = false;

        }
    }
}