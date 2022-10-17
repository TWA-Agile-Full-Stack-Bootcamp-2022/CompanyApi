using System;

namespace CompanyApi.Controllers
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string name, int salary)
        {
            Name = name;
            Salary = salary;
        }

        public int Salary { get; set; }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var employee = obj as Employee;
            return employee != null && Name.Equals(employee.Name) && Salary.Equals(employee.Salary);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Salary);
        }
    }
}