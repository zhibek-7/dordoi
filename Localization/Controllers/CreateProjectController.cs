using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Localization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProjectController : Controller
    {
        // GET: CreateProject
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateProject/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateProject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateProject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateProject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateProject/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateProject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateProject/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
