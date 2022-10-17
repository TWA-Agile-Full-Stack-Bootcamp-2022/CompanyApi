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

        public override bool Equals(object obj)
        {
            var company = obj as Company;
            return company != null && Name.Equals(company.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}