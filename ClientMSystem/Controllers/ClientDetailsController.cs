using ClientMSystem.Data;
using ClientMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientMSystem.Controllers
{
    public class ClientDetailsController : Controller
    {
        private readonly ApplicationContext context;

        public ClientDetailsController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return View();
            }
            else
            {
                var result = context.clientDetails.Where(x => x.UserId == userId).ToList();
                return View(result);
            }
            
        }

        //Create


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ClientDetail model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                 

                if (ModelState.IsValid && userId.HasValue)
                {
                    var rec = new ClientDetail()
                    {
                        //Id = model.Id,
                        //UserId = model.UserId,
                        UserId = userId.Value,
                        Name = model.Name,
                        ClientName = model.ClientName,
                        IssuedDate = model.IssuedDate,
                        DomainName = model.DomainName,
                        Technology = model.Technology,
                        Assigned = model.Assigned,
                    };
                    context.clientDetails.Add(rec);
                    context.SaveChanges();
                    TempData["Message"] = "Sheet Updated Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Enter All Details";
                    return View(model);
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var rec = context.clientDetails.SingleOrDefault(e => e.Id == id);
                context.clientDetails.Remove(rec);
                context.SaveChanges();
                TempData["Error"] = "Update Delete Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult Edit(int id)
        {
            var model = context.clientDetails.SingleOrDefault(e => e.Id == id);
            var result = new ClientDetail()
            {
                UserId = model.UserId,
                Name = model.Name,
                ClientName = model.ClientName,
                IssuedDate = model.IssuedDate,
                DomainName = model.DomainName,
                Technology = model.Technology,
                Assigned = model.Assigned,

            };
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(ClientDetail model)
        {
            try
            {
                var rec = new ClientDetail()
                {
                    Id = model.Id,
                    UserId = model.UserId,
                    Name = model.Name,
                    ClientName = model.ClientName,
                    IssuedDate = model.IssuedDate,
                    DomainName = model.DomainName,
                    Technology = model.Technology,
                    Assigned = model.Assigned,

                };
                context.clientDetails.Update(rec);
                context.SaveChanges();
                TempData["Error"] = "Sheet Updated Successfully";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
