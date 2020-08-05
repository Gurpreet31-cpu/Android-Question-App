using System;
using System.Collections;

using System.Net;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Android_Question_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity,AdapterCallBack
    {
        RecyclerView mRecyclerView;
        MyAdapter mAdapter;
        TextView alertText;
        TextView srTxt;
        Button searchButton;
        TextView searchTextValue;
        ProgressLoader progressLoader;
        ArrayList arrayList;
        Utils utils;
        AdapterCallBack clickListener;
        HttpClient httpClient;
        WebClient client;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            initializeFunction();
            searchButton.Click += SearchButton_Click;
        }
        
        
        private void initializeFunction()               // To initialize the views
        {

          Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            arrayList = new ArrayList();
            utils = new Utils();
            progressLoader = new ProgressLoader(this);
            httpClient = new HttpClient();
            clickListener = this;
            searchButton = FindViewById<Button>(Resource.Id.search_button);
            srTxt = FindViewById<TextView>(Resource.Id.srtxt);
            alertText = FindViewById<TextView>(Resource.Id.alertText);
            searchTextValue = FindViewById<TextInputEditText>(Resource.Id.textInput1);
            

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.subreddit__list);
            var layoutManager = new LinearLayoutManager(this) { Orientation = LinearLayoutManager.Vertical };
            mRecyclerView.SetLayoutManager(layoutManager);
            mRecyclerView.HasFixedSize = true;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {

            if (searchTextValue.Text.Equals(""))
            {
                Toast.MakeText(this, Resources.GetString(Resource.String.empty_toast), ToastLength.Long).Show();
                progressLoader.hideProgressBar();

            }
            else
            {
                if (utils.CheckForInternetConnection())
                {
                    progressLoader.showProgressBar();
                    getApiResultAsync();
                }
                else
                {
                    Toast.MakeText(this, Resources.GetString(Resource.String.net_alert), ToastLength.Long).Show();
                }
            }
     
        }

        private async void getApiResultAsync()                       // Async call using HttpClient to get the Json response
        {

                var response =await httpClient.GetAsync(utils.base_url + searchTextValue.Text);
                

                if (response.IsSuccessStatusCode)
                {

                    var responseData = response.Content;
                   
                 
                   var subreddits = responseData.ReadAsStringAsync().Result;
                 
                    JObject jContent = (JObject)JsonConvert.DeserializeObject<JObject>(subreddits);
                
                    setUI(jContent);
                    progressLoader.hideProgressBar();
                }
           
        }

        private void setUI(JObject subreddits)                 // Set user interface (Recyclerview or adapter) after getting Json response
        {
 
            arrayList.Clear();


            foreach (var subreddit in subreddits["data"]["children"] as JArray)
            {
                var name = subreddit["data"]["display_name"].ToString();

                arrayList.Add(name);

            }
            if (arrayList.Count > 0)
            {
           
                alertText.Visibility = ViewStates.Gone;
                mRecyclerView.Visibility = ViewStates.Visible;
                srTxt.Visibility = ViewStates.Visible;

            }
            else
            {

                alertText.Visibility = ViewStates.Visible;
                mRecyclerView.Visibility = ViewStates.Gone;
                srTxt.Visibility = ViewStates.Gone;

            }

            // Plug in my adapter:
            mAdapter = new MyAdapter(arrayList, this, mRecyclerView,clickListener);
            mRecyclerView.SetAdapter(mAdapter);
         
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async void onMethodCallbackAsync(string subredditName)           // shows the sidebar of a subreddit in the list, when clicked
        {

            if (utils.CheckForInternetConnection())
            {
                progressLoader.showProgressBar();
                client = new WebClient();
             
                string uri = "https://www.reddit.com/r/" + subredditName + "/about/sidebar";
                var sidebarHtml = await client.DownloadStringTaskAsync(uri);
              
                var intent = new Intent(this, typeof(SidebarActivity));
                intent.PutExtra("sidebarHtml", sidebarHtml);
                StartActivity(intent);
                progressLoader.hideProgressBar();
         
            }
            else
            {
                
                Toast.MakeText(this, Resources.GetString(Resource.String.net_alert), ToastLength.Short).Show();
            }
           
        }
    }
}

