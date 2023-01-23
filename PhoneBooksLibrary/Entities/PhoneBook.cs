using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBooksLibrary.Entities.Enums;
using ProtoBuf;

namespace PhoneBooksLibrary.Entities
{
    [ProtoContract]
    public class PhoneBook
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }
        [ProtoMember(2)]
        public string LastName { get; set; }
        [ProtoMember(3)]
        public PhoneType Type { get; set; }
        [ProtoMember(4)]
        public string Number { get; set; }


        public PhoneBook(string firstName, string lastName, PhoneType type, string number)
        {
            FirstName = firstName;
            LastName = lastName;
            Type = type;
            Number = number;
        }


    }
}
