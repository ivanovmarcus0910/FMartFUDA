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
using Microsoft.Data.SqlClient;
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
        private ProductRepository productRepository;
        private OrderHistoryRepository orderHistoryRepository;
        public OrderDetailManagement(Order o, Employee e)
        {
            InitializeComponent();
            orderDetailRepository = new OrderDetailRepository();
            orderRepository = new OrderRepository();
            order = o;
            employee = e;
            customerRepository = new CustomerRepository();
            productRepository = new ProductRepository();
            orderHistoryRepository = new OrderHistoryRepository();
            LoadData();
            LoadProducts();
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

        private async void LoadProducts()
        {
            var products = await productRepository.GetAllAsync();
            ProductGrid.ItemsSource = products;
        }

        private async void txtSearchProduct_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtSearchProduct.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            string keyword = txtSearchProduct.Text.ToLower();
            var allProducts = await productRepository.GetAllAsync();
            var filteredProducts = allProducts.Where(p => p.ProductName.ToLower().Contains(keyword)).ToList();
            ProductGrid.ItemsSource = filteredProducts;
        }

        private async void OrderDetailGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDetailGrid.SelectedItem is OrderDetail selectedDetail)
            {
                txtSTT.Text = selectedDetail.OrderDetailId.ToString();
                txtProductID.Text = selectedDetail.ProductId.ToString();
                var product = await productRepository.GetByIdAsync(selectedDetail.ProductId);
                txtOrderQuantity.Text = selectedDetail.OrderQuantity.ToString();
                txtOrderPrice.Text = selectedDetail.OrderPrice.ToString();
            }
            else
            {
                txtSTT.Text = "";
                txtProductID.Text = "";
                txtOrderQuantity.Text = "";
                txtOrderPrice.Text = "";
            }
        }

        private async void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Product selectedProduct)
            {
                var orderDetails = (List<OrderDetail>)OrderDetailGrid.ItemsSource;

                var existingDetail = orderDetails.FirstOrDefault(od => od.ProductId == selectedProduct.ProductId);

                if (existingDetail != null)
                {
                    existingDetail.OrderQuantity += 1;
                    existingDetail.OrderPrice = existingDetail.OrderQuantity * selectedProduct.ProductPrice;
                    await orderDetailRepository.UpdateAsync(existingDetail);

                    await orderHistoryRepository.AddAsync(new OrderHistory
                    {
                        ActionType = "Update",
                        ActionDate = DateOnly.FromDateTime(DateTime.Now),
                        EmployeeId = employee.EmployeeId,
                        ChangeDecription = $"Cập nhật số lượng sản phẩm ID [{selectedProduct.ProductId}] trong đơn hàng ID [{order.OrderId}], số lượng mới [{existingDetail.OrderQuantity}], giá mới [{existingDetail.OrderPrice}]"
                    });
                }
                else
                {
                    int maxOrderDetailId = await orderDetailRepository.GetMaxOrderDetailIdAsync(order.OrderId);
                    int newOrderDetailId = maxOrderDetailId + 1;

                    var newDetail = new OrderDetail
                    {
                        OrderDetailId = newOrderDetailId,
                        OrderId = order.OrderId,
                        ProductId = selectedProduct.ProductId,
                        OrderQuantity = 1,
                        OrderPrice = selectedProduct.ProductPrice
                    };
                    await orderDetailRepository.AddAsync(newDetail);
                    order.OrderAmount += selectedProduct.ProductPrice;

                    await orderHistoryRepository.AddAsync(new OrderHistory
                    {
                        ActionType = "Create",
                        ActionDate = DateOnly.FromDateTime(DateTime.Now),
                        EmployeeId = employee.EmployeeId,
                        ChangeDecription = $"Thêm chi tiết đơn hàng ID [{newOrderDetailId}] vào đơn hàng ID [{order.OrderId}], sản phẩm ID [{selectedProduct.ProductId}], số lượng [1], giá [{selectedProduct.ProductPrice}]"
                    });
                }

                LoadData();
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDetailGrid.SelectedItem is not OrderDetail selectedDetail)
            {
                MessageBox.Show("Vui lòng chọn một mục để cập nhật!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtProductID.Text, out int productId) ||
                !int.TryParse(txtOrderQuantity.Text, out int quantity) ||
                !double.TryParse(txtOrderPrice.Text, out double price))
            {
                MessageBox.Show("Dữ liệu nhập không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int oldProductId = selectedDetail.ProductId;
            int oldQuantity = selectedDetail.OrderQuantity;
            double oldPrice = selectedDetail.OrderPrice;

            selectedDetail.ProductId = productId;
            selectedDetail.OrderQuantity = quantity;
            selectedDetail.OrderPrice = price;

            await orderDetailRepository.UpdateAsync(selectedDetail);
            order.OrderAmount = order.OrderAmount - oldPrice + price;
            await orderHistoryRepository.AddAsync(new OrderHistory
            {
                ActionType = "Update",
                ActionDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = employee.EmployeeId,
                ChangeDecription = $"Cập nhật chi tiết đơn hàng ID [{selectedDetail.OrderDetailId}] trong đơn hàng ID [{selectedDetail.OrderId}]: " +
                                   $"Sản phẩm ID [{oldProductId}] => [{productId}], " +
                                   $"Số lượng [{oldQuantity}] => [{quantity}], " +
                                   $"Giá [{oldPrice}] => [{price}]"
            });

            LoadData();
            MessageBox.Show("Cập nhật chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDetailGrid.SelectedItem is not OrderDetail selectedDetail)
            {
                MessageBox.Show("Vui lòng chọn một mục để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show("Bạn có chắc muốn xóa chi tiết đơn hàng này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await orderHistoryRepository.AddAsync(new OrderHistory
                {
                    ActionType = "Delete",
                    ActionDate = DateOnly.FromDateTime(DateTime.Now),
                    EmployeeId = employee.EmployeeId,
                    ChangeDecription = $"Xóa chi tiết đơn hàng ID [{selectedDetail.OrderDetailId}] trong đơn hàng ID [{selectedDetail.OrderId}], " +
                                       $"Sản phẩm ID [{selectedDetail.ProductId}], " +
                                       $"Số lượng [{selectedDetail.OrderQuantity}], " +
                                       $"Giá [{selectedDetail.OrderPrice}]"
                });
                order.OrderAmount = order.OrderAmount - selectedDetail.OrderPrice;
                await orderDetailRepository.DeleteAsync(selectedDetail.OrderDetailId, selectedDetail.OrderId);

                LoadData();
                MessageBox.Show("Xóa chi tiết đơn hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new OrderManagement(employee));
        }

        private async void txtOrderQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txtProductID.Text, out int productId) ||
                !int.TryParse(txtOrderQuantity.Text, out int quantity))
            {
                txtOrderPrice.Text = "0";
                return;
            }

            var product = await productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                double totalPrice = product.ProductPrice * quantity;
                txtOrderPrice.Text = totalPrice.ToString("F2");
            }
            else
            {
                txtOrderPrice.Text = "0";
            }
        }
    }
}
