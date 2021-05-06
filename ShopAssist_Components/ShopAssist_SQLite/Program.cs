using ShopAssist_SQLite.Models;
using System;
using System.Collections.Generic;

namespace ShopAssist_SQLite
{
    class Program
    {
        static void Main()
        {
            // TODO : Integreer in ShopAssist app.

            #region Reader
            Console.WriteLine("Products:");
            IList<Products> products = SADatabaseReader.LoadProducts();

            foreach (Products product in products)
            {
                Console.WriteLine(product.ToString());
            }

            Console.WriteLine("\nAppUsers:");
            IList<AppUser> users = SADatabaseReader.LoadAppUsers();

            foreach (AppUser user in users)
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("\nBrands:");
            IList<Brand> brands = SADatabaseReader.LoadBrands();

            foreach (Brand brand in brands)
            {
                Console.WriteLine(brand.ToString());
            }

            Console.WriteLine("\nShoppinglists:");
            IList<ShopList> shopLists = SADatabaseReader.LoadShopLists();

            foreach (ShopList shopList in shopLists)
            {
                Console.WriteLine(shopList.ToString());
            }

            Console.WriteLine("\nShoppinglist members:");
            IList<ShopListMember> shopListMembers = SADatabaseReader.LoadShopListMembers(1);

            foreach (ShopListMember shopListMember in shopListMembers)
            {
                Console.WriteLine(shopListMember.ToString());
            }

            Console.WriteLine("\nShoppinglist content:");
            IList<ShopListItem> items = SADatabaseReader.LoadShopListItems(1);

            foreach (ShopListItem item in items)
            {
                Console.WriteLine(item.ToString());
            }
            #endregion

            #region UpdateShopList test
            /*
            ShopList list = new ShopList();
            //list.ShopListID = SADatabaseReader.GetNewShopListID();
            list.Name = "Kerst kerkdienst middelen";
            list.ListCategory = 2;
            list.DateTimeAdded = "00:00:00 03-01-2021"; // TODO : Method voor het vullen van een datetime in de voorgestelde format;
            // TODO : DateTimeModified instellen, mits changed property op true staat;

            var result = SADatabaseWriter.SaveShopList(list);
            Console.WriteLine($"\nUpdate result is: {result.Item2}");

            Console.WriteLine("\nUpdated shoppinglists:");
            shopLists = SADatabaseReader.LoadShopLists();

            foreach (ShopList shopList in shopLists)
            {
                Console.WriteLine(shopList.ToString());
            }
            */
            #endregion

            #region DeleteShopList test
            /*
            ShopList list = new ShopList();
            list.ShopListID = 2;

            var result = SADatabaseWriter.DeleteShopList(list);
            Console.WriteLine($"\nDelete result is: {result.Item2}");

            Console.WriteLine("\nUpdated shoppinglists:");
            shopLists = SADatabaseReader.LoadShopLists();

            foreach (ShopList shopList in shopLists)
            {
                Console.WriteLine(shopList.ToString());
            }
            */
            #endregion

            #region SaveShopListItem test
            /*
            ShopList list = new ShopList();
            list.ShopListID = 3;

            ShopListItem shopListItem = new ShopListItem();
            shopListItem.ShopListProductID = 1;
            shopListItem.Unit = 2;
            shopListItem.Amount = 10;
            shopListItem.Acquired = 0;
            shopListItem.ItemAdded = DateTimeStamp.Stamp();
            shopListItem.ShopListItemChanged = true;

            var result = SADatabaseWriter.SaveShopListItem(list, shopListItem);
            Console.WriteLine($"\nShopListItem Save result is: {result.Item2}");

            Console.WriteLine("\nShoppinglist content:");
            IList<ShopListItem> shopListItems = SADatabaseReader.LoadShopListItems(3);

            foreach (ShopListItem item in shopListItems)
            {
                Console.WriteLine(item.ToString());
            }
            */
            #endregion

            #region DeleteShopListItem test
            /*
            ShopList list = new ShopList();
            list.ShopListID = 4;

            ShopListItem shopListItem = new ShopListItem();
            shopListItem.ShopListProductID = 1;

            var result = SADatabaseWriter.DeleteShopListItem(list, shopListItem);
            Console.WriteLine($"\nShopListItem Delete result is: {result.Item2}");

            Console.WriteLine("\nShoppinglist content:");
            IList<ShopListItem> shopListItems = SADatabaseReader.LoadShopListItems(3);

            foreach (ShopListItem item in shopListItems)
            {
                Console.WriteLine(item.ToString());
            }
            */
            #endregion

            #region Save ShopListOwner test
            /*
            AppUser user1 = new AppUser();
            user1.ID = 2;

            AppUser user2 = new AppUser();
            user2.ID = 1;

            ShopList list = new ShopList();
            list.ShopListID = 1;

            var result = SADatabaseWriter.UpdateShopListOwner(list, user1);
            Console.WriteLine($"\nShopList owner save result: {result.Item2}");

            var IsUser1ListOwner = SADatabaseReader.GetShopListOwnerID(list, user1);
            Console.WriteLine($"Is user1 the listOwner: {IsUser1ListOwner}");

            var IsUser2ListOwner = SADatabaseReader.GetShopListOwnerID(list, user2);
            Console.WriteLine($"Is user2 the listOwner: {IsUser2ListOwner}");
            */
            #endregion

            #region Delete ShopList owner
            /*
            AppUser user1 = new AppUser();
            user1.ID = 2;
            user1.UserName = "USER1";

            ShopList list = new ShopList();
            list.ShopListID = 1;

            var result = SADatabaseWriter.DeleteShopListOwner(list, user1);
            Console.WriteLine($"\nShopList owner delete result: {result.Item2}");

            var IsUser1ListOwner = SADatabaseReader.GetShopListOwnerID(list, user1);
            Console.WriteLine($"Is user1 nog steeds een listOwner: {IsUser1ListOwner}");
            */
            #endregion

            #region Add ShopList member test
            /*
            AppUser user1 = new AppUser();
            user1.ID = 2;
            user1.UserName = "USER1";

            AppUser user2 = new AppUser();
            user2.ID = 3;
            user2.UserName = "USER2";

            ShopList list = new ShopList();
            list.ShopListID = 1;

            var result = SADatabaseWriter.AddShopListMember(list, user1);
            Console.WriteLine($"\nShopList member add result: {result.Item2}");

            var IsUser1ListMember = SADatabaseReader.GetShopListMemberID(list, user1);
            Console.WriteLine($"Is user1 the listmember: {IsUser1ListMember}");

            var IsUser2ListMember = SADatabaseReader.GetShopListMemberID(list, user2);
            Console.WriteLine($"Is user2 the listmember: {IsUser2ListMember}");
            */
            #endregion

            #region Delete ShopList member
            /*
            AppUser user1 = new AppUser();
            user1.ID = 2;
            user1.UserName = "USER1";

            ShopList list = new ShopList();
            list.ShopListID = 1;

            var result = SADatabaseWriter.DeleteShopListMember(list, user1);
            Console.WriteLine($"\nShopList member delete result: {result.Item2}");

            var IsUser1ListMember = SADatabaseReader.GetShopListMemberID(list, user1);
            Console.WriteLine($"Is user1 nog steeds een listmember: {IsUser1ListMember}");
            */
            #endregion

        }
    }
}
