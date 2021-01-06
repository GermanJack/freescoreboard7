using System;

namespace FreeScoreBoard.Classes
{
	public static class ClsGlobalEvents
	{
		//public static event OwnObjectEventHandler DisplayProfileChanged;
		//public static event OwnObjectEventHandler DisplayPageChanged;
		public static event OwnObjectEventHandler DisplayPropChanged;
		public static event EventHandler ToogleDisplay;
		public static event OwnStringEventHandler PlayAudio;
		//public static event EventHandler Timer_Tick;

		public static void Fire_ToogleDisplay(object sender, EventArgs args)
		{
			EventHandler evt = ToogleDisplay;
			if (evt != null)
			{
				evt(sender, args);
			}
		}

		public static void Fire_PlayAudio(object sender, ClsStringEventArgs args)
		{
			OwnStringEventHandler evt = PlayAudio;
			if (evt != null)
			{
				evt(sender, args);
			}
		}

		/// <summary>
		/// Soll benutzt werden wenn die Anzeige position / größe geändert wird
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public static void Fire_DisplayPropChanged(object sender, ClsObjectEventArgs args)
		{
			OwnObjectEventHandler evt = DisplayPropChanged;
			if (evt != null)
			{
				evt(sender, args);
			}
		}
	}
}
