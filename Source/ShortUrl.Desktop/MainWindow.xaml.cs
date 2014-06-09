using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using ShortUrl.WebUI.Models;

namespace ShortUrl.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //JsonSerializer model = null;
            //var client = new HttpClient();
            //var task = client.GetAsync("http://fakewebsitedenis.azurewebsites.net/Home/CreateShortUrl")
            //    .ContinueWith((taskWithResponse) =>
            //    {
            //        var response = taskWithResponse.Result.Content.ReadAsAsync<JsonSerializer>();
            //        response.Wait();
            //        model = response.Result;
            //    });

            //var s = model;
        }

        //private void AuthenTest()
        //{
        //    var client = new HttpClient();
        //    var model = new LoginModel()
        //    {
        //        Password = "12345678",
        //        UserName = "rosga1",
        //        RememberMe = true
        //    };

        //    var formContent = new MultipartFormDataContent();
        //    formContent.Add();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtRealUrl.Text == "")
            {
                tblShortLink.Text = "Enter an url, please";
            }
            else
            {
                var realUrl = txtRealUrl.Text;
                string model = null;
                var client = new HttpClient();

                //var content = new FormUrlEncodedContent(
                //    new List<KeyValuePair<string, string>> 
                //    {
                //        new KeyValuePair<string, string>("realUrl", realUrl)
                //    });

                HttpContent content = new StringContent(realUrl, Encoding.UTF8, "application/x-www-form-urlencoded");
                
                //var task = client.PostAsJsonAsync("http://fakewebsitedenis.azurewebsites.net/Home/CreateShortUrl", realUrl)
                //    .ContinueWith((taskWithResponse) =>;
                //    {
                //        var response = taskWithResponse.Result.Content.ReadAsAsync<string>();
                //        response.Wait();
                //        model = response.Result;
                //    });
                var task = client.PostAsync("http://localhost:35788/Home/CreateShortUrl", content)
                    .ContinueWith((taskWithResponse) =>
                    {
                        var response = taskWithResponse.Result.Content.ReadAsAsync<string>();
                        response.Wait();
                        model = response.Result;
                    });
                task.Wait();

                tblShortLink.Text = model;
            }


        }
    }
}
