using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
    public static class ClsMannschaftenControler
    {
        private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static DB.Mannschaften Mannschaft(string mannschaftsname)
        {
            try
            {
                DB.Mannschaften lst;

                using (fsbDB FSBDB = new fsbDB())
                {
                    lst = (from s in FSBDB.Mannschaften
                           where s.Name == mannschaftsname
                           select s).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new DB.Mannschaften();
            }
        }

        public static DB.Mannschaften Mannschaft(long MannschaftsID)
        {
            try
            {
                DB.Mannschaften lst;

                using (fsbDB FSBDB = new fsbDB())
                {
                    lst = (from s in FSBDB.Mannschaften
                           where s.ID == MannschaftsID
                           select s).FirstOrDefault();
                }

                return lst;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new DB.Mannschaften();
            }
        }

        public static List<DB.Mannschaften> Mannschaften()
        {
            try
            {
                List<DB.Mannschaften> lst;

                using (fsbDB FSBDB = new fsbDB())
                {
                    lst = (from s in FSBDB.Mannschaften
                           select s).ToList();
                }

                return lst;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new DB.Mannschaften[0].ToList();
            }
        }

        public static void AddMannschaft(DB.Mannschaften mannschaft)
        {
            try
            {
                using (fsbDB FSBDB = new fsbDB())
                {
                    long newID = (from x in FSBDB.Mannschaften select x.ID).DefaultIfEmpty(0).Max() + 1;
                    mannschaft.ID = newID;
                    FSBDB.Mannschaften.Add(mannschaft);
                    FSBDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public static void SaveMannschaft(DB.Mannschaften mannschaft)
        {
            try
            {
                DB.Mannschaften ma;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ma = (from s in FSBDB.Mannschaften
                          where s.ID == mannschaft.ID
                          select s).FirstOrDefault();

                    foreach (PropertyInfo pi in mannschaft.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(ma, pi.GetValue(mannschaft, null), null);
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

        public static void DelMannschaft(string mannschaftsname)
        {
            try
            {
                DB.Mannschaften ma;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ma = (from m in FSBDB.Mannschaften
                          where m.Name == mannschaftsname
                          select m).FirstOrDefault();

                    FSBDB.Mannschaften.Remove(ma);
                    FSBDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public static void DelMannschaft(long mannschaftsID)
        {
            try
            {
                DB.Mannschaften ma;

                using (fsbDB anzeigeDB = new fsbDB())
                {
                    ma = (from m in anzeigeDB.Mannschaften
                          where m.ID == mannschaftsID
                          select m).FirstOrDefault();

                    anzeigeDB.Mannschaften.Remove(ma);
                    anzeigeDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        public static List<Spieler> Spielers()
        {
            try
            {
                List<Spieler> sl;

                using (fsbDB FSBDB = new fsbDB())
                {
                    sl = (from s in FSBDB.Spieler
                          select s).ToList();
                }

                return sl;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Spieler[0].ToList();
            }
        }

        public static Spieler Spieler(int id)
        {
            try
            {
                Spieler ss;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ss = (from s in FSBDB.Spieler
                          where s.ID == id
                          select s).FirstOrDefault();
                }

                return ss;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Spieler();
            }
        }

        public static List<Spieler> Spielers(string mannschaftsname)
        {
            try
            {
                List<Spieler> sl;

                using (fsbDB FSBDB = new fsbDB())
                {
                    long mid = (from s in FSBDB.Mannschaften
                                where s.Name == mannschaftsname
                                select s.ID).FirstOrDefault();

                    sl = (from s in FSBDB.Spieler
                          where s.MannschaftsID == mid
                          select s).ToList();
                }

                return sl;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Spieler[0].ToList();
            }
        }

        public static List<Spieler> Spielers(int mannschaftsID)
        {
            try
            {
                List<Spieler> sl;

                using (fsbDB FSBDB = new fsbDB())
                {
                    long mid = (from s in FSBDB.Mannschaften
                                where s.ID == mannschaftsID
                                select s.ID).FirstOrDefault();

                    sl = (from s in FSBDB.Spieler
                          where s.MannschaftsID == mid
                          select s).ToList();
                }

                return sl;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Spieler[0].ToList();
            }
        }

        public static int AddSpieler(Spieler spieler)
        {
            try
            {
                int sid;

                using (fsbDB FSBDB = new fsbDB())
                {
                    long newID = (from x in FSBDB.Spieler select x.ID).DefaultIfEmpty(0).Max() + 1;
                    spieler.ID = newID;
                    FSBDB.Spieler.Add(spieler);
                    FSBDB.SaveChanges();
                    sid = (int)spieler.ID;
                }

                return sid;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return 0;
            }
        }

        public static void SaveSpieler(Spieler spieler)
        {
            try
            {
                Spieler ss;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ss = (from s in FSBDB.Spieler
                          where s.ID == spieler.ID
                          select s).FirstOrDefault();

                    foreach (PropertyInfo pi in spieler.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(ss, pi.GetValue(spieler, null), null);
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

        public static void DelSpieler(int id)
        {
            try
            {
                Spieler ss;

                using (fsbDB FSBDB = new fsbDB())
                {
                    ss = (from s in FSBDB.Spieler
                          where s.ID == id
                          select s).FirstOrDefault();

                    FSBDB.Spieler.Remove(ss);
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
