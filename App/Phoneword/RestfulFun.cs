using System;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;
using Flurl.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Text;
using Android.Locations;

namespace Phoneword
{
    class RestfulFun
    {
       public static string GetLocationJSON(double lat, double lon)
        {
            Place place = new Phoneword.Place();
            place.location.Add("latitude", lat);
            place.location.Add("longitude", lon);

            return place.ToJson();
        }

       

        public static JObject MakeGETRequest(string param)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://serene-island-67380.herokuapp.com/request");

            var postData = param;
            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Console.WriteLine(responseString);

            return JObject.Parse(responseString);

        }
        
    }
}
