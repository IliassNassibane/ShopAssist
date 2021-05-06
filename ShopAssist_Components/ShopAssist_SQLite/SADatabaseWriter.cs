using ShopAssist_SQLite.Models;
using ShopAssist_SQLite.System;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopAssist_SQLite
{
	// TODO : SADatabaseWriter
	/*
	 * [X] Error handling. SQLiteException.
	 * [] Tuples in eventlog.
	 * [X] DRY cleanup
	 * [] Edge case-ing
	 */

	public sealed class SADatabaseWriter : SADatabase
	{
		private static SADatabaseWriter connectionInstance = null;
		private static readonly object databaseLock = new object();
		public SADatabaseWriter() { }

		public static SADatabaseWriter Instance
		{
			get
			{
				if (connectionInstance == null)
                {
					lock (databaseLock)
					{
						if (connectionInstance == null)
						{
							connectionInstance = new SADatabaseWriter();
						}
					}
                }
				return connectionInstance;
			}
		}

		/// <summary>
		/// Saves the changes made on the passed ShopList object to the ShoppingList (SQLite) database table. And if the shoplist already exists, that list will get updated instead of insterted.
		/// </summary>
		/// <param name="shopList"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> SaveShopList(ShopList shopList)
        {
			Tuple<bool, string> result = new Tuple<bool,string>(false, "");

			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString(), SQLiteOpenFlags.ReadWrite))
			{
                try
                {
					if (SADatabaseReader.DoesShopListExist(shopList))
					{
						_ = connection.Update(shopList);

						result = new Tuple<bool, string>(true, $"ShopList {shopList.Name}({shopList.ShopListID}) has been updated.");
					}
					else
					{
						_ = connection.Insert(shopList);

						result = new Tuple<bool, string>(true, $"ShopList {shopList.Name}({shopList.ShopListID}) has been added.");
					}
				}
                catch (SQLiteException ex)
                {
					// TODO : SADatabaseReader.SaveShopList: Error logging naar log.db
					throw;
                }
			}

			return result;
		}
		
		/// <summary>
		/// Deletes a specific ShopList in the ShoppingList (SQLite) database table, if it exists.
		/// </summary>
		/// <param name="shopList"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> DeleteShopList(ShopList shopList)
        {
			Tuple<bool, string> result = new Tuple<bool, string>(false, "");

			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString(), SQLiteOpenFlags.ReadWrite))
            {
				try
				{
					if (SADatabaseReader.DoesShopListExist(shopList))
					{
						_ = connection.Delete(shopList);

						if (!(SADatabaseReader.DoesShopListExist(shopList)))
						{
							result = new Tuple<bool, string>(true, $"ShopList {shopList.Name}({shopList.ShopListID}) has been deleted.");
						}
						else
						{
							result = new Tuple<bool, string>(false, $"ShopList {shopList.Name}({shopList.ShopListID}) still exists.");
						}
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopList {shopList.Name}({shopList.ShopListID}) hasn't been saved yet or doesn't exist .");
					}
				}
                catch (SQLiteException ex)
                {
					// TODO : SADatabaseReader.DeleteShopList: Error logging naar log.db
					throw;
                }
            }

			return result;
        }

		/// <summary>
		/// Saves or updates a shopList item in the ShopListItem table.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="item"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> SaveShopListItem(ShopList shopList, ShopListItem item)
		{
			Tuple<bool, string> result = new Tuple<bool, string>(false, "");

			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString(), SQLiteOpenFlags.ReadWrite))
			{
				try
				{
					if (SADatabaseReader.DoesShopListExist(shopList))
					{
						if (SADatabaseReader.DoesShopListItemExist((int)shopList.SHOPLISTID, item))
						{
							string updateQuery =
								$"UPDATE ShopListItem SET Unit = {item.Unit}, Amount = {item.Amount}, ItemAcquired = {item.Acquired}, DateTimeModified = '{item.ITEMUPDATED}' " +
								$"WHERE ShopList_ID = {shopList.SHOPLISTID} AND ShopListProduct_ID = {item.ShopListProductID}";

							connection.Execute(updateQuery);

							result = new Tuple<bool, string>(true, $"ShopList item {item.ShopListProductID} is bijgewerkt.");
						}
						else
						{
							string insertQuery =
								"INSERT INTO ShopListItem (ShopList_ID, ShopListProduct_ID, Unit, Amount, ItemAcquired, DateTimeAdded) " +
								$"VALUES ({shopList.SHOPLISTID}, {item.ShopListProductID}, {item.Unit}, {item.Amount}, {item.Acquired}, '{item.ItemAdded}')";

							connection.Execute(insertQuery);

							result = new Tuple<bool, string>(true, $"ShopList item {item.ShopListProductID} is toegevoegd aan {shopList.Name}({shopList.SHOPLISTID}).");
						}
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopList {shopList.Name}({shopList.SHOPLISTID}) bestaat niet of is nog niet opgeslagen.");
					}
				}
                catch (SQLiteException ex)
                {
					// TODO : SADatabaseReader.SaveShopListItem: Error logging naar log.db
					throw;
                }
			}

			return result;
		}

		/// <summary>
		/// Deletes a Shoplist item from the ShopListItem table.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="item"></param>
		/// <returns>
		/// Returns a Tuple<bool, string> with a processing message.
		/// </returns>
		public static Tuple<bool, string> DeleteShopListItem(ShopList shopList, ShopListItem item)
		{
			Tuple<bool, string> result = new Tuple<bool, string>(false, "");

			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
				try
				{
					if (SADatabaseReader.DoesShopListItemExist((int)shopList.SHOPLISTID, item))
					{
						connection.Execute($"DELETE FROM ShopListItem WHERE ShopList_ID = {shopList.SHOPLISTID} AND ShopListProduct_ID = {item.ShopListProductID}");

						result = new Tuple<bool, string>(true, $"ShopList item {item.ProductName}({item.ShopListProductID}) succesvol verwijderd.");
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopList item {item.ProductName}({item.ShopListProductID}) bestaat niet.");
					}
				}
				catch(SQLiteException ex)
                {
					// TODO : SADatabaseReader.DeleteShopListItem: Error logging naar log.db
					throw;
                }
            }
				
			return result;
		}

		/// <summary>
		/// Updates or creates a shoplist owner record in the ShopListOwner table.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="user"></param>
		/// <returns>
		/// Returns a Tuple<bool, string> with a processing message.
		/// </returns>
		public static Tuple<bool, string> UpdateShopListOwner(ShopList shopList, AppUser user)
        {
			Tuple<bool, string> result = new Tuple<bool, string>(false, "");

			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString(), SQLiteOpenFlags.ReadWrite))
			{
				try
				{
					if (SADatabaseReader.DoesShopListExist(shopList))
					{
						connection.Execute(
							"INSERT OR REPLACE INTO ShopListOwner (AppUser_ID, ShopList_ID, DateTimeModified) " +
							$"VALUES ({user.ID}, {shopList.SHOPLISTID}, '{DateTimeStamp.Stamp()}')"
							);

						result = new Tuple<bool, string>(false, $"De owner van shoplist {shopList.Name}({shopList.SHOPLISTID}) is bijgewerkt.");
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopList {shopList.Name}({shopList.SHOPLISTID}) bestaat niet.");
					}
				}
				catch(SQLiteException ex)
                {
					// TODO : SADatabaseReader.UpdateShopListOwner: Error logging naar log.db
					throw;
                }
			}

			return result;
		}

		/// <summary>
		/// Deletes a ShopList member from the ShopListMember table.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="user"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> DeleteShopListOwner(ShopList shopList, AppUser user)
		{
			var result = new Tuple<bool, string>(false, "");

			using (var connection = new SQLiteConnection(LoadConnectionString()))
			{
				try
				{
					connection.Execute($"DELETE FROM ShopListOwner WHERE AppUser_ID = {user.ID} AND ShopList_ID = {shopList.SHOPLISTID}");

					if (!(SADatabaseReader.DoesShopListMemberExist(shopList, user)))
					{
						result = new Tuple<bool, string>(true, $"ShopListOwner {user.UserName}({user.ID}) is verwijderd.");
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopListOwner {user.UserName}({user.ID}) bestaat nog.");
					}
				}
				catch(SQLiteException ex)
                {
					// TODO : SADatabaseReader.DeleteShopListOwner: Error logging naar log.db
					throw;
                }
			}

			return result;
		}

		/// <summary>
		/// Adds a record to the ShopListMember table, with a foreign keys to the AppUser and ShopList tables.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="user"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> AddShopListMember(ShopList shopList, AppUser user)
        {
			var result = new Tuple<bool, string>(false, "");

			using (var connection = new SQLiteConnection(LoadConnectionString()))
			{
				try
				{
					if (SADatabaseReader.DoesShopListExist(shopList))
					{
						var AppUserExists = connection.Query<AppUser>($"SELECT * FROM AppUser WHERE ID = {user.ID}").Count;

						if (AppUserExists > 0)
						{
							if (!(SADatabaseReader.DoesShopListMemberExist(shopList, user)))
							{
								connection.Execute(
									"INSERT OR REPLACE INTO ShopListMember (AppUser_ID, ShopList_ID, DateTimeAdded) " +
									$"VALUES ({user.ID}, {shopList.SHOPLISTID}, '{DateTimeStamp.Stamp()}')"
									);

								result = new Tuple<bool, string>(false, $"Member {user.UserName}({user.ID}) is toegevoegd aan shoplist {shopList.Name}({shopList.SHOPLISTID}).");
							}
							else
							{
								result = new Tuple<bool, string>(false, $"Gebruiker {user.UserName}({user.ID}) bestaat al.");
							}

						}
						else
						{
							result = new Tuple<bool, string>(false, $"Gebruiker {user.UserName}({user.ID}) bestaat niet of is nog niet opgeslagen.");
						}
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopList {shopList.Name}({shopList.SHOPLISTID}) bestaat niet of is nog niet opgeslagen.");
					}
				}
                catch (SQLiteException ex)
                {
					// TODO : SADatabaseReader.AddShopListMember: Error logging naar log.db
					throw;
                }
			}

			return result;
        }
		
		/// <summary>
		/// Deletes a ShopList member from the ShopListMember table.
		/// </summary>
		/// <param name="shopList"></param>
		/// <param name="user"></param>
		/// <returns>
		/// Returns a Tuple<bool, string>, with a processing message.
		/// </returns>
		public static Tuple<bool, string> DeleteShopListMember(ShopList shopList, AppUser user)
        {
			var result = new Tuple<bool, string>(false, "");

			using (var connection = new SQLiteConnection(LoadConnectionString()))
            {
				try
				{
					connection.Execute($"DELETE FROM ShopListMember WHERE AppUser_ID = {user.ID} AND ShopList_ID = {shopList.SHOPLISTID}");

					if (!(SADatabaseReader.DoesShopListMemberExist(shopList, user)))
					{
						result = new Tuple<bool, string>(true, $"ShopListMember {user.UserName}({user.ID}) is verwijderd.");
					}
					else
					{
						result = new Tuple<bool, string>(false, $"ShopListMember {user.UserName}({user.ID}) bestaat nog.");
					}
				}
                catch (SQLiteException ex)
                {
					// TODO : SADatabaseReader.DeleteShopListMember: Error logging naar log.db
					throw;
                }
			}

			return result;
        }

		public static void AssignMemberToItem(ShopList shopList, ShopListItem item, AppUser user)
        {
			// TODO : AssignMemberToItem, wanneer Task systeem is uitgedokterd...
        }
	}
}
