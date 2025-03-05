using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_LTCSDL
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
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
    }
}
