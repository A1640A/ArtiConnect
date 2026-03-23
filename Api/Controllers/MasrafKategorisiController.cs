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
    [RoutePrefix("api/masrafkategorisi")]
    public class MasrafKategorisiController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif masraf kategorilerini getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllMasrafKategorileri()
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
                    var query = @"SELECT Id, KategoriAdi, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM MasrafKategorisi 
                                WHERE Durum = 1
                                ORDER BY KategoriAdi";

                    var kategoriler = connection.Query<MasrafKategorisiResponseModel>(query).ToList();
                    return Ok(kategoriler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorileri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre masraf kategorisini getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetMasrafKategorisiById(int id)
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
                    var query = @"SELECT Id, KategoriAdi, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM MasrafKategorisi 
                                WHERE Id = @Id AND Durum = 1";

                    var kategori = connection.QueryFirstOrDefault<MasrafKategorisiResponseModel>(query, new { Id = id });

                    if (kategori == null)
                        return NotFound();

                    return Ok(kategori);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorisi getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni masraf kategorisi ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddMasrafKategorisi(MasrafKategorisiRequestModel model)
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

                    // Aynı isimde kategori var mı kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE KategoriAdi = @KategoriAdi AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { KategoriAdi = model.KategoriAdi }) > 0;
                    if (exists)
                        return BadRequest("Bu isimde bir masraf kategorisi zaten mevcut.");

                    var now = DateTime.Now;
                    var query = @"INSERT INTO MasrafKategorisi
                                (KategoriAdi, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                VALUES
                                (@KategoriAdi, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        KategoriAdi = model.KategoriAdi,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1, // Aktif
                        Kod = model.Kod,
                        Sirket = model.Sirket
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Masraf kategorisi başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorisi eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Masraf kategorisi bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateMasrafKategorisi(int id, MasrafKategorisiRequestModel model)
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

                    // Önce kategorinin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Aynı isimde başka kategori var mı kontrol et
                    var nameCheckQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE KategoriAdi = @KategoriAdi AND Id != @Id AND Durum = 1";
                    var nameExists = connection.ExecuteScalar<int>(nameCheckQuery, new { KategoriAdi = model.KategoriAdi, Id = id }) > 0;
                    if (nameExists)
                        return BadRequest("Bu isimde başka bir masraf kategorisi zaten mevcut.");

                    var query = @"UPDATE MasrafKategorisi
                                SET KategoriAdi = @KategoriAdi,
                                    Kod = @Kod,
                                    Sirket = @Sirket,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        KategoriAdi = model.KategoriAdi,
                        Kod = model.Kod,
                        Sirket = model.Sirket,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Masraf kategorisi başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorisi güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Masraf kategorisini pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteMasrafKategorisi(int id)
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
                            // Önce kategorinin var olup olmadığını kontrol et
                            var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @Id AND Durum = 1";
                            var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }, transaction) > 0;
                            if (!exists)
                                return NotFound();

                            // Bu kategoriye bağlı masraflar var mı kontrol et
                            var relatedMasraflarQuery = "SELECT COUNT(1) FROM SubeMasraf WHERE MasrafKategorisiId = @KategoriId AND Durum = 1";
                            var hasRelatedMasraflar = connection.ExecuteScalar<int>(relatedMasraflarQuery, new { KategoriId = id }, transaction) > 0;

                            // Kategoriyi pasife çek
                            var query = @"UPDATE MasrafKategorisi
                                        SET Durum = 0,
                                            SilmeTarihi = @SilmeTarihi
                                        WHERE Id = @Id";

                            var parameters = new
                            {
                                Id = id,
                                SilmeTarihi = DateTime.Now
                            };

                            var affectedRows = connection.Execute(query, parameters, transaction);

                            // Eğer bağlı masraflar varsa, bu masrafların kategorisini null olarak güncelle
                            if (hasRelatedMasraflar)
                            {
                                var updateMasraflarQuery = @"UPDATE SubeMasraf
                                                         SET MasrafKategorisiId = NULL,
                                                             GuncellemeTarihi = @GuncellemeTarihi
                                                         WHERE MasrafKategorisiId = @KategoriId AND Durum = 1";

                                connection.Execute(updateMasraflarQuery, new { KategoriId = id, GuncellemeTarihi = DateTime.Now }, transaction);
                            }

                            transaction.Commit();

                            return Ok(new
                            {
                                Success = true,
                                AffectedRows = affectedRows,
                                RelatedMasraflarUpdated = hasRelatedMasraflar,
                                Message = "Masraf kategorisi başarıyla silindi." + (hasRelatedMasraflar ? " İlgili masrafların kategorileri kaldırıldı." : "")
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Masraf kategorisi silinirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorisi silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kategori adına göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchMasrafKategorisi(string term)
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
                    var query = @"SELECT Id, KategoriAdi, KayitTarihi, GuncellemeTarihi, Kod 
                                FROM MasrafKategorisi 
                                WHERE KategoriAdi LIKE @Term AND Durum = 1
                                ORDER BY KategoriAdi";

                    var kategoriler = connection.Query<MasrafKategorisiResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(kategoriler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorisi araması yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kategoriye ait masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("{id}/masraflar")]
        public IHttpActionResult GetMasraflarByKategori(int id)
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

                    // Önce kategorinin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.MasrafKategorisiId = @KategoriId AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(query, new { KategoriId = id }).ToList();

                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Kategori masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Masraf kategorisi istatistiklerini getirir
        /// </summary>
        [HttpGet]
        [Route("{id}/istatistik")]
        public IHttpActionResult GetKategoriIstatistikleri(int id)
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

                    // Önce kategorinin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Kategori bilgisini getir
                    var kategoriQuery = "SELECT KategoriAdi FROM MasrafKategorisi WHERE Id = @Id AND Durum = 1";
                    var kategoriAdi = connection.QueryFirstOrDefault<string>(kategoriQuery, new { Id = id });

                    // Toplam masraf sayısı ve tutar
                    var masraflarQuery = @"SELECT COUNT(Id) AS MasrafSayisi, 
                                          SUM(Tutar) AS ToplamTutar,
                                          MIN(Tutar) AS MinimumTutar,
                                          MAX(Tutar) AS MaksimumTutar,
                                          AVG(Tutar) AS OrtalamaTutar
                                      FROM SubeMasraf 
                                      WHERE MasrafKategorisiId = @KategoriId AND Durum = 1";

                    var masrafIstatistik = connection.QueryFirstOrDefault<dynamic>(masraflarQuery, new { KategoriId = id });

                    // Son 12 ay istatistikleri
                    var sonOnIkiAyQuery = @"SELECT FORMAT(Tarih, 'yyyy-MM') AS Ay, 
                                           SUM(Tutar) AS ToplamTutar,
                                           COUNT(Id) AS MasrafSayisi
                                       FROM SubeMasraf
                                       WHERE MasrafKategorisiId = @KategoriId AND Durum = 1
                                       AND Tarih >= DATEADD(MONTH, -12, GETDATE())
                                       GROUP BY FORMAT(Tarih, 'yyyy-MM')
                                       ORDER BY Ay DESC";

                    var aylikIstatistikler = connection.Query<dynamic>(sonOnIkiAyQuery, new { KategoriId = id }).ToList();

                    // Şube bazlı istatistikler
                    var subeBazliQuery = @"SELECT s.SubeAdi, 
                                          COUNT(sm.Id) AS MasrafSayisi,
                                          SUM(sm.Tutar) AS ToplamTutar
                                      FROM SubeMasraf sm
                                      JOIN Sube s ON sm.SubeId = s.Id
                                      WHERE sm.MasrafKategorisiId = @KategoriId AND sm.Durum = 1
                                      GROUP BY s.SubeAdi
                                      ORDER BY ToplamTutar DESC";

                    var subeBazliIstatistikler = connection.Query<dynamic>(subeBazliQuery, new { KategoriId = id }).ToList();

                    return Ok(new
                    {
                        KategoriId = id,
                        KategoriAdi = kategoriAdi,
                        MasrafIstatistik = masrafIstatistik,
                        AylikIstatistikler = aylikIstatistikler,
                        SubeBazliIstatistikler = subeBazliIstatistikler
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Kategori istatistikleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu masraf kategorisi ekler
        /// </summary>
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult AddBatchMasrafKategorileri([FromBody] List<MasrafKategorisiRequestModel> models)
        {
            if (models == null || !models.Any())
                return BadRequest("En az bir masraf kategorisi eklenmelidir");

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
                                // Aynı isimde kategori var mı kontrol et
                                var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE KategoriAdi = @KategoriAdi AND Durum = 1";
                                var exists = connection.ExecuteScalar<int>(checkQuery, new { KategoriAdi = model.KategoriAdi }, transaction) > 0;
                                if (exists)
                                    continue; // Bu isimde kategori zaten var, bu kategoriyi atla

                                var query = @"INSERT INTO MasrafKategorisi
                                            (KategoriAdi, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                            VALUES
                                            (@KategoriAdi, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

                                var parameters = new
                                {
                                    KategoriAdi = model.KategoriAdi,
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
                                Message = $"{newIds.Count} masraf kategorisi başarıyla eklendi."
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Masraf kategorileri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf kategorileri eklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}