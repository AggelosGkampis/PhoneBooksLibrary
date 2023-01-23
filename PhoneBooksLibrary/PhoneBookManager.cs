using PhoneBooksLibrary.Entities;

namespace PhoneBooksLibrary
{
    public class PhoneBookManager
    {
        private readonly object _lock = new object();

        private Dictionary<string, PhoneBookDTO> _entries;

        private static string _path = "AllStoredPhonebooks.bin";


        public Dictionary<string, PhoneBookDTO> Entries
        {
            get { return _entries; }
            private set { _entries = value; }
        }
        public PhoneBookManager()
        {
            _entries = new Dictionary<string, PhoneBookDTO>();
        }

        public List<PhoneBookDTO> GetAllEntries()
        {
            lock (_lock)
            {
                try
                {
                    if (_entries == null || _entries.Count == 0)
                    {
                        return new List<PhoneBookDTO>();
                    }
                    DeserializeFromProtoBuf();
                    return _entries.Values.ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public PhoneBookDTO GetEntryByNumber(string phoneNumber)  
        {
            lock (_lock)
            {
                try
                {
                    DeserializeFromProtoBuf();
                    return _entries[phoneNumber];
                }
                catch (Exception)
                {
                    
                    throw;
                }
                
            }
        }
        public bool AddEntry(PhoneBookDTO phonebook)
        {
            lock (_lock)
            {
                try
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
                catch (Exception)
                {

                    throw;
                }
                
            }
        }


        public bool DeletePhonebook(string number)
        {
            lock (_lock)
            {
                try
                {
                    if (_entries.ContainsKey(number))
                    {
                        _entries.Remove(number);
                        SerializeToProtoBuf();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("The number does not exist in the phonebook");
                        return false;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
        }

        public bool EditEntry(string number, PhoneBookDTO newData)
        {
            lock (_lock)
            {
                try
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
                catch (Exception)
                {

                    throw;
                }
               
            }
        }


        public List<PhoneBookDTO> IterateEntriesByLastName()
        {
            lock (_lock)
            {
                try
                {
                    var sortedEntries = _entries.Values.OrderBy(entry => entry.LastName).ToList();
                    DeserializeFromProtoBuf();
                    return sortedEntries;
                }
                catch (Exception)
                {

                    throw;
                }
               
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

        public void DeserializeFromProtoBuf()
        {
            lock (_lock)
            {
                using (var file = File.OpenRead(_path))
                {
                    var phoneBooks = ProtoBuf.Serializer.Deserialize<List<PhoneBookDTO>>(file);
                    _entries = phoneBooks.ToDictionary(pb => pb.Number, pb => pb);
                }
            }

        }      
    }
}