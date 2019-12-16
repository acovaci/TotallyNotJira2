using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TotallyNotJira.Models;

namespace TotallyNotJira.Controllers
{
    public class Task1Controller : Controller
    {
        private TaskDBContext db = new TaskDBContext();
        // GET: Task
        public ActionResult Index()
        {
            ViewBag.Tasks = db.Tasks.OrderBy(x => x.TaskTitle);
            return View();
        }
        public ActionResult New()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public ActionResult New(Task1 task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    TempData["message"] = "Task added!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(task);
                }
            }
            catch (Exception e)
            {
                return View(task);
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Project = db.Tasks.Find(id);

            return View();
        }

        // POST: Project/Edit/5
        [HttpPut]
        public ActionResult Edit(int id, Task1 requiredTask)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Task1 task = db.Tasks.Find(id);

                    if (TryUpdateModel(task))
                    {
                        task.TaskTitle = requiredTask.TaskTitle;
                        task.TaskDescription = requiredTask.TaskDescription;
                        task.TaskStartDate = requiredTask.TaskStartDate;
                        task.TaskEndDate = requiredTask.TaskEndDate;
                        task.TaskStatus = requiredTask.TaskStatus;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requiredTask);
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Task1 task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }

}