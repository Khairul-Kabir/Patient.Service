using Patient.Service.Models;

namespace Patient.Service.Service.IService
{
    public interface IPatientService
    {
        public Task<List<PatientViewModel>> GetAllPatients();
    }
}
