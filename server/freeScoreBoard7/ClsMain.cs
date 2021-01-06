using System;
using System.ComponentModel;
using System.Reflection;

namespace FreeScoreBoard
{
	public sealed class ClsMain
	{
		public static string ExeStartupFolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static string AppFolder { set; get; }
		public static string WebFolder { set; get; }
		public static string DisplayFolder { set; get; }
		public static string ControlFolder { set; get; }
		public static string PictureFolder { set; get; }
		public static string AudioFolder { set; get; }
		public static string SlideshowFolder { set; get; }
		public static string FontFolder { set; get; }
		public static string CSSFolder { set; get; }
		public static string TPMLFolder { set; get; }
		public static string WebDesignsFolder { set; get; }

		private static readonly System.Lazy<ClsMain> Lazy = new System.Lazy<ClsMain>(() => new ClsMain());

		public static ClsMain Instance
		{
			get { return Lazy.Value; }
		}

		//private readonly string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

	}
}
