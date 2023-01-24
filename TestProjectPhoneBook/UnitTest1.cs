using PhoneBooksLibrary;
using PhoneBooksLibrary.Entities;
using PhoneBooksLibrary.Entities.Enums;

namespace TestProjectPhoneBook
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddEntryTest()
        {
            // When(given)

            PhoneBookDTO phoneBookDTO = new PhoneBookDTO("Aggelos", "Gkampis", PhoneType.Cellphone, "697865235");
            PhoneBookManager phoneBookManager = new PhoneBookManager();

            // Then(do)
            phoneBookManager.AddEntry(phoneBookDTO);

            //Assert
            Assert.IsTrue(phoneBookManager.GetEntryByNumber("697865235").FirstName == "Aggelos");
            Assert.IsTrue(phoneBookManager.GetEntryByNumber("697865235").LastName == "Gkampis");
            Assert.IsTrue(phoneBookManager.GetEntryByNumber("697865235").Type == PhoneType.Cellphone);
        }


        [Test]
        public void EditEntry()
        {
            // When(given)

            PhoneBookDTO phoneBookDTO = new PhoneBookDTO("Aggelos", "Gkampis", PhoneType.Cellphone, "697865235");
            PhoneBookManager phoneBookManager = new PhoneBookManager();

            // Then(do)
            phoneBookManager.AddEntry(phoneBookDTO);
            phoneBookManager.EditEntry("697865235", new PhoneBookDTO("Aggelos", "Gkampis", PhoneType.Cellphone, "699999999"));

            //Assert
            Assert.IsTrue(phoneBookManager.GetEntryByNumber("699999999") != null);
        }


    }
}