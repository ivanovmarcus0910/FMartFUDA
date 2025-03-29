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
    /// Interaction logic for OrderManagementHistory.xaml
    /// </summary>
    public partial class OrderManagementHistory : Page
    {
        private readonly OrderHistoryRepository orderHistoryRepository;

        public OrderManagementHistory()
        {
            InitializeComponent();
            orderHistoryRepository = new OrderHistoryRepository();
            LoadHistory();
        }

        private async void LoadHistory()
        {
            var history = await orderHistoryRepository.GetAllAsync();
            OrderHistoryGrid.ItemsSource = history;
        }

        private async void FilterHistory(object sender, RoutedEventArgs e)
        {
            string actionType = (cbActionType.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateOnly? startDate = dpStartDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpStartDate.SelectedDate.Value) : null;
            DateOnly? endDate = dpEndDate.SelectedDate.HasValue ? DateOnly.FromDateTime(dpEndDate.SelectedDate.Value) : null;

            var filteredHistory = await orderHistoryRepository.GetFilteredHistoryAsync(actionType, startDate, endDate);
            OrderHistoryGrid.ItemsSource = filteredHistory;
        }

        private void RefreshHistory(object sender, RoutedEventArgs e)
        {
            cbActionType.SelectedIndex = 0;
            dpStartDate.SelectedDate = null;
            dpEndDate.SelectedDate = null;
            LoadHistory();
        }

        private async void DeleteSelectedHistory(object sender, RoutedEventArgs e)
        {
            if (OrderHistoryGrid.SelectedItem is OrderHistory selectedHistory)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa bản ghi lịch sử này?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool isDeleted = await orderHistoryRepository.DeleteAsync(selectedHistory.HistoryId);
                    if (isDeleted)
                    {
                        MessageBox.Show("Xóa bản ghi lịch sử thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        OrderHistoryGrid.ItemsSource = await orderHistoryRepository.GetAllAsync();
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa bản ghi!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bản ghi để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
