using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace ByLearningJson
{
    public class Movie
    {
        public string Name { get; set; }
        public int Year { get; set; }
    }
    public class Employee
    {
        public string Name { get; set; }
        public Employee Manager { get; set; }
        public bool ShouldSerializeManager()
        {
            // don't serialize the Manager property if an employee is their own manager
            return (Manager != this);
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class EmployeePing : Person
    {
        public string Department { get; set; }
        public string JobTitle { get; set; }
    }
    public class PersonConverter : CustomCreationConverter<Person>
    {
        public override Person Create(Type objectType)
        {
            return new EmployeePing();
        }
    }
    public class Account
    {
        public string Email { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Roles { get; set; }
    }
    public class Website
    {
        public string Url { get; set; }
        private Website()
        {
        }
        public Website(Website website)
        {
            if (website == null)
            {
                throw new ArgumentNullException(nameof(website));
            }

            Url = website.Url;
        }
    }

    public class UserViewModel
    {
        public string Name { get; set; }
        public IList<string> Offices { get; private set; }
        public UserViewModel()
        {
            Offices = new List<string>
            {
                "Auckland",
                "Wellington",
                "Christchurch"
            };
        }
    }

    public class PersonPing
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Person Partner { get; set; }
        public decimal? Salary { get; set; }
    }

    public class AccountPing
    {
        public string FullName { get; set; }
        public bool Deleted { get; set; }
    }
    public class EmployeeFoo
    {
        public string Name { get; set; }
        public EmployeeFoo Manager { get; set; }
    }
    public class Directory
    {
        public string Name { get; set; }
        public Directory Parent { get; set; }
        public IList<FileInfo> Files { get; set; }
    }

    public class FileInfo
    {
        public string Name { get; set; }
        public Directory Parent { get; set; }
    }
    public class Flight
    {
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime DepartureDateUtc { get; set; }
        public DateTime DepartureDateLocal { get; set; }
        public TimeSpan Duration { get; set; }
    }
    public abstract class Business
    {
        public string Name { get; set; }
    }

    public class Hotel : Business
    {
        public int Stars { get; set; }
    }

    public class Stockholder
    {
        public string FullName { get; set; }
        public IList<Business> Businesses { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class PersonFoo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
