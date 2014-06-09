using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ShortUrl.Domain.Concrete;

namespace ShortUrl.Domain.Utilities
{
    public static class ShortUrlUtils
    {
        private static ShortUrlContext _context = new ShortUrlContext();

        private const string ShorturlCharsLcase = "abcdefghijklmnopqrstuvwxyz";
        private const string ShorturlCharsUcase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string ShorturlCharsNumeric = "0123456789";


        public static string GetRandomChars()
        {
            //створити двомірний масив із символів, що використовуються
            //для зображення скороченого посилання.
            //кожен одномірний масив відповідає одній групі символів
            var charGroup = new char[][]
            {
                ShorturlCharsLcase.ToCharArray(),
                ShorturlCharsUcase.ToCharArray(),
                ShorturlCharsNumeric.ToCharArray()
            };

            //масив цілих. Використовується для зберігання кількості 
            //ще не використаних символів у кожній групі символів
            var charsLeftInGroup = new int[charGroup.Length];

            for (var i = 0; i < charsLeftInGroup.Length; i++)
            {
                charsLeftInGroup[i] = charGroup[i].Length;
            }

            //----------------------------------------------------------------------------------------------------------
            //Використаємо криптографічний засіб генерації випадкових чисел
            //щоб назначити початкові дані стандартному ГВЧ 
 
            //масив із 4 байтів
            var randomBytes = new byte[4];
            //криптографічний ГВЧ
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            //Перетворити 4 байти у 32-бітне ціле
            var seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            //Стандартний ГВЧ
            var random = new Random(seed);

            //масив з 5 символів, що відповідає скороченому повідомленню
            var url = new char[5];

            for (int i = 0; i < url.Length; i++)
            {
                //згенерувати випадковий символ
                var groupIndex = random.Next(0, charGroup.Length);
                var charIndex = random.Next(0, charsLeftInGroup[groupIndex]);

                url[i] = charGroup[groupIndex][charIndex];

                //поміняти місьцями використаний символ із останнім у групі
                var temp = charGroup[groupIndex][charIndex];
                charGroup[groupIndex][charIndex] = charGroup[groupIndex][charsLeftInGroup[groupIndex] - 1];
                charGroup[groupIndex][charsLeftInGroup[groupIndex] - 1] = temp;

                charsLeftInGroup[groupIndex]--;
            }

            var shortened = new string(url);

            if (!CheckIfNoExist(shortened))
            {
                shortened = GetRandomChars();
            }
            
            return shortened;
        }

        /// <summary>
        /// Перевіряє чи знаходиться значення url у базі даних
        /// </summary>
        /// <param name="url">значення, яке перевіряється на наявнсть у базі даних</param>
        /// <returns>True, якщо відповідного значення немає</returns>
        private static bool CheckIfNoExist(string url)
        {
            return _context.Urls.Find(url) == null;
        }
    }
}
