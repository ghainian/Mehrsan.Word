using Mehrsan.Common.Interface;

namespace Mehrsan.Business.Interface
{
    public interface IBaseRepository
    {
        ILogger Logger { get; }
    }
}