using Models.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Employee employeeCurrent;
        public MainWindow(Employee employee)
        {
            InitializeComponent();
            employeeCurrent = employee;
        }

      

        
        private void CustomerManagement(object sender, RoutedEventArgs e)
        {

        }

        private void OrderManagement(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrderManagement(employeeCurrent));
        }
        private void OrderDetailManagement(object sender, RoutedEventArgs e)
        {

        }
        private void CategoryManagement(object sender, RoutedEventArgs e)
        {

        }
        private void ProductManagement(object sender, RoutedEventArgs e)
        {

        }
        private void HistoryManagement(object sender, RoutedEventArgs e)
        {

        }
        private void StaffManagement(object sender, RoutedEventArgs e)
        {
            // Kiểm tra quyền của nhân viên hiện tại
            if (employeeCurrent.Role.RoleName == "Manager")
            {
                MainFrame.Navigate(new StaffManagement(employeeCurrent));
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập trang này!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FinancialManagement(object sender, RoutedEventArgs e)
        {
            FinancialManagement next = new FinancialManagement(employeeCurrent);
            next.Show();
            Close();
        }


    }
}