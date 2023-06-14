using Patient.Service.Models;
using Patient.Service.Repository.IRepository;
using Patient.Service.Service.IService;

namespace Patient.Service.Service
{
    public class PatientService : IPatientService
    {
        public readonly IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)=> _patientRepository = patientRepository;

        public async Task<List<PatientViewModel>> GetAllPatients()
        {
            return await _patientRepository.GetAllPatients();
        }
    }
}
