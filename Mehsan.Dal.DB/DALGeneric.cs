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

        private static readonly IDALGeneric<T> _instance;

        #endregion

        #region Properties

        public static WordEntities WordEntitiesInstance { get { return new WordEntities(WordEntities.Options); } }

        public static IDALGeneric<T> Instance { get { return _instance; } }

        #endregion

        #region Methods

        static DALGeneric()
        {
            _instance = new DALGeneric<T>();
        }

        private DALGeneric()
        {

        }

        public bool Create(T item)
        {
            using (var dbContext = WordEntitiesInstance)
            {
                
                if (item != null)
                {
                    dbContext.Set<T>().Add(item);
                    dbContext.SaveChanges();
                }
                return true;
            }
        }

        public bool Delete(object id)
        {
            using (var dbContext = WordEntitiesInstance)
            {
                T item = Load(id);
                if (item != null)
                {
                    dbContext.Set<T>().Remove(item);
                    dbContext.SaveChanges();
                }
                return true;
            }
        }

        public bool Exists(object id)
        {
            var result = false;
            using (var dbContext = WordEntitiesInstance)
            {

                var param = Expression.Parameter(typeof(T), "alias");

                var query = dbContext.Set<T>().AsQueryable();
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
            }
            return result;
        }


        public List<T> Load(List<string> parameters, List<object> values)
        {
            if (parameters.Count != values.Count)
            {
                throw new Exception("Number of parameters and values should be the same");
            }

            List<T> result = null;
            using (var dbContext = WordEntitiesInstance)
            {
                var param = Expression.Parameter(typeof(T), "alias");


                var query = dbContext.Set<T>().AsQueryable();
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

            }
            return result;
        }

        public T Load(object id)
        {
            T result = null;
            using (var dbContext = WordEntitiesInstance)
            {
                var param = Expression.Parameter(typeof(T), "alias");
                var exp = Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(
                        Expression.Property(param, "Id"),
                        Expression.Constant(id)
                    ),
                    param
                );

                var query = dbContext.Set<T>().Where(exp);
                if (query.Any())
                {
                    result = query.FirstOrDefault();
                }

            }
            return result;
        }

        #endregion
    }
}
