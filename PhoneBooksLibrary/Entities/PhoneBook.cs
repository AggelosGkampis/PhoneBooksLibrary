using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBooksLibrary.Entities.Enums;

namespace PhoneBooksLibrary.Entities
{
    [Serializable]
    public class PhoneBook
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PhoneType Type { get; set; }

        public string Number { get; set; }


        public PhoneBook(string firstName, string lastName, PhoneType type, string number)
        {
            FirstName = firstName;
            LastName = lastName;
            Type = type;
            Number = number;
        }

        public PhoneBook()
        {

        }

    }
}
