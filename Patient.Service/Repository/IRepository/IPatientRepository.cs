using Patient.Service.Models;

namespace Patient.Service.Repository.IRepository
{
    public interface IPatientRepository
    {
        public Task<List<PatientViewModel>> GetAllPatients();

        Task<PatientViewModel> SavePatient(PatientViewModel model);
    }
}
