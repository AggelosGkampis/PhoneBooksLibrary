using PhoneBooksLibrary.Entities;

namespace PhoneBooksLibrary
{
    /// <summary>
    /// PhoneBookManager class is used to manage a phone book stored as a dictionary of PhoneBookDTO objects.
    /// It has methods to add, delete, and edit entries, retrieve all entries or a specific entry by phone number,
    /// and retrieve entries sorted by last name. It also has methods to serialize the phone book to a binary file
    /// using the Protocol Buffers format and deserialize it from the same file. Additionally, the class uses a 
    /// private object "_lock" to prevent multiple threads from accessing the phone book at the same time.
    /// </summary>
    public class PhoneBookManager
    {
        private object _lock = new object();

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

        /// <summary>
        /// Gets all the phonebook entries
        /// </summary>
        /// <returns>List of PhoneBookDTO objects</returns>
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

        /// <summary>
        /// Gets phonebook entry for the given phone number
        /// </summary>
        /// <param name="phoneNumber">string phone number</param>
        /// <returns>PhoneBookDTO object</returns>
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

        /// <summary>
        /// Adds a new entry in the phonebook
        /// </summary>
        /// <param name="phonebook">PhoneBookDTO object</param>
        /// <returns>bool indicating success or failure</returns>
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

        /// <summary>
        /// Deletes phonebook entry for the given phone number
        /// </summary>
        /// <param name="number">string phone number</param>
        /// <returns>bool indicating success or failure</returns>
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

        /// <summary>
        /// Edits an existing phonebook entry
        /// </summary>
        /// <param name="number">string phone number of the entry to be edited</param>
        /// <param name="newData">PhoneBookDTO object with new data</param>
        /// <returns>bool indicating success or failure</returns>
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

        /// <summary>
        /// Iterates through all phonebook entries and returns a list of PhoneBookDTO objects sorted by last name
        /// </summary>
        /// <returns>List of PhoneBookDTO objects sorted by last name</returns>
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

        /// <summary>
        /// Serializes the phonebook to a binary file in Protocol Buffers format
        /// </summary>
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

        /// <summary>
        /// Deserializes the phonebook from a binary file in Protocol Buffers format
        /// </summary>
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