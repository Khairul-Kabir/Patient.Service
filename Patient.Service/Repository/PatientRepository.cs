using Dapper;
using Patient.Service.Entities.Context;
using Patient.Service.Models;
using Patient.Service.Repository.IRepository;

namespace Patient.Service.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _dbContext;

        public PatientRepository(PatientDbContext dbContext) => _dbContext = dbContext;
        public async Task<List<PatientViewModel>> GetAllPatients()
        {
            var query = "SELECT * FROM Patient";
            
            using(var connection = _dbContext.CreateConnection())
            {
                var patients = await connection.QueryAsync<PatientViewModel>(query);
                return patients.ToList();
            }
        }
    }
}
