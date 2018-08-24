using Mehrsan.Dal.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;

namespace Mehrsan.Test.Controllers
{
    public class Setup
    {
        #region Fields

        protected static string _userId = "7d8d23dd-2983-4ae0-8507-87a17e12bb9a";

        #endregion

        #region Methods

        public Setup()
        {
            ConfigureDataOptions();
            InitialiseCommon();

        }

        private  void ConfigureDataOptions()
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

        public void InitialiseCommon()
        {
            var maxImg = ConfigurationManager.AppSettings["MaxImagePerWord"].ToString();
            var maxImagePerWord = int.Parse(ConfigurationManager.AppSettings["MaxImagePerWord"].ToString());
            var agentPath = ConfigurationManager.AppSettings["AgentPath"].ToString();
            var backupDir = ConfigurationManager.AppSettings["BackupDir"].ToString();
            var videoDirectory = ConfigurationManager.AppSettings["VideoDirectory"].ToString();
            var downloadGoogleImageWaitTime = int.Parse(ConfigurationManager.AppSettings["DownloadGoogleImageWaitTime"].ToString());
            var nofRelatedSentences = int.Parse(ConfigurationManager.AppSettings["NofRelatedSentences"].ToString());
            var logDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();
            Common.Common.Initialise(maxImagePerWord, agentPath, backupDir,
            videoDirectory, downloadGoogleImageWaitTime, nofRelatedSentences, logDirectory);
        } 

        #endregion
    }
}