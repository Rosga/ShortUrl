using System.Linq;
using ShortUrl.Domain.Abstract;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Domain.Concrete
{
    /// <summary>
    /// Репозиторій. Представляє собою сховище бази даних
    /// </summary>
    public class EfUrlRepository : IUrlRepository
    {
        private ShortUrlContext _context = new ShortUrlContext();
        
        //таблиця Urls
        public IQueryable<Url> Urls {
            get { return _context.Urls; }
        }

        /// <summary>
        /// Додає новий запис у таблицю Urls
        /// </summary>
        /// <param name="url">Дані, котрі записуються в таблицю</param>
        public void AddNewUrl(Url url)
        {
            _context.Urls.Add(url);
            _context.SaveChanges();
        }
    }
}
