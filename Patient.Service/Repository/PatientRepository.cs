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

        public async Task<PatientViewModel> SavePatient(PatientViewModel patientViewModel)
        {
            // Assuming you have a Patients table in your database
            const string insertQuery = @"
            INSERT INTO Patients (FirstName, LastName, Age, Gender, Image, Phone)
            VALUES (@FirstName, @LastName, @Age, @Gender, @Image, @Phone);
            SELECT LAST_INSERT_ID();";

            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Execute the insert query and retrieve the last inserted ID
                        int patientId = await connection.QuerySingleAsync<int>(insertQuery, patientViewModel, transaction);

                        // Set the ID of the view model with the newly inserted ID
                        patientViewModel.Id = patientId;

                        // Commit the transaction
                        transaction.Commit();

                        return patientViewModel;
                    }
                    catch (Exception)
                    {
                        // Handle exceptions, log, or rollback the transaction
                        transaction.Rollback();
                        throw; // You may want to handle or log the exception according to your needs
                    }
                }
            }
        }
    }
}
