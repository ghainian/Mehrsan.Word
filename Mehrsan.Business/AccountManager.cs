using Mehrsan.Dal.DB;
using System.Collections.Generic;
using Mehrsan.Dal.DB.Interface;

namespace Mehrsan.Business
{
    public sealed class AccountRepository
    {
        #region Fields

        #endregion

        #region Properties
        public static AccountRepository Instance { get; } = new AccountRepository();
        public IDAL DalInstance { get; } = new DAL();
        #endregion

        #region Methods
        
        private AccountRepository()
        {

        }

        public List<AspNetUser> GetUsers(string searchText)
        {
            using (var dbContext = DalInstance.NewWordEntitiesInstance())
            {
                return DalInstance.GetUsers(searchText);
            }
        }

        public List<AspNetUserClaim> GetUserClaims(string searchText)
        {
            using (var dbContext = DalInstance.NewWordEntitiesInstance())
            {
                return DalInstance.GetUserClaims(searchText);
            }
        }

        public bool CreateClaim(AspNetUserClaim userClaim)
        {
            using (var dbContext = DalInstance.NewWordEntitiesInstance())
            {
                new DALGeneric<AspNetUserClaim>(dbContext) .Create(userClaim);
            }
            return true;
        } 
        #endregion
    }
}
