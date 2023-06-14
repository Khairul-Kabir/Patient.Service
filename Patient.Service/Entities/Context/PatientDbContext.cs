using System.Data;
using System.Data.SqlClient;

namespace Patient.Service.Entities.Context
{
    public class PatientDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public PatientDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("PatientDBConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
