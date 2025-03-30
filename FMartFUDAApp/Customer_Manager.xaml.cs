using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Models.Models;
using Repositories;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for Customer_Manager.xaml
    /// </summary>
    public partial class Customer_Manager : Page
    {
        CustomerRepository _repository;
        Employee _currentUser;
        CustomerHistoryRepository _historyRepo; // Để ghi log

        public Customer_Manager()
        {
            InitializeComponent();

            // Tuỳ ý: vô hiệu hoá trang, hoặc hiển thị cảnh báo
            MessageBox.Show("Trang Quản lý Khách hàng yêu cầu thông tin nhân viên đăng nhập!",
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            this.IsEnabled = false;

            // Nếu muốn vẫn dùng repository (dù không có user), bạn có thể khởi tạo:
            _repository = new CustomerRepository();
            _historyRepo = new CustomerHistoryRepository();

            // Hoặc bỏ qua, tuỳ nhu cầu
        }
        public Customer_Manager(Employee currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // Kiểm tra quyền ngay trong constructor (bỏ hàm CheckRolePermission)
            if (_currentUser.RoleId != 1 && _currentUser.RoleId != 3)
            {
                MessageBox.Show("Bạn không có quyền truy cập trang Quản lý Khách hàng!",
                                "Permission Denied",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                // Vô hiệu hoá toàn bộ trang
                this.IsEnabled = false;
                // Hoặc bạn có thể điều hướng sang trang khác hoặc đóng cửa sổ tại đây
            }

            // Khởi tạo repository
            _repository = new CustomerRepository();
            _historyRepo = new CustomerHistoryRepository();

            // Sự kiện Loaded sẽ gọi hàm load data
            this.Loaded += Customer_Manager_Loaded;
        }

        private async void Customer_Manager_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadCustomersAsync();
        }

        // 1) Tải danh sách khách hàng
        private async Task LoadCustomersAsync()
        {
            try
            {
                var list = await _repository.GetAllAsync();
                dgCustomers.ItemsSource = null;
                dgCustomers.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message);
            }
        }

        // 2) Thêm khách hàng
        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newCustomer = new Customer
                {
                    CustomerName = txtCustomerName.Text,
                    CustomerPhone = txtCustomerPhone.Text,
                    CustomerAddress = txtCustomerAddress.Text,
                    CustomerEmail = txtCustomerEmail.Text
                };

                await _repository.AddAsync(newCustomer);

                // Ghi log
                await _historyRepo.AddAsync(new CustomerHistory
                {
                    ActionType = "Insert",
                    ActionDate = DateOnly.FromDateTime(DateTime.Now),
                    EmployeeId = _currentUser.EmployeeId,
                    ChangeDecription = $"Thêm khách hàng: {newCustomer.CustomerName}"
                });

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);

                // Load lại danh sách
                await LoadCustomersAsync();

                // Xoá trắng form
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        // 3) Sửa khách hàng
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy khách hàng đang chọn trên DataGrid
                if (dgCustomers.SelectedItem is Customer selected)
                {
                    string oldName = selected.CustomerName;
                    // Cập nhật thông tin từ form
                    selected.CustomerName = txtCustomerName.Text;
                    selected.CustomerPhone = txtCustomerPhone.Text;
                    selected.CustomerAddress = txtCustomerAddress.Text;
                    selected.CustomerEmail = txtCustomerEmail.Text;

                    await _repository.UpdateAsync(selected);

                    // Ghi log
                    await _historyRepo.AddAsync(new CustomerHistory
                    {
                        ActionType = "Update",
                        ActionDate = DateOnly.FromDateTime(DateTime.Now),
                        EmployeeId = _currentUser.EmployeeId,
                        ChangeDecription = $"Cập nhật khách hàng: từ {oldName} thành {selected.CustomerName}"
                    });
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    // Load lại danh sách
                    await LoadCustomersAsync();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Hãy chọn 1 khách hàng để cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        // 4) Xoá khách hàng
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy khách hàng đang chọn
                if (dgCustomers.SelectedItem is Customer selected)
                {
                    bool result = await _repository.DeleteAsync(selected.CustomerId);
                    if (result)
                    {
                        // Ghi log
                        await _historyRepo.AddAsync(new CustomerHistory
                        {
                            ActionType = "Delete",
                            ActionDate = DateOnly.FromDateTime(DateTime.Now),
                            EmployeeId = _currentUser.EmployeeId,
                            ChangeDecription = $"Xoá khách hàng: {selected.CustomerName}"
                        });

                        MessageBox.Show("Xoá khách hàng thành công!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadCustomersAsync();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng để xoá.");
                    }
                }
                else
                {
                    MessageBox.Show("Hãy chọn 1 khách hàng để xoá.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xoá khách hàng: " + ex.Message);
            }
        }

        // Khi chọn 1 dòng trong DataGrid, đưa thông tin lên form
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selected)
            {
                txtCustomerName.Text = selected.CustomerName;
                txtCustomerPhone.Text = selected.CustomerPhone;
                txtCustomerAddress.Text = selected.CustomerAddress;
                txtCustomerEmail.Text = selected.CustomerEmail;
            }
        }

        // Hàm xoá trắng form
        private void ClearForm()
        {
            txtCustomerName.Text = string.Empty;
            txtCustomerPhone.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtCustomerEmail.Text = string.Empty;
        }
    }
}
