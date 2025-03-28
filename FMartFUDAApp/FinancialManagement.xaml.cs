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

        }

        private void FinacialReport(object sender, RoutedEventArgs e)
        {

        }
    }
}
