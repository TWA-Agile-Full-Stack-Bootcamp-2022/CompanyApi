using System;

namespace CompanyApi.Controllers
{
    public class Company
    {
        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public string CompanyID { get; set; } = Guid.NewGuid().ToString();

        public override bool Equals(object obj)
        {
            var company = obj as Company;
            return company != null && Name.Equals(company.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void UpdateBy(Company updatedCompany)
        {
            Name = updatedCompany.Name;
        }
    }
}