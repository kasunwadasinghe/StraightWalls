using StraightWalls.API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StraightWalls.API.Controllers
{
    public class LogInController : ApiController
    {
        // GET: api/LogIn
        public int Get(string UN,string PW)
        {
            using(StraightWallsEntities context = new StraightWallsEntities())
            {
                try
                {
                    
                    var user = context.Users.Where(w => w.user_name == UN && w.password == PW).Select(s => s.Employee).Where(w => w.is_active == true).FirstOrDefault();
                    if (user!=null)
                    {
                        return user.employee_id;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }
}
