using LiveCharts.Wpf;
using LiveCharts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FMartFUDAApp
{
    /// <summary>
    /// Interaction logic for Revenue.xaml
    /// </summary>
    public partial class Revenue : Page
    {
        OrderRepository orderRepo;
        public SeriesCollection RevenueValues { get; set; }
        public List<string> Dates { get; set; }
        public double TotalRevenue { get; set; }
        public Revenue()
        {

            InitializeComponent();
            orderRepo = new OrderRepository();
            DataContext = this;
            LoadRevenueData(DateTime.Now.AddMonths(-1), DateTime.Now);

        }
        private async void LoadRevenueData(DateTime StartDate, DateTime EndDate)
        {
            DateOnly startDate = DateOnly.FromDateTime(StartDate);
            DateOnly endDate = DateOnly.FromDateTime(EndDate);
            var orders = (await orderRepo.GetAllAsync())
                  .Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate)
                  .ToList();
            var groupedOrders = orders.GroupBy(o => o.OrderDate)
                                         .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.OrderAmount) })
                                         .OrderBy(g => g.Date)
                                         .ToList();
            RevenueValues = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Doanh thu",
                        Values = new ChartValues<double>(groupedOrders.Select(g => g.Revenue))
                    }
                };
            Dates = groupedOrders.Select(g => g.Date.ToString("dd/MM/yyyy")).ToList();
            TotalRevenue = groupedOrders.Sum(g => g.Revenue);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
