using ArtiConnect.Api.Modals;
using ArtiConnect.Api.Modals.Enums;
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
    [RoutePrefix("api/stokkarti")]
    public class StokKartiController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif stok kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllStokKartlari()
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Durum = 1";

                    var stokKartlari = connection.Query<StokKartiResponseModel>(query).ToList();
                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre stok kartı getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetStokKartiById(int id)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Id = @Id AND sk.Durum = 1";

                    var stokKarti = connection.QueryFirstOrDefault<StokKartiResponseModel>(query, new { Id = id });
                    if (stokKarti == null)
                        return NotFound();

                    return Ok(stokKarti);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Barkod'a göre stok kartı getirir
        /// </summary>
        [HttpGet]
        [Route("barkod/{barkod}")]
        public IHttpActionResult GetStokKartiByBarkod(string barkod)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Barkod = @Barkod AND sk.Durum = 1";

                    var stokKarti = connection.QueryFirstOrDefault<StokKartiResponseModel>(query, new { Barkod = barkod });
                    if (stokKarti == null)
                        return NotFound();

                    return Ok(stokKarti);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kod'a göre stok kartı getirir
        /// </summary>
        [HttpGet]
        [Route("kod/{kod}")]
        public IHttpActionResult GetStokKartiByKod(string kod)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Kod = @Kod AND sk.Durum = 1";

                    var stokKarti = connection.QueryFirstOrDefault<StokKartiResponseModel>(query, new { Kod = kod });
                    if (stokKarti == null)
                        return NotFound();

                    return Ok(stokKarti);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni stok kartı ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddStokKarti(StokKartiRequestModel model)
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
                    if (!string.IsNullOrEmpty(model.Barkod))
                    {
                        var checkQuery = "SELECT COUNT(1) FROM StokKarti WHERE Barkod = @Barkod AND Durum = 1";
                        var exists = connection.ExecuteScalar<int>(checkQuery, new { Barkod = model.Barkod }) > 0;
                        if (exists)
                            return BadRequest("Bu barkod ile kayıtlı bir stok kartı zaten mevcut.");
                    }

                    // Kod kontrolü
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var checkQuery = "SELECT COUNT(1) FROM StokKarti WHERE Kod = @Kod AND Durum = 1";
                        var exists = connection.ExecuteScalar<int>(checkQuery, new { Kod = model.Kod }) > 0;
                        if (exists)
                            return BadRequest("Bu kod ile kayıtlı bir stok kartı zaten mevcut.");
                    }

                    var now = DateTime.Now;
                    var query = @"INSERT INTO StokKarti
                                (StokTuru, StokKartiKategoriId, Barkod, StokAdi, AlisKdv, SatisKdv, DepoId,
                                 Birim1, BirimCarpani1, Birim2, BirimCarpani2, Birim3, BirimCarpani3,
                                 Birim4, BirimCarpani4, Kod, KayitTarihi, GuncellemeTarihi, Durum)
                                VALUES
                                (@StokTuru, @StokKartiKategoriId, @Barkod, @StokAdi, @AlisKdv, @SatisKdv, @DepoId,
                                 @Birim1, @BirimCarpani1, @Birim2, @BirimCarpani2, @Birim3, @BirimCarpani3,
                                 @Birim4, @BirimCarpani4, @Kod, @KayitTarihi, @GuncellemeTarihi, @Durum);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        StokTuru = model.StokTuru,
                        StokKartiKategoriId = model.StokKartiKategoriId,
                        Barkod = model.Barkod,
                        StokAdi = model.StokAdi,
                        AlisKdv = model.AlisKdv,
                        SatisKdv = model.SatisKdv,
                        DepoId = model.DepoId,
                        Birim1 = model.Birim1,
                        BirimCarpani1 = model.BirimCarpani1,
                        Birim2 = model.Birim2,
                        BirimCarpani2 = model.BirimCarpani2,
                        Birim3 = model.Birim3,
                        BirimCarpani3 = model.BirimCarpani3,
                        Birim4 = model.Birim4,
                        BirimCarpani4 = model.BirimCarpani4,
                        Kod = model.Kod,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1 // Aktif
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Stok kartı başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok kartı bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateStokKarti(int id, StokKartiRequestModel model)
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

                    // Önce stok kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Barkod değiştirilmişse ve yeni barkod başka bir stok kartında kullanılıyorsa kontrol et
                    if (!string.IsNullOrEmpty(model.Barkod))
                    {
                        var barkodCheckQuery = "SELECT COUNT(1) FROM StokKarti WHERE Barkod = @Barkod AND Id != @Id AND Durum = 1";
                        var barkodExists = connection.ExecuteScalar<int>(barkodCheckQuery, new { Barkod = model.Barkod, Id = id }) > 0;
                        if (barkodExists)
                            return BadRequest("Bu barkod ile kayıtlı başka bir stok kartı zaten mevcut.");
                    }

                    // Kod değiştirilmişse ve yeni kod başka bir stok kartında kullanılıyorsa kontrol et
                    if (!string.IsNullOrEmpty(model.Kod))
                    {
                        var kodCheckQuery = "SELECT COUNT(1) FROM StokKarti WHERE Kod = @Kod AND Id != @Id AND Durum = 1";
                        var kodExists = connection.ExecuteScalar<int>(kodCheckQuery, new { Kod = model.Kod, Id = id }) > 0;
                        if (kodExists)
                            return BadRequest("Bu kod ile kayıtlı başka bir stok kartı zaten mevcut.");
                    }

                    var query = @"UPDATE StokKarti
                                SET StokTuru = @StokTuru,
                                    StokKartiKategoriId = @StokKartiKategoriId,
                                    Barkod = @Barkod,
                                    StokAdi = @StokAdi,
                                    AlisKdv = @AlisKdv,
                                    SatisKdv = @SatisKdv,
                                    DepoId = @DepoId,
                                    Birim1 = @Birim1,
                                    BirimCarpani1 = @BirimCarpani1,
                                    Birim2 = @Birim2,
                                    BirimCarpani2 = @BirimCarpani2,
                                    Birim3 = @Birim3,
                                    BirimCarpani3 = @BirimCarpani3,
                                    Birim4 = @Birim4,
                                    BirimCarpani4 = @BirimCarpani4,
                                    Kod = @Kod,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        StokTuru = model.StokTuru,
                        StokKartiKategoriId = model.StokKartiKategoriId,
                        Barkod = model.Barkod,
                        StokAdi = model.StokAdi,
                        AlisKdv = model.AlisKdv,
                        SatisKdv = model.SatisKdv,
                        DepoId = model.DepoId,
                        Birim1 = model.Birim1,
                        BirimCarpani1 = model.BirimCarpani1,
                        Birim2 = model.Birim2,
                        BirimCarpani2 = model.BirimCarpani2,
                        Birim3 = model.Birim3,
                        BirimCarpani3 = model.BirimCarpani3,
                        Birim4 = model.Birim4,
                        BirimCarpani4 = model.BirimCarpani4,
                        Kod = model.Kod,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Stok kartı başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok kartını pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteStokKarti(int id)
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

                    // Önce stok kartının var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM StokKarti WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE StokKarti
                                SET Durum = 0,
                                    SilmeTarihi = @SilmeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Stok kartı başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Birden fazla stok kartını toplu olarak getirir
        /// </summary>
        [HttpPost]
        [Route("getByIds")]
        public IHttpActionResult GetStokKartlariByIds([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Stok kartı ID'leri boş olamaz");

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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Id IN @Ids AND sk.Durum = 1";

                    var stokKartlari = connection.Query<StokKartiResponseModel>(query, new { Ids = ids }).ToList();
                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok adına veya barkoda göre arama yapar
        /// </summary>
        [HttpGet]
        [Route("search")]
        public IHttpActionResult SearchStokKarti(string term)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE (sk.StokAdi LIKE @Term OR sk.Barkod LIKE @Term OR sk.Kod LIKE @Term) AND sk.Durum = 1";

                    var stokKartlari = connection.Query<StokKartiResponseModel>(
                        query,
                        new { Term = $"%{term}%" }
                    ).ToList();

                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartı araması yapılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Kategori ID'sine göre stok kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("kategori/{kategoriId}")]
        public IHttpActionResult GetStokKartlariByKategori(int kategoriId)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.StokKartiKategoriId = @KategoriId AND sk.Durum = 1";

                    var stokKartlari = connection.Query<StokKartiResponseModel>(query, new { KategoriId = kategoriId }).ToList();
                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Depo ID'sine göre stok kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("depo/{depoId}")]
        public IHttpActionResult GetStokKartlariByDepo(int depoId)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.DepoId = @DepoId AND sk.Durum = 1";
                    
                    var stokKartlari = connection.Query<StokKartiResponseModel>(query, new { DepoId = depoId }).ToList();
                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Stok türüne göre stok kartlarını getirir
        /// </summary>
        [HttpGet]
        [Route("tur/{stokTuru}")]
        public IHttpActionResult GetStokKartlariByTur(StokTuru stokTuru)
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
                    var query = @"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.StokTuru = @StokTuru AND sk.Durum = 1";
                    
                    var stokKartlari = connection.Query<StokKartiResponseModel>(query, new { StokTuru = (int)stokTuru }).ToList();
                    return Ok(stokKartlari);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Filtreleme ve sıralama ile stok kartlarını getirir
        /// </summary>
        [HttpPost]
        [Route("filter")]
        public IHttpActionResult FilterStokKartlari(StokKartiFilterModel filter)
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
                    queryBuilder.Append(@"SELECT sk.Id, sk.StokTuru, sk.StokKartiKategoriId, sk.Barkod, sk.StokAdi, 
                                    sk.AlisKdv, sk.SatisKdv, sk.DepoId, sk.Birim1, sk.BirimCarpani1, 
                                    sk.Birim2, sk.BirimCarpani2, sk.Birim3, sk.BirimCarpani3, 
                                    sk.Birim4, sk.BirimCarpani4, sk.Kod, sk.KayitTarihi,
                                    skk.KategoriAdi as StokKategoriAdi, d.DepoAdi
                                FROM StokKarti sk
                                LEFT JOIN StokKartiKategori skk ON sk.StokKartiKategoriId = skk.Id
                                LEFT JOIN Depo d ON sk.DepoId = d.Id
                                WHERE sk.Durum = 1");

                    var parameters = new DynamicParameters();
                    
                    // Filtreleri ekle
                    if (filter.StokTuru.HasValue)
                    {
                        queryBuilder.Append(" AND sk.StokTuru = @StokTuru");
                        parameters.Add("StokTuru", (int)filter.StokTuru.Value);
                    }
                    
                    if (filter.StokKartiKategoriId.HasValue)
                    {
                        queryBuilder.Append(" AND sk.StokKartiKategoriId = @StokKartiKategoriId");
                        parameters.Add("StokKartiKategoriId", filter.StokKartiKategoriId.Value);
                    }
                    
                    if (filter.DepoId.HasValue)
                    {
                        queryBuilder.Append(" AND sk.DepoId = @DepoId");
                        parameters.Add("DepoId", filter.DepoId.Value);
                    }
                    
                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        queryBuilder.Append(" AND (sk.StokAdi LIKE @SearchTerm OR sk.Barkod LIKE @SearchTerm OR sk.Kod LIKE @SearchTerm)");
                        parameters.Add("SearchTerm", $"%{filter.SearchTerm}%");
                    }
                    
                    // Sıralama
                    if (!string.IsNullOrEmpty(filter.SortBy))
                    {
                        var sortDirection = filter.SortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";
                        switch (filter.SortBy.ToLower())
                        {
                            case "stokadi":
                                queryBuilder.Append($" ORDER BY sk.StokAdi {sortDirection}");
                                break;
                            case "barkod":
                                queryBuilder.Append($" ORDER BY sk.Barkod {sortDirection}");
                                break;
                            case "kod":
                                queryBuilder.Append($" ORDER BY sk.Kod {sortDirection}");
                                break;
                            case "kayittarihi":
                                queryBuilder.Append($" ORDER BY sk.KayitTarihi {sortDirection}");
                                break;
                            default:
                                queryBuilder.Append(" ORDER BY sk.Id ASC");
                                break;
                        }
                    }
                    else
                    {
                        queryBuilder.Append(" ORDER BY sk.Id ASC");
                    }
                    
                    // Sayfalama
                    if (filter.PageSize > 0)
                    {
                        var offset = (filter.Page - 1) * filter.PageSize;
                        queryBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                        parameters.Add("Offset", offset);
                        parameters.Add("PageSize", filter.PageSize);
                    }
                    
                    var stokKartlari = connection.Query<StokKartiResponseModel>(queryBuilder.ToString(), parameters).ToList();
                    
                    // Toplam kayıt sayısını al
                    var countQueryBuilder = new StringBuilder();
                    countQueryBuilder.Append("SELECT COUNT(1) FROM StokKarti sk WHERE sk.Durum = 1");
                    
                    if (filter.StokTuru.HasValue)
                    {
                        countQueryBuilder.Append(" AND sk.StokTuru = @StokTuru");
                    }
                    
                    if (filter.StokKartiKategoriId.HasValue)
                    {
                        countQueryBuilder.Append(" AND sk.StokKartiKategoriId = @StokKartiKategoriId");
                    }
                    
                    if (filter.DepoId.HasValue)
                    {
                        countQueryBuilder.Append(" AND sk.DepoId = @DepoId");
                    }
                    
                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        countQueryBuilder.Append(" AND (sk.StokAdi LIKE @SearchTerm OR sk.Barkod LIKE @SearchTerm OR sk.Kod LIKE @SearchTerm)");
                    }
                    
                    var totalCount = connection.ExecuteScalar<int>(countQueryBuilder.ToString(), parameters);
                    
                    return Ok(new { 
                        Items = stokKartlari, 
                        TotalCount = totalCount,
                        Page = filter.Page,
                        PageSize = filter.PageSize,
                        TotalPages = filter.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / filter.PageSize) : 1
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Stok kartları filtrelenirken hata oluştu: {ex.Message}");
            }
        }
    }
}