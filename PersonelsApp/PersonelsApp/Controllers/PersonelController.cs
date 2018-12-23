using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PersonelsApp.ViewModel;
using PersonelsApp.Models;

namespace PersonelsApp.Controllers
{
    public class PersonelController : Controller
    {
        PersonelDBEntities db = new PersonelDBEntities();

        // GET: Personel
        public ActionResult Index()
        {
            var model = db.Personels.Include(x => x.Departman).ToList();
            return View(model);
        }

        public ActionResult Yeni()
        {
            var model = new PersonelViewModel()
            {
                Departmanlar = db.Departmans.ToList(),
                Personel = new Personel()
            };

            return View("PersonelForms", model);
        }

        public ActionResult Guncelle(Personel personel)
        {

            var model = new PersonelViewModel
            {
                Personel = db.Personels.Find(personel.Id),
                Departmanlar = db.Departmans.ToList()
            };

            return View("PersonelForms", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Kaydet(Personel personel)
        {
            if (ModelState.IsValid)
            {
                if (personel.Id == 0)
                {
                    db.Personels.Add(personel);
                }
                else
                {
                    //var model = db.Personels.Find(personelViewModel.Personel.Id);
                    //if (model != null)
                    //{
                        //model.Ad = personelViewModel.Personel.Ad;
                        //model.Soyad = personelViewModel.Personel.Soyad;
                        //model.Maas = personelViewModel.Personel.Maas;
                        //model.Yas = personelViewModel.Personel.Yas;
                        //model.DogumTarihi = personelViewModel.Personel.DogumTarihi;
                        //model.Cinsiyet = personelViewModel.Personel.Cinsiyet;
                        //model.EvliMi = personelViewModel.Personel.EvliMi;
                        //model.DepartmanId = personelViewModel.Personel.DepartmanId;

                        db.Entry(personel).State = EntityState.Modified;
                        
                    //}
                    //else
                    //{
                    //    return HttpNotFound();
                    //}
                }
            }
            else
            {
                var model = new PersonelViewModel
                {
                    Departmanlar = db.Departmans.ToList(),
                    Personel = personel
                };

                return View("PersonelForms", model);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Sil(int id)
        {
            var model = db.Personels.Find(id);

            if (model != null)
            {
                //db.Personels.Remove(model);
                db.Entry(model).State = EntityState.Deleted;
                db.SaveChanges();
            }
            else
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        public ActionResult _PersonelleriListele(int id)
        {
            var model = db.Personels.Where(x => x.DepartmanId == id).ToList();

            return PartialView(model);
        }

        public int? ToplamMaas()
        {
            return db.Personels.Sum(x => x.Maas);
        }
    }
}