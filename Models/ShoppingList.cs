using System;
using System.Collections.Generic;

namespace ShopAssist.Models
{
    // Controlled by user...
    public class ShoppingList
    {
        public int ID;
        public string Name;
        public AppContact owner;
        // TODO : public string ListCharacter; // Dit is een soort avatar uit een selectie avatars
        public IList<ListItem> items;
        public IList<AppContact> shopListMembers;
        public DateTime created;
        public DateTime modified;
    }
}