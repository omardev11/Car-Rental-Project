?using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace CarRentalDataLayer.Settings
{
    public class DataAccessSetting
    {
        public static string _connectionString;

        public static void Initialize(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CarRentalDB");
        }
    }
}
