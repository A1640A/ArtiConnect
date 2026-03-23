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
    [RoutePrefix("api/sube")]
    public class SubeController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif şubeleri getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSubeler()
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

                    var query = @"SELECT Id, SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi 
                                 FROM Sube 
                                 WHERE Durum = 1";

                    var subeler = connection.Query<SubeResponseModel>(query).ToList();

                    return Ok(subeler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şubeler getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre şube getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubeById(int id)
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

                    var query = @"SELECT Id, SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi 
                                 FROM Sube 
                                 WHERE Id = @Id AND Durum = 1";

                    var sube = connection.QueryFirstOrDefault<SubeResponseModel>(query, new { Id = id });

                    if (sube == null)
                        return NotFound();

                    return Ok(sube);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kod'a göre şube getirir
        /// </summary>
        [HttpGet]
        [Route("kod/{kod}")]
        public IHttpActionResult GetSubeByKod(string kod)
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

                    var query = @"SELECT Id, SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi 
                                 FROM Sube 
                                 WHERE Kod = @Kod AND Durum = 1";

                    var sube = connection.QueryFirstOrDefault<SubeResponseModel>(query, new { Kod = kod });

                    if (sube == null)
                        return NotFound();

                    return Ok(sube);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni şube ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddSube(SubeRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

                    // Önce aynı kodla şube var mı kontrol et
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var checkQuery = "SELECT COUNT(1) FROM Sube WHERE Kod = @Kod AND Durum = 1";
                        var exists = connection.ExecuteScalar<int>(checkQuery, new { Kod = model.Kod }) > 0;

                        if (exists)
                            return BadRequest("Bu kod ile kayıtlı bir şube zaten mevcut.");
                    }

                    var now = DateTime.Now;

                    var query = @"INSERT INTO Sube 
                                (SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi, GuncellemeTarihi, Durum) 
                                VALUES 
                                (@SubeAdi, @Adres, @Yetkili, @Telefon, @Kod, @KayitTarihi, @GuncellemeTarihi, @Durum);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        SubeAdi = model.SubeAdi,
                        Adres = model.Adres,
                        Yetkili = model.Yetkili,
                        Telefon = model.Telefon,
                        Kod = model.Kod,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1 // Aktif
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);

                    return Ok(new { Id = newId, Success = true, Message = "Şube başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSube(int id, SubeRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

                    // Önce şubenin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;

                    if (!exists)
                        return NotFound();

                    // Kod değiştirilmişse ve yeni kod başka bir şubede kullanılıyorsa kontrol et
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var kodCheckQuery = "SELECT COUNT(1) FROM Sube WHERE Kod = @Kod AND Id != @Id AND Durum = 1";
                        var kodExists = connection.ExecuteScalar<int>(kodCheckQuery, new { Kod = model.Kod, Id = id }) > 0;

                        if (kodExists)
                            return BadRequest("Bu kod ile kayıtlı başka bir şube zaten mevcut.");
                    }

                    var query = @"UPDATE Sube 
                                SET SubeAdi = @SubeAdi, 
                                    Adres = @Adres, 
                                    Yetkili = @Yetkili, 
                                    Telefon = @Telefon, 
                                    Kod = @Kod, 
                                    GuncellemeTarihi = @GuncellemeTarihi 
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SubeAdi = model.SubeAdi,
                        Adres = model.Adres,
                        Yetkili = model.Yetkili,
                        Telefon = model.Telefon,
                        Kod = model.Kod,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);

                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şubeyi pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSube(int id)
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

                    // Önce şubenin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;

                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE Sube 
                                SET Durum = 0, 
                                    SilmeTarihi = @SilmeTarihi 
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);

                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Birden fazla şubeyi toplu olarak getirir
        /// </summary>
        [HttpPost]
        [Route("getByIds")]
        public IHttpActionResult GetSubelerByIds([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Şube ID'leri boş olamaz");

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

                    var query = @"SELECT Id, SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi 
                                 FROM Sube 
                                 WHERE Id IN @Ids AND Durum = 1";

                    var subeler = connection.Query<SubeResponseModel>(query, new { Ids = ids }).ToList();

                    return Ok(subeler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şubeler getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube adına göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchSube(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Arama terimi boş olamaz");

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

                    var query = @"SELECT Id, SubeAdi, Adres, Yetkili, Telefon, Kod, KayitTarihi 
                                 FROM Sube 
                                 WHERE (SubeAdi LIKE @Term OR Kod LIKE @Term) AND Durum = 1";

                    var subeler = connection.Query<SubeResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(subeler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube araması yapılırken hata oluştu: {ex.Message}");
            }
        }
    }
}