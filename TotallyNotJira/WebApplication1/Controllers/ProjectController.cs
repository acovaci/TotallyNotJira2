using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TotallyNotJira.Models;

namespace TotallyNotJira.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Project
        [Authorize(Roles = "Member,Organizator,Administrator")]
        public ActionResult Index()
        {
            var projects = db.Projects.Include("Administrator").Include("Organizator");
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.Projects = db.Projects.OrderBy(x => x.Name);

            return View();
        }
        [Authorize(Roles = "Member,Organizator,Administrator")]
        public ActionResult Show(int id)
        {
            Project project = db.Projects.Find(id);
            return View(project);

        }
        // GET: Project/Create
        [Authorize(Roles = "Member,Organizator,Administrator")]
        public ActionResult New()
        {
            Project project = new Project();
            // Preluam ID-ul utilizatorului curent
            project.UserId = User.Identity.GetUserId();

            return View(project);
        }

        // POST: Project/Create
        [HttpPost]
        [Authorize(Roles = "Member, Organizator,Administrator")]
        public ActionResult New(Project project)
        {
            try
            {
                db.Projects.Add(project);
                db.SaveChanges();

                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));                UserManager.RemoveFromRole(project.UserId, "Member");                UserManager.AddToRole(project.UserId, "Organizator");
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Organizator,Administrator")]
        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.Project = project;
            if (project.UserId == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                return View(project);
            }
            else
            {
                TempData["message"] = "You can not modify this project!";
          return RedirectToAction("Index");
            }

        }

        [HttpPut]
        [Authorize(Roles = "Organizator,Administrator")]
        public ActionResult Edit(int id, Project requestProject)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    Project project = db.Projects.Find(id);
                    if (project.UserId == User.Identity.GetUserId() ||
                        User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(project))
                        {
                            project.Name = requestProject.Name;
                            project.Description = requestProject.Description;
                            db.SaveChanges();
                            TempData["message"] = "Project edited!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "You can not modify this project!";
                        return RedirectToAction("Index");
                    }

                }
                else
                {
                    return View(requestProject);
                }

            }
            catch (Exception e)
            {
                return View(requestProject);
            }
        }

        // GET: Project/Delete/5
        [HttpDelete]
        [Authorize(Roles = "Organizator,Administrator")]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            if (project.UserId == User.Identity.GetUserId() ||
            User.IsInRole("Administrator"))
            {
                db.Projects.Remove(project);
                db.SaveChanges();
                TempData["message"] = "Project deleted!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "You can not delete that project!";
            return RedirectToAction("Index");
            }
        }
    }
}
