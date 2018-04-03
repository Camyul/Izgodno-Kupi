using IzgodnoKupi.Models;
using IzgodnoKupi.Web.Areas.Admin.Models.UserViewModel;
using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index(int? page)
        {
            List<UserListViewModel> users = new List<UserListViewModel>();

            users = userManager.Users
                               .Select(u => new UserListViewModel
                               {
                                   Id = u.Id,
                                   Email = u.Email
                                   //RoleName = roleManager.Roles.FirstOrDefault(r => r.Id == u.).Name
                                   //RoleName = userManager.GetRolesAsync(u).Result.Single().ToString()
                               })
                               .ToList();

            Pager pager = new Pager(users.Count(), page);

            IndexUserViewModel viewPageIndexModel = new IndexUserViewModel
            {
                Items = users.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult SearchUser(string searchTerm, int? page)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            List<UserListViewModel>  users = userManager.Users
                                                   .Where(x => x.Email.ToString().ToLower().Contains(searchTerm.ToLower()))
                                                   .Select(u => new UserListViewModel
                                                   {
                                                       Id = u.Id,
                                                       Email = u.Email
                                                       //RoleName = roleManager.Roles.FirstOrDefault(r => r.Id == u.).Name
                                                       //RoleName = userManager.GetRolesAsync(u).Result.Single().ToString()
                                                   })
                                                   .ToList();

            Pager pager = new Pager(users.Count(), page);

            IndexUserViewModel viewPageIndexModel = new IndexUserViewModel
            {
                Items = users.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            ViewData["searchTerm"] = searchTerm;

            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();

            model.ApplicationRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            return View("AddUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    IdentityRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);

                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);

                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            EditUserViewModel model = new EditUserViewModel();

            model.ApplicationRoles = roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            if (!String.IsNullOrEmpty(id))
            {
                User user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    model.Email = user.Email;

                    string role = userManager.GetRolesAsync(user).Result.Single();

                    model.ApplicationRoleId = roleManager.Roles.Single(r => r.Name == role).Id;
                }
            }
            return View("EditUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    user.UserName = model.Email;
                    user.Email = model.Email;

                    string existingRole = userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = roleManager.Roles.Single(r => r.Name == existingRole).Id;

                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleResult = await userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                IdentityRole applicationRole = await roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            string name = string.Empty;
            UserListViewModel model = new UserListViewModel();

            if (!String.IsNullOrEmpty(id))
            {
                User applicationUser = await userManager.FindByIdAsync(id);


                if (applicationUser != null)
                {
                    model.Id = applicationUser.Id;
                    model.Email = applicationUser.Email;
                    model.RoleName = userManager.GetRolesAsync(applicationUser).Result.Single();
                }
            }
            return View("DeleteUser", model);
        }

        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                User applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(applicationUser);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View("Index");
        }
    }
}
