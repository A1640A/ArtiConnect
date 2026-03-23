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
    [RoutePrefix("api/subemasraf")]
    public class SubeMasrafController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Tüm aktif şube masraflarını getirir
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllSubeMasraflar()
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
                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(query).ToList();
                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// ID'ye göre şube masrafını getirir
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetSubeMasrafById(int id)
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
                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.Id = @Id AND sm.Durum = 1";

                    var masraf = connection.QueryFirstOrDefault<SubeMasrafResponseModel>(query, new { Id = id });

                    if (masraf == null)
                        return NotFound();

                    return Ok(masraf);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafı getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Yeni şube masrafı ekler
        /// </summary>
        [HttpPost]
        [Route("")]
        public IHttpActionResult AddSubeMasraf(SubeMasrafRequestModel model)
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

                    // Şube kontrolü
                    if (model.SubeId.HasValue)
                    {
                        var subeCheckQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @SubeId AND Durum = 1";
                        var subeExists = connection.ExecuteScalar<int>(subeCheckQuery, new { SubeId = model.SubeId }) > 0;
                        if (!subeExists)
                            return BadRequest("Belirtilen şube bulunamadı.");
                    }

                    // Masraf kategorisi kontrolü (zorunlu değil)
                    if (model.MasrafKategorisiId.HasValue)
                    {
                        var kategoriCheckQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @MasrafKategorisiId AND Durum = 1";
                        var kategoriExists = connection.ExecuteScalar<int>(kategoriCheckQuery, new { MasrafKategorisiId = model.MasrafKategorisiId }) > 0;
                        if (!kategoriExists)
                        {
                            // Masraf kategorisi bulunamazsa null olarak ayarla
                            model.MasrafKategorisiId = null;
                        }
                    }

                    // Ödeme yöntemi kontrolü (zorunlu değil)
                    if (model.OdemeYontemiId.HasValue)
                    {
                        var odemeYontemiCheckQuery = "SELECT COUNT(1) FROM OdemeYontemi WHERE Id = @OdemeYontemiId AND Durum = 1";
                        var odemeYontemiExists = connection.ExecuteScalar<int>(odemeYontemiCheckQuery, new { OdemeYontemiId = model.OdemeYontemiId }) > 0;
                        if (!odemeYontemiExists)
                        {
                            // Ödeme yöntemi bulunamazsa null olarak ayarla
                            model.OdemeYontemiId = null;
                        }
                    }

                    var now = DateTime.Now;
                    var query = @"INSERT INTO SubeMasraf
                                (Tarih, SubeId, MasrafKategorisiId, Aciklama, Tutar, OdemeYontemiId, KayitTarihi, GuncellemeTarihi, Durum, Kod, Sirket)
                                VALUES
                                (@Tarih, @SubeId, @MasrafKategorisiId, @Aciklama, @Tutar, @OdemeYontemiId, @KayitTarihi, @GuncellemeTarihi, @Durum, @Kod, @Sirket);
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    var parameters = new
                    {
                        Tarih = model.Tarih,
                        SubeId = model.SubeId,
                        MasrafKategorisiId = model.MasrafKategorisiId,
                        Aciklama = model.Aciklama,
                        Tutar = model.Tutar,
                        OdemeYontemiId = model.OdemeYontemiId,
                        KayitTarihi = now,
                        GuncellemeTarihi = now,
                        Durum = 1, // Aktif
                        Kod = model.Kod,
                        Sirket = model.Sirket
                    };

                    var newId = connection.ExecuteScalar<int>(query, parameters);
                    return Ok(new { Id = newId, Success = true, Message = "Şube masrafı başarıyla eklendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafı eklenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube masraf bilgilerini günceller
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult UpdateSubeMasraf(int id, SubeMasrafRequestModel model)
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

                    // Önce masrafın var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeMasraf WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    // Şube kontrolü
                    if (model.SubeId.HasValue)
                    {
                        var subeCheckQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @SubeId AND Durum = 1";
                        var subeExists = connection.ExecuteScalar<int>(subeCheckQuery, new { SubeId = model.SubeId }) > 0;
                        if (!subeExists)
                            return BadRequest("Belirtilen şube bulunamadı.");
                    }

                    // Masraf kategorisi kontrolü (zorunlu değil)
                    if (model.MasrafKategorisiId.HasValue)
                    {
                        var kategoriCheckQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @MasrafKategorisiId AND Durum = 1";
                        var kategoriExists = connection.ExecuteScalar<int>(kategoriCheckQuery, new { MasrafKategorisiId = model.MasrafKategorisiId }) > 0;
                        if (!kategoriExists)
                        {
                            // Masraf kategorisi bulunamazsa null olarak ayarla
                            model.MasrafKategorisiId = null;
                        }
                    }

                    // Ödeme yöntemi kontrolü (zorunlu değil)
                    if (model.OdemeYontemiId.HasValue)
                    {
                        var odemeYontemiCheckQuery = "SELECT COUNT(1) FROM OdemeYontemi WHERE Id = @OdemeYontemiId AND Durum = 1";
                        var odemeYontemiExists = connection.ExecuteScalar<int>(odemeYontemiCheckQuery, new { OdemeYontemiId = model.OdemeYontemiId }) > 0;
                        if (!odemeYontemiExists)
                        {
                            // Ödeme yöntemi bulunamazsa null olarak ayarla
                            model.OdemeYontemiId = null;
                        }
                    }

                    var query = @"UPDATE SubeMasraf
                                SET Tarih = @Tarih,
                                    SubeId = @SubeId,
                                    MasrafKategorisiId = @MasrafKategorisiId,
                                    Aciklama = @Aciklama,
                                    Tutar = @Tutar,
                                    OdemeYontemiId = @OdemeYontemiId,
                                    Kod = @Kod,
                                    Sirket = @Sirket,
                                    GuncellemeTarihi = @GuncellemeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        Tarih = model.Tarih,
                        SubeId = model.SubeId,
                        MasrafKategorisiId = model.MasrafKategorisiId,
                        Aciklama = model.Aciklama,
                        Tutar = model.Tutar,
                        OdemeYontemiId = model.OdemeYontemiId,
                        Kod = model.Kod,
                        Sirket = model.Sirket,
                        GuncellemeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube masrafı başarıyla güncellendi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafı güncellenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube masrafını pasife çeker (soft delete)
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubeMasraf(int id)
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

                    // Önce masrafın var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM SubeMasraf WHERE Id = @Id AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { Id = id }) > 0;
                    if (!exists)
                        return NotFound();

                    var query = @"UPDATE SubeMasraf
                                SET Durum = 0,
                                    SilmeTarihi = @SilmeTarihi
                                WHERE Id = @Id";

                    var parameters = new
                    {
                        Id = id,
                        SilmeTarihi = DateTime.Now
                    };

                    var affectedRows = connection.Execute(query, parameters);
                    return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube masrafı başarıyla silindi." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafı silinirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şubeye göre masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}")]
        public IHttpActionResult GetSubeMasraflarBySube(int subeId)
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
                    var checkQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @SubeId AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { SubeId = subeId }) > 0;
                    if (!exists)
                        return BadRequest("Belirtilen şube bulunamadı.");

                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.SubeId = @SubeId AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(query, new { SubeId = subeId }).ToList();
                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Tarih aralığına göre masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("tarih/{baslangicTarih}/{bitisTarih}")]
        public IHttpActionResult GetSubeMasraflarByTarihAraligi(DateTime baslangicTarih, DateTime bitisTarih)
        {
            try
            {
                var ayar = db.Ayars.FirstOrDefault();
                if (ayar == null)
                {
                    return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
                }

                var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

                // Bitiş tarihini günün sonuna ayarla
                bitisTarih = bitisTarih.Date.AddDays(1).AddTicks(-1);

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(
                        query,
                        new { BaslangicTarih = baslangicTarih, BitisTarih = bitisTarih }
                    ).ToList();

                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Şube ve tarih aralığına göre masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("sube/{subeId}/tarih/{baslangicTarih}/{bitisTarih}")]
        public IHttpActionResult GetSubeMasraflarBySubeAndTarihAraligi(int subeId, DateTime baslangicTarih, DateTime bitisTarih)
        {
            try
            {
                var ayar = db.Ayars.FirstOrDefault();
                if (ayar == null)
                {
                    return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
                }

                var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

                // Bitiş tarihini günün sonuna ayarla
                bitisTarih = bitisTarih.Date.AddDays(1).AddTicks(-1);

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Önce şubenin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM Sube WHERE Id = @SubeId AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { SubeId = subeId }) > 0;
                    if (!exists)
                        return BadRequest("Belirtilen şube bulunamadı.");

                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.SubeId = @SubeId AND sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(
                        query,
                        new { SubeId = subeId, BaslangicTarih = baslangicTarih, BitisTarih = bitisTarih }
                    ).ToList();

                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Masraf kategorisine göre masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("kategori/{kategoriId}")]
        public IHttpActionResult GetSubeMasraflarByKategori(int kategoriId)
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
                    var checkQuery = "SELECT COUNT(1) FROM MasrafKategorisi WHERE Id = @KategoriId AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { KategoriId = kategoriId }) > 0;
                    if (!exists)
                        return BadRequest("Belirtilen masraf kategorisi bulunamadı.");

                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.MasrafKategorisiId = @KategoriId AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(query, new { KategoriId = kategoriId }).ToList();
                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Ödeme yöntemine göre masrafları getirir
        /// </summary>
        [HttpGet]
        [Route("odemeyontemi/{odemeYontemiId}")]
        public IHttpActionResult GetSubeMasraflarByOdemeYontemi(int odemeYontemiId)
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

                    // Önce ödeme yönteminin var olup olmadığını kontrol et
                    var checkQuery = "SELECT COUNT(1) FROM OdemeYontemi WHERE Id = @OdemeYontemiId AND Durum = 1";
                    var exists = connection.ExecuteScalar<int>(checkQuery, new { OdemeYontemiId = odemeYontemiId }) > 0;
                    if (!exists)
                        return BadRequest("Belirtilen ödeme yöntemi bulunamadı.");

                    var query = @"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.OdemeYontemiId = @OdemeYontemiId AND sm.Durum = 1
                                ORDER BY sm.Tarih DESC";

                    var masraflar = connection.Query<SubeMasrafResponseModel>(query, new { OdemeYontemiId = odemeYontemiId }).ToList();
                    return Ok(masraflar);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları getirilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Filtreleme ve sıralama ile şube masraflarını getirir
        /// </summary>
        [HttpPost]
        [Route("filter")]
        public IHttpActionResult FilterSubeMasraflar(SubeMasrafFilterModel filter)
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
                    queryBuilder.Append(@"SELECT sm.Id, sm.Tarih, sm.SubeId, sm.MasrafKategorisiId, sm.Aciklama, 
                                    sm.Tutar, sm.OdemeYontemiId, sm.KayitTarihi, sm.Kod,
                                    s.SubeAdi, mk.KategoriAdi AS MasrafKategoriAdi, oy.OdemeYontemiAdi
                                FROM SubeMasraf sm
                                LEFT JOIN Sube s ON sm.SubeId = s.Id
                                LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                WHERE sm.Durum = 1");
                    
                    var parameters = new DynamicParameters();
                    
                    // Filtreleri ekle
                    if (filter.SubeId.HasValue)
                    {
                        queryBuilder.Append(" AND sm.SubeId = @SubeId");
                        parameters.Add("SubeId", filter.SubeId.Value);
                    }
                    
                    if (filter.MasrafKategorisiId.HasValue)
                    {
                        queryBuilder.Append(" AND sm.MasrafKategorisiId = @MasrafKategorisiId");
                        parameters.Add("MasrafKategorisiId", filter.MasrafKategorisiId.Value);
                    }
                    
                    if (filter.OdemeYontemiId.HasValue)
                    {
                        queryBuilder.Append(" AND sm.OdemeYontemiId = @OdemeYontemiId");
                        parameters.Add("OdemeYontemiId", filter.OdemeYontemiId.Value);
                    }
                    
                    if (filter.BaslangicTarih.HasValue)
                    {
                        queryBuilder.Append(" AND sm.Tarih >= @BaslangicTarih");
                        parameters.Add("BaslangicTarih", filter.BaslangicTarih.Value);
                    }
                    
                    if (filter.BitisTarih.HasValue)
                    {
                        // Bitiş tarihini günün sonuna ayarla
                        var bitisTarih = filter.BitisTarih.Value.Date.AddDays(1).AddTicks(-1);
                        queryBuilder.Append(" AND sm.Tarih <= @BitisTarih");
                        parameters.Add("BitisTarih", bitisTarih);
                    }
                    
                    if (filter.MinTutar.HasValue)
                    {
                        queryBuilder.Append(" AND sm.Tutar >= @MinTutar");
                        parameters.Add("MinTutar", filter.MinTutar.Value);
                    }
                    
                    if (filter.MaxTutar.HasValue)
                    {
                        queryBuilder.Append(" AND sm.Tutar <= @MaxTutar");
                        parameters.Add("MaxTutar", filter.MaxTutar.Value);
                    }
                    
                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        queryBuilder.Append(" AND (sm.Aciklama LIKE @SearchTerm OR s.SubeAdi LIKE @SearchTerm OR mk.KategoriAdi LIKE @SearchTerm)");
                        parameters.Add("SearchTerm", $"%{filter.SearchTerm}%");
                    }
                    
                    // Sıralama
                    if (!string.IsNullOrEmpty(filter.SortBy))
                    {
                        var sortDirection = filter.SortDirection?.ToUpper() == "DESC" ? "DESC" : "ASC";
                        switch (filter.SortBy.ToLower())
                        {
                            case "tarih":
                                queryBuilder.Append($" ORDER BY sm.Tarih {sortDirection}");
                                break;
                            case "tutar":
                                queryBuilder.Append($" ORDER BY sm.Tutar {sortDirection}");
                                break;
                            case "subeadi":
                                queryBuilder.Append($" ORDER BY s.SubeAdi {sortDirection}");
                                break;
                            case "kategoriadi":
                                queryBuilder.Append($" ORDER BY mk.KategoriAdi {sortDirection}");
                                break;
                            case "odemeyontemiadi":
                                queryBuilder.Append($" ORDER BY oy.OdemeYontemiAdi {sortDirection}");
                                break;
                            default:
                                queryBuilder.Append(" ORDER BY sm.Tarih DESC");
                                break;
                        }
                    }
                    else
                    {
                        queryBuilder.Append(" ORDER BY sm.Tarih DESC");
                    }
                    
                    // Sayfalama
                    if (filter.PageSize > 0)
                    {
                        var offset = (filter.Page - 1) * filter.PageSize;
                        queryBuilder.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
                        parameters.Add("Offset", offset);
                        parameters.Add("PageSize", filter.PageSize);
                    }
                    
                    var masraflar = connection.Query<SubeMasrafResponseModel>(queryBuilder.ToString(), parameters).ToList();
                    
                    // Toplam kayıt sayısını al
                    var countQueryBuilder = new StringBuilder();
                    countQueryBuilder.Append("SELECT COUNT(1) FROM SubeMasraf sm WHERE sm.Durum = 1");
                    
                    if (filter.SubeId.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.SubeId = @SubeId");
                    }
                    
                    if (filter.MasrafKategorisiId.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.MasrafKategorisiId = @MasrafKategorisiId");
                    }
                    
                    if (filter.OdemeYontemiId.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.OdemeYontemiId = @OdemeYontemiId");
                    }
                    
                    if (filter.BaslangicTarih.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.Tarih >= @BaslangicTarih");
                    }
                    
                    if (filter.BitisTarih.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.Tarih <= @BitisTarih");
                    }
                    
                    if (filter.MinTutar.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.Tutar >= @MinTutar");
                    }
                    
                    if (filter.MaxTutar.HasValue)
                    {
                        countQueryBuilder.Append(" AND sm.Tutar <= @MaxTutar");
                    }
                    
                    if (!string.IsNullOrEmpty(filter.SearchTerm))
                    {
                        countQueryBuilder.Append(" AND (sm.Aciklama LIKE @SearchTerm)");
                    }
                    
                    var totalCount = connection.ExecuteScalar<int>(countQueryBuilder.ToString(), parameters);
                    
                    return Ok(new {
                        Items = masraflar,
                        TotalCount = totalCount,
                        Page = filter.Page,
                        PageSize = filter.PageSize,
                        TotalPages = filter.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / filter.PageSize) : 1
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Şube masrafları filtrelenirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Toplu masraf raporu özeti getirir (şube, kategori ve tarih bazlı)
        /// </summary>
        [HttpPost]
        [Route("rapor")]
        public IHttpActionResult GetMasrafRaporu(MasrafRaporRequestModel model)
        {
            try
            {
                var ayar = db.Ayars.FirstOrDefault();
                if (ayar == null)
                {
                    return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
                }
                
                var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";
                
                // Bitiş tarihini günün sonuna ayarla
                var bitisTarih = model.BitisTarih.Date.AddDays(1).AddTicks(-1);
                
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Gruplama tipine göre sorgu oluştur
                    string query;
                    
                    switch (model.RaporTuru)
                    {
                        case "sube":
                            query = @"SELECT s.SubeAdi AS GrupAdi, COUNT(sm.Id) AS MasrafSayisi, SUM(sm.Tutar) AS ToplamTutar
                                    FROM SubeMasraf sm
                                    JOIN Sube s ON sm.SubeId = s.Id
                                    WHERE sm.Durum = 1 AND sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih
                                    GROUP BY s.SubeAdi
                                    ORDER BY ToplamTutar DESC";
                            break;
                            
                        case "kategori":
                            query = @"SELECT COALESCE(mk.KategoriAdi, 'Kategorisiz') AS GrupAdi, COUNT(sm.Id) AS MasrafSayisi, SUM(sm.Tutar) AS ToplamTutar
                                    FROM SubeMasraf sm
                                    LEFT JOIN MasrafKategorisi mk ON sm.MasrafKategorisiId = mk.Id
                                    WHERE sm.Durum = 1 AND sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih
                                    GROUP BY mk.KategoriAdi
                                    ORDER BY ToplamTutar DESC";
                            break;
                            
                        case "odemeyontemi":
                            query = @"SELECT COALESCE(oy.OdemeYontemiAdi, 'Belirtilmemiş') AS GrupAdi, COUNT(sm.Id) AS MasrafSayisi, SUM(sm.Tutar) AS ToplamTutar
                                    FROM SubeMasraf sm
                                    LEFT JOIN OdemeYontemi oy ON sm.OdemeYontemiId = oy.Id
                                    WHERE sm.Durum = 1 AND sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih
                                    GROUP BY oy.OdemeYontemiAdi
                                    ORDER BY ToplamTutar DESC";
                            break;
                            
                        case "aylik":
                            query = @"SELECT FORMAT(sm.Tarih, 'yyyy-MM') AS GrupAdi, COUNT(sm.Id) AS MasrafSayisi, SUM(sm.Tutar) AS ToplamTutar
                                    FROM SubeMasraf sm
                                    WHERE sm.Durum = 1 AND sm.Tarih BETWEEN @BaslangicTarih AND @BitisTarih
                                    GROUP BY FORMAT(sm.Tarih, 'yyyy-MM')
                                    ORDER BY GrupAdi";
                            break;
                            
                        default:
                            return BadRequest("Geçersiz rapor türü. Kabul edilen değerler: sube, kategori, odemeyontemi, aylik");
                    }
                    
                    var parameters = new { BaslangicTarih = model.BaslangicTarih, BitisTarih = bitisTarih };
                    
                    var raporSonuclari = connection.Query<MasrafRaporResponseModel>(query, parameters).ToList();
                    
                    // Toplam değerleri hesapla
                    var toplamMasrafSayisi = raporSonuclari.Sum(r => r.MasrafSayisi);
                    var toplamTutar = raporSonuclari.Sum(r => r.ToplamTutar);
                    
                    return Ok(new {
                        RaporTuru = model.RaporTuru,
                        BaslangicTarih = model.BaslangicTarih,
                        BitisTarih = model.BitisTarih,
                        Sonuclar = raporSonuclari,
                        ToplamMasrafSayisi = toplamMasrafSayisi,
                        ToplamTutar = toplamTutar
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Masraf raporu oluşturulurken hata oluştu: {ex.Message}");
            }
        }
    }
}