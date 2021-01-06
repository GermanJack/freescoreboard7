using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Core.DBControler
{
    class ClsTimerControler
    {
        private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static List<DB.Timer> Timers()
        {
            try
            {
                List<DB.Timer> tel;

                using (fsbDB FSBDB = new fsbDB())
                {
                    tel = (from x in FSBDB.Timer select x).ToList();
                }

                if (tel == null)
                {
                    return new DB.Timer[0].ToList();
                }
                else
                {
                    return tel;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new DB.Timer[0].ToList();
            }
        }

        public static DB.Timer Timer(int id)
        {
            try
            {
                DB.Timer te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timer
                          where x.ID == id
                          select x).FirstOrDefault();
                }

                return te;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new DB.Timer();
            }
        }

        public static int TimerID(int TimerNr)
        {
            try
            {
                DB.Timer te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timer
                          where x.Nr == TimerNr
                          select x).FirstOrDefault();
                }

                return (int)te.ID;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return 0;
            }
        }

        public static void DelTimer(int id)
        {
            try
            {
                DB.Timer te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timer
                          where x.ID == id
                          select x).FirstOrDefault();

                    FSBDB.Timer.Remove(te);
                    FSBDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public static void SaveTimer(DB.Timer timer)
        {
            try
            {
                DB.Timer te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timer
                          where x.ID == timer.ID
                          select x).FirstOrDefault();

                    foreach (PropertyInfo pi in timer.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(te, pi.GetValue(timer, null), null);
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

        public static int AddTimer(DB.Timer timer)
        {
            try
            {
                using (fsbDB FSBDB = new fsbDB())
                {
                    long newID = (from x in FSBDB.Timer select x.ID).DefaultIfEmpty(0).Max() + 1;
                    timer.ID = newID;
                    FSBDB.Timer.Add(timer);
                    FSBDB.SaveChanges();
                    return (int)timer.ID;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return 0;
            }
        }

        // -----------Timerevents-----------------------
        public static List<Timerevent> TimerEvents()
        {
            try
            {
                List<Timerevent> tel;

                using (fsbDB FSBDB = new fsbDB())
                {
                    tel = (from x in FSBDB.Timerevent select x).ToList();
                }

                if (tel == null)
                {
                    return new Timerevent[0].ToList();
                }
                else
                {
                    return tel;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Timerevent[0].ToList();
            }
        }

        public static List<Timerevent> TimerEvents(int TimerNr)
        {
            try
            {
                List<Timerevent> tel;

                using (fsbDB FSBDB = new fsbDB())
                {
                    tel = (from x in FSBDB.Timerevent where x.TimerNr == TimerNr select x).ToList();
                }

                if (tel == null)
                {
                    return new Timerevent[0].ToList();
                }
                else
                {
                    return tel;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Timerevent[0].ToList();
            }
        }

        public static List<Timerevent> TimerEvents(int TimerNr, string order = "ASC")
        {
            try
            {
                List<Timerevent> tel;

                using (fsbDB FSBDB = new fsbDB())
                {
                    if (order == "DESC")
                    {
                        tel = (from x in FSBDB.Timerevent where x.TimerNr == TimerNr orderby x.Sekunden descending, x.Eventtype descending select x).ToList();
                    }
                    else
                    {
                        tel = (from x in FSBDB.Timerevent where x.TimerNr == TimerNr orderby x.Sekunden ascending, x.Eventtype descending select x).ToList();
                    }
                }

                if (tel == null)
                {
                    return new Timerevent[0].ToList();
                }
                else
                {
                    return tel;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Timerevent[0].ToList();
            }
        }

        public static List<Timerevent> AktiveTimerEvents(int TimerNr, string order = "ASC")
        {
            try
            {
                List<Timerevent> tel;

                using (fsbDB FSBDB = new fsbDB())
                {
                    if (order == "DESC")
                    {
                        tel = (from x in FSBDB.Timerevent where x.TimerNr == TimerNr && x.Active == true orderby x.Sekunden descending, x.Eventtype descending select x).ToList();
                    }
                    else
                    {
                        tel = (from x in FSBDB.Timerevent where x.TimerNr == TimerNr && x.Active == true orderby x.Sekunden ascending, x.Eventtype descending select x).ToList();
                    }
                }

                if (tel == null)
                {
                    return new Timerevent[0].ToList();
                }
                else
                {
                    return tel;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Timerevent[0].ToList();
            }
        }

        public static Timerevent TimerEvent(int ID)
        {
            try
            {
                Timerevent te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timerevent
                          where x.ID == ID
                          select x).FirstOrDefault();
                }

                return te;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Timerevent();
            }
        }

        public static void DelTimerEvent(int ID)
        {
            try
            {
                Timerevent te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timerevent
                          where x.ID == ID
                          select x).FirstOrDefault();

                    FSBDB.Timerevent.Remove(te);
                    FSBDB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public static void SaveTimerEvent(Timerevent timerevent)
        {
            try
            {
                Timerevent te;

                using (fsbDB FSBDB = new fsbDB())
                {
                    te = (from x in FSBDB.Timerevent
                          where x.ID == timerevent.ID
                          select x).FirstOrDefault();

                    foreach (PropertyInfo pi in timerevent.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(te, pi.GetValue(timerevent, null), null);
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

        public static int AddTimerEvent(Timerevent timerevent)
        {
            try
            {
                using (fsbDB FSBDB = new fsbDB())
                {
                    long newID = (from x in FSBDB.Timerevent select x.ID).DefaultIfEmpty(0).Max() + 1;
                    timerevent.ID = newID;
                    FSBDB.Timerevent.Add(timerevent);
                    FSBDB.SaveChanges();
                    return (int)timerevent.ID;
                }
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return 0;
            }
        }
    }
}
