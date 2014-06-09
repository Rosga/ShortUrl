using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace ShortUrl.WebUI.Models
{
    public class UrlViewModel
    {
        private string _shortenedUrl;
        public string RealUrl { get; set; }
        public string ShortenedUrl
        {
            get { return _shortenedUrl; }
            set { _shortenedUrl = "http://" + ConfigurationManager.AppSettings["DomainName"] + "/" + value; }
        }
        //public string Image { get; set; }
        public byte[] Image { get; set; }

        public bool RecentlyAdded { get; set; }
    }
}