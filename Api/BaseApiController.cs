using ArtiConnect.DataAccess;
using ArtiConnect.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ArtiConnect.Api
{
    public class BaseApiController : ApiController
    {
        protected AppDbContext db = new AppDbContext(); 
    }
}
