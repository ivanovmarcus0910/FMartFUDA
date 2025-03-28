using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for FrmProduct.xaml
    /// </summary>
    public partial class FrmProduct : Page
    {
        ProductRepository productRepository;
        CategoryRepository categoryRepository;
        public FrmProduct()
        {
            InitializeComponent();
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
            LoadproductList();
            LoadComboCategory();
        }

        public async void LoadproductList()
        {
            var products = await productRepository.GetAllAsync();
            dgData.ItemsSource = null;
            dgData.ItemsSource = products;

        }
        public async void LoadComboCategory()
        {
            var list = await categoryRepository.GetAllAsync();
            cboCategory.ItemsSource = list;
            cboCategory.DisplayMemberPath = "CategoryName";
            cboCategory.SelectedValuePath = "CategoryId";
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product selectedProduct)
            {
                txtProductId.Text = selectedProduct.ProductId.ToString();
                txtProductName.Text = selectedProduct.ProductName;
                txtProductPrice.Text = selectedProduct.ProductPrice.ToString();
                txtProductEntryPrice.Text = selectedProduct.ProductEntryPrice.ToString();
                txtProductDescription.Text = selectedProduct.ProductDecription;
                cboCategory.SelectedItem = selectedProduct.CategoryId;

                // Cập nhật ảnh sản phẩm
                if (!string.IsNullOrEmpty(selectedProduct.ProductImage))
                    if (!string.IsNullOrEmpty(selectedProduct.ProductImage))
                    {
                        try
                        {
                            // Tạo đường dẫn ảnh từ đường dẫn tương đối
                            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                            string absolutePath = System.IO.Path.Combine(projectDirectory, selectedProduct.ProductImage);

                            BitmapImage bitmap = new BitmapImage();
                            bitmap.BeginInit();
                            bitmap.UriSource = new Uri(absolutePath, UriKind.Absolute);
                            bitmap.EndInit();
                            imgProduct.Source = bitmap;
                            imgProduct.Tag = selectedProduct.ProductImage; // Lưu lại đường dẫn ảnh
                        }
                        catch
                        {
                            imgProduct.Source = null; // Nếu ảnh bị lỗi, xóa ảnh khỏi giao diện
                        }
                    }
                    else
                    {
                        imgProduct.Source = null;
                        imgProduct.Tag = null;
                    }
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               

                Product p = new Product
                {
                    ProductName = txtProductName.Text,
                    ProductPrice = Double.Parse(txtProductPrice.Text), // float
                    ProductEntryPrice = Double.Parse(txtProductEntryPrice.Text),
                    ProductDecription = txtProductDescription.Text,
                    ProductImage = imgProduct.Tag?.ToString(),
                    CategoryId = Convert.ToInt32(cboCategory.SelectedValue),
                                                                           
                };

                await productRepository.AddAsync(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                LoadproductList();
            }
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hộp thoại chọn ảnh
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*",
                    Title = "Select a Product Image"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Lấy đường dẫn thư mục của project
                    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string imagesFolderPath = System.IO.Path.Combine(projectDirectory, "Images");

                    // Tạo thư mục Images nếu chưa tồn tại
                    if (!Directory.Exists(imagesFolderPath))
                    {
                        Directory.CreateDirectory(imagesFolderPath);
                    }

                    // Lấy tên file ảnh và tạo đường dẫn đích
                    string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                    string destinationPath = System.IO.Path.Combine(imagesFolderPath, fileName);

                    // Sao chép ảnh vào thư mục Images (ghi đè nếu trùng tên)
                    File.Copy(openFileDialog.FileName, destinationPath, true);
                    MessageBox.Show($"Ảnh được copy đến: {destinationPath}");
                    // Hiển thị ảnh trên giao diện
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(destinationPath, UriKind.Absolute);
                    bitmap.EndInit();
                    imgProduct.Source = bitmap;

                    // Lưu đường dẫn tương đối vào Tag của imgProduct để dùng sau
                    imgProduct.Tag = $"Images/{fileName}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}");
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtProductId.Text))
                {
                    Product p = await productRepository.GetByIdAsync(Int32.Parse(txtProductId.Text));
                    if (p != null)
                    {
                        p.ProductId = Int32.Parse(txtProductId.Text);
                        p.ProductName = txtProductName.Text;
                        p.ProductPrice = double.Parse(txtProductPrice.Text);
                        p.ProductEntryPrice = double.Parse(txtProductEntryPrice.Text);
                        p.ProductDecription = txtProductDescription.Text;
                        p.ProductImage = imgProduct.Tag?.ToString() ?? p.ProductImage; ;
                        p.CategoryId = Convert.ToInt32(cboCategory.SelectedValue);
                        await productRepository.UpdateAsync(p);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn một sản phẩm!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadproductList();
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtProductId.Text))
                {
                    Product p = await productRepository.GetByIdAsync(Int32.Parse(txtProductId.Text));
                    if (p != null)
                    {
                        await productRepository.DeleteAsync(p.ProductId);
                    }
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn một sản phẩm!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                LoadproductList();
            }
        }

    }
}
