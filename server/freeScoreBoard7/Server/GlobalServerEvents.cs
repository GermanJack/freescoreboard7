using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeScoreBoard.Classes;

namespace FreeScoreBoard.Server
{
	public static class GlobalServerEvents
	{
		public static string ServerIP { get; set; }

		public static event OwnStringEventHandler GotMessage;
		public static event OwnStringEventHandler NewClient;
		public static event OwnStringEventHandler ClientClosed;
		public static event OwnStringEventHandler MessageSend;
		public static event EventHandler Timer_Tick;

		public static void Fire_GotMessage(Object sender, ClsStringEventArgs args)
		{
			GotMessage?.Invoke(sender, args);
		}

		public static void Fire_NewClient(Object sender, ClsStringEventArgs args)
		{
			NewClient?.Invoke(sender, args);
		}

		public static void Fire_ClientClosed(Object sender, ClsStringEventArgs args)
		{
			ClientClosed?.Invoke(sender, args);
		}

		public static void SendMessage(Object sender, ClsStringEventArgs args)
		{
			Server.ClsServer.Instance.SendMessage(args.Argument);
			MessageSend?.Invoke(sender, args);
		}

		public static void Fire_Timer_Tick(object sender, EventArgs args)
		{
			Timer_Tick?.Invoke(sender, args);
		}
	}
}
