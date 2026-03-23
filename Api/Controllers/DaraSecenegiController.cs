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
    [RoutePrefix("api/darasecenegi")]
    public class DaraSecenegiController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif dara seçeneklerini getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllDaraSecenekleri()
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
                    var query = @"SELECT Id, DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM DaraSecenegi 
                                WHERE Durum = 1
                                ORDER BY Gramaj";

                    var daraSecenekleri = connection.Query<DaraSecenegiResponseModel>(query).ToList();
                    return Ok(daraSecenekleri);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçenekleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre dara seçeneğini getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetDaraSecenegiById(int id)
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
                    var query = @"SELECT Id, DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM DaraSecenegi 
                                WHERE Id = @Id AND Durum = 1";

                    var daraSecenegi = connection.QueryFirstOrDefault<DaraSecenegiResponseModel>(query, new { Id = id });

                    if (daraSecenegi == null)
                        return NotFound();

                    return Ok(daraSecenegi);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçeneği getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni dara seçeneği ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddDaraSecenegi(DaraSecenegiRequestModel model)
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

                    // Aynı isimde dara seçeneği var mı kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM DaraSecenegi WHERE DaraAdi = @DaraAdi AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { DaraAdi = model.DaraAdi }) > 0;
                    if (exists)
                        return BadRequest("Bu isimde bir dara seçeneği zaten mevcut.");

                    var now = DateTime.Now;
                    var query = @"INSERT INTO DaraSecenegi
                                (DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                VALUES
                                (@DaraAdi, @Gramaj, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        DaraAdi = model.DaraAdi,
                        Gramaj = model.Gramaj,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1, // Aktif
                        Kod = model.Kod,
                        Sirket = model.Sirket
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Dara seçeneği başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçeneği eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Dara seçeneği bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateDaraSecenegi(int id, DaraSecenegiRequestModel model)
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

                    // Önce dara seçeneğinin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM DaraSecenegi WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Aynı isimde başka dara seçeneği var mı kontrol et
                    var nameCheckQuery = "SELECT COUNT(1) FROM DaraSecenegi WHERE DaraAdi = @DaraAdi AND Id != @Id AND Durum = 1";
                    var nameExists = connection.ExecuteScalar<int>(nameCheckQuery, new { DaraAdi = model.DaraAdi, Id = id }) > 0;
                    if (nameExists)
                        return BadRequest("Bu isimde başka bir dara seçeneği zaten mevcut.");

                    var query = @"UPDATE DaraSecenegi
                                SET DaraAdi = @DaraAdi,
                                    Gramaj = @Gramaj,
                                    Kod = @Kod,
                                    Sirket = @Sirket,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        DaraAdi = model.DaraAdi,
                        Gramaj = model.Gramaj,
                        Kod = model.Kod,
                        Sirket = model.Sirket,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Dara seçeneği başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçeneği güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Dara seçeneğini pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteDaraSecenegi(int id)
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

                    // Önce dara seçeneğinin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM DaraSecenegi WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE DaraSecenegi
                                SET Durum = 0,
                                    SilmeTarihi = @SilmeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Dara seçeneği başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçeneği silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Gramaj değerine göre dara seçeneklerini getirir
        /// </summary>
        [HttpGet]
        [Route("gramaj/{minGramaj}/{maxGramaj}")]
        public IHttpActionResult GetDaraSecenekleriByGramaj(float minGramaj, float maxGramaj)
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
                    var query = @"SELECT Id, DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM DaraSecenegi 
                                WHERE Gramaj >= @MinGramaj AND Gramaj <= @MaxGramaj AND Durum = 1
                                ORDER BY Gramaj";

                    var daraSecenekleri = connection.Query<DaraSecenegiResponseModel>(
                        query,
                        new { MinGramaj = minGramaj, MaxGramaj = maxGramaj }
                    ).ToList();

                    return Ok(daraSecenekleri);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçenekleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Dara adına göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchDaraSecenegi(string term)
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
                    var query = @"SELECT Id, DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM DaraSecenegi 
                                WHERE DaraAdi LIKE @Term AND Durum = 1
                                ORDER BY Gramaj";

                    var daraSecenekleri = connection.Query<DaraSecenegiResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(daraSecenekleri);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçeneği araması yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu dara seçeneği ekler
        /// </summary>
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult AddBatchDaraSecenekleri([FromBody] List<DaraSecenegiRequestModel> models)
        {
            if (models == null || !models.Any())
                return BadRequest("En az bir dara seçeneği eklenmelidir");

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
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var newIds = new List<int>();
                            var now = DateTime.Now;

                            foreach (var model in models)
                            {
                                // Aynı isimde dara seçeneği var mı kontrol et
                                var checkQuery = "SELECT COUNT(1) FROM DaraSecenegi WHERE DaraAdi = @DaraAdi AND Durum = 1";
                                var exists = connection.ExecuteScalar<int>(checkQuery, new { DaraAdi = model.DaraAdi }, transaction) > 0;
                                if (exists)
                                    continue; // Bu isimde dara seçeneği zaten var, bu seçeneği atla

                                var query = @"INSERT INTO DaraSecenegi
                                            (DaraAdi, Gramaj, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                            VALUES
                                            (@DaraAdi, @Gramaj, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

                                var parameters = new
                                {
                                    DaraAdi = model.DaraAdi,
                                    Gramaj = model.Gramaj,
                                    KayitTarihi = now,
                                    GuncellemeTarihi = now,
                                    Durum = 1, // Aktif
                                    Kod = model.Kod,
                                    Sirket = model.Sirket
                                };

                                var newId = connection.ExecuteScalar<int>(query, parameters, transaction);
                                newIds.Add(newId);
                            }

                            transaction.Commit();
                            return Ok(new
                            {
                                Success = true,
                                AddedCount = newIds.Count,
                                TotalCount = models.Count,
                                Ids = newIds,
                                Message = $"{newIds.Count} dara seçeneği başarıyla eklendi."
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Dara seçenekleri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Dara seçenekleri eklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}