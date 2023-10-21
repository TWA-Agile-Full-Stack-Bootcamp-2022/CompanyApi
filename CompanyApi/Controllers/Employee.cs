using System;

namespace CompanyApi.Controllers
{
    public class Employee
    {
        public Employee(string name, int salary)
        {
            Name = name;
            Salary = salary;
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public int Salary { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   Name == employee.Name &&
                   Salary == employee.Salary;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Salary);
        }
    }
}