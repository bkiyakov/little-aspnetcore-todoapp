using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<IdentityUser> _usermanager;
        
        public TodoController(ITodoItemService todoItemService, UserManager<IdentityUser> userManager)
        {
            _todoItemService = todoItemService;
            _usermanager = userManager;
        }
        public async Task<IActionResult> Index(){
            var currentUser = await _usermanager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

            var model = new TodoViewModel
            {
                Items = items
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem){
            var currentUser = await _usermanager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            if (!ModelState.IsValid){
                return RedirectToAction("Index");
            }

            var succesfull = await _todoItemService.AddItemAsync(newItem, currentUser);

            if(!succesfull){
                return BadRequest("Could not add item");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id){
            if (id == Guid.Empty){
                return RedirectToAction("Index");
            }

            var currentUser = await _usermanager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var succesfull = await _todoItemService.MarkDoneAsync(id, currentUser);
        
            if (!succesfull){
                return BadRequest("Could not mark item as done");
            }

            return RedirectToAction("Index");
        }
    }
}