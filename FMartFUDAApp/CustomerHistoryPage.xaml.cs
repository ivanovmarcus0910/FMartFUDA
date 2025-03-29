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
    public partial class CustomerHistoryPage : Page
    {
        private Employee _currentUser;
        private CustomerHistoryRepository _historyRepo;
        private List<CustomerHistory> _allHistories = new(); // To store full list

        public CustomerHistoryPage()
        {
            InitializeComponent();
            MessageBox.Show("Trang này yêu cầu Manager!",
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            this.IsEnabled = false;
        }

        public CustomerHistoryPage(Employee currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            if (_currentUser.RoleId != 3)
            {
                MessageBox.Show("Bạn không có quyền xem lịch sử khách hàng!",
                                "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.IsEnabled = false;
            }
            else
            {
                _historyRepo = new CustomerHistoryRepository();
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
                var list = await _historyRepo.GetAllAsync();
                _allHistories = list.ToList(); // lưu trữ để lọc
                dgCustomerHistory.ItemsSource = _allHistories;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử khách hàng: " + ex.Message);
            }
        }

        // =========================
        // ✅ XỬ LÝ BỘ LỌC
        // =========================
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var filtered = _allHistories.AsEnumerable();

            // Lọc theo mô tả
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                filtered = filtered.Where(h =>
                    h.ChangeDecription != null &&
                    h.ChangeDecription.Contains(txtSearch.Text, StringComparison.OrdinalIgnoreCase));
            }

            // Lọc từ ngày
            if (dpFrom.SelectedDate.HasValue)
            {
                var fromDate = DateOnly.FromDateTime(dpFrom.SelectedDate.Value);
                filtered = filtered.Where(h => h.ActionDate >= fromDate);
            }

            // Lọc đến ngày
            if (dpTo.SelectedDate.HasValue)
            {
                var toDate = DateOnly.FromDateTime(dpTo.SelectedDate.Value);
                filtered = filtered.Where(h => h.ActionDate <= toDate);
            }

            dgCustomerHistory.ItemsSource = filtered.ToList();
        }

        // =========================
        // ✅ LÀM MỚI DỮ LIỆU
        // =========================
        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            dpFrom.SelectedDate = null;
            dpTo.SelectedDate = null;
            await LoadHistoryAsync();
        }
    }
}
