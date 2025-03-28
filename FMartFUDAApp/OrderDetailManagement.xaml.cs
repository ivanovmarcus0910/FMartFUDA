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
        public OrderDetailManagement(Order o)
        {
            InitializeComponent();
            orderDetailRepository = new OrderDetailRepository();
            orderRepository = new OrderRepository();
            order = o;
            LoadData();
        }

        private async void LoadData()
        {
            txtOrderIDHeader.Text = order.OrderId.ToString();
            txtBuyer.Text = order.Customer.CustomerName;
            txtCreator.Text = order.Employee.EmployeeName;
            txtNgayTao.Text = order.OrderDate.ToString("dd/MM/yyyy");
            txtTongCong.Text = order.OrderAmount.ToString();
            var orderDetails = await orderDetailRepository.GetAllByOrderIdAsync(order.OrderId);
            OrderDetailGrid.ItemsSource = orderDetails;
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
