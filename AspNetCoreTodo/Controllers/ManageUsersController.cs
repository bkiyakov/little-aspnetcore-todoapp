using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _usermanager;

        public ManageUsersController(UserManager<IdentityUser> usermanager)
        {
            _usermanager = usermanager;
        }

        public async Task<IActionResult> Index(){
            var admins = (await _usermanager.GetUsersInRoleAsync("Administator")).ToArray();
            var everyone = await _usermanager.Users.ToArrayAsync();

            var model = new ManageUsersViewModel(){
                Administrators = admins,
                Everyone = everyone
            };

            return View(model);
        }
    }
}