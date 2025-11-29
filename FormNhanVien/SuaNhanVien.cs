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

namespace Nhom5_QuanLyBida.FormNhanVien
{
    public partial class SuaNhanVien : Form
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public SuaNhanVien()
        {
            InitializeComponent();
        }

        private void SuaNhanVien_Load(object sender, EventArgs e)
        {
            txtMaNV.Text = MaNV;
            txtTenNV.Text = TenNV;
            txtNgaySinh.Text = NgaySinh.ToString("dd/MM/yyyy");
            rdoNam.Checked = GioiTinh == "Nam" ? true : false;
            rdoNu.Checked = GioiTinh == "Nữ" ? true : false;
            txtSDT.Text = SDT;
            txtDiaChi.Text = DiaChi;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaNV.Text) || string.IsNullOrWhiteSpace(txtTenNV.Text) ||
                string.IsNullOrWhiteSpace(txtNgaySinh.Text) || string.IsNullOrWhiteSpace(txtSDT.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Phải điền thông tin đầy đủ");
                return;
            }
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string sql = "UPDATE NhanVien SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, SoDienThoai=@sdt, DiaChi=@dc WHERE MaNV=@ma";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ten", txtTenNV.Text);
                    DateTime ns;
                    if (!DateTime.TryParseExact(txtNgaySinh.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ns))
                    {
                        MessageBox.Show("Ngày sinh không đúng định dạng dd/MM/yyyy");
                        return;
                    }
                    cmd.Parameters.AddWithValue("@ns", ns);
                    cmd.Parameters.AddWithValue("@gt", rdoNam.Checked ? "Nam" : "Nữ");
                    cmd.Parameters.AddWithValue("@sdt", txtSDT.Text);
                    cmd.Parameters.AddWithValue("@dc", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@ma", txtMaNV.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Sửa nhân viên thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message);
            }
        }
    }
}
