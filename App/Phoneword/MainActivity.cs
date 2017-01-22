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
using Android.Locations;
using Android.Gms.Maps;
using System.Net;
using System.IO;

namespace Phoneword
{


    [Activity(Label = "Danger Zone", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {


            base.OnCreate(bundle);

         //   Console.WriteLine(RestfulFun.GetLocationJSON(41.870480, -87.652329));



            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our UI controls from the loaded layout
            RelativeLayout layout = FindViewById<RelativeLayout>(Resource.Id.compass);
            Button JSONButton = FindViewById<Button>(Resource.Id.JSONButton);
            Button MapButton = FindViewById<Button>(Resource.Id.switch2);

            TextView text = FindViewById<TextView>(Resource.Id.json);



            JObject locationDanger;

            double lat = 41.783401;
            double lon = -87.639758;


            JSONButton.Click += (object sender, EventArgs e) =>
            {

                MakeCompass(lat, lon, layout, text);
                


            };


            MapButton.Click += (object sender, EventArgs e) =>
            {
                SetContentView(Resource.Layout.Map);

                Button CompassButton = FindViewById<Button>(Resource.Id.switch3);
                CompassButton.Click += (object s, EventArgs en) =>
                {
                    SetContentView(Resource.Layout.Main);
                    OnCreate(bundle);
                };

            };



        }


        private void MakeCompass(double lat, double lon, RelativeLayout layout, TextView text)
        {
            JObject locationDanger;
          //  JSONButton.Text = "FUCK";
            Console.WriteLine("Here2!");
            locationDanger = RestfulFun.MakeGETRequest(RestfulFun.GetLocationJSON(lat, lon));
            lat += .001;
            lon += .001;

            //   double danger = (double)locationDanger["danger"]["north"];

            for (int i = 0; i != 3; i++)
            {
                layout.AddView(DrawGodamnArrow(i, locationDanger, layout));
            }
            text.Text = "North is " + Math.Round((double)locationDanger["danger"]["north"], 2) + "x dangerous," + "\n"
               + "West is " + Math.Round((double)locationDanger["danger"]["west"], 1) + "x dangerous," + "\n"
               + "East is " + Math.Round((double)locationDanger["danger"]["north"], 1) + "x dangerous, and" + "\n"
               + "South is " + Math.Round((double)locationDanger["danger"]["north"], 1) + "x dangerous" + "\n"
               + "than the average spot in Chicago";

        }

        private ImageView DrawGodamnArrow(int direction, JObject locationDanger, RelativeLayout layout)
        {
            ImageView arrow = new ImageView(this);
            arrow.SetMaxHeight(-1);
            arrow.SetMaxWidth(-1);
            //     arrow.SetForegroundGravity(GravityFlags.CenterHorizontal);


            Console.WriteLine("Error 1");

            switch (direction)
            {
                case 0:
                    double top = (double)locationDanger["danger"]["north"];
                    if (top < 1)
                    {

                        arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.UG));
                    }
                    else
                    {
                        arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.UR));
                    }
                    break;
                case 1:
                    double west = (double)locationDanger["danger"]["west"];
                    double here = (double)locationDanger["danger"]["here"];
                    double east = (double)locationDanger["danger"]["east"];

                    if (west < 1)
                    {
                        if (here < 1)
                        {
                            if (east < 1)
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.GGG));
                            }
                            else
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.GGR));
                            }
                        }
                        else
                        {
                            if (east < 1)
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.GRG));
                            }
                            else
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.GRR));
                            }
                        }
                    }
                    else
                    {
                        if (here < 1)
                        {
                            if (east < 1)
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.RGG));
                            }
                            else
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.RGR));
                            }
                        }
                        else
                        {
                            if (east < 1)
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.RRG));
                            }
                            else
                            {
                                arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.RRR));
                            }
                        }
                    }
                    break;
                case 2:
                    double bot = (double)locationDanger["danger"]["south"];
                    if (bot < 1)
                    {

                        arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.DG));
                    }
                    else
                    {
                        arrow.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.DR));
                    }
                    break;


            }


            Console.WriteLine("Error 2");

            Console.WriteLine("Error 3");
            return arrow;
        }

    }

    }