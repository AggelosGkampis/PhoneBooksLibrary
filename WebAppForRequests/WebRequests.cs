using Microsoft.AspNetCore.Mvc;
using PhoneBooksLibrary;
using PhoneBooksLibrary.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebAppForRequests
{
    [ApiController]
    [Route("api/phonebook")]
    public class WebRequestsController : ControllerBase
    {
        public static PhoneBookManager phoneBookManager = new PhoneBookManager();

        [HttpGet]
        [Description("Returns all the phone books of all Users(meaning for each phone number)")]
        [Route("get/all/phones")]

        public ActionResult<IEnumerable<PhoneBookDTO>> GetAllEntries()
        {
            var phones = phoneBookManager.GetAllEntries();
            if (phones == null || !phones.Any())
            {
                return NotFound("No phonebooks found");
            }
            return Ok(phones);

        }


        [HttpGet]
        [Description("Returns the phone book of a specific user by phone number")]
        [Route("get/phone/{phone}")]
        public ActionResult<PhoneBookDTO> GetPhoneBook(string phoneNumber)
        {
            var phonebook = phoneBookManager.GetEntryByNumber(phoneNumber);
            if (phonebook == null)
            {
                return NotFound($"No phonebook found with phone number {phoneNumber}");
            }
            return Ok(phonebook);
        }


        [HttpGet]
        [Description("Returns all phone books sorted by last name")]
        [Route("phones/by/users/lastname")]
        public ActionResult<IEnumerable<PhoneBookDTO>> GetEntriesByLastName()
        {
            var entries = phoneBookManager.IterateEntriesByLastName();
            if (entries == null || !entries.Any())
            {
                return NotFound("No phonebooks found");
            }
            return Ok(entries);
        }



        [HttpPost]
        [Route("add/phone")]
        public ActionResult<PhoneBookDTO> AddEntry(PhoneBookDTO phoneBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = phoneBookManager.AddEntry(phoneBook);
            if (result)
            {
                return Ok(phoneBookManager.GetEntryByNumber(phoneBook.Number));
            }
            else
            {
                return BadRequest("Error adding entry to phonebook. Number already exists.");
            }
        }


        [HttpDelete]
        [Route("delete/phone/{number}")]
        public ActionResult DeleteEntry(string number)
        {
           
                if (string.IsNullOrWhiteSpace(number))
                {
                    return BadRequest("Invalid phone number provided");
                }

                bool isDeleted = phoneBookManager.DeletePhonebook(number);
                if (isDeleted)
                {
                    return Ok("Number Deleted Successfully");
                }
                else
                {
                    return NotFound("Number not found in Phone Book");
                }
                   
        }


        [HttpPatch]
        [Route("update/phone/{number}")]
        public ActionResult UpdateEntry(string number, PhoneBookDTO newData)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return BadRequest("Invalid phone number provided");
            }

            bool isEdited = phoneBookManager.EditEntry(number, newData);
            if (isEdited)
            {
                return Ok(phoneBookManager.GetEntryByNumber(newData.Number));
            }
            else
            {
                return BadRequest("Number not found in Phone Book");
            }
        }



    }

}






