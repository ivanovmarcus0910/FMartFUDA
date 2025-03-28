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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Models;
using Repositories;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for OrderDetailManagement.xaml
    /// </summary>
    public partial class OrderDetailManagement : Page
    {
        private OrderDetailRepository orderDetailRepository;
        private OrderRepository orderRepository;
        private Order order;
        private CustomerRepository customerRepository;
        private Employee employee;
        public OrderDetailManagement(Order o, Employee e)
        {
            InitializeComponent();
            orderDetailRepository = new OrderDetailRepository();
            orderRepository = new OrderRepository();
            order = o;
            employee = e;
            customerRepository = new CustomerRepository();
            LoadData();
        }

        private async void LoadData()
        {
            txtOrderIDHeader.Text = order.OrderId.ToString();
            var customer = await customerRepository.GetByIdAsync(order.CustomerId);
            txtBuyer.Text = customer.CustomerName;
            txtCreator.Text = employee.EmployeeName;
            txtNgayTao.Text = order.OrderDate.ToString("dd/MM/yyyy");
            txtTongCong.Text = order.OrderAmount.ToString();
            var orderDetails = await orderDetailRepository.GetAllByOrderIdAsync(order.OrderId);
            OrderDetailGrid.ItemsSource = orderDetails;
        }

        private void OrderDetailGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDetailGrid.SelectedItem is OrderDetail selectedDetail)
            {
                txtSTT.Text = selectedDetail.OrderDetailId.ToString();
                txtOrderID.Text = selectedDetail.OrderId.ToString();
                txtProductID.Text = selectedDetail.ProductId.ToString();
                txtOrderQuantity.Text = selectedDetail.OrderQuantity.ToString();
                txtOrderPrice.Text = selectedDetail.OrderPrice.ToString();
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
