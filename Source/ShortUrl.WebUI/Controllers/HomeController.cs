using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;
using System.Web.Security;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using ShortUrl.Domain.Abstract;
using ShortUrl.Domain.Entities;
using ShortUrl.WebUI.Models;
using ShortUrl.WebUI.Utilities;

using MyUrl = ShortUrl.Domain.Entities.Url;

namespace ShortUrl.WebUI.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Об'єкт сховища
        /// </summary>
        private IUrlRepository _repository;

        /// <summary>
        /// Створює новий об'єкт контролера Home
        /// </summary>
        /// <param name="repository">Об'єкт сховища, що реалізує інтерфейс IUrlRepository</param>
        public HomeController(IUrlRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Викликає представлення Index
        /// </summary>
        /// <returns>Представлення Index</returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecentShortenetUrl()
        {
            var userName = User.Identity.Name;
            var url = _repository.Urls.Where(u => u.UserName == userName)
                .OrderByDescending(u => u.Date).First();

            var model = new UrlViewModel()
            {
                ShortenedUrl = url.ShortenedUrl,
                RealUrl = url.RealUrl,
                //Image = "Image"
            };

            var jsonStr = JsonConvert.SerializeObject(model);

            return Json(jsonStr);
        }

        /// <summary>
        /// Скорочує посилання
        /// </summary>
        /// <param name="realUrl">Справжнє посилання на веб-сторінку</param>
        /// <returns>Коротке значення посилання</returns>
        [HttpPost]
        public JsonResult CreateShortUrl(string realUrl)
        {

            if (realUrl == null)
            {
                realUrl = Request.Form.Get(0);
            }

            //var membershipUser = Membership.GetUser();


            //Створити новий об'єкт Url
            var url = new Url()
            {
                RealUrl = realUrl,
                UserName = User.Identity.Name
            };
            
            //записати створений об'єкт в базу даних
            _repository.AddNewUrl(url);

            var link = "http://" + ConfigurationManager.AppSettings["DomainName"] + "/" + url.ShortenedUrl;

            //повернути коротке значення посилання
            return Json(link);
        }

        

        /// <summary>
        /// Візуалізує щойно скорочене посилання та останні 10 поточного користувача
        /// </summary>
        /// <param name="realUrl">Справжнє посилання для скорочення</param>
        /// <returns>Html розмітку відображення інформації про скорочені посилання</returns>
        public ActionResult UrlsView(string realUrl = null)
        {
            var userName = User.Identity.IsAuthenticated
                ? User.Identity.Name
                : "";

            var listModel = new List<UrlViewModel>();

            if (realUrl == null){
                if (User.Identity.IsAuthenticated)
                {
                    var urls = _repository.Urls.Where(u => u.UserName == userName)
        .OrderByDescending(u => u.Date).Take(10);
                    foreach (var u in urls)
                    {
                        //var image = u.Image == null ? "" : String.Format("data:image/Bmp;base64,{0}", Convert.ToBase64String(u.Image));
                        listModel.Add(
                            new UrlViewModel
                            {
                                ShortenedUrl = u.ShortenedUrl,
                                RealUrl = u.RealUrl,
                                Image = u.Image,
                                RecentlyAdded = false
                            });
                    }
                    return View(listModel);

                }
                else
                {
                    return View();
                }
            }

            var im = Thumbnail.CreateThumbnailImage(realUrl, 120, 80);


            var myu = new MyUrl ()
            {
                UserName = userName,
                Image = im,
                RealUrl = realUrl,
            };

            _repository.AddNewUrl(myu);


            //------------------------------------------

            listModel.Add(
                new UrlViewModel
                {
                    ShortenedUrl = myu.ShortenedUrl,
                    RealUrl = myu.RealUrl,
                    Image = im,
                    RecentlyAdded = true
                });



            if (User.Identity.IsAuthenticated)
            {
                var urls = _repository.Urls.Where(u => u.UserName == userName)
                    .OrderByDescending(u => u.Date).Skip(1).Take(10);

                foreach (var u in urls)
                {
                    listModel.Add(
                        new UrlViewModel
                        {
                            ShortenedUrl = u.ShortenedUrl,
                            RealUrl = u.RealUrl,
                            Image = u.Image,
                            RecentlyAdded = false
                        });
                }
            }


            return View(listModel);
        }

        /// <summary>
        /// Перенаправлює на справжню веб-сторінку скороченого посилання str
        /// </summary>
        /// <param name="str">Скорочений вигляд посилання</param>
        /// <returns>Перенаправлення на потрібну веб-сторінку</returns>
        public ActionResult Redirection(string str)
        {
            var realUrl = _repository.Urls.FirstOrDefault(u => u.ShortenedUrl == str).RealUrl;

            return RedirectPermanent(realUrl);
        }

        public ActionResult ForTest()
        {
            var urls = new Url []
            {
                new Url("11111") {RealUrl = "RealUrl1", UserName = "Rosga"},
                new Url("22222") {RealUrl = "RealUrl2", UserName = "Rosga"},
                new Url("33333") {RealUrl = "RealUrl3", UserName = "Rosga"}
            };

            var json = JsonConvert.SerializeObject(urls);

            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            foreach (var url in urls)
            {
                sw.Write(url);
            }

            //var file = urls.

            return Json(json);
        }
    }
}
