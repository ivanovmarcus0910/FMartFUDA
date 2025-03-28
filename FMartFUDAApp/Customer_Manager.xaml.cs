using System;
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
using System.Windows.Shapes;
using Models.Models;
using Repositories;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for Customer_Manager.xaml
    /// </summary>
    public partial class Customer_Manager : Page
    {
        private CustomerRepository _repository;
        private List<Customer> _customers;

        public Customer_Manager()
        {
            InitializeComponent();

            // Khởi tạo repository
            _repository = new CustomerRepository();

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
                _customers = list.ToList();

                // Gán vào DataGrid
                dgCustomers.ItemsSource = _customers;
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
                    // Cập nhật thông tin từ form
                    selected.CustomerName = txtCustomerName.Text;
                    selected.CustomerPhone = txtCustomerPhone.Text;
                    selected.CustomerAddress = txtCustomerAddress.Text;
                    selected.CustomerEmail = txtCustomerEmail.Text;

                    await _repository.UpdateAsync(selected);

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
                    var result = await _repository.DeleteAsync(selected.CustomerId);
                    if (result)
                    {
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
        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
