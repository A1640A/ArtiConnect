using ArtiConnect.Api.Modals;
using ArtiConnect.DataAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/ayar")]
    public class AyarController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Mevcut sistem ayarlarını getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAyarlar()
        {
            try
            {
                var ayar = db.Ayars.FirstOrDefault();
                if (ayar == null)
                {
                    return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
                }

                var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"SELECT * FROM Ayar WHERE Durum = 1";

                    var sistemAyarlari = connection.QueryFirstOrDefault<AyarResponseModel>(query);

                    if (sistemAyarlari == null)
                        return NotFound();

                    return Ok(sistemAyarlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Ayarlar getirilirken hata oluştu: {ex.Message}");
            }
        }
    }
}