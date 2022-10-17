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

        public string Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        
        public string CompanyId { get; set; }
    }
}