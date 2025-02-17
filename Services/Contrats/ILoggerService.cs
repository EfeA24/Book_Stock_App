using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contrats
{
    public interface ILoggerService
    {
        void LogWarning(string message);
        void LogError(string message);
        void LogInfo(string message);
        void LogDebug(string message);
    }
}
