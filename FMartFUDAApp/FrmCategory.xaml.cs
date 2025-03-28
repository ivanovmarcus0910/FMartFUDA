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
    /// Interaction logic for FrmCategory.xaml
    /// </summary>
    public partial class FrmCategory : Page
    {
        CategoryRepository categoryRepository;
        public FrmCategory()
        {
            InitializeComponent();
            categoryRepository = new CategoryRepository();
            LoadCategoryList();

        }
        public async void LoadCategoryList()
        {
            var Categorys = await categoryRepository.GetAllAsync();
            dgData.ItemsSource = null;
            dgData.ItemsSource = Categorys;

        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Category selectedCategory)
            {
                txtCategoryId.Text = selectedCategory.CategoryId.ToString();
                txtCategoryName.Text = selectedCategory.CategoryName.ToString();
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Category s = new Category
                {
                    CategoryName = txtCategoryName.Text,
                };
                await categoryRepository.AddAsync(s);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadCategoryList();
            }
        }
     
        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCategoryId.Text))
                {
                    Category s = await categoryRepository.GetByIdAsync(Int32.Parse(txtCategoryId.Text));
                    if (s != null)
                    {
                        s.CategoryId = Int32.Parse(txtCategoryId.Text);
                        s.CategoryName = txtCategoryName.Text;
                        await categoryRepository.UpdateAsync(s);
                    }

                }
                else
                {
                    MessageBox.Show("You must select a Category!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadCategoryList();
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!string.IsNullOrEmpty(txtCategoryId.Text))
                {
                    Category g = await categoryRepository.GetByIdAsync(Int32.Parse(txtCategoryId.Text));
                    await categoryRepository.DeleteAsync(g.CategoryId);
                }
                else
                {
                    MessageBox.Show("You must select a Category!");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                LoadCategoryList();
            }
        }
    }
}
