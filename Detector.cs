﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooruDownloader
{
    class Detector
    {

        static Dictionary<string, string> engines = new Dictionary<string, string>
        {
            {"safebooru.org", "Gel"},
            {"safebooru.donmai.us", "Dan"},
            {"danbooru.donmai.us", "Dan"},
            {"gelbooru.org", "Gel"},
            {"rule34.xxx", "Gel"}
        };

        public static String detectEngine(String url)
        {
            if (url.StartsWith("http://"))
                url = url.Substring(7);
            if (url.StartsWith("https://"))
                url = url.Substring(8);
            foreach (var pair in engines)
            {
                if(url.StartsWith(pair.Key))
                    return pair.Value;
            }
            return "";
        }
    }
}
