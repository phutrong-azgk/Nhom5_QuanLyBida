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
        private bool isStarted = false;
        public UserControl_Ban()
        {
            InitializeComponent();
        }

        private void btnBatGio_Click(object sender, EventArgs e)
        {
            isStarted = !isStarted; // Toggle state

            if (isStarted)
            {
                btnBatGio.Text = "Tính tiền";
                btnBatGio.BackColor = Color.Red;
            }
            else
            {
                btnBatGio.Text = "Bật giờ";
                btnBatGio.BackColor = Color.Green;
            }
        }
    }
}
