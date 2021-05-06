using ShopAssist_SQLite.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace ShopAssist_SQLite
{
    public class SADatabaseReader : SADatabase
    {
        // SOURCE: https://www.youtube.com/watch?v=ayp3tHEkRc0&ab_channel=IAmTimCorey

        // TODO : 
        // [X] Error handling
        // [] Event logging
        // [] Async

        public static List<AppUser> LoadAppUsers()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<AppUser> output = null;

                try
                {
                    output = connection.Query<AppUser>("SELECT * FROM AppUser");
                }
                catch (SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadAppUsers: Error logging naar log.db
                    throw;
                }

                return output.ToList();
            }
        }

        public static List<Products> LoadProducts()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<Products> output = null;

                try
                {
                    output = connection.Query<Products>(
                        @"
                            SELECT
	                            p.ID as 'ProdID',
	                            b.ID as 'BrandID',
	                            b.BrandName as 'Brand',
	                            p.ProdName as 'ProdName',
	                            b.ImgPath as 'BrandImg',
	                            slp.ImgPath as 'ProductImg',
	                            (
		                            SELECT group_concat(pc.Category,'|')
		                            FROM ProductCategory pc
		                            WHERE pc.Product_ID = p.ID
	                            ) as 'Categories'
                            FROM ShopListProduct as slp
                            JOIN ProductCategory as pc on pc.Product_ID = slp.Product_ID
                            JOIN Products as p on p.ID = slp.Product_ID
                            JOIN Brands as b on b.ID = slp.Brand_ID
                            GROUP BY slp.ID"
                        );
                }
                catch(SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadProducts: Error logging naar log.db
                    throw;
                }
                
                return output.ToList();
            }
        }

        public static List<Brand> LoadBrands()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<Brand> output = null;

                try
                {
                    output = connection.Query<Brand>(
                        @"
                            SELECT 
	                            ID,
	                            BrandName as 'Name',
	                            ImgPath as 'Logo',
	                            DateTimeDeleted as 'Deleted'
                            FROM Brands
                        ");
                }
                catch(SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadBrands: Error logging naar log.db
                    throw;
                }

                return output.ToList();
            }
        }

        public static List<ShopList> LoadShopLists()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<ShopList> output = null;

                try
                {
                    output = connection.Query<ShopList>("SELECT * FROM ShoppingList as sl");
                }
                catch (SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadShopLists: Error logging naar log.db
                    throw;
                }

                return output.ToList();
            }
        }

        public static int? GetNewShopListID()
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<ShopList> ShopLists = connection.Query<ShopList>("SELECT * FROM ShoppingList Order By ID DESC LIMIT 1");
                return (int)(ShopLists.First().ShopListID) + 1;
            }
        }

        public static bool DoesShopListExist(ShopList shopList)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                ShopList output = null;

                try
                {
                    output = connection.FindWithQuery<ShopList>($"SELECT * FROM ShoppingList WHERE ID = {shopList.SHOPLISTID}");

                    if (output != null)
                    {
                        return true;
                    }
                }
                catch (SQLiteException ex)
                {
                    // TODO : SADatabaseReader.DoesShopListExist: Error logging naar log.db
                    throw;
                }
            }

            return false;
        }

        public static List<ShopListMember> LoadShopListMembers(int shopListID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<ShopListMember> output = null;

                try
                {
                    output = connection.Query<ShopListMember>(
                        @"
                            SELECT
	                            au.ID as 'MemberID',
	                            au.UserName as 'Name',
	                            au.UserHash as 'Hash',
	                            au.ImgPath as 'Icon'
                            FROM ShopListMember as slm
                            JOIN AppUser as au on au.ID = slm.AppUser_ID " +
                            $"WHERE slm.ShopList_ID = {shopListID}"
                        );
                }
                catch (SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadShopListMembers: Error logging naar log.db
                    throw;
                }                

                return output.ToList();
            }
        }

        public static bool DoesShopListOwnerExist(ShopList shopList, AppUser user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                ShopListMember output = null;

                try
                {
                    output = connection.FindWithQuery<ShopListMember>(
                        @"SELECT
                            au.ID as 'MemberID',
                            au.UserName as 'Name',
                            au.UserHash as 'Hash',
                            au.ImgPath as 'Icon'
                        FROM ShopListOwner as slo
                        JOIN AppUser as au on au.ID = slo.AppUser_ID " +
                        $"WHERE slo.ShopList_ID = {shopList.SHOPLISTID} AND slo.AppUser_ID = {user.ID}"
                        );

                    if (output != null)
                    {
                        return true;
                    }
                }
                catch(SQLiteException ex)
                {
                    // TODO : SADatabaseReader.DoesShopListOwnerExist: Error logging naar log.db
                    throw;
                }
            }

            return false;
        }

        public static bool DoesShopListMemberExist(ShopList shopList, AppUser user)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                ShopListMember output = null;

                try
                {
                    output = connection.FindWithQuery<ShopListMember>(
                        @"SELECT 
	                        au.ID as 'MemberID',
	                        au.UserName as 'Name',
	                        au.UserHash as 'Hash',
	                        au.ImgPath as 'Icon'
                        FROM ShopListMember as slm
                        JOIN AppUser as au on au.ID = slm.AppUser_ID " +
                        $"WHERE slm.ShopList_ID = {shopList.SHOPLISTID} AND slm.AppUser_ID = {user.ID}"
                        );

                    if (output != null)
                    {
                        return true;
                    }
                }
                catch(SQLiteException ex)
                {
                    // TODO : SADatabaseReader.DoesShopListMemberExist: Error logging naar log.db
                    throw;
                }
            }

            return false;
        }

        public static List<ShopListItem> LoadShopListItems(int ShopListID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                List<ShopListItem> output = null;

                try
                {
                    output = connection.Query<ShopListItem>(
                        @"
                            SELECT
	                            slp.ID as 'ShopListProductID',
	                            b.BrandName as 'BrandName',
	                            p.ProdName as 'ProductName',
	                            b.ImgPath as 'BrandLogo',
	                            slp.ImgPath as 'ProductImage',
	                            (
		                            SELECT group_concat(pc.Category,'|')
		                            FROM ProductCategory pc
		                            WHERE pc.Product_ID = p.ID
	                            ) as 'Categories',
	                            sli.Amount,
	                            sli.Unit,
	                            sli.ItemAcquired as 'Acquired',
	                            sli.DateTimeAdded as 'ItemAdded',
	                            sli.DateTimeModified as 'ItemUpdated'
                            FROM ShopListItem as sli
                            JOIN ShopListProduct as slp on slp.ID = sli.ShopListProduct_ID
                            JOIN Products as p on p.ID = slp.Product_ID
                            JOIN Brands as b on b.ID = slp.Brand_ID
                            JOIN ProductCategory as pc on pc.Product_ID = p.ID " +
                            $"WHERE sli.ShopList_ID = {ShopListID} GROUP BY slp.ID"
                        );
                }
                catch (SQLiteException ex)
                {
                    // TODO : SADatabaseReader.LoadShopListItems: Error logging naar log.db
                    throw;
                }

                return output.ToList();
            }
        }

        public static bool DoesShopListItemExist(int shopListID, ShopListItem shopListItem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.FindWithQuery<ShopListItem>(
                    @"
                        SELECT
	                        slp.ID as 'ShopListProductID',
	                        b.BrandName as 'BrandName',
	                        p.ProdName as 'ProductName',
	                        b.ImgPath as 'BrandLogo',
	                        slp.ImgPath as 'ProductImage',
	                        (
		                        SELECT group_concat(pc.Category,'|')
		                        FROM ProductCategory pc
		                        WHERE pc.Product_ID = p.ID
	                        ) as 'Categories',
	                        sli.Amount,
	                        sli.Unit,
	                        sli.ItemAcquired as 'Acquired',
	                        sli.DateTimeAdded as 'ItemAdded',
	                        sli.DateTimeModified as 'ItemUpdated'
                        FROM ShopListItem as sli
                        JOIN ShopListProduct as slp on slp.ID = sli.ShopListProduct_ID
                        JOIN Products as p on p.ID = slp.Product_ID
                        JOIN Brands as b on b.ID = slp.Brand_ID
                        JOIN ProductCategory as pc on pc.Product_ID = p.ID " +
                        $"WHERE sli.ShopList_ID = {shopListID} AND ShopListProduct_ID = {shopListItem.ShopListProductID} GROUP BY slp.ID"
                    );

                if (output != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
