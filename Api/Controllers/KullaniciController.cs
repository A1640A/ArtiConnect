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
    [RoutePrefix("api/kullanici")]
    public class KullaniciController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif kullanıcıları getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllKullanicilar()
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
                    var query = @"SELECT k.*,
                                (SELECT STUFF(
                                    (SELECT ', ' + s.SubeAdi
                                     FROM KullaniciAktifSube kas
                                     JOIN Sube s ON kas.SubeId = s.Id
                                     WHERE kas.KullaniciId = k.Id AND kas.Durum = 1
                                     FOR XML PATH('')), 1, 2, '')) AS AktifSubeler
                            FROM Kullanici k
                            WHERE k.Durum = 1";

                    var kullanicilar = connection.Query<KullaniciResponseModel>(query).ToList();

                    return Ok(kullanicilar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Kullanıcılar getirilirken hata oluştu: {ex.Message}");
            }
        }  
    }
}