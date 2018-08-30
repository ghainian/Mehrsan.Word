using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mehrsan.Core.Web.Models;
using Mehrsan.Business;
using System.Net;
using System.IO;
using Mehrsan.Dal.DB;
using Microsoft.Extensions.Logging;
using Mehrsan.Business.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Mehrsan.Core.Web.Controllers
{

    public class BaseController:Controller 
    {

        #region Fields

        #endregion

        #region Properties

        protected Mehrsan.Common.Interface.ILogger  Logger { get; }

        #endregion


        #region Methods

        public BaseController(Common.Interface.ILogger logger)
        {
            try
            {
                Logger = logger;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        #endregion

    }
}
