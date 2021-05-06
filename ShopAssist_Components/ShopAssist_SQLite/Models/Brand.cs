using System;
using System.Collections.Generic;
using System.Text;

namespace ShopAssist_SQLite.Models
{
    public class Brand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Deleted { private get; set; }

        /// <summary>
        /// This property returns Brand.Deleted as a boolean.
        /// </summary>
        public bool BrandDeleted
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Deleted));
            }
        }

        public override string ToString()
        {
            return $"{Name} ({ID}), deleted: {BrandDeleted}";
        }
    }
}
