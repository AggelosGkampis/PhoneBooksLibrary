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

            return Ok(phones);
        }


        [HttpGet]
        [Route("api/phonesN")]
        public ActionResult PhoneBook(string phone)
        {
            var das = phoneBookManager.GetEntryNumber(phone);

            return Ok(phone);
        }

        [HttpGet]
        [Route("api/phones/LastName")]
        public ActionResult<IEnumerable<PhoneBook>> GetEntriesByLastName()
        {
            var entries = phoneBookManager.IterateEntriesByLastName();
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
            phoneBookManager.AddEntry(phoneBook.Number, phoneBook);
            return Ok(phoneBookManager.GetEntryNumber(phoneBook.Number));
        }

        [HttpDelete]
        [Route("api/phones/delete")]
        public ActionResult DeleteEntry(string number)
        {
            phoneBookManager.DeleteEntry(number);
            if (!phoneBookManager._entries.ContainsKey(number))
            {
                return Ok("Number Deleted Successfully");
            }
            return BadRequest("Number not found in Phone Book");
        }

        [HttpPatch]
        [Route("api/phones/update/{number}")]
        public ActionResult UpdateEntry(string number, PhoneBook newData)
        {
            phoneBookManager.EditEntry(number, newData);
            return Ok(phoneBookManager.GetEntryNumber(number));
        }

    }

}






