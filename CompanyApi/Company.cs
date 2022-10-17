using System;

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

            return Equals((Company)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
        
        protected bool Equals(Company other)
        {
            return Id == other.Id && Name == other.Name;
        }
    }
}