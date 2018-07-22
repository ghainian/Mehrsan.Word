using Mehrsan.Dal.DB;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace Mehrsan.Test.Controllers
{
    public class Setup
    {
        public Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WordEntities>();
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();

            var connectionString = ConfigurationManager.ConnectionStrings["WordEntities"].ToString();
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder(connectionString);
            connectionString = entityConnectionStringBuilder.ProviderConnectionString;
            optionsBuilder.UseSqlServer(connectionString);

            using (var _context = new WordEntities(optionsBuilder.Options))
            {
            }

        }
    }
}