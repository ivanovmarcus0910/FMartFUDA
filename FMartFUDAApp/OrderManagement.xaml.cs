using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        private OrderHistoryRepository orderHistoryRepository;
        public OrderManagement(Employee emp)
        {
            InitializeComponent();
            orderRepository = new OrderRepository();
            employee = emp;
            customerRepository = new CustomerRepository();
            orderDetailRepository = new OrderDetailRepository();
            orderHistoryRepository = new OrderHistoryRepository();
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

            var newOrderHistory = new OrderHistory
            {
                ActionType = "Create",
                ActionDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = employee.EmployeeId,
                ChangeDecription = $"Thêm đơn hàng: {newOrder.OrderId} của khách hàng có id là [{newOrder.CustomerId}]"
            };
            await orderHistoryRepository.AddAsync(newOrderHistory);
            
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
                var oldOrderId = selectedOrder.OrderId;
                if (int.TryParse(txtBuyer.Text, out int customerId))
                {
                    var newCustomer = await customerRepository.GetByIdAsync(customerId);
                    if (newCustomer == null)
                    {
                        MessageBox.Show("Mã khách hàng không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    selectedOrder.CustomerId = customerId;


                    await orderHistoryRepository.AddAsync(new OrderHistory
                    {
                        ActionType = "Update",
                        ActionDate = DateOnly.FromDateTime(DateTime.Now),
                        EmployeeId = employee.EmployeeId,
                        ChangeDecription = $"Cập nhật đơn hàng: từ khách hàng có id là [{oldOrderId}] => [{customerId}]"
                    });

                    await orderRepository.UpdateAsync(selectedOrder);
                    LoadOrders();
                    MessageBox.Show("Cập nhật đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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

                    await orderHistoryRepository.AddAsync(new OrderHistory
                    {
                        ActionType = "Delete",
                        ActionDate = DateOnly.FromDateTime(DateTime.Now),
                        EmployeeId = employee.EmployeeId,
                        ChangeDecription = $"Xóa đơn hàng ID [{selectedOrder.OrderId}] của khách hàng ID [{selectedOrder.CustomerId}]"
                    });

                    // Xóa tất cả chi tiết đơn hàng trước
                    foreach (var detail in orderDetails)
                    {
                        await orderDetailRepository.DeleteAsync(detail.OrderDetailId, detail.OrderId);
                    }

                    // Xóa đơn hàng chính
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
                txtCreator.Text = employee.EmployeeName;
                txtCreatedDate.Text = selectedOrder.OrderDate.ToString("dd/MM/yyyy"); 
                txtTotalAmount.Text = selectedOrder.OrderAmount.ToString();
            }
        }
    }
}
