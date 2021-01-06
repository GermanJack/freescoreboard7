using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace FreeScoreBoard.Forms.Controls
{
	public partial class CtrlBrowser : UserControl
	{
		private ChromiumWebBrowser chromeBrowser;
		//private WebFunctions WebFunctions = new WebFunctions();
		private bool allowZoom = false;
		private string url = "";

		public CtrlBrowser(bool DevTools = false)
		{
			this.InitializeComponent();

			//this.WebFunctions.ZoomIn += new EventHandler(this.ZoomIn);
			//this.WebFunctions.ZoomOut += new EventHandler(this.ZoomOut);
			//this.WebFunctions.DevTools += new EventHandler(this.ShowDevTools);


			if (!Cef.IsInitialized)
			{
				CefSettings settings = new CefSettings();
				//settings.BrowserSubprocessPath = @"x86\CefSharp.BrowserSubprocess.exe";
				settings.UserAgent = "CEF";
				settings.Locale = "de";
				settings.CefCommandLineArgs.Add("mute-audio", "false");
				settings.CefCommandLineArgs.Add("enable-media-stream", "1");
				settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
				Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
				//Cef.Initialize(settings);
			}

			this.InitializeChromium();
			this.chromeBrowser.BrowserSettings.WebSecurity = CefState.Disabled;
			CefSharpSettings.LegacyJavascriptBindingEnabled = true;
			//this.chromeBrowser.RegisterJsObject("nativeHost", this.WebFunctions);
			//this.chromeBrowser.RequestHandler = new MyRequestHandler("Jonas", "Test");

			//DownloadHandler downer = new DownloadHandler(this);
			//this.chromeBrowser.DownloadHandler = downer;
			//downer.OnBeforeDownloadFired += OnBeforeDownloadFired;
			//downer.OnDownloadUpdatedFired += OnDownloadUpdatedFired;




			if (DevTools)
			{
				this.BtnR.Visible = true;
				this.BtnD.Visible = true;
			}
		}

		public bool AllowZoom
		{
			get
			{
				return this.allowZoom;
			}

			set
			{
				this.allowZoom = value;
			}
		}

		public string URL
		{
			get
			{
				return this.url;
			}

			set
			{
				this.url = value;
				this.RefreshBrowser();
			}
		}

		private void InitializeChromium()
		{
			// Create a browser component
			this.chromeBrowser = new ChromiumWebBrowser(this.url);
			// Add it to the form and fill it to the form window.
			this.Controls.Add(this.chromeBrowser);
			this.chromeBrowser.Dock = DockStyle.Fill;
		}

		public void RefreshBrowser()
		{

			this.chromeBrowser.Load(this.url);
			if (this.chromeBrowser.IsBrowserInitialized)
			{
				this.chromeBrowser.SetZoomLevel(0);
			}
		}

		public void ShowDevTools(object sender, EventArgs e)
		{
			this.chromeBrowser.ShowDevTools();
		}


		private async void ZoomIn(object sender, EventArgs e)
		{
			if (!this.allowZoom)
			{
				return;
			}

			double zl = await this.chromeBrowser.GetZoomLevelAsync();
			if (zl > -10)
			{
				this.chromeBrowser.SetZoomLevel(zl - 0.1);
			}
		}

		private async void ZoomOut(object sender, EventArgs e)
		{
			if (!this.allowZoom)
			{
				return;
			}

			double zl = await this.chromeBrowser.GetZoomLevelAsync();
			if (zl < 10)
			{
				this.chromeBrowser.SetZoomLevel(zl + 0.1);
			}
		}

		private void CtrlBrowser_ControlRemoved(object sender, ControlEventArgs e)
		{
			this.chromeBrowser.Dispose();
			Cef.Shutdown();
		}

		private void BtnR_Click(object sender, EventArgs e)
		{
			this.RefreshBrowser();
		}

		private void BtnD_Click(object sender, EventArgs e)
		{
			this.ShowDevTools(null, null);
		}

		//private void OnBeforeDownloadFired(object sender, DownloadItem e)
		//{
		//	this.UpdateDownloadAction("OnBeforeDownload", e);
		//}

		//private void OnDownloadUpdatedFired(object sender, DownloadItem e)
		//{
		//	this.UpdateDownloadAction("OnDownloadUpdated", e);
		//}

		//private void UpdateDownloadAction(string downloadAction, DownloadItem downloadItem)
		//{
		//}
	}

	//public class WebFunctions
	//{
	//	public event EventHandler ZoomIn;
	//	public event EventHandler ZoomOut;
	//	public event EventHandler DevTools;

	//	public void Zoom(string direction)
	//	{
	//		if (direction == "up")
	//		{
	//			this.ZoomIn?.Invoke(this, null);
	//		}

	//		if (direction == "down")
	//		{
	//			this.ZoomOut?.Invoke(this, null);
	//		}
	//	}

	//	public void Devtools()
	//	{
	//		this.DevTools?.Invoke(this, null);
	//	}
	//}

	////public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser, string originUrl, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
	////{
	////    callback.Continue(userName, password);

	////    return true;
	////}


	//public class DownloadHandler : IDownloadHandler
	//{
	//	public event EventHandler<DownloadItem> OnBeforeDownloadFired;

	//	public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

	//	CtrlBrowser mainForm;

	//	public DownloadHandler(CtrlBrowser form)
	//	{
	//		mainForm = form;
	//	}

	//	public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
	//	{
	//		var handler = OnBeforeDownloadFired;
	//		if (handler != null)
	//		{
	//			handler(this, downloadItem);
	//		}

	//		if (!callback.IsDisposed)
	//		{
	//			using (callback)
	//			{
	//				callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
	//			}
	//		}
	//	}

	//	public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
	//	{
	//		var handler = OnDownloadUpdatedFired;
	//		if (handler != null)
	//		{
	//			handler(this, downloadItem);
	//		}
	//	}
	//}
}
