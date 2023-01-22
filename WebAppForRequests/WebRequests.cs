using Microsoft.AspNetCore.Mvc;
using PhoneBooksLibrary;
using System.Collections.Generic;

namespace WebAppForRequests
{
    public class WebRequestsController : ControllerBase
    {
        PhoneBookManager phoneBookManager;

        [HttpGet]
        public IActionResult Get()
        {
            var phones = phoneBookManager.GetAllEntries();

            return Json(phones);
        }
    }
}
