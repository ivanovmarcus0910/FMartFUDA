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
namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for FinancialManagement.xaml
    /// </summary>
    public partial class FinancialManagement : Window
    {
        Employee employeeCurrent;
        public FinancialManagement(Employee x)
        {
            InitializeComponent();
            employeeCurrent = x;
        }

        private void Revenue(object sender, RoutedEventArgs e)
        {

        }

        private void ManagerStaff(object sender, RoutedEventArgs e)
        {
            if (employeeCurrent.Role.RoleName == "Manager")
            {
                MainFrame.Navigate(new StaffManagement(employeeCurrent));
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập trang này!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
                employeeCurrent = null;
                RedirectToLogin();
            }
        }
        private void RedirectToLogin()
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close(); // Đóng cửa sổ hiện tại
        }

        private void FinancialReport(object sender, RoutedEventArgs e)
        {

        }

        private void BackBT(object sender, RoutedEventArgs e)
        {
            MainWindow next = new MainWindow(employeeCurrent);
            next.Show();
            Close();
        }
    }

    
}
