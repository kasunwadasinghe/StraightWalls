using Newtonsoft.Json;
using StraightWalls.API.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StraightWalls.API.Controllers
{
    public class HomeController : Controller
    {

        string apiUrl = ConfigurationManager.AppSettings["baseurl"] ;
        string apiLogIn = ConfigurationManager.AppSettings["baseurl"] + "/api/LogIn";
        string apiHoliday = ConfigurationManager.AppSettings["baseurl"] + "/api/Holiday";
        HttpClient client;
        public HomeController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ActionResult> Index()
        {
            int empId =(int)Session["EmpLoyeeId"];
            var holiday = new List<HolidayViewModel>();
            HttpResponseMessage responseMessage = await client.GetAsync(apiHoliday + String.Format("/{0}",empId));
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                holiday = JsonConvert.DeserializeObject<List<HolidayViewModel>>(result);

            }
            if (holiday != null)
            {
                return View(holiday);
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogIn()
        {
            ViewBag.Message = "";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var employee = 0;
            HttpResponseMessage responseMessage = await client.GetAsync(apiLogIn+ String.Format("?UN={0}&PW={1}",model.UserName,model.Password));
            if (responseMessage.IsSuccessStatusCode)
            {
                var result  = responseMessage.Content.ReadAsStringAsync().Result;
                employee = JsonConvert.DeserializeObject<int>(result);

            }
            if (employee != 0)
            {
                Session["EmpLoyeeId"] = employee;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["EmpLoyeeId"] = null;
                ViewBag.Message = "Your user name or passowrd is incorrect";
                return View();
            }
        }

        public async Task<ActionResult> CancelHoliday(int id)
        {
            var holiday = new List<HolidayViewModel>();
            HttpResponseMessage responseMessage = await client.DeleteAsync(apiHoliday + String.Format("/{0}", id));
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                holiday = JsonConvert.DeserializeObject<List<HolidayViewModel>>(result);

            }
            if (holiday != null)
            {
                return View("Index", holiday);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HolidayRequest(HolidayViewModel model)
        {
            var isSuccess =false;
            model.EmployeeId = (int)Session["EmpLoyeeId"];
            HttpResponseMessage responseMessage = await client.PostAsJsonAsync(apiHoliday,model);
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;
                isSuccess = JsonConvert.DeserializeObject<bool>(result);
            }
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ApplyHoliday()
        {
            return View(new HolidayViewModel());
        }
    }
}