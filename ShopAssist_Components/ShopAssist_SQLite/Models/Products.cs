using ShopAssist_SQLite.Enums;
using System;
using System.Linq;

namespace ShopAssist_SQLite.Models
{
    public class Products
    {
        public int ProdID { get; set; }
        public int BrandID { get; set; }
        public string Brand { get; set; }
        public string ProdName { get; set; }
        public string BrandImg { get; set; }
        public string ProductImg { get; set; }
        public string Categories { get; set; }

        /// <summary>
        /// This property returns Products.Categories as an int array.
        /// </summary>
        public int[] CategoriesAsIntArr
        {
            get
            {
                return Categories.Split('|').Select(n => Convert.ToInt32(n)).ToArray();
            }
        }

        /// <summary>
        /// This property returns Products.Categories as a list of name representations of the Category Enum.
        /// </summary>
        public string CategoriesAsCategoryString
        {
            get
            {
                int[] categories = CategoriesAsIntArr;
                string output = "";

                foreach (int category in categories)
                {
                    output+=((CATEGORY)category).ToString();
                }

                return output;
            }
        }

        public override string ToString()
        {
            return $"(Product ID:{ProdID}, Brand ID: {BrandID}) - {Brand}'s {ProdName} (Category: {Categories})";
        }
    }
}
