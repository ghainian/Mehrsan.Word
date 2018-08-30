using Mehrsan.Business.Interface;
using Mehrsan.Common;
using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mehrsan.Dal.DB.Interface;
using Microsoft.Extensions.Logging;


namespace Mehrsan.Business
{
    public class BaseRepository : IBaseRepository
    {
        #region Properties

        public Common.Interface.ILogger Logger { get; }

        #endregion

        #region Methods
        public BaseRepository(Common.Interface.ILogger loggerInstance)
        {
            Logger = loggerInstance;
        } 
        #endregion


    }
}
