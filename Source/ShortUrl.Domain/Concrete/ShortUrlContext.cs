using System.Data.Entity;
using ShortUrl.Domain.Entities;

namespace ShortUrl.Domain.Concrete
{
    /// <summary>
    /// Клас контексту бали даних
    /// </summary>
    public class ShortUrlContext : DbContext
    {
        public ShortUrlContext()
            : base("DefaultConnection")
        {
            //Database.Create();
        }

        /// <summary>
        /// таблиця Urls в базі даних
        /// </summary>
        public DbSet<Url> Urls { get; set; } 
    }
}
