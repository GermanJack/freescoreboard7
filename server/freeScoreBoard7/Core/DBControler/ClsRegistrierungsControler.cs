using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Interpreter.Error;
using System;
using System.Linq;
using System.Reflection;

namespace FreeScoreBoard.Core.DBControler
{
    public static class ClsRegistrierungsControler
    {
        private static readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static Registrierung registrierung()
        {
            try
            {
                Registrierung rg;

                using (fsbDB FSBDB = new fsbDB())
                {
                    rg = (from x in FSBDB.Registrierung
                          select x).FirstOrDefault();
                }

                return rg;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return new Registrierung();
            }
        }

        public static void SaveRegistrierung(Registrierung registrierung)
        {
            try
            {
                Registrierung rg;

                using (fsbDB FSBDB = new fsbDB())
                {
                    rg = (from x in FSBDB.Registrierung
                          where x.ID == registrierung.ID
                          select x).FirstOrDefault();

                    foreach (PropertyInfo pi in registrierung.GetType().GetProperties())
                    {
                        if (pi.CanWrite)
                        {
                            pi.SetValue(rg, pi.GetValue(registrierung, null), null);
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

        public static int AddRegistrierung(Registrierung registrierung)
        {
            try
            {
                using (fsbDB FSBDB = new fsbDB())
                {
                    long newID = (from x in FSBDB.Registrierung select x.ID).DefaultIfEmpty(0).Max() + 1;
                    registrierung.ID = newID;
                    FSBDB.Registrierung.Add(registrierung);
                    FSBDB.SaveChanges();
                }

                return (int)registrierung.ID;
            }
            catch (Exception ex)
            {
                ClsError.CoreError(Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return 0;
            }
        }
    }
}
