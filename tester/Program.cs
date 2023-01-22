using PhoneBooksLibrary;
using PhoneBooksLibrary.Entities;
using PhoneBooksLibrary.Entities.Enums;

class Program
{
    static void Main(string[] args)
    {
        var phoneBookManager = new PhoneBookManager();

        var phoneBook = new PhoneBook("John", "Doe", PhoneType.Home, "555-555-5555");

        phoneBookManager.AddEntry("555-555-5555", phoneBook);

        Console.WriteLine(phoneBookManager.ToString());

    }
}
