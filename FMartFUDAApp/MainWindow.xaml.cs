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

        }

        private void FinancialManagement(object sender, RoutedEventArgs e)
        {
            FinancialManagement next = new FinancialManagement(employeeCurrent);
            next.Show();
            Close();
        }
    }
}