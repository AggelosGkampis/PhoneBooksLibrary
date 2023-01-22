using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using PhoneBooksLibrary.Entities;

namespace PhoneBooksLibrary
{
    public class PhoneBookManager
    {
        public Dictionary<string, PhoneBook> _entries;

        string filePath = "X:\\phonebook.bin";

        public PhoneBookManager()
        {
            _entries = new Dictionary<string, PhoneBook>();
        }

        public List<PhoneBook> GetAllEntries()
        {
            Load();
            return _entries.Values.ToList();
        }

        public PhoneBook GetEntryNumber(string phoneN)
        {
            return _entries[phoneN];
        }
        public void AddEntry(string number, PhoneBook phonebook)
        {
            if (!_entries.ContainsKey(number))
            {
                _entries.Add(number, phonebook);
                SaveToFile(phonebook);
            }
            else
            {
                Console.WriteLine("The number already exists in the phonebook");
            }
        }

        public void DeleteEntry(string number)
        {
            if (_entries.ContainsKey(number))
            {
                _entries.Remove(number);
            }
            else
            {
                Console.WriteLine("The number does not exist in the phonebook");
            }
        }

        public void EditEntry(string number, PhoneBook newData)
        {
            if (_entries.ContainsKey(number))
            {
                _entries[number] = newData;
            }
            else
            {
                Console.WriteLine("The number does not exist in the phonebook");
            }
        }

        public List<PhoneBook> IterateEntriesByLastName()
        {
            var sortedEntries = _entries.Values.OrderBy(entry => entry.LastName).ToList();
            return sortedEntries;
        }

        private void SaveToFile(PhoneBook phoneBook)
        {
            using (var stream = File.OpenWrite(filePath))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, phoneBook);
            }
        }
        private void Load()
        {
            if (!File.Exists(filePath))
                return;

            using (var stream = File.OpenRead(filePath))
            {
                var formatter = new BinaryFormatter();
                _entries = (Dictionary<string, PhoneBook>)formatter.Deserialize(stream);
            }
        }

        public override string ToString()
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