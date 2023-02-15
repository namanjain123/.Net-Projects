using AsyncTypeAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AsyncTypeAPI.Database
{
    public class reqDbContext :DbContext
    {
        public reqDbContext(DbContextOptions<reqDbContext> options) : base(options)
        {

        }
       public  DbSet<Request> Request => Set<Request>();
    }
}
