﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models.Models;
using Repositories;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for EmployeeManagementPage.xaml
    /// </summary>
    public partial class EmployeeManagementPage : Page
    {
        EmployeeRepository EmployeeRepository;
        RoleRepository RoleRepository;
        public EmployeeManagementPage()
        {
            InitializeComponent();
            EmployeeRepository = new EmployeeRepository();
            RoleRepository = new RoleRepository();
            showData();
            LoadComboRole();
        }
        public async void showData()
        {
            var list = await EmployeeRepository.GetAllAsync();
            dgEmployee.ItemsSource = null;
            dgEmployee.ItemsSource = list;
        }

        public async void LoadComboRole()
        {
            var list = await RoleRepository.GetAllAsync();
            cboRole.ItemsSource = null;
            cboRole.ItemsSource = list;
            cboRole.DisplayMemberPath = "RoleName";
            cboRole.SelectedValuePath = "RoleId";
        }

        private async void dgEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgEmployee.SelectedItem is Employee x)
                {
                    txtEmployeeID.Text = x.EmployeeId.ToString();
                    txtEmployeeEmail.Text = x.EmployeeEmail;
                    txtEmployeeName.Text = x.EmployeeName;
                    txtEmployeePhone.Text = x.EmployeePhone;
                    txtPass.Text = x.Pass;
                    cboRole.SelectedValue = x.RoleId;
                    dpEmployeeBirthDay.SelectedDate = DateTime.Parse(x.EmployeeBirthDay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmployeePhone.Text) ||
                    string.IsNullOrWhiteSpace(txtEmployeeEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtPass.Text) ||
                    dpEmployeeBirthDay.SelectedDate == null ||
                    cboRole.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Tạo đối tượng nhân viên mới
                Employee newEmployee = new Employee
                {
                    EmployeeName = txtEmployeeName.Text.Trim(),
                    EmployeePhone = txtEmployeePhone.Text.Trim(),
                    EmployeeEmail = txtEmployeeEmail.Text.Trim(),
                    Pass = txtPass.Text,
                    EmployeeBirthDay = dpEmployeeBirthDay.SelectedDate.Value.ToString("yyyy-MM-dd"), // Định dạng ngày tháng
                    RoleId = (int)cboRole.SelectedValue // Lấy ID của role đã chọn
                };

                // Gọi phương thức thêm vào CSDL
                await EmployeeRepository.AddAsync(newEmployee);

                // Load lại danh sách
                showData();

                // Xóa form sau khi thêm
                ClearForm();

                MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void ClearForm()
        {
            txtEmployeeID.Clear();
            txtEmployeeName.Clear();
            txtEmployeePhone.Clear();
            txtEmployeeEmail.Clear();
            txtPass.Clear();
            dpEmployeeBirthDay.SelectedDate = null;
            cboRole.SelectedIndex = -1;
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) ||
                    string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
                    string.IsNullOrWhiteSpace(txtEmployeePhone.Text) ||
                    string.IsNullOrWhiteSpace(txtEmployeeEmail.Text) ||
                    string.IsNullOrWhiteSpace(txtPass.Text) ||
                    dpEmployeeBirthDay.SelectedDate == null ||
                    cboRole.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên và nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Lấy ID của nhân viên cần cập nhật
                int employeeId = int.Parse(txtEmployeeID.Text);

                // Lấy nhân viên từ database
                Employee employeeToUpdate = await EmployeeRepository.GetByIdAsync(employeeId);
                if (employeeToUpdate == null)
                {
                    MessageBox.Show("Không tìm thấy nhân viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Cập nhật thông tin nhân viên
                employeeToUpdate.EmployeeName = txtEmployeeName.Text.Trim();
                employeeToUpdate.EmployeePhone = txtEmployeePhone.Text.Trim();
                employeeToUpdate.EmployeeEmail = txtEmployeeEmail.Text.Trim();
                employeeToUpdate.Pass = txtPass.Text;
                employeeToUpdate.EmployeeBirthDay = dpEmployeeBirthDay.SelectedDate.Value.ToString("yyyy-MM-dd"); // Định dạng ngày tháng
                employeeToUpdate.RoleId = (int)cboRole.SelectedValue; // Lấy ID của Role

                // Gọi phương thức cập nhật trong Repository
                await EmployeeRepository.UpdateAsync(employeeToUpdate);

                // Load lại danh sách
                showData();

                // Xóa form sau khi cập nhật
                ClearForm();

                MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
