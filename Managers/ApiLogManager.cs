using ArtiConnect.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Managers
{
    public static class ApiLogManager
    {
        public static event EventHandler<ApiLog> LogAdded;

        public static void NotifyLogAdded(ApiLog log)
        {
            LogAdded?.Invoke(null, log);
        }
    }
}
