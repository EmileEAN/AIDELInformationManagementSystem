using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEANWorks.WinFormsApps.AIDEL_IMS_Reports
{
    public class FullName
    {
        public FullName(string _firstName, string _middleName, string _paternalSurname, string _maternalSurname)
        {
            FirstName = _firstName;
            MiddleName = _middleName;
            PaternalSurname = _paternalSurname;
            MaternalSurname = _maternalSurname;
        }

        public string FirstName { get; }
        public string MiddleName { get; }
        public string PaternalSurname { get; }
        public string MaternalSurname { get; }

        public override string ToString()
        {
            return FirstName 
                + ((MiddleName != string.Empty) ? (" " + MiddleName) : string.Empty) 
                + ((PaternalSurname != string.Empty) ? (" " + PaternalSurname) : string.Empty) 
                + ((MaternalSurname != string.Empty) ? (" " + MaternalSurname) : string.Empty);
        }
    }

    public abstract class Person
    {
        public Person(FullName _name)
        {
            Name = _name;
        }
        public Person(FullName _name, string _preferredEmail, string _phone)
        {
            Name = _name;

            PreferredEmail = _preferredEmail;

            Phone = _phone;
        }

        public FullName Name { get; set; }

        public string PreferredEmail { get; set; }

        public string Phone { get; set; }
    }

    public abstract class IberoCommunityMember : Person
    {
        public IberoCommunityMember(FullName _name,
                                    int _accountNumber) : base(_name)
        {
            AccountNumber = _accountNumber;
        }
        public IberoCommunityMember(FullName _name, string _preferredEmail, string _phone,
                                    int _accountNumber, string _organizationEmail) : base(_name, _preferredEmail, _phone)
        {
            AccountNumber = _accountNumber;

            OrganizationEmail = _organizationEmail;
        }

        public int AccountNumber { get; set; }

        public string OrganizationEmail { get; set; }
    }

    public class Student : IberoCommunityMember
    {
        public Student(int _accountNumber, FullName _name) : base(_name, _accountNumber)
        {
        }
        public Student(int _accountNumber, FullName _name, string _organizationEmail, string _preferredEmail, string _phone,
                        Major _major, Semester _semesterAdmitted) : base(_name, _preferredEmail, _phone, _accountNumber, _organizationEmail)
        {
            Major = _major;

            SemesterAdmitted = _semesterAdmitted;
        }

        public Major Major { get; set; }

        public Semester SemesterAdmitted { get; set; }
    }

    public class Faculty : IberoCommunityMember
    {
        public Faculty(int _accountNumber, FullName _name,
            string _username, string _password, ePermissionsType _permissionsType) : base(_name, _accountNumber)
        {
            Username = _username;
            Password = _password;
            PermissionsType = _permissionsType;
        }
        public Faculty(int _accountNumber, FullName _name, string _organizationEmail, string _preferredEmail, string _phone,
            string _username, string _password, ePermissionsType _permissionsType) : base(_name, _preferredEmail, _phone, _accountNumber, _organizationEmail)
        {
            Username = _username;
            Password = _password;
            PermissionsType = _permissionsType;
        }

        public string Username { get; }
        public string Password { get; }
        public ePermissionsType PermissionsType { get; }
    }

    public class NonIberoCommunityMember : Person
    {
        public NonIberoCommunityMember(FullName _name, string _preferredEmail, string _phone) : base(_name, _preferredEmail, _phone)
        {
        }
    }
}
