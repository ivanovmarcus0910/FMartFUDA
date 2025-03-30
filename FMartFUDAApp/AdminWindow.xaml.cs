using Models.Models;
using Repositories;
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

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Employee employeeCurrent;

        public AdminWindow(Employee employee)
        {
            InitializeComponent();
            employeeCurrent = employee;
            MainFrame.Navigate(new EmployeeManagementPage());
        }
     
  
  
        private void OpenPage1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeeManagementPage());

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


    }
}
