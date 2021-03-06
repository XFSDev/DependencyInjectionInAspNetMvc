﻿using ContosoUniversity.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using ContosoUniversity.RepositoryLayer.UnitOfWork;

namespace ContosoUniversity.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Department
        public ActionResult Index()
        {
            //var departments = db.Departments.Include(d => d.Administrator);
            IQueryable<Department> departments = _unitOfWork.Repository<Department>().GetAllEntities();
            //IEnumerable<Department> departments = _unitOfWork.Repository<Department>().GetAllEntity(/*includeProperties: "Administrator"*/);
            return View(departments.ToList());
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);

            //string query = "SELECT * FROM Department WHERE DepartmentID = @p0";
            //Department department = await db.Departments.SqlQuery(query, id).SingleOrDefaultAsync();
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            AdministratorDropDownList();
            return View();
        }

        public void AdministratorDropDownList(object selectedAdministator = null)
        {
            IEnumerable<Instructor> instructors = _unitOfWork.Repository<Instructor>().GetAllEntities(orderBy: q => q.OrderBy(x => x.FirstName));
            ViewBag.InstructorList = new SelectList(instructors, "ID", "FirstName", selectedAdministator);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().InsertEntity(department);
                _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<Department>().UpdateEntity(department);
                _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            AdministratorDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.Repository<Department>().DeleteEntity(id);
            _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
