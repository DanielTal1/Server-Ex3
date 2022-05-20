﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server_try.Data;

namespace server_try.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationController : Controller
    {
        private readonly server_tryContext _context;

        public InvitationController(server_tryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dictionary<string, string> data)
        {
            string from = data["from"];
            string to = data["to"];
            string server = data["server"];
            var currentUser = await _context.User.Include(x => x.ContactsList).FirstOrDefaultAsync(u => u.Id == to);
            if (currentUser == null)
            {
                return Json("user wasnt found");
            }
            var checkIfAlreadyContact = currentUser.ContactsList.Where(m => m.id == from);
            if (checkIfAlreadyContact.Any())
            {
                return Json("Already Contact");
            }
            var addedContact = new Contact(from, currentUser.Id, from, server);
            currentUser.ContactsList.Add(addedContact);
            await _context.SaveChangesAsync();
            return Json("Success");
        }
    }
}