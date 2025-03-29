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
using System.Windows.Threading;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Employee employeeCurrent;
        private DispatcherTimer timer;
        public MainWindow(Employee employee)
             

        {
            InitializeComponent();
            employeeCurrent = employee;
            welcomeTextBlock.Text = $"Welcome, {employee.EmployeeName}";
            StartTimer();
        }

      

        
        private void CustomerManagement(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Customer_Manager(employeeCurrent));
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
            MainFrame.Navigate(new CustomerHistoryPage(employeeCurrent));
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
        private void Logoutbtn(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Logout Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                RedirectToLogin();
            }
        }
        private void RedirectToLogin()
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close(); // Đóng cửa sổ hiện tại
        }
        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Cập nhật mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }


    }
}