using System.Collections.Generic;

namespace Mehrsan.Dal.DB
{
    public interface IDALGeneric<T> where T : class
    {
        bool Delete(object id);
        bool Exists(object id);
        T Load(object id);
        List<T> Load(List<string> parameters, List<object> values);
        bool Create(T Item);
    }
}