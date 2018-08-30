using System;
using Microsoft.Extensions.Logging;

namespace Mehrsan.Common.Interface
{
    public interface ILogger
    {
        Microsoft.Extensions.Logging.ILogger LoggerInstance { get; }
        void Log(Exception ex, string layer);
        void Log(string message);
        void Log(Exception ex);
    }
}