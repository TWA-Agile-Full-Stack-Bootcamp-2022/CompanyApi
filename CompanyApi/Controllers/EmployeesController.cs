using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
    [ApiController]
    [Route("companies/{companyId}/employees")]
    public class EmployeesController
    {
        private static List<Employee> employees = new List<Employee>();
        public static List<Employee> Employees
        {
            get => employees;
            set => employees = value;
        }

        [HttpPost]
        public ActionResult<Employee> Add(string companyId, Employee employee)
        {
            employee.Id = Guid.NewGuid().ToString();
            employee.CompanyId = companyId;
            employees.Add(employee);
            return new CreatedResult("companies/{companyId}/employees", employee);
        }

        [HttpGet]
        public List<Employee> ListByCompany(string companyId)
        {
            return employees.Where(employee => companyId.Equals(employee.CompanyId)).ToList();
        }

        [HttpPut("{id}")]
        public Employee Update(string companyId, string id, Employee employeeInfo)
        {
            var employee = employees.First(employee => companyId.Equals(employee.CompanyId) && id.Equals(employee.Id));
            employee.Name = employeeInfo.Name;
            employee.Salary = employeeInfo.Salary;
            return employee;
        }

        [HttpDelete("{id}")]
        public void Del(string companyId, string id)
        {
            var employee = employees.First(employee => companyId.Equals(employee.CompanyId) && id.Equals(employee.Id));
            employees.Remove(employee);
        }
    }
}