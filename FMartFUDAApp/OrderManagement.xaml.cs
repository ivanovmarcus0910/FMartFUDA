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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Models.Models;
using Repositories;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for OrderManagement.xaml
    /// </summary>
    public partial class OrderManagement : Page
    {
        private OrderRepository orderRepository;
        private Employee employee;
        private CustomerRepository customerRepository;
        private OrderDetailRepository orderDetailRepository;
        public OrderManagement(Employee emp)
        {
            InitializeComponent();
            orderRepository = new OrderRepository();
            employee = emp;
            customerRepository = new CustomerRepository();
            orderDetailRepository = new OrderDetailRepository();
            LoadOrders();
        }

        private async void LoadOrders()
        {
            var orders = await orderRepository.GetAllAsync();
            OrderGrid.ItemsSource = orders;
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtBuyer.Text, out int customerId))
            {
                MessageBox.Show("Mã khách hàng không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var customerExists = await customerRepository.GetByIdAsync(customerId);
            if (customerExists == null) 
            {
                MessageBox.Show("Mã khách hàng không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newOrder = new Order
            {
                CustomerId = customerId,
                EmployeeId = employee.EmployeeId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderAmount = 0
            };

            await orderRepository.AddAsync(newOrder);
            LoadOrders();

            MessageBoxResult result = MessageBox.Show("Thêm đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                NavigationService?.Navigate(new OrderDetailManagement(newOrder, employee));
            }
        }


        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                if (int.TryParse(txtBuyer.Text, out int customerId))
                {
                    var customerExists = await customerRepository.GetByIdAsync(customerId);
                    if (customerExists == null)
                    {
                        MessageBox.Show("Mã khách hàng không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    selectedOrder.CustomerId = customerId;
                }

                await orderRepository.UpdateAsync(selectedOrder);
                LoadOrders();
                MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                var result = MessageBox.Show(
                    "Đơn hàng này có thể chứa các chi tiết đơn hàng. Bạn có chắc chắn muốn xóa cả đơn hàng và chi tiết đơn hàng không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    var orderDetails = await orderDetailRepository.GetAllByOrderIdAsync(selectedOrder.OrderId);

                    foreach (var detail in orderDetails)
                    {
                        await orderDetailRepository.DeleteAsync(detail.OrderDetailId);
                    }

                    await orderRepository.DeleteAsync(selectedOrder.OrderId);

                    LoadOrders();
                    MessageBox.Show("Xóa đơn hàng và chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }


        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Order selectedOrder)
            {
                NavigationService.Navigate(new OrderDetailManagement(selectedOrder, employee));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderGrid.SelectedItem is Order selectedOrder)
            {
                txtOrderID.Text = selectedOrder.OrderId.ToString();
                txtBuyer.Text = selectedOrder.CustomerId.ToString();
                txtCreator.Text = employee.EmployeeName; // Không thay đổi người tạo
                txtCreatedDate.Text = selectedOrder.OrderDate.ToString("dd/MM/yyyy"); // Ngày tạo chỉ hiển thị, không chỉnh sửa
                txtTotalAmount.Text = selectedOrder.OrderAmount.ToString();
            }
        }
    }
}
