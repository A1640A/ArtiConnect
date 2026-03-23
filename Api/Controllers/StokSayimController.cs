using ArtiConnect.Api.Modals;
using ArtiConnect.DataAccess;
using Dapper;
using DevExpress.CodeParser;
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
    [RoutePrefix("api/stoksayim")]
    public class StokSayimController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif stok sayım kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllStokSayimKartlari()
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.Durum = 1";

                    var stokSayimKartlari = connection.Query<StokSayimKartiResponseModel>(query).ToList();

                    // Her kart için stok sayım ürünlerini getir
                    foreach (var kart in stokSayimKartlari)
                    {
                        var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                        sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                        su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                        FROM StokSayimUrun su
                                        INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                        WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                        kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                            urunQuery,
                            new { KartId = kart.Id }
                        ).ToList();
                    }

                    return Ok(stokSayimKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre stok sayım kartı getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetStokSayimKartiById(int id)
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.Id = @Id AND sk.Durum = 1";

                    var kart = connection.QueryFirstOrDefault<StokSayimKartiResponseModel>(
                        query,
                        new { Id = id }
                    );

                    if (kart == null)
                        return NotFound();

                    // Stok sayım ürünlerini getir
                    var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                    sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                    su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                    FROM StokSayimUrun su
                                    INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                    WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                    kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                        urunQuery,
                        new { KartId = kart.Id }
                    ).ToList();

                    return Ok(kart);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım kartı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ID'sine göre stok sayım kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}")]
        public IHttpActionResult GetStokSayimKartlariBySubeId(int subeId)
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.SubeId = @SubeId AND sk.Durum = 1";

                    var kartlar = connection.Query<StokSayimKartiResponseModel>(
                        query,
                        new { SubeId = subeId }
                    ).ToList();

                    // Her kart için stok sayım ürünlerini getir
                    foreach (var kart in kartlar)
                    {
                        var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                        sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                        su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                        FROM StokSayimUrun su
                                        INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                        WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                        kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                            urunQuery,
                            new { KartId = kart.Id }
                        ).ToList();
                    }

                    return Ok(kartlar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şubeye ait stok sayım kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni stok sayım kartı ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddStokSayimKarti(StokSayimKartiRequestModel model)
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

                    // Transaction başlat
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var now = DateTime.Now;

                            // Stok Sayım Kartı ekle
                            var kartQuery = @"INSERT INTO StokSayimKarti 
                                           (BaslangicTarihi, BitisTarihi, SayimKodu, SubeId, KayitTarihi, GuncellemeTarihi, Durum) 
                                           VALUES 
                                           (@BaslangicTarihi, @BitisTarihi, @SayimKodu, @SubeId, @KayitTarihi, @GuncellemeTarihi, @Durum);
                                           SELECT CAST(SCOPE_IDENTITY() as int)";

                            var kartParameters = new
                            {
                                BaslangicTarihi = model.BaslangicTarihi,
                                BitisTarihi = model.BitisTarihi,
                                SayimKodu = model.SayimKodu,
                                SubeId = model.SubeId,
                                KayitTarihi = now,
                                GuncellemeTarihi = now,
                                Durum = 1 // Aktif
                            };

                            var kartId = connection.ExecuteScalar<int>(kartQuery, kartParameters, transaction);

                            // Stok Sayım Ürünlerini ekle
                            if (model.StokSayimUrunleri != null && model.StokSayimUrunleri.Any())
                            {
                                var urunQuery = @"INSERT INTO StokSayimUrun 
                                               (StokSayimKartiId, StokKartiId, Tarih, Devir, Giren, Cikan, Kalan, Sayim, Fark, KayitTarihi, GuncellemeTarihi, Durum) 
                                               VALUES 
                                               (@StokSayimKartiId, @StokKartiId, @Tarih, @Devir, @Giren, @Cikan, @Kalan, @Sayim, @Fark, @KayitTarihi, @GuncellemeTarihi, @Durum)";

                                foreach (var urun in model.StokSayimUrunleri)
                                {
                                    var urunParameters = new
                                    {
                                        StokSayimKartiId = kartId,
                                        StokKartiId = urun.StokKartiId,
                                        Tarih = urun.Tarih,
                                        Devir = urun.Devir,
                                        Giren = urun.Giren,
                                        Cikan = urun.Cikan,
                                        Kalan = urun.Kalan,
                                        Sayim = urun.Sayim,
                                        Fark = urun.Fark,
                                        KayitTarihi = now,
                                        GuncellemeTarihi = now,
                                        Durum = 1 // Aktif
                                    };

                                    connection.Execute(urunQuery, urunParameters, transaction);
                                }
                            }

                            transaction.Commit();

                            return Ok(new { Id = kartId, Success = true, Message = "Stok sayım kartı başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Stok sayım kartı eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok sayım kartını günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateStokSayimKarti(int id, StokSayimKartiRequestModel model)
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

                    // Önce stok sayım kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;

                    if (!exists)
                        return NotFound();

                    // Transaction başlat
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var now = DateTime.Now;

                            // Stok Sayım Kartını güncelle
                            var kartQuery = @"UPDATE StokSayimKarti 
                                           SET BaslangicTarihi = @BaslangicTarihi, 
                                               BitisTarihi = @BitisTarihi, 
                                               SayimKodu = @SayimKodu, 
                                               SubeId = @SubeId, 
                                               GuncellemeTarihi = @GuncellemeTarihi 
                                           WHERE Id = @Id";

                            var kartParameters = new
                            {
                                Id = id,
                                BaslangicTarihi = model.BaslangicTarihi,
                                BitisTarihi = model.BitisTarihi,
                                SayimKodu = model.SayimKodu,
                                SubeId = model.SubeId,
                                GuncellemeTarihi = now
                            };

                            connection.Execute(kartQuery, kartParameters, transaction);

                            // Mevcut ürünleri pasife çek (soft delete)
                            var deleteUrunQuery = @"UPDATE StokSayimUrun 
                                                 SET Durum = 0, 
                                                     SilmeTarihi = @SilmeTarihi 
                                                 WHERE StokSayimKartiId = @KartId";

                            connection.Execute(deleteUrunQuery, new { KartId = id, SilmeTarihi = now }, transaction);

                            // Yeni ürünleri ekle
                            if (model.StokSayimUrunleri != null && model.StokSayimUrunleri.Any())
                            {
                                var urunQuery = @"INSERT INTO StokSayimUrun 
                                               (StokSayimKartiId, StokKartiId, Tarih, Devir, Giren, Cikan, Kalan, Sayim, Fark, KayitTarihi, GuncellemeTarihi, Durum) 
                                               VALUES 
                                               (@StokSayimKartiId, @StokKartiId, @Tarih, @Devir, @Giren, @Cikan, @Kalan, @Sayim, @Fark, @KayitTarihi, @GuncellemeTarihi, @Durum)";

                                foreach (var urun in model.StokSayimUrunleri)
                                {
                                    var urunParameters = new
                                    {
                                        StokSayimKartiId = id,
                                        StokKartiId = urun.StokKartiId,
                                        Tarih = urun.Tarih,
                                        Devir = urun.Devir,
                                        Giren = urun.Giren,
                                        Cikan = urun.Cikan,
                                        Kalan = urun.Kalan,
                                        Sayim = urun.Sayim,
                                        Fark = urun.Fark,
                                        KayitTarihi = now,
                                        GuncellemeTarihi = now,
                                        Durum = 1 // Aktif
                                    };

                                    connection.Execute(urunQuery, urunParameters, transaction);
                                }
                            }

                            transaction.Commit();

                            return Ok(new { Success = true, Message = "Stok sayım kartı başarıyla güncellendi." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Stok sayım kartı güncellenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok sayım kartını pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteStokSayimKarti(int id)
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

                    // Önce stok sayım kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;

                    if (!exists)
                        return NotFound();

                    // Transaction başlat
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var now = DateTime.Now;

                            // Stok Sayım Kartını pasife çek
                            var kartQuery = @"UPDATE StokSayimKarti 
                                           SET Durum = 0, 
                                               SilmeTarihi = @SilmeTarihi 
                                           WHERE Id = @Id";

                            connection.Execute(kartQuery, new { Id = id, SilmeTarihi = now }, transaction);

                            // İlişkili ürünleri de pasife çek
                            var urunQuery = @"UPDATE StokSayimUrun 
                                           SET Durum = 0, 
                                               SilmeTarihi = @SilmeTarihi 
                                           WHERE StokSayimKartiId = @KartId";

                            connection.Execute(urunQuery, new { KartId = id, SilmeTarihi = now }, transaction);

                            transaction.Commit();

                            return Ok(new { Success = true, Message = "Stok sayım kartı başarıyla silindi." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Stok sayım kartı silinirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli bir tarih aralığındaki stok sayım kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("tarih")]
        public IHttpActionResult GetStokSayimKartlariByDateRange(DateTime baslangic, DateTime bitis)
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.BaslangicTarihi >= @Baslangic 
                                 AND sk.BitisTarihi <= @Bitis 
                                 AND sk.Durum = 1";

                    var kartlar = connection.Query<StokSayimKartiResponseModel>(
                        query,
                        new { Baslangic = baslangic, Bitis = bitis }
                    ).ToList();

                    // Her kart için stok sayım ürünlerini getir
                    foreach (var kart in kartlar)
                    {
                        var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                        sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                        su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                        FROM StokSayimUrun su
                                        INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                        WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                        kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                            urunQuery,
                            new { KartId = kart.Id }
                        ).ToList();
                    }

                    return Ok(kartlar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Tarih aralığındaki stok sayım kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok sayım kartına yeni ürün ekler
        /// </summary>
        [HttpPost]
        [Route("{kartId}/urun")]
        public IHttpActionResult AddStokSayimUrun(int kartId, StokSayimUrunRequestModel model)
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

                    // Önce stok sayım kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = kartId }) > 0;

                    if (!exists)
                        return NotFound();

                    var now = DateTime.Now;

                    var query = @"INSERT INTO StokSayimUrun 
                               (StokSayimKartiId, StokKartiId, Tarih, Devir, Giren, Cikan, Kalan, Sayim, Fark, KayitTarihi, GuncellemeTarihi, Durum) 
                               VALUES 
                               (@StokSayimKartiId, @StokKartiId, @Tarih, @Devir, @Giren, @Cikan, @Kalan, @Sayim, @Fark, @KayitTarihi, @GuncellemeTarihi, @Durum);
                               SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        StokSayimKartiId = kartId,
                        StokKartiId = model.StokKartiId,
                        Tarih = model.Tarih,
                        Devir = model.Devir,
                        Giren = model.Giren,
                        Cikan = model.Cikan,
                        Kalan = model.Kalan,
                        Sayim = model.Sayim,
                        Fark = model.Fark,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1 // Aktif
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);

                    return Ok(new { Id = newId, Success = true, Message = "Stok sayım ürünü başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım ürünü eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok sayım ürününü günceller
        /// </summary>
        [HttpPut]
        [Route("urun/{urunId}")]
        public IHttpActionResult UpdateStokSayimUrun(int urunId, StokSayimUrunRequestModel model)
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

                    // Önce stok sayım ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = urunId }) > 0;

                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE StokSayimUrun 
                               SET StokKartiId = @StokKartiId, 
                                   Tarih = @Tarih, 
                                   Devir = @Devir, 
                                   Giren = @Giren, 
                                   Cikan = @Cikan, 
                                   Kalan = @Kalan, 
                                   Sayim = @Sayim, 
                                   Fark = @Fark, 
                                   GuncellemeTarihi = @GuncellemeTarihi 
                               WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = urunId,
                        StokKartiId = model.StokKartiId,
                        Tarih = model.Tarih,
                        Devir = model.Devir,
                        Giren = model.Giren,
                        Cikan = model.Cikan,
                        Kalan = model.Kalan,
                        Sayim = model.Sayim,
                        Fark = model.Fark,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);

                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Stok sayım ürünü başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım ürünü güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok sayım ürününü pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("urun/{urunId}")]
        public IHttpActionResult DeleteStokSayimUrun(int urunId)
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

                    // Önce stok sayım ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = urunId }) > 0;

                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE StokSayimUrun 
                               SET Durum = 0, 
                                   SilmeTarihi = @SilmeTarihi 
                               WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = urunId,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);

                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Stok sayım ürünü başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım ürünü silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli bir stok kartına ait tüm sayım kayıtlarını getirir
        /// </summary>
        [HttpGet]
        [Route("stok/{stokKartiId}")]
        public IHttpActionResult GetStokSayimUrunleriByStokKartiId(int stokKartiId)
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

                    var query = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                FROM StokSayimUrun su
                                INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                WHERE su.StokKartiId = @StokKartiId AND su.Durum = 1";

                    var urunler = connection.Query<StokSayimUrunResponseModel>(
                        query,
                        new { StokKartiId = stokKartiId }
                    ).ToList();

                    return Ok(urunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartına ait sayım kayıtları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Sayım kodu ile stok sayım kartı getirir
        /// </summary>
        [HttpGet]
        [Route("kod/{sayimKodu}")]
        public IHttpActionResult GetStokSayimKartiBySayimKodu(string sayimKodu)
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.SayimKodu = @SayimKodu AND sk.Durum = 1";

                    var kart = connection.QueryFirstOrDefault<StokSayimKartiResponseModel>(
                        query,
                        new { SayimKodu = sayimKodu }
                    );

                    if (kart == null)
                        return NotFound();

                    // Stok sayım ürünlerini getir
                    var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                    sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                    su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                    FROM StokSayimUrun su
                                    INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                    WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                    kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                        urunQuery,
                        new { KartId = kart.Id }
                    ).ToList();

                    return Ok(kart);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Sayım kodu ile stok sayım kartı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu stok sayım ürünü ekleme
        /// </summary>
        [HttpPost]
        [Route("{kartId}/urunBulk")]
        public IHttpActionResult AddStokSayimUrunBulk(int kartId, List<StokSayimUrunRequestModel> models)
        {
            if (models == null || !models.Any())
                return BadRequest("Eklenecek ürün bulunamadı.");

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

                    // Önce stok sayım kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokSayimKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = kartId }) > 0;

                    if (!exists)
                        return NotFound();

                    // Transaction başlat
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var now = DateTime.Now;
                            int totalAffectedRows = 0;

                            var query = @"INSERT INTO StokSayimUrun 
                                       (StokSayimKartiId, StokKartiId, Tarih, Devir, Giren, Cikan, Kalan, Sayim, Fark, KayitTarihi, GuncellemeTarihi, Durum) 
                                       VALUES 
                                       (@StokSayimKartiId, @StokKartiId, @Tarih, @Devir, @Giren, @Cikan, @Kalan, @Sayim, @Fark, @KayitTarihi, @GuncellemeTarihi, @Durum)";

                            foreach (var model in models)
                            {
                                var parameters = new
                                {
                                    StokSayimKartiId = kartId,
                                    StokKartiId = model.StokKartiId,
                                    Tarih = model.Tarih,
                                    Devir = model.Devir,
                                    Giren = model.Giren,
                                    Cikan = model.Cikan,
                                    Kalan = model.Kalan,
                                    Sayim = model.Sayim,
                                    Fark = model.Fark,
                                    KayitTarihi = now,
                                    GuncellemeTarihi = now,
                                    Durum = 1 // Aktif
                                };

                                totalAffectedRows += connection.Execute(query, parameters, transaction);
                            }

                            transaction.Commit();

                            return Ok(new { Success = true, AffectedRows = totalAffectedRows, Message = $"{models.Count} adet stok sayım ürünü başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Stok sayım ürünleri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli bir şube ve tarih aralığına göre stok sayım raporunu getirir
        /// </summary>
        [HttpGet]
        [Route("rapor")]
        public IHttpActionResult GetStokSayimRaporu(int subeId, DateTime baslangic, DateTime bitis)
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

                    var query = @"SELECT sk.Id, sk.BaslangicTarihi, sk.BitisTarihi, sk.SayimKodu, 
                                 sk.SubeId, s.SubeAdi, sk.KayitTarihi
                                 FROM StokSayimKarti sk
                                 INNER JOIN Sube s ON sk.SubeId = s.Id
                                 WHERE sk.SubeId = @SubeId 
                                 AND sk.BaslangicTarihi >= @Baslangic 
                                 AND sk.BitisTarihi <= @Bitis 
                                 AND sk.Durum = 1";

                    var kartlar = connection.Query<StokSayimKartiResponseModel>(
                        query,
                        new { SubeId = subeId, Baslangic = baslangic, Bitis = bitis }
                    ).ToList();

                    // Her kart için stok sayım ürünlerini getir
                    foreach (var kart in kartlar)
                    {
                        var urunQuery = @"SELECT su.Id, su.StokSayimKartiId, su.StokKartiId, 
                                        sk.Kod as StokKodu, sk.StokAdi, su.Tarih, 
                                        su.Devir, su.Giren, su.Cikan, su.Kalan, su.Sayim, su.Fark
                                        FROM StokSayimUrun su
                                        INNER JOIN StokKarti sk ON su.StokKartiId = sk.Id
                                        WHERE su.StokSayimKartiId = @KartId AND su.Durum = 1";

                        kart.StokSayimUrunleri = connection.Query<StokSayimUrunResponseModel>(
                            urunQuery,
                            new { KartId = kart.Id }
                        ).ToList();
                    }

                    return Ok(kartlar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok sayım raporu getirilirken hata oluştu: {ex.Message}");
            }
        }
    }
}