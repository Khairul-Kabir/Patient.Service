namespace Patient.Service.Models
{
    public class PatientViewModel
    {
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public int Age { get; set; }
		public string? Gander { get; set; }
		public byte[]? Image { get; set; }
		public decimal Phone { get; set; }
	}
}
