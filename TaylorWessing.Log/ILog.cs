using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaylorWessing.Log
{
    public interface ILog
    {
        void Info(object message);
        void Debug(object message);
        void Error(object message, Exception exception);
        void Fatal(object message);
    }
}
