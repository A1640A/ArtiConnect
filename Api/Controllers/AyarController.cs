using ArtiConnect.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/ayar")]
    public class AyarsController : BaseApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm ayarları getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSettings()
        {
            var response = db.Ayars.ToList();
            return Ok(response);
        }
    }
}