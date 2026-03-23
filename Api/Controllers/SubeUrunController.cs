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
    [RoutePrefix("api/subeurun")]
    public class SubeUrunController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif şube ürünlerini getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSubeUrunler()
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre şube ürünü getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubeUrunById(int id)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Id = @Id AND su.Durum = 1";

                    var subeUrun = connection.QueryFirstOrDefault<SubeUrunResponseModel>(query, new { Id = id });

                    if (subeUrun == null)
                        return NotFound();

                    return Ok(subeUrun);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Barkod'a göre şube ürünü getirir
        /// </summary>
        [HttpGet]
        [Route("barkod/{barkod}")]
        public IHttpActionResult GetSubeUrunByBarkod(string barkod)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.BarkodNo = @Barkod AND su.Durum = 1";

                    var subeUrun = connection.QueryFirstOrDefault<SubeUrunResponseModel>(query, new { Barkod = barkod });

                    if (subeUrun == null)
                        return NotFound();

                    return Ok(subeUrun);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok kodu'na göre şube ürünü getirir
        /// </summary>
        [HttpGet]
        [Route("stokkodu/{stokkodu}")]
        public IHttpActionResult GetSubeUrunByStokKodu(string stokkodu)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.StokKodu = @StokKodu AND su.Durum = 1";

                    var subeUrun = connection.QueryFirstOrDefault<SubeUrunResponseModel>(query, new { StokKodu = stokkodu });

                    if (subeUrun == null)
                        return NotFound();

                    return Ok(subeUrun);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kod'a göre şube ürünü getirir
        /// </summary>
        [HttpGet]
        [Route("kod/{kod}")]
        public IHttpActionResult GetSubeUrunByKod(string kod)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Kod = @Kod AND su.Durum = 1";

                    var subeUrun = connection.QueryFirstOrDefault<SubeUrunResponseModel>(query, new { Kod = kod });

                    if (subeUrun == null)
                        return NotFound();

                    return Ok(subeUrun);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni şube ürünü ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddSubeUrun(SubeUrunRequestModel model)
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

                    // Barkod kontrolü
                    if (!string.IsNullOrEmpty(model.BarkodNo))
                    {
                        var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE BarkodNo = @BarkodNo AND Durum = 1";
                        var exists = connection.ExecuteScalar<int>(checkQuery, new { BarkodNo = model.BarkodNo }) > 0;
                        if (exists)
                            return BadRequest("Bu barkod ile kayıtlı bir şube ürünü zaten mevcut.");
                    }

                    // Kod kontrolü
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Kod = @Kod AND Durum = 1";
                        var exists = connection.ExecuteScalar<int>(checkQuery, new { Kod = model.Kod }) > 0;
                        if (exists)
                            return BadRequest("Bu kod ile kayıtlı bir şube ürünü zaten mevcut.");
                    }

                    var now = DateTime.Now;
                    var query = @"INSERT INTO SubeUrun
                                (SubeId, StokKodu, BarkodNo, SubeUrunGrubuId, UrunAdi, 
                                 AlisFiyat, Fiyat, KdvOrani, PosEkranindaGoster, ElTerminalindeGoster,
                                 KayitTarihi, GuncellemeTarihi, Durum, Kod, UrunBirimiId, Favori, Sirket)
                                VALUES
                                (@SubeId, @StokKodu, @BarkodNo, @SubeUrunGrubuId, @UrunAdi, 
                                 @AlisFiyat, @Fiyat, @KdvOrani, @PosEkranindaGoster, @ElTerminalindeGoster,
                                 @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @UrunBirimiId, @Favori, @Sirket);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        SubeId = model.SubeId,
                        StokKodu = model.StokKodu,
                        BarkodNo = model.BarkodNo,
                        SubeUrunGrubuId = model.SubeUrunGrubuId,
                        UrunAdi = model.UrunAdi,
                        AlisFiyat = model.AlisFiyat,
                        Fiyat = model.Fiyat,
                        KdvOrani = model.KdvOrani,
                        PosEkranindaGoster = model.PosEkranindaGoster,
                        ElTerminalindeGoster = model.ElTerminalindeGoster,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1, // Aktif
                        Kod = model.Kod,
                        UrunBirimiId = model.UrunBirimiId,
                        Favori = model.Favori,
                        Sirket = model.Sirket
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Şube ürünü başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürünü bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSubeUrun(int id, SubeUrunRequestModel model)
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

                    // Önce şube ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Barkod değiştirilmişse ve yeni barkod başka bir şube ürününde kullanılıyorsa kontrol et
                    if (!string.IsNullOrEmpty(model.BarkodNo))
                    {
                        var barkodCheckQuery = "SELECT COUNT(1) FROM SubeUrun WHERE BarkodNo = @BarkodNo AND Id != @Id AND Durum = 1";
                        var barkodExists = connection.ExecuteScalar<int>(barkodCheckQuery, new { BarkodNo = model.BarkodNo, Id = id }) > 0;
                        if (barkodExists)
                            return BadRequest("Bu barkod ile kayıtlı başka bir şube ürünü zaten mevcut.");
                    }

                    // Kod değiştirilmişse ve yeni kod başka bir şube ürününde kullanılıyorsa kontrol et
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var kodCheckQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Kod = @Kod AND Id != @Id AND Durum = 1";
                        var kodExists = connection.ExecuteScalar<int>(kodCheckQuery, new { Kod = model.Kod, Id = id }) > 0;
                        if (kodExists)
                            return BadRequest("Bu kod ile kayıtlı başka bir şube ürünü zaten mevcut.");
                    }

                    var query = @"UPDATE SubeUrun
                                SET SubeId = @SubeId,
                                    StokKodu = @StokKodu,
                                    BarkodNo = @BarkodNo,
                                    SubeUrunGrubuId = @SubeUrunGrubuId,
                                    UrunAdi = @UrunAdi,
                                    AlisFiyat = @AlisFiyat,
                                    Fiyat = @Fiyat,
                                    KdvOrani = @KdvOrani,
                                    PosEkranindaGoster = @PosEkranindaGoster,
                                    ElTerminalindeGoster = @ElTerminalindeGoster,
                                    Kod = @Kod,
                                    UrunBirimiId = @UrunBirimiId,
                                    Favori = @Favori,
                                    Sirket = @Sirket,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SubeId = model.SubeId,
                        StokKodu = model.StokKodu,
                        BarkodNo = model.BarkodNo,
                        SubeUrunGrubuId = model.SubeUrunGrubuId,
                        UrunAdi = model.UrunAdi,
                        AlisFiyat = model.AlisFiyat,
                        Fiyat = model.Fiyat,
                        KdvOrani = model.KdvOrani,
                        PosEkranindaGoster = model.PosEkranindaGoster,
                        ElTerminalindeGoster = model.ElTerminalindeGoster,
                        Kod = model.Kod,
                        UrunBirimiId = model.UrunBirimiId,
                        Favori = model.Favori,
                        Sirket = model.Sirket,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube ürünü başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürününü pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubeUrun(int id)
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

                    // Önce şube ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE SubeUrun
                                SET Durum = 0,
                                    SilmeTarihi = @SilmeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube ürünü başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Birden fazla şube ürününü toplu olarak getirir
        /// </summary>
        [HttpPost]
        [Route("getByIds")]
        public IHttpActionResult GetSubeUrunlerByIds([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Şube ürünü ID'leri boş olamaz");

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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Id IN @Ids AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query, new { Ids = ids }).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Ürün adına, barkoda veya stok koduna göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchSubeUrun(string term)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE (su.UrunAdi LIKE @Term OR su.BarkodNo LIKE @Term OR su.StokKodu LIKE @Term OR su.Kod LIKE @Term) 
                            AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü araması yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ID'sine göre ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}")]
        public IHttpActionResult GetSubeUrunlerBySube(int subeId)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.SubeId = @SubeId AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query, new { SubeId = subeId }).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Ürün grubu ID'sine göre şube ürünlerini getirir
        /// </summary>
        [HttpGet]
        [Route("grup/{grupId}")]
        public IHttpActionResult GetSubeUrunlerByGrup(int grupId)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.SubeUrunGrubuId = @GrupId AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query, new { GrupId = grupId }).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Favori ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("favori")]
        public IHttpActionResult GetFavoriUrunler()
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Favori = 1 AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Favori ürünler getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// POS ekranında gösterilecek ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("posekrani")]
        public IHttpActionResult GetPosEkraniUrunler()
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.PosEkranindaGoster = 1 AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"POS ekranı ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// El terminalinde gösterilecek ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("elterminal")]
        public IHttpActionResult GetElTerminalUrunler()
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.ElTerminalindeGoster = 1 AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"El terminali ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Filtreleme ve sıralama ile şube ürünlerini getirir
        /// </summary>
        [HttpPost]
        [Route("filter")]
        public IHttpActionResult FilterSubeUrunler(SubeUrunFilterModel filter)
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
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append(@"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.Durum = 1");

                    var parameters = new DynamicParameters();

                    // Filtreleri ekle
                    if (filter.SubeId.HasValue)
                    {
                        queryBuilder.Append(" AND su.SubeId = @SubeId");
                        parameters.Add("SubeId", filter.SubeId.Value);
                    }

                    if (filter.SubeUrunGrubuId.HasValue)
                    {
                        queryBuilder.Append(" AND su.SubeUrunGrubuId = @SubeUrunGrubuId");
                        parameters.Add("SubeUrunGrubuId", filter.SubeUrunGrubuId.Value);
                    }

                    if (filter.UrunBirimiId.HasValue)
                    {
                        queryBuilder.Append(" AND su.UrunBirimiId = @UrunBirimiId");
                        parameters.Add("UrunBirimiId", filter.UrunBirimiId.Value);
                    }

                    if (filter.Favori.HasValue)
                    {
                        queryBuilder.Append(" AND su.Favori = @Favori");
                        parameters.Add("Favori", filter.Favori.Value);
                    }

                    if (filter.PosEkranindaGoster.HasValue)
                    {
                        queryBuilder.Append(" AND su.PosEkranindaGoster = @PosEkranindaGoster");
                        parameters.Add("PosEkranindaGoster", filter.PosEkranindaGoster.Value);
                    }

                    if (filter.ElTerminalindeGoster.HasValue)
                    {
                        queryBuilder.Append(" AND su.ElTerminalindeGoster = @ElTerminalindeGoster");
                        parameters.Add("ElTerminalindeGoster", filter.ElTerminalindeGoster.Value);
                    }

                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        queryBuilder.Append(" AND (su.UrunAdi LIKE @SearchTerm OR su.BarkodNo LIKE @SearchTerm OR su.StokKodu LIKE @SearchTerm OR su.Kod LIKE @SearchTerm)");
                        parameters.Add("SearchTerm", $"%{filter.SearchTerm}%");
                    }

                    // Sıralama
                    if (!string.IsNullOrEmpty(filter.SortBy))
                    {
                        var sortDirection = filter.SortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";
                        switch (filter.SortBy.ToLower())
                        {
                            case "urunadi":
                                queryBuilder.Append($" ORDER BY su.UrunAdi {sortDirection}");
                                break;
                            case "barkodno":
                                queryBuilder.Append($" ORDER BY su.BarkodNo {sortDirection}");
                                break;
                            case "stokkodu":
                                queryBuilder.Append($" ORDER BY su.StokKodu {sortDirection}");
                                break;
                            case "fiyat":
                                queryBuilder.Append($" ORDER BY su.Fiyat {sortDirection}");
                                break;
                            case "kod":
                                queryBuilder.Append($" ORDER BY su.Kod {sortDirection}");
                                break;
                            case "kayittarihi":
                                queryBuilder.Append($" ORDER BY su.KayitTarihi {sortDirection}");
                                break;
                            default:
                                queryBuilder.Append(" ORDER BY su.Id ASC");
                                break;
                        }
                    }
                    else
                    {
                        queryBuilder.Append(" ORDER BY su.Id ASC");
                    }

                    // Sayfalama
                    if (filter.PageSize > 0)
                    {
                        var offset = (filter.Page - 1) * filter.PageSize;
                        queryBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                        parameters.Add("Offset", offset);
                        parameters.Add("PageSize", filter.PageSize);
                    }

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(queryBuilder.ToString(), parameters).ToList();

                    // Toplam kayıt sayısını al
                    var countQueryBuilder = new StringBuilder();
                    countQueryBuilder.Append("SELECT COUNT(1) FROM SubeUrun su WHERE su.Durum = 1");

                    if (filter.SubeId.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.SubeId = @SubeId");
                    }

                    if (filter.SubeUrunGrubuId.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.SubeUrunGrubuId = @SubeUrunGrubuId");
                    }

                    if (filter.UrunBirimiId.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.UrunBirimiId = @UrunBirimiId");
                    }

                    if (filter.Favori.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.Favori = @Favori");
                    }

                    if (filter.PosEkranindaGoster.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.PosEkranindaGoster = @PosEkranindaGoster");
                    }

                    if (filter.ElTerminalindeGoster.HasValue)
                    {
                        countQueryBuilder.Append(" AND su.ElTerminalindeGoster = @ElTerminalindeGoster");
                    }

                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        countQueryBuilder.Append(" AND (su.UrunAdi LIKE @SearchTerm OR su.BarkodNo LIKE @SearchTerm OR su.StokKodu LIKE @SearchTerm OR su.Kod LIKE @SearchTerm)");
                    }

                    var totalCount = connection.ExecuteScalar<int>(countQueryBuilder.ToString(), parameters);

                    return Ok(new
                    {
                        Items = subeUrunler,
                        TotalCount = totalCount,
                        Page = filter.Page,
                        PageSize = filter.PageSize,
                        TotalPages = filter.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / filter.PageSize) : 1
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri filtrelenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ve şube ürün grubu ID'sine göre ürünleri getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}/grup/{grupId}")]
        public IHttpActionResult GetSubeUrunlerBySubeAndGrup(int subeId, int grupId)
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
                    var query = @"SELECT su.Id, su.SubeId, su.StokKodu, su.BarkodNo, su.SubeUrunGrubuId, 
                                   su.UrunAdi, su.AlisFiyat, su.Fiyat, su.KdvOrani, su.PosEkranindaGoster, 
                                   su.ElTerminalindeGoster, su.KayitTarihi, su.Kod, su.UrunBirimiId, su.Favori,
                                   s.SubeAdi, sug.UrunGrubuAdi, ub.BirimAdi
                            FROM SubeUrun su
                            LEFT JOIN Sube s ON su.SubeId = s.Id
                            LEFT JOIN SubeUrunGrubu sug ON su.SubeUrunGrubuId = sug.Id
                            LEFT JOIN UrunBirimi ub ON su.UrunBirimiId = ub.Id
                            WHERE su.SubeId = @SubeId AND su.SubeUrunGrubuId = @GrupId AND su.Durum = 1";

                    var subeUrunler = connection.Query<SubeUrunResponseModel>(query, new { SubeId = subeId, GrupId = grupId }).ToList();
                    return Ok(subeUrunler);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu şube ürünü ekler
        /// </summary>
        [HttpPost]
        [Route("batch")]
        public IHttpActionResult AddBatchSubeUrun([FromBody] List<SubeUrunRequestModel> models)
        {
            if (models == null || !models.Any())
                return BadRequest("En az bir şube ürünü eklenmelidir");

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
                                // Barkod kontrolü
                                if (!string.IsNullOrEmpty(model.BarkodNo))
                                {
                                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE BarkodNo = @BarkodNo AND Durum = 1";
                                    var exists = connection.ExecuteScalar<int>(checkQuery, new { BarkodNo = model.BarkodNo }, transaction) > 0;
                                    if (exists)
                                        continue; // Bu barkod zaten var, bu ürünü atla
                                }

                                // Kod kontrolü
                                if (!string.IsNullOrEmpty(model.Kod))
                                {
                                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Kod = @Kod AND Durum = 1";
                                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Kod = model.Kod }, transaction) > 0;
                                    if (exists)
                                        continue; // Bu kod zaten var, bu ürünü atla
                                }

                                var query = @"INSERT INTO SubeUrun
                                            (SubeId, StokKodu, BarkodNo, SubeUrunGrubuId, UrunAdi, 
                                             AlisFiyat, Fiyat, KdvOrani, PosEkranindaGoster, ElTerminalindeGoster,
                                             KayitTarihi, GuncellemeTarihi, Durum, Kod, UrunBirimiId, Favori, Sirket)
                                            VALUES
                                            (@SubeId, @StokKodu, @BarkodNo, @SubeUrunGrubuId, @UrunAdi, 
                                             @AlisFiyat, @Fiyat, @KdvOrani, @PosEkranindaGoster, @ElTerminalindeGoster,
                                             @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @UrunBirimiId, @Favori, @Sirket);
                                            SELECT CAST(SCOPE_IDENTITY() as int)";

                                var parameters = new
                                {
                                    SubeId = model.SubeId,
                                    StokKodu = model.StokKodu,
                                    BarkodNo = model.BarkodNo,
                                    SubeUrunGrubuId = model.SubeUrunGrubuId,
                                    UrunAdi = model.UrunAdi,
                                    AlisFiyat = model.AlisFiyat,
                                    Fiyat = model.Fiyat,
                                    KdvOrani = model.KdvOrani,
                                    PosEkranindaGoster = model.PosEkranindaGoster,
                                    ElTerminalindeGoster = model.ElTerminalindeGoster,
                                    KayitTarihi = now,
                                    GuncellemeTarihi = now,
                                    Durum = 1, // Aktif
                                    Kod = model.Kod,
                                    UrunBirimiId = model.UrunBirimiId,
                                    Favori = model.Favori,
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
                                Message = $"{newIds.Count} şube ürünü başarıyla eklendi."
                            });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return BadRequest($"Şube ürünleri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünleri eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürününün favori durumunu günceller
        /// </summary>
        [HttpPut]
        [Route("{id}/favori/{favoriDurumu}")]
        public IHttpActionResult UpdateFavoriDurumu(int id, bool favoriDurumu)
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

                    // Önce şube ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE SubeUrun
                                SET Favori = @Favori,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        Favori = favoriDurumu,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new
                    {
                        Success = true,
                        AffectedRows = affectedRows,
                        Message = $"Şube ürünü favori durumu {(favoriDurumu ? "eklendi" : "kaldırıldı")}."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü favori durumu güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ürününün POS ekranında gösterilme durumunu günceller
        /// </summary>
        [HttpPut]
        [Route("{id}/posekrani/{gosterDurumu}")]
        public IHttpActionResult UpdatePosEkraniDurumu(int id, bool gosterDurumu)
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

                    // Önce şube ürününün var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeUrun WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE SubeUrun
                                SET PosEkranindaGoster = @PosEkranindaGoster,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        PosEkranindaGoster = gosterDurumu,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new
                    {
                        Success = true,
                        AffectedRows = affectedRows,
                        Message = $"Şube ürünü POS ekranında gösterilme durumu {(gosterDurumu ? "aktifleştirildi" : "deaktifleştirildi")}."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube ürünü POS ekranında gösterilme durumu güncellenirken hata oluştu: {ex.Message}");
            }
        }
    }
}