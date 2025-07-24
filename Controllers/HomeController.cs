using ASPMVCEntity.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPMVCEntity.Controllers
{
    public class HomeController : Controller
    {
        StudentRegistrationEntities s = new StudentRegistrationEntities();
        public ActionResult Index()
        {
            var res = s.getStudent().ToList<getStudent_Result>();
            return View(res);
        }
        public ActionResult Create()
        {
            ViewBag.degree = new SelectList( s.BindDtls("degree", "").ToList<BindDtls_Result>(),"Id","Name");
            ViewBag.branch= new SelectList(s.BindDtls("branch", "").ToList<BindDtls_Result>(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student_Info st, HttpPostedFileBase photo, HttpPostedFileBase doc)
        {
            var idParameter = new ObjectParameter("id", typeof(int));
            string photoPath = "";
            string docPath = "";
            if (photo != null && photo.ContentLength > 0)
            {
                string fileName = Path.GetFileName(photo.FileName);
                string filePath = Server.MapPath("~/Files/Images/" + fileName);
                photo.SaveAs(filePath);
                photoPath = "/Files/Images/" + fileName;
            }
            if (doc != null && doc.ContentLength > 0)
            {
                string docName = Path.GetFileName(doc.FileName);
                string docFilePath = Server.MapPath("~/Files/Docs/" + docName);
                doc.SaveAs(docFilePath);
                docPath = "/Files/Docs/" + docName;
            }
            s.insertStudent(st.studentName, st.dob, st.gender, st.degreeId, st.branchId, st.emailId, st.mobileNo, st.iActive, photoPath, docPath, idParameter);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var res = s.Student_Info.Where(s => s.SrNo == id);

            return View(res);
        }
        [HttpPost]
        public ActionResult Edit(Student_Info st, HttpPostedFileBase photo, HttpPostedFileBase doc)
        {
            string photoPath = "";
            string docPath = "";
            if (photo != null && photo.ContentLength > 0)
            {
                string fileName = Path.GetFileName(photo.FileName);
                string filePath = Server.MapPath("~/Files/Images/" + fileName);
                photo.SaveAs(filePath);
                photoPath = "/Files/Images/" + fileName;
            }
            if (doc != null && doc.ContentLength > 0)
            {
                string docName = Path.GetFileName(doc.FileName);
                string docFilePath = Server.MapPath("~/Files/Docs/" + docName);
                docPath = "/Files/Docs/" + docName;
            }
            s.updateStudent(st.studentName, st.dob, st.gender, st.degreeId, st.branchId, st.emailId, st.mobileNo, st.iActive, photoPath, docPath, st.SrNo);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            s.deleteStudent(id);
            return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}