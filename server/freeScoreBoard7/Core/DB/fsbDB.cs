namespace FreeScoreBoard.Core.DB
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity;
	using System.Linq;

	public partial class fsbDB : DbContext
	{
		public fsbDB()
			: base("name=fsbDB")
		{
		}

		public virtual DbSet<Anzeigetabelle> Anzeigetabelle { get; set; }
		public virtual DbSet<EreignissTyp> EreignissTyp { get; set; }
		public virtual DbSet<HotKey> HotKey { get; set; }
		public virtual DbSet<Kontrols> Kontrols { get; set; }
		public virtual DbSet<Mannschaften> Mannschaften { get; set; }
		public virtual DbSet<Options1> Options1 { get; set; }
		public virtual DbSet<Options2> Options2 { get; set; }
		public virtual DbSet<Options21> Options21 { get; set; }
		public virtual DbSet<Options3> Options3 { get; set; }
		public virtual DbSet<Registrierung> Registrierung { get; set; }
		public virtual DbSet<Spieler> Spieler { get; set; }
		public virtual DbSet<Strafen> Strafen { get; set; }
		public virtual DbSet<TabellenSort> TabellenSort { get; set; }
		public virtual DbSet<TEreignisse> TEreignisse { get; set; }
		public virtual DbSet<TGruppen> TGruppen { get; set; }
		public virtual DbSet<Timer> Timer { get; set; }
		public virtual DbSet<Timerevent> Timerevent { get; set; }
		public virtual DbSet<TKopf> TKopf { get; set; }
		public virtual DbSet<TRunden> TRunden { get; set; }
		public virtual DbSet<TSpiele> TSpiele { get; set; }
		public virtual DbSet<TTabellen> TTabellen { get; set; }
		public virtual DbSet<Variablen> Variablen { get; set; }
		public virtual DbSet<Version> Version { get; set; }
		public virtual DbSet<User> User { get; set; }
		public virtual DbSet<WebKontrols> WebKontrols { get; set; }
		public virtual DbSet<A_Text> A_Text { get; set; }
		public virtual DbSet<DisplayObject> DisplayObject { get; set; }
		public virtual DbSet<DisplayPage> DisplayPage { get; set; }
		public virtual DbSet<DisplayPageSet> DisplayPageSet { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		}
	}
}
