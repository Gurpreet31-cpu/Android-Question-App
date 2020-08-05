
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace Android_Question_App
{
    [Activity(Label = "SidebarActivity")]
    public class SidebarActivity : Activity
    {
        WebView webView;
        string sidebarHtml;
        Utils utils;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
             sidebarHtml = Intent.Extras.GetString("sidebarHtml");
            utils = new Utils();
       
            if (utils.CheckForInternetConnection())
            {
                webView = new WebView(this);
                webView.Settings.JavaScriptEnabled = true;
                webView.Settings.BuiltInZoomControls = true;
                webView.Settings.SetSupportZoom(true);
                webView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
                webView.ScrollbarFadingEnabled = false;
                webView.SetWebViewClient(new MyWebViewClient());
                AddContentView(webView, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.MatchParent));
                webView.LoadData(sidebarHtml, "text/html", "utf-8");
           
            }
            else
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.net_alert), ToastLength.Long).Show();
            }
        }

        private class MyWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                
                view.LoadUrl(request.Url.ToString());
                return true;
            }

        }

        public override void OnBackPressed()
        {
            if (webView.CanGoBack())
            {
                webView.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
           
        }
    }
}