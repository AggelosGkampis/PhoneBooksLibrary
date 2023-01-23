using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Google.Protobuf;
using PhoneBooksLibrary.Entities;

namespace PhoneBooksLibrary
{
    public class PhoneBookManager
    {
        private readonly object _lock = new object();

        private Dictionary<string, PhoneBook> _entries;

        public Dictionary<string, PhoneBook> Entries
        {
            get { return _entries; }
            private set { _entries = value; }
        }
        public PhoneBookManager()
        {
            _entries = new Dictionary<string, PhoneBook>();
        }

        public List<PhoneBook> GetAllEntries()
        {
            try
            {
                if (_entries == null || _entries.Count == 0)
                {
                    return new List<PhoneBook>();
                    //or return "No phonebook entries found.";
                }
                return _entries.Values.ToList();
            }
            catch (Exception ex)
            {
                //log the exception
                return new List<PhoneBook>();
            }
        }


        public PhoneBook GetEntryNumber(string phoneN)
        {
            lock (_lock)
            {
                return _entries[phoneN];
            }
        }
        public bool AddEntry(PhoneBook phonebook)
        {
            lock (_lock)
            {
                if (!_entries.ContainsKey(phonebook.Number))
                {
                    _entries.Add(phonebook.Number, phonebook);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public bool DeletePhonebook(string number)
        {
            lock (_lock)
            {
                if (_entries.ContainsKey(number))
                {
                    _entries.Remove(number);
                    return true;
                }
                else
                {
                    Console.WriteLine("The number does not exist in the phonebook");
                    return false;
                }
            }
        }

        public bool EditEntry(string number, PhoneBook newData)
        {
            lock (_lock)
            {
                if (_entries.ContainsKey(number))
                {
                    _entries[number] = newData;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public List<PhoneBook> IterateEntriesByLastName()
        {
            lock (_lock)
            {
                var sortedEntries = _entries.Values.OrderBy(entry => entry.LastName).ToList();
                return sortedEntries;
            }
                
        }

        public void SerializeToProtoBuf(string fileName)
        {
            lock (_lock)
            {
                using (var file = File.Create(fileName))
                {
                    ProtoBuf.Serializer.Serialize(file, _entries.Values);
                }

            }
                
        }

        public void DeserializeFromProtoBuf(string fileName)
        {
            lock (_lock)
            {
                using (var file = File.OpenRead(fileName))
                {
                    var phoneBooks = ProtoBuf.Serializer.Deserialize<List<PhoneBook>>(file);
                    _entries = phoneBooks.ToDictionary(pb => pb.Number, pb => pb);
                }
            }

        }

        public override string ToString()
        {
            lock (_lock)
            {
                var sb = new StringBuilder();
                foreach (var entry in _entries)
                {
                    sb.AppendLine(entry.Value.ToString());
                }
                return sb.ToString();

            }

        }
    }
}