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
    /// Interaction logic for StaffManagement.xaml
    /// </summary>
    public partial class StaffManagement : Page
    {
        Employee employeeCurrent;
        EmployeeRepository EmployeeRepository;

        public StaffManagement(Employee employee)
        {
            InitializeComponent();
            employeeCurrent = employee;
            EmployeeRepository = new EmployeeRepository();
        }

        public async void showData()
        {
            var list = await EmployeeRepository.GetAllAsync();
            
            dgEmployee.ItemsSource = null;
            dgEmployee.ItemsSource = list;
        }





    }
}
