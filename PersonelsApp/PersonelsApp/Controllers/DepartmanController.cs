using PersonelsApp.Models;
using PersonelsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonelsApp.Controllers
{
    public class DepartmanController : Controller
    {
        PersonelDBEntities db = new PersonelDBEntities(); // database ile bağlantı kurabilmek için entitymodel imizi tanımlıyoruz

        // GET: Departman
        public ActionResult Index()
        {
            var model = db.Departmans.ToList();
            return View(model);
        }

        public ActionResult Yeni()
        {
            return View("DepartmanForms", new Departman());
        }

        public ActionResult Guncelle(int id)
        {
            var dep = db.Departmans.Find(id);
            return View("DepartmanForms", dep);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Kaydet(Departman departman)
        {
            MesajViewModel mesajView = new MesajViewModel();

            if (ModelState.IsValid)
            {
                if (departman.Id == 0)
                {
                    db.Departmans.Add(departman);
                    mesajView.Mesaj = departman.Ad + " başarıyla eklendi...";
                }
                else
                {
                    var model = db.Departmans.Find(departman.Id);
                    if (model != null)
                    {
                        model.Ad = departman.Ad;
                        mesajView.Mesaj = departman.Ad + " başarıyla güncellendi...";
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
            }
            else
            {
                return View("DepartmanForms");
            }

            db.SaveChanges();

            mesajView.Status = true;
            mesajView.LinkText = "Departman Listesi";
            mesajView.Url = "/Departman";

            return View("_Mesaj",mesajView);
        }

        public ActionResult Sil(int id)
        {
            var modelDepartman = db.Departmans.Find(id);
            var modelPersonel = db.Personels.Where(x => x.DepartmanId == id).ToList();

            if (modelDepartman != null)
            {
                if (modelPersonel != null)
                {
                    db.Departmans.Remove(modelDepartman);
                    foreach (var item in modelPersonel)
                    {
                        db.Personels.Remove(item);
                    }

                    db.SaveChanges();
                }
                else
                {
                    db.Departmans.Remove(modelDepartman);
                    db.SaveChanges();
                }
            }
            else
            {
                return HttpNotFound();
            }
            

            return RedirectToAction("Index");
        }
    }
}