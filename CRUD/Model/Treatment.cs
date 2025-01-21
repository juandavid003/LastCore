using System;

public class Treatment
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public decimal StandardCost { get; set; }
    public decimal StandardConsumption { get; set; }
    public int PatientId { get; set; }
    public int SpecialistId { get; set; }
    public int AdminId { get; set; }
    public DateTime EndDate { get; set; }
}
