using Microsoft.AspNetCore.Mvc;
using PhoneBooksLibrary;
using PhoneBooksLibrary.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebAppForRequests
{
    [ApiController]
    [Route("api/phones")]
    public class WebRequestsController : ControllerBase
    {
        public static PhoneBookManager phoneBookManager = new PhoneBookManager();

        [HttpGet]
        [Description("Returns all the phone books of all Users(meaning for each phone number)")]
        [Route("api/phones")]
        public ActionResult<IEnumerable<PhoneBook>> GetAllEntries()
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
        [Route("api/phones/{phone}")]
        public ActionResult<PhoneBook> GetPhoneBook(string phone)
        {
            var phonebook = phoneBookManager.GetEntryNumber(phone);
            if (phonebook == null)
            {
                return NotFound($"No phonebook found with phone number {phone}");
            }
            return Ok(phonebook);
        }

        [HttpGet]
        [Description("Returns all phone books sorted by last name")]
        [Route("api/phones/lastname")]
        public ActionResult<IEnumerable<PhoneBook>> GetEntriesByLastName()
        {
            var entries = phoneBookManager.IterateEntriesByLastName();
            if (entries == null || !entries.Any())
            {
                return NotFound("No phonebooks found");
            }
            return Ok(entries);
        }



        [HttpPost]
        [Route("api/phones/add")]
        public ActionResult<PhoneBook> AddEntry(PhoneBook phoneBook)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = phoneBookManager.AddEntry(phoneBook);
            if (result)
            {
                return Ok(phoneBookManager.GetEntryNumber(phoneBook.Number));
            }
            else
            {
                return BadRequest("Error adding entry to phonebook. Number already exists.");
            }
        }


        [HttpDelete]
        [Route("api/phones/delete/{number}")]
        public ActionResult DeleteEntry(string number)
        {
            try
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
            catch (Exception ex)
            {
                //log the exception
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPatch]
        [Route("api/phones/update/{number}")]
        public ActionResult UpdateEntry(string number, PhoneBook newData)
        {
            bool isEdited = phoneBookManager.EditEntry(number, newData);
            if (isEdited)
            {
                return Ok(phoneBookManager.GetEntryNumber(number));
            }
            else
            {
                return BadRequest("Number not found in Phone Book");
            }
        }
    }

}






