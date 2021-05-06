using ShopAssist_SQLite.Enums;
using ShopAssist_SQLite.System;
using SQLite;

namespace ShopAssist_SQLite.Models
{
    [Table("ShoppingList")]
    public class ShopList
    {
        [PrimaryKey, Column("ID")]
        public int? ShopListID { get; set; }

        public int? SHOPLISTID
        {
            get => ShopListID.HasValue ? ShopListID : SADatabaseReader.GetNewShopListID();
        }

        [Column("Name")]
        public string Name { get; set; }

        [Column("ListCat")]
        public int ListCategory { get; set; }

        [Column("DateTimeAdded")]
        public string DateTimeAdded { get; set; }

        [Column("DateTimeModified")]
        public string DateTimeModified { get; set; }

        public string DATETIMEMODIFIED
        {
            get => ShoppingListChanged ? DateTimeStamp.Stamp() : null;
        }

        public bool ShoppingListChanged = false;
        
        /// <summary>
        /// This property returns ShopList.ListCategory as the ListCategory string representation.
        /// </summary>
        public string ListCategoryAsString
        {
            get => ((LISTCATEGORY)ListCategory).ToString();
        }

        public override string ToString() => $"ShopList {Name} ({ShopListID}), {ListCategoryAsString}.";
    }
}
