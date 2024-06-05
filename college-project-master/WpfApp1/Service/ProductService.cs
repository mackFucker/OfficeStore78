using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using WpfApp1.db;

namespace WpfApp1.Service
{
    public class ProductService : INotifyPropertyChanged
    {
        private readonly MySqlConnection connection;
        private readonly Cart _cart;
        private List<CartItem> _cartItems;
        private decimal _totalPrice;

        public ProductService()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            connection = new MySqlConnection(connectionString);
            EnsureProductsTableExists();
            _cart = new Cart();
        }

        public List<CartItem> CartItems
        {
            get { return _cartItems; }
            set
            {
                _cartItems = value;
                OnPropertyChanged(nameof(CartItems));
            }
        }

        public decimal TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }


        private void EnsureProductsTableExists()
        {
            try
            {
                connection.Open();
                MySqlCommand command = new("CREATE TABLE IF NOT EXISTS supplies (" +
                                           "ProductID INT AUTO_INCREMENT PRIMARY KEY, " +
                                           "ProductName VARCHAR(255) NOT NULL, " +
                                           "ProductDescription TEXT, " +
                                           "ProductImage BLOB, " +
                                           "ProductPrice DECIMAL(10, 2) NOT NULL" +
                                           ");", connection);
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error ensuring supplies table exists: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        public bool AddProduct(string productName, string productDescription, byte[] productImage, decimal productPrice)
        {
            try
            {
                connection.Open();
                MySqlCommand command = new("INSERT INTO supplies (ProductName, ProductDescription, ProductImage, ProductPrice) VALUES (@productName, @productDescription, @productImage, @productPrice);", connection);
                command.Parameters.AddWithValue("@productName", productName);
                command.Parameters.AddWithValue("@productDescription", productDescription);
                command.Parameters.AddWithValue("@productImage", productImage);
                command.Parameters.AddWithValue("@productPrice", productPrice);
                command.ExecuteNonQuery();

                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand command = new("SELECT ProductID, ProductName, ProductDescription, ProductImage, ProductPrice FROM supplies;", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product(
                        reader.GetInt32("ProductID"),
                        reader.GetString("ProductName"),
                        reader.GetString("ProductDescription"),
                        reader["ProductImage"] as byte[],
                        reader.GetDecimal("ProductPrice")
                    ));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error fetching products: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return products;
        }


        public void AddToCart(Product product, int quantity)
        {
            _cart.AddToCart(product, quantity);
        }

        public void RemoveFromCart(Product product)
        {
            _cart.RemoveFromCart(product);
        }

        public List<CartItem> GetCartItems()
        {
            return _cart.GetCartItems();
        }

        public decimal GetTotalPrice()
        {
            return _cart.GetTotalPrice();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ClearCart()
        {
            _cart.ClearCart();

            // Update CartItems and TotalPrice properties
            CartItems = _cart.GetCartItems();
            TotalPrice = _cart.GetTotalPrice();
        }
    }

    
}
