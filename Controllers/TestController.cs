using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using productOpp.EDM;
using System.IO;

namespace productOpp.Controllers
{
    public class TestController : Controller
    {
        ProductCrudEntities dc = new ProductCrudEntities();

        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(tblproduct obj, HttpPostedFileBase file)
        {
            var allowedExtension = new[] { ".JPG", ".jpg", ".png", ".PNG", ".jpeg" };

            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName);

                if(allowedExtension.Contains(ext))
                {
                    var filename = file.FileName;
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    file.SaveAs(path);
                    obj.product_photo = filename;
                    dc.tblproducts.Add(obj);
                    dc.SaveChanges();
                    ViewBag.msg = "DATA submitted";
                }
            }
            else
            {
                ViewBag.msg = "your image extension should be in .JPG, .jpg, .png, .PNG, .jpeg";
            }
            return View();
        }
        public ActionResult display()
        {
            var c = dc.tblproducts.ToList();
            return View(c);
        }

        public ActionResult Edit(int id)
        {
            var pp = dc.tblproducts.Find(id);
            return View(pp);
            
        }
        [HttpPost]
        public ActionResult Edit(tblproduct obj, HttpPostedFileBase file)
        {
            var allowedExtension = new[] { ".JPG", ".jpg", ".jpeg", ".JPEG", ".PNG", ".png" };
            if (file != null)
            { 
                var ext = Path.GetExtension(file.FileName);
                if(allowedExtension.Contains(ext))
                {
                    var filename = file.FileName;
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    obj.product_photo = filename;
                    file.SaveAs(path);

                    dc.Entry(obj).State = EntityState.Modified;
                    dc.SaveChanges();
                }
                else
                {
                    ViewBag.msg = "only image is uploaded";
                    return View();
                }
            }
            //return RedirectToAction("Display");
            return View();
        }
        //public ActionResult Delete(int id)
        //{
        //    db.Employees.Remove(db.Employees.Find(id));
        //    db.SaveChanges();
        //    return RedirectToAction("Display");
        //}
        public ActionResult Delete(int id)
        {
            dc.tblproducts.Remove(dc.tblproducts.Find(id));
            dc.SaveChanges();
            return RedirectToAction("display");
        }
    }
}