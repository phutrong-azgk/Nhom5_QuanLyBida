using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_QuanLyBida
{
    public partial class UserControl_Ban : UserControl
    {

        public UserControl_Ban()
        {
            InitializeComponent();
        }
        


        private void UserControl_Ban_Load(object sender, EventArgs e)
        {
            btnTinhTien.Enabled = false;
            lblNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblThoiGian.Text = DateTime.Now.ToShortTimeString();
            
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
            btnTinhTien.Enabled=true;
            btnBatGio.Enabled=false;

        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            btnBatGio.Enabled =true;
            btnBatGio.BackColor = Color.LightGreen;
            btnBatGio.Focus();
            btnTinhTien.Enabled=false;
        }
    }
}
