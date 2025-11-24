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
    public partial class UserControl_BaoCao : UserControl
    {
        public UserControl_BaoCao()
        {
            InitializeComponent();
        }

        

        private void UserControl_BaoCao_Load(object sender, EventArgs e)
        {
            NgayBatDau.CustomFormat = "dd/MM/yyyy";
            NgayKetThuc.CustomFormat = "dd/MM/yyyy";
            NgayBatDau.MaxDate = DateTime.Now;
            NgayKetThuc.MaxDate = DateTime.Now;
        }

        
    }
}
