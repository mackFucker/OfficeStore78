using System;

namespace WpfApp1.db
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public byte[] ProductImage { get; set; }
        public decimal ProductPrice { get; set; }

        public Product(int productId, string productName, string productDescription, byte[] productImage, decimal productPrice)
        {
            ProductID = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductImage = productImage;
            ProductPrice = productPrice;
        }
    }
}
