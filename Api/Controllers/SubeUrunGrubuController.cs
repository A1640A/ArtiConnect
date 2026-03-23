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
    [RoutePrefix("api/subeurungrubu")]
    public class SubeUrunGrubuController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif şube ürün gruplarını getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSubeUrunGruplari()
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
                    var query = @"SELECT sug.SiraNo, sug.Id, sug.SubeId, sug.UrunGrubuAdi, sug.KayitTarihi, 
                                   sug.Kod, s.SubeAdi
                            FROM SubeUrunGrubu sug
                            LEFT JOIN Sube s ON sug.SubeId = s.Id
                            WHERE sug.Durum = 1";

                    var subeUrunGruplari = connection.Query<SubeUrunGrubuResponseModel>(query).ToList();
                    return Ok(subeUrunGruplari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grupları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre şube ürün grubunu getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubeUrunGrubuById(int id)
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
                    var query = @"SELECT sug.SiraNo, sug.Id, sug.SubeId, sug.UrunGrubuAdi, sug.KayitTarihi, 
                                   sug.Kod, s.SubeAdi
                            FROM SubeUrunGrubu sug
                            LEFT JOIN Sube s ON sug.SubeId = s.Id
                            WHERE sug.Id = @Id AND sug.Durum = 1";

                    var subeUrunGrubu = connection.QueryFirstOrDefault<SubeUrunGrubuResponseModel>(query, new { Id = id });

                    if (subeUrunGrubu == null)
                        return NotFound();

                    return Ok(subeUrunGrubu);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ID'sine göre ürün gruplarını getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}")]
        public IHttpActionResult GetSubeUrunGruplariBySubeId(int subeId)
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
                    var query = @"SELECT sug.SiraNo, sug.Id, sug.SubeId, sug.UrunGrubuAdi, sug.KayitTarihi, 
                                   sug.Kod, s.SubeAdi
                            FROM SubeUrunGrubu sug
                            LEFT JOIN Sube s ON sug.SubeId = s.Id
                            WHERE sug.SubeId = @SubeId AND sug.Durum = 1";

                    var subeUrunGruplari = connection.Query<SubeUrunGrubuResponseModel>(query, new { SubeId = subeId }).ToList();
                    return Ok(subeUrunGruplari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grupları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni şube ürün grubu ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddSubeUrunGrubu(SubeUrunGrubuRequestModel model)
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

                    // Aynı şube için aynı isimde grup var mı kontrolü
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE SubeId = @SubeId AND UrunGrubuAdi = @UrunGrubuAdi AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { SubeId = model.SubeId, UrunGrubuAdi = model.UrunGrubuAdi }) > 0;
                    if (exists)
                        return BadRequest("Bu şube için aynı isimde bir ürün grubu zaten mevcut.");

                    var now = DateTime.Now;
                    var query = @"INSERT INTO SubeUrunGrubu
                                (SubeId, UrunGrubuAdi, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                VALUES
                                (@SubeId, @UrunGrubuAdi, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        SubeId = model.SubeId,
                        UrunGrubuAdi = model.UrunGrubuAdi,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1, // Aktif
                        Kod = model.Kod,
                        Sirket = model.Sirket
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Şube ürün grubu başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürün grubu bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSubeUrunGrubu(int id, SubeUrunGrubuRequestModel model)
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

                    // Önce grubun var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Aynı şube için aynı isimde başka grup var mı kontrolü
                    var nameCheckQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE SubeId = @SubeId AND UrunGrubuAdi = @UrunGrubuAdi AND Id != @Id AND Durum = 1";
                    var nameExists = connection.ExecuteScalar<int>(nameCheckQuery, new { SubeId = model.SubeId, UrunGrubuAdi = model.UrunGrubuAdi, Id = id }) > 0;
                    if (nameExists)
                        return BadRequest("Bu şube için aynı isimde başka bir ürün grubu zaten mevcut.");

                    var query = @"UPDATE SubeUrunGrubu
                                SET SubeId = @SubeId,
                                    UrunGrubuAdi = @UrunGrubuAdi,
                                    Kod = @Kod,
                                    Sirket = @Sirket,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SubeId = model.SubeId,
                        UrunGrubuAdi = model.UrunGrubuAdi,
                        Kod = model.Kod,
                        Sirket = model.Sirket,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube ürün grubu başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürün grubunu pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubeUrunGrubu(int id)
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

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Önce grubun var olup olmadığını kontrol et
                            var checkQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE Id = @Id AND Durum = 1";
                            var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }, transaction) > 0;
                            if (!exists)
                                return NotFound();

                            // Bu gruba bağlı ürünler var mı kontrol et
                            var relatedProductsQuery = "SELECT COUNT(1) FROM SubeUrun WHERE SubeUrunGrubuId = @GrupId AND Durum = 1";
                            var hasRelatedProducts = connection.ExecuteScalar<int>(relatedProductsQuery, new { GrupId = id }, transaction) > 0;

                            // Eğer bağlı ürünler varsa onları da pasife çek
                            if (hasRelatedProducts)
                            {
                                var updateProductsQuery = @"UPDATE SubeUrun
                                                        SET Durum = 0,
                                                            SilmeTarihi = @SilmeTarihi
                                                        WHERE SubeUrunGrubuId = @GrupId AND Durum = 1";

                                connection.Execute(updateProductsQuery, new { GrupId = id, SilmeTarihi = DateTime.Now }, transaction);
                            }

                            // Grubu pasife çek
                            var query = @"UPDATE SubeUrunGrubu
                                        SET Durum = 0,
                                            SilmeTarihi = @SilmeTarihi
                                        WHERE Id = @Id";

                            var parameters = new
                            {
                                Id = id,
                                SilmeTarihi = DateTime.Now
                            };

                            var affectedRows = connection.Execute(query, parameters, transaction);

                            transaction.Commit();

                            return Ok(new
                            {
                                Success = true,
                                AffectedRows = affectedRows,
                                RelatedProductsAffected = hasRelatedProducts,
                                Message = "Şube ürün grubu başarıyla silindi." + (hasRelatedProducts ? " Gruba ait ürünler de silindi." : "")
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Şube ürün grubu silinirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürün grubu adına göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchSubeUrunGrubu(string term)
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
                    var query = @"SELECT sug.SiraNo, sug.Id, sug.SubeId, sug.UrunGrubuAdi, sug.KayitTarihi, 
                                   sug.Kod, s.SubeAdi
                            FROM SubeUrunGrubu sug
                            LEFT JOIN Sube s ON sug.SubeId = s.Id
                            WHERE sug.UrunGrubuAdi LIKE @Term AND sug.Durum = 1";

                    var subeUrunGruplari = connection.Query<SubeUrunGrubuResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(subeUrunGruplari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu araması yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Bir şube ürün grubu altındaki ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("{id}/urunler")]
        public IHttpActionResult GetUrunlerByGrupId(int id)
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

                    // Önce grubun var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.SubeUrunGrubuId = @GrupId AND su.Durum = 1";

                    var urunler = connection.Query<SubeUrunResponseModel>(query, new { GrupId = id }).ToList();

                    return Ok(urunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grubu ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu şube ürün grubu ekler
        /// </summary>
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult AddBatchSubeUrunGrubu([FromBody] List<SubeUrunGrubuRequestModel> models)
        {
            if (models == null || !models.Any())
                return BadRequest("En az bir şube ürün grubu eklenmelidir");

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
                                // Aynı şube için aynı isimde grup var mı kontrolü
                                var checkQuery = "SELECT COUNT(1) FROM SubeUrunGrubu WHERE SubeId = @SubeId AND UrunGrubuAdi = @UrunGrubuAdi AND Durum = 1";
                                var exists = connection.ExecuteScalar<int>(checkQuery, new { SubeId = model.SubeId, UrunGrubuAdi = model.UrunGrubuAdi }, transaction) > 0;
                                if (exists)
                                    continue; // Bu isimde grup zaten var, bu grubu atla

                                var query = @"INSERT INTO SubeUrunGrubu
                                            (SubeId, UrunGrubuAdi, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                            VALUES
                                            (@SubeId, @UrunGrubuAdi, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

                                var parameters = new
                                {
                                    SubeId = model.SubeId,
                                    UrunGrubuAdi = model.UrunGrubuAdi,
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
                                Message = $"{newIds.Count} şube ürün grubu başarıyla eklendi."
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Şube ürün grupları eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürün grupları eklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}