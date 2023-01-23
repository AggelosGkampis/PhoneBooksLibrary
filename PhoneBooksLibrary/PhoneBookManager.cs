using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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

        private static string _path = "AllStoredPhonebooks.bin";


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
            lock (_lock)
            {
                try
                {
                    if (_entries == null || _entries.Count == 0)
                    {
                        return new List<PhoneBook>();
                    }
                    return _entries.Values.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public PhoneBook GetEntryByNumber(string phoneNumber)
        {
            lock (_lock)
            {              
                return _entries[phoneNumber];
            }
        }
        public bool AddEntry(PhoneBook phonebook)
        {
            lock (_lock)
            {
                if (!_entries.ContainsKey(phonebook.Number))
                {
                    _entries.Add(phonebook.Number, phonebook);
                    SerializeToProtoBuf();
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
                    SerializeToProtoBuf();
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

        public void SerializeToProtoBuf()
        {
            lock (_lock)
            {
                using (var file = File.Create(_path))
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