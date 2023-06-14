using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient.Service.Models;
using Patient.Service.Service.IService;

namespace Patient.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)=>_patientService = patientService;

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _patientService.GetAllPatients();
            return Ok(patients);
        }
    }
}
