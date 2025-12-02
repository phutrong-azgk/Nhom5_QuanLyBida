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
using Nhom5_QuanLyBida.Forms;

namespace Nhom5_QuanLyBida
{
    public partial class MainForm : Form
    {
        public string vaitro = DangNhap.vaitro;
        private bool isLoggingOut = false;
        public MainForm()
        {
            InitializeComponent();

        }

        private void ShowScreen(UserControl screen)
        {
            PanelMain.Controls.Clear();
            screen.Dock = DockStyle.Fill;
            PanelMain.Controls.Add(screen);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (DesignMode) return;

            lblTenDN.Text = "Xin chào " + vaitro;
            if (vaitro == "ADMIN")
            {
                
            }
            else if (vaitro == "Quản Lý")
            {
                btnTaiKhoan.Enabled = false;
            }
            else if (vaitro == "Thu Ngân")
            {
                btnTaiKhoan.Enabled = false;
                btnNhanVien.Enabled = false;
                btnKho.Enabled = false;
            }
            else if (vaitro == "Thủ Kho")
            {
                btnTaiKhoan.Enabled = false;
                btnKhach.Enabled = false;
                btnNhanVien.Enabled = false;
                btnGoiMon.Enabled = false;
                btnBan.Enabled = false;
            }
            ShowScreen(new UserControl_Ban());
            
        }
        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
               "Bạn có chắc chắn muốn đăng xuất?",
               "Xác nhận đăng xuất",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                isLoggingOut = true;
                this.Close();  
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isLoggingOut)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình?",
                    "Xác nhận thoát",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
        public bool IsLoggingOut
        {
            get { return isLoggingOut; }
        }

        private void btnBan_EnabledChanged(object sender, EventArgs e)
        {
            Button currentButton = (Button)sender;
            if (currentButton.Enabled == false)
            {
                currentButton.BackColor = Color.LightGray; 
                currentButton.ForeColor = Color.DarkOrchid; 
            }
            else
            {
                currentButton.BackColor = SystemColors.Control;
                currentButton.ForeColor = SystemColors.ControlText;
            }
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_Ban());
        }

        private void btnGoiMon_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_GoiMon());
        }

        private void btnBaoCao_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_BaoCao());
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_QLKho());
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_QuanLyTK());
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_NhanVien());
        }

        private void btnKhach_Click(object sender, EventArgs e)
        {
            ShowScreen(new UserControl_Khach());
        }
    }


    public static class DatabaseHelper//class để lết nối csdl
    {
        private static string connstr = "Data Source=.;Initial Catalog=QuanLyBida;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connstr);
        }

        public static string ConnectionString
        {
            get { return connstr; }
        }
    }

}
