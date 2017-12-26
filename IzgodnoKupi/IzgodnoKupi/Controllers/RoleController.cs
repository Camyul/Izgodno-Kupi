using Bytes2you.Validation;
using IzgodnoKupi.Web.Models.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IzgodnoKupi.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<RoleViewModel> modelsList = new List<RoleViewModel>();

            modelsList = roleManager.Roles.Select(r => new RoleViewModel
            {
                RoleName = r.Name,
                Id = r.Id
            }).ToList();

            return View(modelsList);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole(string id)
        {
            RoleViewModel model = new RoleViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                IdentityRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                }
            }
            return View("AddApplicationRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole applicationRole =  new IdentityRole();

                applicationRole.Name = model.RoleName;

                IdentityResult roleRuslt =  await roleManager.CreateAsync(applicationRole);

                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RoleViewModel model = new RoleViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                IdentityRole applicationRole = await roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                }
            }
            return View("EditApplicationRole", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return NotFound();
                }

                IdentityRole applicationRole = await roleManager.FindByIdAsync(id);

                applicationRole.Name = model.RoleName;

                IdentityResult roleRuslt = await roleManager.UpdateAsync(applicationRole);

                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            string name = string.Empty;

            RoleViewModel model = new RoleViewModel();

            if (!String.IsNullOrEmpty(id))
            {
                IdentityRole applicationRole = await roleManager.FindByIdAsync(id);

                if (applicationRole != null)
                {
                    model.RoleName = applicationRole.Name;
                    model.Id = applicationRole.Id;
                }
                else
                {
                    return NotFound();
                }
            }
            return View("DeleteApplicationRole", model);
        }

        [HttpPost, ActionName("DeleteRole")]
        public async Task<IActionResult> DeleteApplicationRole(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                IdentityRole applicationRole = await roleManager.FindByIdAsync(id);

                if (applicationRole != null)
                {
                    IdentityResult roleResult = roleManager.DeleteAsync(applicationRole).Result;
                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
    }
}
