using System;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Phoneword
{
    class RestfulFun
    {
       public static JObject makeGETrequest(string location)
        {
            Console.WriteLine("YOOO");
            WebRequest wrGETURL = WebRequest.Create(location);
            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            string line = "";
           
            do
            {
                sLine = objReader.ReadLine();
                line += sLine;
            } while (sLine != null);

       

            return JObject.Parse(line);
        
        }
        
    }
}
