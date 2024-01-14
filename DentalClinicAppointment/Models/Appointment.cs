using System;

public class Appointment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Time { get; set; }
    public string Patient { get; set; }
    public string Procedure { get; set; }
    public string Notes { get; set; }

    public Appointment(int id, DateTime date, string time, string patient, string procedure, string notes)
    {
        Id = id;
        Date = date;
        Time = time;
        Patient = patient;
        Procedure = procedure;
        Notes = notes;
    }
}
