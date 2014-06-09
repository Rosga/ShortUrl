using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;

namespace ShortUrl.WebUI.Utilities
{
    public class Thumbnail
    {
        public string Url { get; set; }
        public Bitmap ThumbnailImage { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int BrowserWidth { get; set; }
        public int BrowserHeight { get; set; }

        public string ErrorMessage { get; set; }
        public Thumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            this.Url = Url;
            this.BrowserWidth = BrowserWidth;
            this.BrowserHeight = BrowserHeight;
            this.Height = ThumbnailHeight;
            this.Width = ThumbnailWidth;
        }
        public Bitmap GenerateThumbnail()
        {
            Thread thread = new Thread(new ThreadStart(GenerateThumbnailInteral));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return ThumbnailImage;
        }
        private void GenerateThumbnailInteral()
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.ScrollBarsEnabled = false;
            webBrowser.Navigate(this.Url);
            //try
            //{
            //    webBrowser.Dispose();
            //    webBrowser.Navigate(this.Url);

            //}
            //catch (Exception exception)
            //{
            //    ErrorMessage = exception.Message;
            //    ThumbnailImage = null;
            //    webBrowser.Dispose();
            //    return;
            //}
            //webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            //try
            //{
            //    while (webBrowser.ReadyState != WebBrowserReadyState.Complete)
            //        System.Windows.Forms.Application.DoEvents();
            //}
            //catch (Exception exception)
            //{
            //    ErrorMessage = exception.Message;
            //    ThumbnailImage = null;
            //    webBrowser.Dispose();
            //    return;
            //}

            ThumbnailImage = new Bitmap(120, 80);
            webBrowser.Dispose();
        }
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //WebBrowser webBrowser = (WebBrowser)sender;
            //webBrowser.ClientSize = new Size(this.BrowserWidth, this.BrowserHeight);
            //webBrowser.ScrollBarsEnabled = false;
            //this.ThumbnailImage = new Bitmap(webBrowser.Bounds.Width, webBrowser.Bounds.Height);
            //webBrowser.BringToFront();
            //webBrowser.DrawToBitmap(ThumbnailImage, webBrowser.Bounds);
            //this.ThumbnailImage = (Bitmap)ThumbnailImage.GetThumbnailImage(Width, Height, null, IntPtr.Zero);
        }

        public static byte[] CreateThumbnailImage(string url, int width, int height)
        {
            Thumbnail thumbnail = new Thumbnail(url, 1024, 768, width, height);
            Bitmap image = thumbnail.GenerateThumbnail();

            //var image = new Bitmap(120, 80);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            byte[] bitmapData = ms.ToArray();

            return bitmapData;
        }

    }
}
