using ShopAssist_SQLite.Enums;
using ShopAssist_SQLite.System;
using System;
using System.Linq;

namespace ShopAssist_SQLite.Models
{
    public class ShopListItem
    {
        public int ShopListProductID { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string BrandLogo { get; set; }
        public string ProductImage { get; set; }
        //public int AssignedUser_ID;      // TODO : ShopListMember koppelen als taak
        public string Categories { get; set; }
        public float Amount { get; set; }
        public int Unit { get; set; }
        public int Acquired { get; set; }
        public string ItemAdded { get; set; }
        public string ItemUpdated { get; set; }

        public string ITEMUPDATED
        {
            get => ShopListItemChanged ? DateTimeStamp.Stamp() : null;
        }

        public bool ShopListItemChanged = false;

        /// <summary>
        /// This property returns ShopListItem.Categories as an int array.
        /// </summary>
        public int[] CategoriesAsIntArr
        {
            get => Categories.Split('|').Select(n => Convert.ToInt32(n)).ToArray();
        }

        /// <summary>
        /// This property returns ShopListItem.Categories as a list of name representations of the Category Enum.
        /// </summary>
        public string CategoriesAsCategoryString
        {
            get
            {
                int[] categories = CategoriesAsIntArr;
                string output = "";

                foreach (int category in categories)
                {
                    output += ((CATEGORY)category).ToString();
                }

                return output;
            }
        }

        /// <summary>
        /// This property returns ShopListItem.Categories as a list of name representations of the Category Enum.
        /// </summary>
        public string UnitAsString
        {
            get => ((UNITS)Unit).ToString();
        }

        public override string ToString() => $"{Amount} {UnitAsString} {BrandName}'s {ProductName} ({ShopListProductID})";
    }
}
