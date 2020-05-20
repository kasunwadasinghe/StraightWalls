using StraightWalls.API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StraightWalls.API.Controllers
{
    public class HolidayController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<HolidayViewModel> Get(int id)
        {
            using(StraightWallsEntities context = new StraightWallsEntities())
            {
                var holiday = context.HolidayManagements.Where(w=>w.employee_id == id).Select(s => new HolidayViewModel
                {
                    HolidayId = s.id,
                    CreatedOn = s.created_on,
                    From = s.from_date,
                    To = s.to_date,
                    Status = s.is_approved == null ? "Pending" : s.is_approved == true ? "Approved" : "Rejected",
                    isCanceled = s.is_canceled
                }).ToList();

                return holiday;
            }
        }

        // POST api/<controller>
        public bool? Post([FromBody]HolidayViewModel holiday)
        {
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                try
                {
                    double leavecount = 30;
                    var emp = context.Employees.Where(w => w.employee_id == holiday.EmployeeId).FirstOrDefault();
                    if (emp != null)
                    {
                        var service = (int)(DateTime.Now - emp.join_date.Value).TotalDays / 365;
                        if ((service / 5) > 1)
                        {
                            leavecount += (int)(service / 5);
                        }

                        var leaveutilized = context.HolidayManagements.Where(w => w.employee_id == holiday.EmployeeId).Select(s => new HolidayViewModel
                        {
                            From = s.from_date,
                            To = s.to_date
                        }).Sum(s => s.NoDays);
                        if (!(leavecount >= (leaveutilized + holiday.NoDays))){
                            var holidayManagement = new HolidayManagement
                            {
                                from_date = holiday.From,
                                to_date = holiday.To,
                                is_approved = null,
                                is_canceled = false,
                                employee_id = holiday.EmployeeId,
                                created_on = DateTime.Now,
                                updated_on = DateTime.Now,
                                created_by = holiday.EmployeeId.ToString(),
                                updated_by = holiday.EmployeeId.ToString()
                            };

                            context.HolidayManagements.Add(holidayManagement);
                            context.SaveChanges();

                            return true;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public IEnumerable<HolidayViewModel> Delete(int id)
        {
            using (StraightWallsEntities context = new StraightWallsEntities())
            {
                var holiday = context.HolidayManagements.Where(w => w.id == id).FirstOrDefault();
                var holidays = new List<HolidayViewModel>();
                if (holiday != null)
                {
                    holiday.is_canceled = true;
                    context.SaveChanges();

                    holidays = context.HolidayManagements.Where(w => w.employee_id == holiday.employee_id).Select(s => new HolidayViewModel
                    {
                        HolidayId = s.id,
                        CreatedOn = s.created_on,
                        From = s.from_date,
                        To = s.to_date,
                        Status = s.is_approved == null ? "Pending" : s.is_approved == true ? "Approved" : "Rejected",
                        isCanceled = s.is_canceled
                    }).ToList();
                }

                return holidays;
            }
        }
    }
}