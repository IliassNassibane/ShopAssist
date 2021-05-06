using System;

namespace ShopAssist.Models
{
    public class ProductAtShop
    {
        public Product product;
        public Shop shop;
        public bool relevant;
        public float price;
        public float discountPrice;
    }
}