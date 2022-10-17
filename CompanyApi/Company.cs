namespace CompanyApi
{
    public class Company
    {
        public Company()
        {
        }

        public Company(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public Company(string name)
        {
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}