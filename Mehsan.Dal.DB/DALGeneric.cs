using Mehrsan.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Mehrsan.Dal.DB
{
    public sealed class DALGeneric<T> : IDALGeneric<T> where T : class
    {
        #region Fields


        #endregion

        #region Properties
        public WordEntities DbContext { get; set; }
        #endregion

        #region Methods


        public void NewSession()
        {
            DbContext = new WordEntities(WordEntities.Options);
        }

        public DALGeneric(WordEntities dbContext)
        {
            this.DbContext = dbContext;
        }

        

        public bool Create(T item)
        {


            if (item != null)
            {
                DbContext.Set<T>().Add(item);
                DbContext.SaveChanges();
            }
            return true;

        }

        public bool Delete(object id)
        {

            T item = Load(id);
            if (item != null)
            {
                DbContext.Set<T>().Remove(item);
                DbContext.SaveChanges();
            }
            return true;

        }

        public bool Exists(object id)
        {
            var result = false;

            var param = Expression.Parameter(typeof(T), "alias");

            var query = DbContext.Set<T>().AsQueryable();
            var expressions = new List<Expression>();

            var exp = Expression.Lambda<Func<T, bool>>(
            Expression.Equal(
                    Expression.Property(param, "Id"),
                    Expression.Constant(id)
                ),
                param
            );

            query = query.Where(exp);
            result = query.Any();

            return result;
        }


        public List<T> Load(List<string> parameters, List<object> values)
        {
            if (parameters.Count != values.Count)
            {
                throw new Exception("Number of parameters and values should be the same");
            }
            List<T> result = null;

            var param = Expression.Parameter(typeof(T), "alias");
            
            var query = DbContext.Set<T>().AsQueryable();
            var expressions = new List<Expression>();
            for (int i = 0; i < parameters.Count; i++)
            {
                var exp = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                        Expression.Property(param, parameters[i]),
                        Expression.Constant(values[i])
                    ),
                    param
                );

                query = query.Where(exp);


            }


            if (query.Any())
            {
                result = query.ToList();
            }


            return result;
        }

        public T Load(object id)
        {
            T result = null;
            
                var param = Expression.Parameter(typeof(T), "alias");
                var exp = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(param, "Id"),
                        Expression.Constant(id)
                    ),
                    param
                );

                var query = DbContext.Set<T>().Where(exp);
                if (query.Any())
                {
                    result = query.FirstOrDefault();
                }

            
            return result;
        }

        #endregion
    }
}
