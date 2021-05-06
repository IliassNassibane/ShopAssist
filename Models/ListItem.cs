using System;
using ShopAssist.Enums;

namespace ShopAssist.Models
{
    // Controlled by user...
    public class ListItem
    {
        public int ID;
        public Product product;
        public UNITS unit;
        public float amount;
        public bool acquired;
        public DateTime added;
        public DateTime modified;
    }
}