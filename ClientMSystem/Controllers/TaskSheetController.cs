using ClientMSystem.Data;
using Microsoft.AspNetCore.Mvc;
using ClientMSystem.Models;
using IronPdf;

namespace ClientMSystem.Controllers
{
    public class TaskSheetController : Controller
    {
        private readonly ApplicationContext context;

        public TaskSheetController(ApplicationContext context)
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
                var result = context.timeSheets.ToList(); // to show thw details on view
                return View(result);
            }

            
        }

        //Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TimeSheet model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            try
            {
                if (ModelState.IsValid && userId.HasValue)
                {
                    var res = new TimeSheet()
                    {
                        Date = model.Date,
                        Module = model.Module,
                        ExpectedTaskToCompleted = model.ExpectedTaskToCompleted,
                        ExpectedHours = model.ExpectedHours,
                        CompletedTasks = model.CompletedTasks,
                        UnPlannedTask = model.UnPlannedTask,
                        ActualHours = model.ActualHours,
                        CommentsForAnyDealy = model.CommentsForAnyDealy,
                        QuestionsActionsToBeAsked = model.QuestionsActionsToBeAsked,
                    };
                    context.timeSheets.Add(res);
                    context.SaveChanges();
                    TempData["error"] = "Sheet Updated Successfully";
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
                var rec = context.timeSheets.SingleOrDefault(e => e.Id == id);
                context.timeSheets.Remove(rec);
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
            var model = context.timeSheets.SingleOrDefault(e => e.Id == id);
            var result = new TimeSheet()
            {
               
                Date = model.Date,
                Module = model.Module,
                ExpectedTaskToCompleted = model.ExpectedTaskToCompleted,
                ExpectedHours = model.ExpectedHours,
                CompletedTasks = model.CompletedTasks,
                UnPlannedTask = model.UnPlannedTask,
                ActualHours = model.ActualHours,
                CommentsForAnyDealy = model.CommentsForAnyDealy,
                QuestionsActionsToBeAsked = model.QuestionsActionsToBeAsked,

            };
            return View(result);
        }
        [HttpPost]
        public IActionResult Edit(TimeSheet model)
        {
            try
            {
                var rec = new TimeSheet()
                {
                    Id = model.Id,
                    Date = model.Date,
                    Module = model.Module,
                    ExpectedTaskToCompleted = model.ExpectedTaskToCompleted,
                    ExpectedHours = model.ExpectedHours,
                    CompletedTasks = model.CompletedTasks,
                    UnPlannedTask = model.UnPlannedTask,
                    ActualHours = model.ActualHours,
                    CommentsForAnyDealy = model.CommentsForAnyDealy,
                    QuestionsActionsToBeAsked = model.QuestionsActionsToBeAsked,

                };
                context.timeSheets.Update(rec);
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
