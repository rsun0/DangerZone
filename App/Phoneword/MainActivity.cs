#define TRACE
using System;
using System.Diagnostics;

using Newtonsoft.Json.Linq;
using System.Drawing;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
	[Activity (Label = "Phoneword", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{

          
            base.OnCreate (bundle);


          

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Get our UI controls from the loaded layout

            Button JSONButton = FindViewById<Button>(Resource.Id.JSONButton);
            Button MapButton = FindViewById<Button>(Resource.Id.switch2);
            
            TextView text = FindViewById<TextView>(Resource.Id.json);
            ImageView image = FindViewById<ImageView>(Resource.Id.imageView1);

        
            JObject locationDanger; 


            JSONButton.Click += (object sender, EventArgs e) =>
            {
                JSONButton.Text = "FUCK";
                Console.WriteLine("Here2!");
                locationDanger = RestfulFun.makeGETrequest("https://serene-island-67380.herokuapp.com/request");
                text.Text = locationDanger.ToString();
            };

            MapButton.Click += (object sender, EventArgs e) =>
            {
                SetContentView(Resource.Layout.Map);
                Button CompassButton = FindViewById<Button>(Resource.Id.switch3);
                CompassButton.Click += (object s, EventArgs en) =>
                {
                    SetContentView(Resource.Layout.Main);
                };

            };

            

        }
	}

    
}