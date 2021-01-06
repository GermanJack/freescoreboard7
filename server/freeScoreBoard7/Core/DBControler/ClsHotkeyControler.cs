using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FreeScoreBoard.Core.DBControler
{
    public static class ClsHotkeyControler
    {
        private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static List<HotKey> hotkeys()
        {
            List<HotKey> lst = new List<HotKey>();
            using (fsbDB FSBDB = new fsbDB())
            {
                lst = (from s in FSBDB.HotKey
                       orderby s.Sort
                       select s).ToList();
            }

            return lst;
        }

        public static HotKey hotkey(int id)
        {
            HotKey res = new HotKey();
            using (fsbDB FSBDB = new fsbDB())
            {
                res = (from s in FSBDB.HotKey
                       where s.ID == id
                       select s).FirstOrDefault();
            }

            return res;
        }

        public static void SaveHotKey(HotKey hotkey)
        {
            try
            {
                HotKey ma;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ma = (from s in FSBDB.HotKey
                          where s.ID == hotkey.ID
                          select s).FirstOrDefault();

                    foreach (PropertyInfo pi in hotkey.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(ma, pi.GetValue(hotkey, null), null);
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
    }
}
