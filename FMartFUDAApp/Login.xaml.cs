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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        EmployeeRepository employeeRepo;
        public Login()
        {

            InitializeComponent();
            employeeRepo = new EmployeeRepository();

        }

        private async void login(object sender, RoutedEventArgs e)
        {
            string Email = email.Text;
            string Pass = password.Password;
            Employee employee = await getUser(Email, Pass);
            if (employee!= null)
            {
                if (employee.Role.RoleName == "Administrator")
                {
                    AdminWindow next = new AdminWindow(employee);
                    next.Show();
                    Close();
                }
                else
                {
                    MainWindow next = new MainWindow(employee);
                    next.Show();

                    Close();
                }    
            }
        }
        private async Task<Employee> getUser(string Email, string Pass)
        {
            try
            {
                Employee _user = (from c in await employeeRepo.GetAllAsync()
                                  where c.EmployeeEmail == Email   
                       select c).FirstOrDefault();
                if (_user != null)
                {
                    if (_user.Pass==Pass)
                    {
                        return _user;
                    }
                    else
                    {
                        MessageBox.Show("Sai mật khẩu!");
                        return null;
                    }    
                }
                else
                {
                    MessageBox.Show("Sai email đăng nhập!");
                    return null;
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }
    }
}
