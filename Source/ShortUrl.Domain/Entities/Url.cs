using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShortUrl.Domain.Utilities;


namespace ShortUrl.Domain.Entities
{
    /// <summary>
    /// Клас відповідає сутності в базі даних
    /// </summary>
    public class Url
    {
        public Url()
        {
            Date = DateTime.Now;

            ShortenedUrl = ShortUrlUtils.GetRandomChars();

        }

        public Url(string realUrl)
        {
            RealUrl = realUrl;
            Date = DateTime.Now;
            ShortenedUrl = ShortUrlUtils.GetRandomChars();
            //Image = Thumbnail.CreateThumbnailImage(RealUrl, 240, 160);

        }

        //-------------------властивості-----------------
        //--кожна властивіть відповідає за поле таблиці--
        [Key]
        public string ShortenedUrl { get; set; }
        public string RealUrl { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public byte[] Image { get; set; }

        public string GetImageString()
        {
            return String.Format("data:image/Bmp;base64,{0}", Convert.ToBase64String(Image));
        }
    }
}
