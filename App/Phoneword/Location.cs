using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Phoneword
{
    class Place
    {
        public Dictionary<string, double> location   { get; set; }

        public Place()
        {
            location = new Dictionary<string, double>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}