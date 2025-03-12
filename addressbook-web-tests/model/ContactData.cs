using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace WebAddressbookTests
{
    public class ContactData : IEquatable<ContactData>, IComparable<ContactData>
    {
        private string middlename = "";
        private string nickname = "";
        private string photo = "";
        private string delete = "";
        private string company = "";
        private string title = "";
        private string address = "";
        private string homePhone = "";
        private string mobilePhone = "";
        private string workPhone = "";
        private string fax = "";
        private string email = "";
        private string email2 = "";
        private string email3 = "";
        private string homepage = "";
        private string birthday = "";
        private string anniversary = "";
        private string allPhones;
        private string allEmails;
        private string allInfo;
        private string name;
        private string phones;
        private string emails;

        public ContactData()
        {
        }

        public ContactData(string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        public bool Equals(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }
            return Firstname == other.Firstname && Lastname == other.Lastname;  
        } 

        public override int GetHashCode()
        {
            return HashCode.Combine(Firstname, Lastname);
        }
        public override string ToString()
        {
            return Lastname + " " + Firstname + " ";
        }

        public int CompareTo(ContactData other)
        {
            if (Object.ReferenceEquals(other, null))
            {
                return 1;
            }
            int result = Lastname.CompareTo(other.Lastname);
            if (result == 0)
            {
                result = Firstname.CompareTo(other.Firstname);
            }
            return result;
        }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Nickname { get; set; }
        public string Middlename { get; set; }
        public string Photo { get; set; }
        public string Delete { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string AllPhones
        {
            get
            {
                if (allPhones != null)
                {
                    return allPhones;
                }
                else 
                {
                    return (CleanUp(HomePhone) + CleanUp(MobilePhone) + CleanUp(WorkPhone)).Trim();
                }
            }
            set
            {
                allPhones = value;
            }
        }

        private string CleanUp(string phone)
        {
            if(phone == null || phone =="")
            {
                return "";
            }
            return Regex.Replace(phone, "[ -()]", "")  + "\r\n"; 
        }

        public string Fax { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string Email3 { get; set; }
        public string AllEmails
        {
            get
            {
                if (allEmails != null)
                {
                    return allEmails;
                }
                    return (CleanUp(Email) + CleanUp(Email2) + CleanUp(Email3)).Trim();
            }
            set
            {
                allEmails = value;
            }
        }
        public string Homepage { get; set; }
        public string Birthday { get; set; }
        public string Anniversary { get; set; }
        public string AllInfo
        {
            get
            {
                if (allInfo != null)
                {
                    return allInfo;
                }
                return (Name + "\r\n" + CleanUpEmpty(Address) + "\r\n" + CleanUpEmpty(Phones)  + CleanUpEmpty(Emails)).Trim();
            }
            set
            {
                allInfo = value;
            }
        }

        public string Name
        {
            get
            {
                if (name != null)
                {
                    return name;
                }
                return (CleanUpEmptyName(Firstname) + " " + CleanUpEmptyName(Lastname)).Trim();
            }
            set
            {
                name = value;
            }
        }

        public string Phones
        {
            get
            {
                if (phones != null)
                {
                    return phones;
                }
                return (CleanUpEmpty(HomePhone) + CleanUpEmpty(MobilePhone) + CleanUpEmpty(WorkPhone) + CleanUpEmpty(Fax));
            }
            set
            {
                phones = value;
            }
        }

        public string Emails
        {
            get
            {
                if (emails != null)
                {
                    return emails;
                }
                return (CleanUpEmpty(Email) + CleanUpEmpty(Email2) + CleanUpEmpty(Email3)).Trim();
            }
            set
            {
                emails = value;
            }
        }


        private string CleanUpEmptyName(string nameInfo)
        {
            {
                {
                    if (nameInfo == null || nameInfo == "")
                    {
                        return "";
                    }
                    return nameInfo;
                }
            }
        }

        private string CleanUpEmpty(string info)
        {
            {
                if (info == null || info == "")
                {
                    return "";
                }
                return info + "\r\n";
            }
        }
    }
}
