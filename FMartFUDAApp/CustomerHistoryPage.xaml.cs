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
    /// Interaction logic for CustomerHistoryPage.xaml
    /// </summary>
    public partial class CustomerHistoryPage : Page
    {
         Employee _currentUser;
         CustomerHistoryRepository _historyRepo;

        // Constructor không tham số (cần cho WPF)
        public CustomerHistoryPage()
        {
            InitializeComponent();
            // Thông báo và vô hiệu hoá nếu gọi constructor này
            MessageBox.Show("Trang này yêu cầu Manager!",
                            "Warning",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
            this.IsEnabled = false;
        }

        // Constructor có tham số: nhận Employee đang đăng nhập
        public CustomerHistoryPage(Employee currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // Chỉ cho phép Manager (RoleId = 3) xem lịch sử
            if (_currentUser.RoleId != 3)
            {
                MessageBox.Show("Bạn không có quyền xem lịch sử khách hàng!",
                                "Permission Denied",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                this.IsEnabled = false;
            }
            else
            {
                // Khởi tạo repository để load dữ liệu từ bảng CustomerHistory
                _historyRepo = new CustomerHistoryRepository();

                // Khi trang đã load, ta gọi hàm LoadHistoryAsync
                this.Loaded += CustomerHistoryPage_Loaded;
            }
        }

        private async void CustomerHistoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            try
            {
                // Gọi repository để lấy tất cả dòng lịch sử
                var list = await _historyRepo.GetAllAsync();
                dgCustomerHistory.ItemsSource = null;
                dgCustomerHistory.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử khách hàng: " + ex.Message);
            }
        }
    }
}
