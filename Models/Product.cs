using System;
using System.Collections.Generic;
using ShopAssist.Enums;

namespace ShopAssist.Models
{
    // Not controlled by user...
    public class Product
    {
        public int ID;
        public string productName;
        public string imgPath;
        public CATEGORY category;
        public Brand brand;
        public IList<UNITS> allowedUnits;
    }
}