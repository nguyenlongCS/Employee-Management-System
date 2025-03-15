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

namespace BTL_LTCSDL
{
    public partial class Form_Main : Form
    {
        string connectionString = "Server=LONG_ACER\\SQLEXPRESS;Database=QL_NhanVien;Integrated Security=True;";

        public Form_Main()
        {
            InitializeComponent();
            SetupDataGridView();
            LoadData(); // Nạp dữ liệu từ SQL
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_RealTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000; // Cập nhật mỗi giây
            timer1.Tick += timer1_Tick;
            timer1.Start();
            // Mặc định chọn thời gian hiện tại
            radioButton_RealTime.Checked = true;
            radioButton_RealTime2.Checked = true;
            dateTimePicker1.Enabled = false; // Ẩn chọn tay
            dateTimePicker2.Enabled = false; // Ẩn chọn tay
            // Thiết lập tiêu đề ListView
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            // Thêm cột vào ListView
            listView1.Columns.Add("Mã NV", 80);
            listView1.Columns.Add("Tên Nhân Viên", 150);
            listView1.Columns.Add("Ngày", 100);
            listView1.Columns.Add("Check-in", 100);
            listView1.Columns.Add("Check-out", 100);
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = !radioButton_RealTime.Checked; // Tắt nếu chọn "Thời gian hiện tại"
        }
        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = !radioButton_RealTime2.Checked; // Tắt nếu chọn "Thời gian hiện tại"
        }

        
        private void button_CheckIn_Click(object sender, EventArgs e)
        {
            string checkInTime;
            if (radioButton_RealTime.Checked)
            {
                checkInTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"); // Lấy giờ hiện tại
            }
            else
            {
                checkInTime = dateTimePicker1.Value.ToString("HH:mm:ss dd/MM/yyyy"); // Lấy giờ từ DateTimePicker
            }

            MessageBox.Show("Mã nhân viên: \n" + "Tên Nhân Viên: \n" + "Đã check-in lúc: " + checkInTime, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_CheckOut_Click(object sender, EventArgs e)
        {
            string checkInTime;
            if (radioButton_RealTime2.Checked)
            {
                checkInTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"); // Lấy giờ hiện tại
            }
            else
            {
                checkInTime = dateTimePicker2.Value.ToString("HH:mm:ss dd/MM/yyyy"); // Lấy giờ từ DateTimePicker
            }

            MessageBox.Show("Mã nhân viên: \n" + "Tên Nhân Viên: \n" + "Đã check-out lúc: " + checkInTime, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            bool gioiTinh = radioBut_Male.Checked; // Nam: true, Nữ: false

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra trùng Mã Nhân Viên
                string checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE MaNV = @MaNV";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@MaNV", textBox_Employeed.Text);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Mã Nhân Viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Kiểm tra trùng CCCD
                checkQuery = "SELECT COUNT(*) FROM NhanVien WHERE CCCD = @CCCD";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CCCD", textBox_CCCD.Text);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("CCCD đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Chèn vào SQL Server
                string insertQuery = "INSERT INTO NhanVien (MaNV, HoNV, TenNV, DiaChi, SoDT, Email, NgaySinh, GioiTinh, CCCD, ChucVu, MaPB) " +
                                     "VALUES (@MaNV, @HoNV, @TenNV, @DiaChi, @SoDT, @Email, @NgaySinh, @GioiTinh, @CCCD, @ChucVu, @MaPB)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", textBox_Employeed.Text);
                    cmd.Parameters.AddWithValue("@HoNV", textBox_Surname.Text);
                    cmd.Parameters.AddWithValue("@TenNV", textBox_Name.Text);
                    cmd.Parameters.AddWithValue("@DiaChi", textBox_DiaChi.Text);
                    cmd.Parameters.AddWithValue("@SoDT", textBox_SDT.Text);
                    cmd.Parameters.AddWithValue("@Email", textBox_email.Text);
                    cmd.Parameters.AddWithValue("@NgaySinh", dateTime_DateOfBirth.Value);
                    cmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                    cmd.Parameters.AddWithValue("@CCCD", textBox_CCCD.Text);
                    cmd.Parameters.AddWithValue("@ChucVu", textBox_ChucVu.Text);
                    cmd.Parameters.AddWithValue("@MaPB", textBox_MaPhongBan.Text);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            LoadData(); // Tải lại dữ liệu sau khi thêm
            ClearInputFields();
        }

        // Hàm xóa nội dung nhập sau khi thêm nhân viên
        private void ClearInputFields()
        {
            textBox_Employeed.Clear();
            textBox_Name.Clear();
            textBox_Surname.Clear();
            textBox_CCCD.Clear();
            textBox_SDT.Clear();
            textBox_MaPhongBan.Clear();
            textBox_ChucVu.Clear();
            radioBut_Male.Checked = false;
            radioBut_Female.Checked = false;
        }

        private void LoadData()
        {
            dataGridView1.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MaNV, HoNV + ' ' + TenNV AS [Tên Nhân Viên], DiaChi, SoDT, Email, " +
                               "NgaySinh, CASE WHEN GioiTinh = 1 THEN 'Nam' WHEN GioiTinh = 0 THEN 'Nữ' ELSE NULL END AS GioiTinh, " +
                               "CCCD, ChucVu, MaPB FROM NhanVien";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ngaySinh = reader["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(reader["NgaySinh"]).ToString("dd/MM/yyyy") : "N/A";
                        string gioiTinh = reader["GioiTinh"] != DBNull.Value ? reader["GioiTinh"].ToString() : "Không xác định";

                        dataGridView1.Rows.Add(
                            reader["MaNV"], reader["Tên Nhân Viên"], reader["DiaChi"],
                            reader["SoDT"], reader["Email"], ngaySinh,
                            gioiTinh, reader["CCCD"], reader["ChucVu"], reader["MaPB"]
                        );
                        dataGridView_DSNV.Rows.Add(
                            reader["MaNV"], reader["Tên Nhân Viên"], reader["DiaChi"],
                            reader["SoDT"], reader["Email"], ngaySinh,
                            gioiTinh, reader["CCCD"], reader["ChucVu"], reader["MaPB"]
                        );
                    }
                }
            }
        }





        private void SetupDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false; // Không cho phép thêm dòng trống
            
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("MaNV", "Mã NV");
            dataGridView1.Columns.Add("TenNV", "Tên Nhân Viên");
            dataGridView1.Columns.Add("DiaChi", "Địa Chỉ");
            dataGridView1.Columns.Add("SoDT", "Số ĐT");
            dataGridView1.Columns.Add("Email", "Email");
            dataGridView1.Columns.Add("NgaySinh", "Ngày Sinh");
            dataGridView1.Columns.Add("GioiTinh", "Giới Tính");
            dataGridView1.Columns.Add("CCCD", "CCCD");
            dataGridView1.Columns.Add("ChucVu", "Chức Vụ");
            dataGridView1.Columns.Add("MaPB", "Mã PB");

            dataGridView_DSNV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_DSNV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_DSNV.AllowUserToAddRows = false;

            dataGridView_DSNV.Columns.Clear();
            dataGridView_DSNV.Columns.Add("MaNV", "Mã NV");
            dataGridView_DSNV.Columns.Add("TenNV", "Tên Nhân Viên");
            dataGridView_DSNV.Columns.Add("DiaChi", "Địa Chỉ");
            dataGridView_DSNV.Columns.Add("SoDT", "Số ĐT");
            dataGridView_DSNV.Columns.Add("Email", "Email");
            dataGridView_DSNV.Columns.Add("NgaySinh", "Ngày Sinh");
            dataGridView_DSNV.Columns.Add("GioiTinh", "Giới Tính");
            dataGridView_DSNV.Columns.Add("CCCD", "CCCD");
            dataGridView_DSNV.Columns.Add("ChucVu", "Chức Vụ");
            dataGridView_DSNV.Columns.Add("MaPB", "Mã PB");
        }

        private void button_Show_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = tabPage_DanhSachNV;
        }

        private void TabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabPage_CaLam) 
            {
                e.Cancel = true; // Hủy sự kiện chọn tab
            }
            if (e.TabPage == tabPage_Report)
            {
                e.Cancel = true; // Hủy sự kiện chọn tab
            }
        }
    }
}
