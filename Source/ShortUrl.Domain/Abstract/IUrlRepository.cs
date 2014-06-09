using System.Linq;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Domain.Abstract
{
    /// <summary>
    /// Інтерфейс, ще реалізується репозиторієм
    /// </summary>
    public interface IUrlRepository
    {
        //представляє таблицю Urls
        IQueryable<Url> Urls { get; }

        /// <summary>
        /// Додає новий запис у таблицю
        /// </summary>
        /// <param name="url">Представляє дані, котрі запиуються в таблицю</param>
        void AddNewUrl(Url url);
    }
}
