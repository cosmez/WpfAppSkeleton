﻿namespace WpfApp.Library.Models;
public class Employee
{
    public int EmployeeId { get; set; }
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public DateTime BirthDate { get; set; }
    public string? Photo { get; set; }
    public string? Notes { get; set; }
}