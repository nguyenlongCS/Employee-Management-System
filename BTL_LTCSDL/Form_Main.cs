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

            MessageBox.Show("Đã check-in lúc: " + checkInTime, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            MessageBox.Show("Đã check-out lúc: " + checkInTime, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        
    }
}
