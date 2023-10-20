using System;

namespace CompanyApi.Controllers
{
    public class Company
    {
        public Company(string name)
        {
            Name = name;
        }

        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
    }
}
