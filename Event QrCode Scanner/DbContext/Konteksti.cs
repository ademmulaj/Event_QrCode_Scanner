using Event_QrCode_Scanner.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_QrCode_Scanner.DbContext
{
    public class Konteksti : Microsoft.EntityFrameworkCore.DbContext
    {
        public Konteksti(DbContextOptions<Konteksti> options) : base(options)
        {
        }

        public DbSet<QrCodeData> QrCodeData { get; set; }
    }
}