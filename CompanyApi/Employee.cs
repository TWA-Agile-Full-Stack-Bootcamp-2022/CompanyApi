using System;

namespace CompanyApi
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(string id, string name, double salary)
        {
            Id = id;
            Name = name;
            Salary = salary;
        }

        public Employee(string name, double salary)
        {
            Name = name;
            Salary = salary;
        }

        public Employee(string id, string companyId, string name, int salary)
        {
            Id = id;
            CompanyId = companyId;
            Name = name;
            Salary = salary;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        
        public string CompanyId { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Employee)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Salary, CompanyId);
        }

        protected bool Equals(Employee other)
        {
            return Id == other.Id && Name == other.Name && Salary.Equals(other.Salary) && CompanyId == other.CompanyId;
        }
    }
}