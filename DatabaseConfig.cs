using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialApp
{
    public static class DatabaseConfig
    {
        // Centralized connection string
        public static string ConnectionString { get; } =
            "Data Source=personal\\SQLEXPRESS;Initial Catalog=UserRegistrationDB;Integrated Security=True;Trust Server Certificate=True";

    }
}
