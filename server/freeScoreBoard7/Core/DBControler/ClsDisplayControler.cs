using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
	public static class ClsDisplayControler
	{
		private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		// PageSet
		public static List<DisplayPageSet> DisplayPageSets()
		{
			List<DisplayPageSet> lst = new List<DisplayPageSet>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.DisplayPageSet
					   orderby s.Sort
					   select s).ToList();
			}

			return lst;
		}

		public static DisplayPageSet DisplayPageSet(int id)
		{
			DisplayPageSet res = new DisplayPageSet();
			using (fsbDB FSBDB = new fsbDB())
			{
				res = (from s in FSBDB.DisplayPageSet
					   where s.ID == id
					   select s).FirstOrDefault();
			}

			return res;
		}

		public static long AddDisplayPageSet(DisplayPageSet DisplayPageSet)
		{
			try
			{
				long id = 0;

				using (fsbDB FSBDB = new fsbDB())
				{
					FSBDB.DisplayPageSet.Add(DisplayPageSet);
					FSBDB.SaveChanges();
					id = DisplayPageSet.ID;
				}

				return id;
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		public static void SaveDisplayPageSet(DisplayPageSet DisplayPageSet)
		{
			try
			{
				DisplayPageSet ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.DisplayPageSet
						  where s.ID == DisplayPageSet.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in DisplayPageSet.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(DisplayPageSet, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		/// <summary>
		/// delets all objects, pages and the pageset itself
		/// </summary>
		/// <param name="DisplayPageSetID"></param>
		public static void DelDisplayPageSet(long DisplayPageSetID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				// del pages
				DelDisplayPagesForPageSet(DisplayPageSetID);

				// del pageset
				DisplayPageSet el = (from s in FSBDB.DisplayPageSet
									 where s.ID == DisplayPageSetID
									 select s).FirstOrDefault();
				FSBDB.DisplayPageSet.Remove(el);
				FSBDB.SaveChanges();
			}
		}

		// Page
		public static List<DisplayPage> DisplayPagesForPageSet(long PageSetID)
		{
			List<DisplayPage> lst = new List<DisplayPage>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.DisplayPage
					   where s.PageSetNo == PageSetID
					   orderby s.Sort
					   select s).ToList();
			}

			return lst;
		}

		public static DisplayPage DisplayPage(int id)
		{
			DisplayPage res = new DisplayPage();
			using (fsbDB FSBDB = new fsbDB())
			{
				res = (from s in FSBDB.DisplayPage
					   where s.ID == id
					   select s).FirstOrDefault();
			}

			return res;
		}

		public static void AddDisplayPage(DisplayPage DisplayPage)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					FSBDB.DisplayPage.Add(DisplayPage);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void SaveDisplayPage(DisplayPage DisplayPage)
		{
			try
			{
				DisplayPage ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.DisplayPage
						  where s.ID == DisplayPage.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in DisplayPage.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(DisplayPage, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void DelDisplayPage(long DisplayPageID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				// del objects
				DelDisplayObjectsForPage(DisplayPageID);

				// del page
				DisplayPage el = (from s in FSBDB.DisplayPage
								  where s.ID == DisplayPageID
								  select s).FirstOrDefault();
				FSBDB.DisplayPage.Remove(el);
				FSBDB.SaveChanges();
			}
		}

		public static void DelDisplayPagesForPageSet(long PageSetID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				List<DisplayPage> ol = DisplayPagesForPageSet(PageSetID);

				for (int i = 0; i < ol.Count; i++)
				{
					// del DisplayPage
					DelDisplayPage((int)ol[i].ID);
				}
			}
		}

		// Object
		public static List<DisplayObject> DisplayObjectsForPage(long PageID)
		{
			List<DisplayObject> lst = new List<DisplayObject>();
			using (fsbDB FSBDB = new fsbDB())
			{
				lst = (from s in FSBDB.DisplayObject
					   where s.PageNo == PageID
					   orderby s.Zindex
					   select s).ToList();
			}

			return lst;
		}

		public static DisplayObject DisplayObject(long id)
		{
			DisplayObject res = new DisplayObject();
			using (fsbDB FSBDB = new fsbDB())
			{
				res = (from s in FSBDB.DisplayObject
					   where s.ID == id
					   select s).FirstOrDefault();
			}

			return res;
		}

		public static void AddDisplayObject(DisplayObject DisplayObject)
		{
			try
			{
				using (fsbDB FSBDB = new fsbDB())
				{
					FSBDB.DisplayObject.Add(DisplayObject);
					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}
		public static void SaveDisplayObject(DisplayObject DisplayObject)
		{
			try
			{
				DisplayObject ma;

				using (fsbDB FSBDB = new fsbDB())
				{
					ma = (from s in FSBDB.DisplayObject
						  where s.ID == DisplayObject.ID
						  select s).FirstOrDefault();

					foreach (PropertyInfo pi in DisplayObject.GetType().GetProperties())
					{
						if (pi.CanWrite)
						{
							pi.SetValue(ma, pi.GetValue(DisplayObject, null), null);
						}
					}

					FSBDB.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public static void DelDisplayObject(long DisplayObjectID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				// del object
				DisplayObject el = (from s in FSBDB.DisplayObject
									where s.ID == DisplayObjectID
									select s).FirstOrDefault();
				FSBDB.DisplayObject.Remove(el);
				FSBDB.SaveChanges();
			}
		}

		public static void DelDisplayObjectsForPage(long PageID)
		{
			using (fsbDB FSBDB = new fsbDB())
			{
				List<DisplayObject> ol = DisplayObjectsForPage(PageID);

				for (int i = 0; i < ol.Count; i++)
				{
					DelDisplayObject((int)ol[i].ID);
				}
			}
		}

	}
}