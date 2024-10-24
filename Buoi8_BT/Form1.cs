using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Buoi8_BT
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bindingSource = new BindingSource();
            bindingNavigator1.BindingSource = bindingSource;  // Liên kết với BindingNavigator
            dgvSinhvien.DataSource = bindingSource;
            LoadData();
            // TODO: This line of code loads data into the 'schoolDBDataSet.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.schoolDBDataSet.Students);

        } 
        // Hàm để load và gán dữ liệu vào BindingSource và DataGridView
        private void LoadData()
        {
            using (var context = new SchoolDB())
            {
                var students = context.Students.ToList();
                bindingSource.DataSource = students;
            }
        }

        // Thêm sinh viên
        private void btnThem_Click_1(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                using (var context = new SchoolDB())
                {
                    var student = new Student
                    {
                        FullName = txtFullName.Text,
                        Age = int.Parse(txtAge.Text),
                        Major = cbbMajor.Text,
                    };

                    context.Students.Add(student);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        // Xóa sinh viên
        private void bttXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0)
            {
                int id = (int)dgvSinhvien.SelectedRows[0].Cells[0].Value;

                using (var context = new SchoolDB())
                {
                    var student = context.Students.Find(id);
                    if (student != null)
                    {
                        context.Students.Remove(student);
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        // Sửa thông tin sinh viên
        private void btnSua_Click_1(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0 && ValidateInputs())
            {
                int id = (int)dgvSinhvien.SelectedRows[0].Cells[0].Value;

                using (var context = new SchoolDB())
                {
                    var student = context.Students.Find(id);
                    if (student != null)
                    {
                        student.FullName = txtFullName.Text;
                        student.Age = int.Parse(txtAge.Text);
                        student.Major = cbbMajor.Text;
                        context.SaveChanges();
                        LoadData();
                    }
                }
            }
        }

        // Hiển thị thông tin sinh viên khi người dùng chọn trong DataGridView




        // Kiểm tra các trường nhập liệu
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên.");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Tuổi phải là một số hợp lệ.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbbMajor.Text))
            {
                MessageBox.Show("Vui lòng chọn chuyên ngành.");
                return false;
            }

            return true;
        }

        private void dgvSinhvien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSinhvien.SelectedRows.Count > 0) // Kiểm tra có hàng nào được chọn hay không
            {
                var selectedRow = dgvSinhvien.SelectedRows[0]; // Truy cập hàng đầu tiên được chọn

                // Sử dụng DataBoundItem để lấy đối tượng hiện tại từ DataBinding
                var sinhvien = selectedRow.DataBoundItem as Student; // Sử dụng model Student thay vì SchoolDB

                if (sinhvien != null)
                {
                    // Gán giá trị từ đối tượng sinhvien
                    txtFullName.Text = sinhvien.FullName ?? "";  // Sử dụng null-coalescing để tránh lỗi null
                    txtAge.Text = sinhvien.Age.ToString();       // Chuyển đổi Age về dạng chuỗi
                    cbbMajor.Text = sinhvien.Major ?? "";        // Tránh null khi hiển thị Major
                }
            }
            else
            {
                // Xóa các TextBox nếu không có hàng nào được chọn
                txtFullName.Text = "";
                txtAge.Text = "";
                cbbMajor.Text = "";
            }
        }

 

    }
}
