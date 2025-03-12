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
        string connectionString = "Server=LONG_ACER\\SQLEXPRESS;Database=NhanVien;Integrated Security=True;";

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
        // Định nghĩa class lưu thông tin nhân viên và lịch sử chấm công
        class EmployeeRecord
        {
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public string Date { get; set; }
            public string CheckIn { get; set; }
            public string CheckOut { get; set; }
        }

        // Danh sách lịch sử chấm công
        List<EmployeeRecord> history = new List<EmployeeRecord>
        {
            new EmployeeRecord { EmployeeID = "NV001", EmployeeName = "Nguyễn Văn A", Date = "02/05/2025", CheckIn = "08:00", CheckOut = "17:00" },
            new EmployeeRecord { EmployeeID = "NV001", EmployeeName = "Nguyễn Văn A", Date = "02/05/2025", CheckIn = "09:00", CheckOut = "18:00" },
            new EmployeeRecord { EmployeeID = "NV001", EmployeeName = "Nguyễn Văn A", Date = "03/05/2025", CheckIn = "08:15", CheckOut = "17:05" }
        };
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

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_Search_ChamCong_Click(object sender, EventArgs e)
        {

        }

        private void textBox_Search_ChamCong_TextChanged(object sender, EventArgs e)
        {

        }


        private void button_LichSuChamCong_Click(object sender, EventArgs e)
        {
            // Xóa dữ liệu cũ trong ListView
            listView1.Items.Clear();

            // Thêm dữ liệu lịch sử vào ListView
            foreach (var record in history)
            {
                ListViewItem item = new ListViewItem(record.EmployeeID);
                item.SubItems.Add(record.EmployeeName);
                item.SubItems.Add(record.Date);
                item.SubItems.Add(record.CheckIn);
                item.SubItems.Add(record.CheckOut);
                listView1.Items.Add(item);
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            string gender = radioBut_Male.Checked ? "Male" : "Female";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra trùng Mã Nhân Viên
                string checkQuery = "SELECT COUNT(*) FROM Employee WHERE EmployeeID = @EmployeeID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@EmployeeID", textBox_Employeed.Text);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Mã Nhân Viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Kiểm tra trùng CCCD
                checkQuery = "SELECT COUNT(*) FROM Employee WHERE CCCD = @CCCD";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CCCD", textBox_CCCD.Text);
                    int countCCCD = (int)checkCmd.ExecuteScalar();
                    if (countCCCD > 0)
                    {
                        MessageBox.Show("CCCD đã tồn tại! Vui lòng nhập CCCD khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Chèn vào SQL Server
                string insertQuery = "INSERT INTO Employee (EmployeeID, Name, Surname, DateOfBirth, Gender, CCCD, SDT, DepartmentID, Division) " +
                                     "VALUES (@EmployeeID, @Name, @Surname, @DateOfBirth, @Gender, @CCCD, @SDT, @DepartmentID, @Division)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmployeeID", textBox_Employeed.Text);
                    cmd.Parameters.AddWithValue("@Name", textBox_Name.Text);
                    cmd.Parameters.AddWithValue("@Surname", textBox_Surname.Text);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateTime_DateOfBirth.Value);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@CCCD", textBox_CCCD.Text);
                    cmd.Parameters.AddWithValue("@SDT", textBox_SDT.Text);
                    cmd.Parameters.AddWithValue("@DepartmentID", textBox_DepartmentID.Text);
                    cmd.Parameters.AddWithValue("@Division", textBox_Division.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Thêm dữ liệu vào DataGridView từ row[1]
            dataGridView1.Rows.Add(
                textBox_Employeed.Text,
                textBox_Name.Text + " " + textBox_Surname.Text,
                dateTime_DateOfBirth.Value.ToString("dd/MM/yyyy"),
                (DateTime.Now.Year - dateTime_DateOfBirth.Value.Year).ToString(), // Tính tuổi
                gender,
                textBox_CCCD.Text,
                textBox_SDT.Text,
                textBox_DepartmentID.Text,
                textBox_Division.Text
            );
            // Thêm dữ liệu vào DataGridView từ row[1]
            dataGridView1.Rows.Add(
                textBox_Employeed.Text,
                textBox_Name.Text + " " + textBox_Surname.Text,
                dateTime_DateOfBirth.Value.ToString("dd/MM/yyyy"),
                (DateTime.Now.Year - dateTime_DateOfBirth.Value.Year).ToString(), // Tính tuổi
                gender,
                textBox_CCCD.Text,
                textBox_SDT.Text,
                textBox_DepartmentID.Text,
                textBox_Division.Text
            );

            // Xóa nội dung các TextBox sau khi thêm
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
            textBox_DepartmentID.Clear();
            textBox_Division.Clear();
            radioBut_Male.Checked = false;
            radioBut_Female.Checked = false;
        }



        private void LoadData()
        {
            dataGridView1.Rows.Clear(); // Xóa dữ liệu cũ, giữ lại tiêu đề row[0]
            dataGridView_DSNV.ClearSelection();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT EmployeeID, Name + ' ' + Surname AS [Tên Nhân Viên], 
                   DateOfBirth, 
                   DATEDIFF(YEAR, DateOfBirth, GETDATE()) AS Tuổi,
                   Gender, CCCD, SDT, DepartmentID, Division 
            FROM Employee";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataGridView1.Rows.Add(
                            reader["EmployeeID"],
                            reader["Tên Nhân Viên"],
                            Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd/MM/yyyy"),
                            reader["Tuổi"],
                            reader["Gender"],
                            reader["CCCD"],
                            reader["SDT"],
                            reader["DepartmentID"],
                            reader["Division"]
                        );
                        dataGridView_DSNV.Rows.Add(
                            reader["EmployeeID"],
                            reader["Tên Nhân Viên"],
                            Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd/MM/yyyy"),
                            reader["Tuổi"],
                            reader["Gender"],
                            reader["CCCD"],
                            reader["SDT"],
                            reader["DepartmentID"],
                            reader["Division"]
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
            dataGridView_DSNV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_DSNV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_DSNV.AllowUserToAddRows = false; 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_DSNV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_Show_Click(object sender, EventArgs e)
        {
            TabControl.SelectedTab = tabPage_DanhSachNV;
        }
    }
}
