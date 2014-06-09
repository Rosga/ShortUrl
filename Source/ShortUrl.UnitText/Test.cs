using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShortUrl.Domain.Abstract;
using ShortUrl.Domain.Entities;
using ShortUrl.WebUI.Controllers;

namespace ShortUrl.UnitTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void IsRightRealUrl()
        {
            //------------------------------Організація--------------------------------
            //Створення імітованого сховища
            var mock = new Mock<IUrlRepository>();
            //Заповнення сховища значеннями
            var urls = new Url[]
            {
                new Url {RealUrl = "RealUrl1", ShortenedUrl = "11111"},
                new Url {RealUrl = "RealUrl2", ShortenedUrl = "22222"},
                new Url  {RealUrl = "RealUrl3", ShortenedUrl = "33333"}
            }.AsQueryable();

            mock.Setup(u => u.Urls).Returns(urls);
            //Створення контролера
            var target = new HomeController(mock.Object);

            //-------------------------------Дія----------------------
            //отримати "справжні" посиланн за коротким посиланням
            var shorts = urls.Select(u => u.ShortenedUrl).ToArray();
            var result = new RedirectResult[urls.Count()];
            for (int i = 0; i < urls.Count(); i++)
            {
                result[i] = (RedirectResult) target.Redirection(shorts[i]);
            }

            //--------------------------------Твердження------------------------
            //перевірка на тотожність повернутих "справжніх" посилань
            Assert.AreEqual("RealUrl1", result[0].Url);
            Assert.AreEqual("RealUrl2", result[1].Url);
            Assert.AreEqual("RealUrl3", result[2].Url);
        }

        [TestMethod]
        public void CheckReturnObjects()
        {
            //Створення імітованого сховища
            var mock = new Mock<IUrlRepository>();
            //Заповнення сховища значеннями
            var urls = new Url[]
            {
                new Url {RealUrl = "RealUrl1", ShortenedUrl = "11111"},
                new Url {RealUrl = "RealUrl2", ShortenedUrl = "22222"},
                new Url  {RealUrl = "RealUrl3", ShortenedUrl = "33333"}
            }.AsQueryable();

            mock.Setup(u => u.Urls).Returns(urls);
            //Створення контролера
            var controller = new HomeController(mock.Object);


            //var array = new MemoryStream(controller.ForTest());


            var jsonObject = ((JsonResult) controller.ForTest()).Data as string;

            var ie = JsonConvert.DeserializeObject(jsonObject) as JArray;

            var list = ie.ToObject<List<Url>>();

            var url = list[0].ShortenedUrl;

            Assert.AreEqual(url, "ShortUrl1");
        }
    }
}
