﻿using Mehrsan.Dal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Business
{
    public sealed class AccountManager
    {
        #region Fields

        private static AccountManager _instance;

        #endregion

        #region Properties
        public static AccountManager Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Methods
        static AccountManager()
        {
            _instance = new AccountManager();
        }

        private AccountManager()
        {

        }

        public List<AspNetUser> GetUsers(string searchText)
        {
            using (var dbContext = DAL.Instance.NewWordEntitiesInstance())
            {
                return DAL.Instance.GetUsers(searchText);
            }
        }

        public List<AspNetUserClaim> GetUserClaims(string searchText)
        {
            using (var dbContext = DAL.Instance.NewWordEntitiesInstance())
            {
                return DAL.Instance.GetUserClaims(searchText);
            }
        }

        public bool CreateClaim(AspNetUserClaim userClaim)
        {
            using (var dbContext = DAL.Instance.NewWordEntitiesInstance())
            {
                DALGeneric<AspNetUserClaim>.Instance.Create(userClaim);
            }
            return true;
        } 
        #endregion
    }
}