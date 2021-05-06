using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Content;

namespace ShopAssist
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button shoplistbtn;
        Button shopmapbtn;
        Button shopplanbtn;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Routing:
            shoplistbtn = FindViewById<Button>(Resource.Id.shoplistbtn);
            // groplistbtn = FindViewById<Button>(Resource.Id.groplistbtn);     // Bundelen met shoppinglist activity, omdat hetzelfde is en in de activity gebruikt kan worden.
            shopmapbtn = FindViewById<Button>(Resource.Id.shopmapbtn);
            shopplanbtn = FindViewById<Button>(Resource.Id.shopplanbtn);

            shoplistbtn.Click += shoplistbtn_Click;
            shopmapbtn.Click += shopmapbtn_Click;
            shopplanbtn.Click += shopplanbtn_Click;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private void shoplistbtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ShoppingListActivity)); // Specify Activity
            // Mogelijk actie uitvoeren zoals db querie voor laatste shoppinglist
            StartActivity(intent);
        }
        private void shopmapbtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ShoppingMapActivity)); // Specify Activity
            StartActivity(intent);
        }
        private void shopplanbtn_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ShoppingPlannerActivity)); // Specify Activity
            StartActivity(intent);
        }
    }
}