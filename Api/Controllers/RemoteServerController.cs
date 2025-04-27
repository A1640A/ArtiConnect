using ArtiConnect.Api.Modals;
using ArtiConnect.DataAccess;
using Swashbuckle.Swagger;
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
    [RoutePrefix("api/remoteServer")]
    public class RemoteServerController : ApiController
    {
        private AppDbContext db = new AppDbContext();


        /// <summary>
        /// Şube satış verilerini uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("addSubeSatis")]
        public IHttpActionResult AddSubeSatis(AddSubeSatisRequestModal modal)
        {
            var ayar = db.Ayars.FirstOrDefault();

            if (ayar == null)
            {
                return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
            }

            var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    // SQL sorgusu oluştur
                    var query = @"INSERT INTO SubeSatis 
                        (SubeKodu, StokKodu, Miktar, Fiyat, KayitTarihi, GuncellemeTarihi, Durum, 
                        Kod, FisId, AdisyonNo, FisTarihi, FisNoDurumu, KullaniciAdi, 
                        UrunAdi, UrunGrubuAdi, IsIade, IsZayi, IsKaporaSatis, Aciklama) 
                        VALUES 
                        (@SubeKodu, @StokKodu, @Miktar, @Fiyat, @KayitTarihi, @GuncellemeTarihi, @Durum, 
                        @Kod, @FisId, @AdisyonNo, @FisTarihi, @FisNoDurumu, @KullaniciAdi, 
                        @UrunAdi, @UrunGrubuAdi, @IsIade, @IsZayi, @IsKaporaSatis, @Aciklama)";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@SubeKodu", modal.SubeKodu);
                        command.Parameters.AddWithValue("@StokKodu", modal.StokKodu);
                        command.Parameters.AddWithValue("@Miktar", modal.Miktar);
                        command.Parameters.AddWithValue("@Fiyat", modal.Fiyat);
                        command.Parameters.AddWithValue("@KayitTarihi", modal.Tarih);
                        command.Parameters.AddWithValue("@GuncellemeTarihi", modal.Tarih);
                        command.Parameters.AddWithValue("@Durum", 1);
                        command.Parameters.AddWithValue("@Kod", "");
                        command.Parameters.AddWithValue("@FisId", modal.FisId);
                        command.Parameters.AddWithValue("@AdisyonNo", modal.AdisyonNo ?? "");
                        command.Parameters.AddWithValue("@FisTarihi", modal.FisTarihi);
                        command.Parameters.AddWithValue("@FisNoDurumu", modal.FisNoDurumu);
                        command.Parameters.AddWithValue("@KullaniciAdi", modal.KullaniciAdi ?? "");
                        command.Parameters.AddWithValue("@UrunAdi", modal.UrunAdi ?? "");
                        command.Parameters.AddWithValue("@UrunGrubuAdi", modal.UrunGrubuAdi ?? "");
                        command.Parameters.AddWithValue("@IsIade", modal.IsIade);
                        command.Parameters.AddWithValue("@IsZayi", modal.IsZayi);
                        command.Parameters.AddWithValue("@IsKaporaSatis", modal.IsKaporaSatis);
                        command.Parameters.AddWithValue("@Aciklama", modal.Aciklama ?? "");

                        // Sorguyu çalıştır
                        int affectedRows = command.ExecuteNonQuery();

                        return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube satış verisi başarıyla eklendi." });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Şube satış verisi eklenirken hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Şube satış verilerini toplu olarak uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("addSubeSatisBulk")]
        public IHttpActionResult AddSubeSatisBulk(List<AddSubeSatisRequestModal> modalList)
        {
            if (modalList == null || !modalList.Any())
            {
                return BadRequest("Eklenecek veri bulunamadı.");
            }

            var ayar = db.Ayars.FirstOrDefault();

            if (ayar == null)
            {
                return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
            }

            var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    // Transaction başlat
                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            int totalAffectedRows = 0;

                            foreach (var modal in modalList)
                            {
                                var query = @"INSERT INTO SubeSatis 
                                    (SubeKodu, StokKodu, Miktar, Fiyat, KayitTarihi, GuncellemeTarihi, Durum, 
                                    Kod, FisId, AdisyonNo, FisTarihi, FisNoDurumu, KullaniciAdi, 
                                    UrunAdi, UrunGrubuAdi, IsIade, IsZayi, IsKaporaSatis, Aciklama) 
                                    VALUES 
                                    (@SubeKodu, @StokKodu, @Miktar, @Fiyat, @KayitTarihi, @GuncellemeTarihi, @Durum, 
                                    @Kod, @FisId, @AdisyonNo, @FisTarihi, @FisNoDurumu, @KullaniciAdi, 
                                    @UrunAdi, @UrunGrubuAdi, @IsIade, @IsZayi, @IsKaporaSatis, @Aciklama)";

                                using (var command = new SqlCommand(query, sqlConnection, transaction))
                                {
                                    // Parametreleri ekle
                                    command.Parameters.AddWithValue("@SubeKodu", modal.SubeKodu);
                                    command.Parameters.AddWithValue("@StokKodu", modal.StokKodu);
                                    command.Parameters.AddWithValue("@Miktar", modal.Miktar);
                                    command.Parameters.AddWithValue("@Fiyat", modal.Fiyat);
                                    command.Parameters.AddWithValue("@KayitTarihi", modal.Tarih);
                                    command.Parameters.AddWithValue("@GuncellemeTarihi", modal.Tarih);
                                    command.Parameters.AddWithValue("@Durum", 1);
                                    command.Parameters.AddWithValue("@Kod", "");
                                    command.Parameters.AddWithValue("@FisId", modal.FisId);
                                    command.Parameters.AddWithValue("@AdisyonNo", modal.AdisyonNo ?? "");
                                    command.Parameters.AddWithValue("@FisTarihi", modal.FisTarihi);
                                    command.Parameters.AddWithValue("@FisNoDurumu", modal.FisNoDurumu);
                                    command.Parameters.AddWithValue("@KullaniciAdi", modal.KullaniciAdi ?? "");
                                    command.Parameters.AddWithValue("@UrunAdi", modal.UrunAdi ?? "");
                                    command.Parameters.AddWithValue("@UrunGrubuAdi", modal.UrunGrubuAdi ?? "");
                                    command.Parameters.AddWithValue("@IsIade", modal.IsIade);
                                    command.Parameters.AddWithValue("@IsZayi", modal.IsZayi);
                                    command.Parameters.AddWithValue("@IsKaporaSatis", modal.IsKaporaSatis);
                                    command.Parameters.AddWithValue("@Aciklama", modal.Aciklama ?? "");

                                    // Sorguyu çalıştır
                                    totalAffectedRows += command.ExecuteNonQuery();
                                }
                            }

                            // Transaction'ı commit et
                            transaction.Commit();

                            return Ok(new { Success = true, AffectedRows = totalAffectedRows, Message = $"{modalList.Count} adet şube satış verisi başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            // Hata durumunda transaction'ı geri al
                            transaction.Rollback();

                            return BadRequest($"Şube satış verileri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Şube ödeme verilerini uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("addSubeOdeme")]
        public IHttpActionResult AddSubeOdeme(AddSubeOdemeRequestModal modal)
        {
            var ayar = db.Ayars.FirstOrDefault();

            if (ayar == null)
            {
                return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
            }

            var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    // SQL sorgusu oluştur
                    var query = @"INSERT INTO SubeOdeme 
                (SubeKodu, FisId, AdisyonNo, FisTarihi, KullaniciAdi, OdemeYontemiAdi, Miktar, KayitTarihi, Durum) 
                VALUES 
                (@SubeKodu, @FisId, @AdisyonNo, @FisTarihi, @KullaniciAdi, @OdemeYontemiAdi, @Miktar, @KayitTarihi, @Durum)";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@SubeKodu", modal.SubeKodu);
                        command.Parameters.AddWithValue("@FisId", modal.FisId);
                        command.Parameters.AddWithValue("@AdisyonNo", modal.AdisyonNo);
                        command.Parameters.AddWithValue("@FisTarihi", modal.FisTarihi);
                        command.Parameters.AddWithValue("@KullaniciAdi", modal.KullaniciAdi);
                        command.Parameters.AddWithValue("@OdemeYontemiAdi", modal.OdemeYontemiAdi);
                        command.Parameters.AddWithValue("@Miktar", modal.Miktar); 
                        command.Parameters.AddWithValue("@KayitTarihi", modal.Tarih);
                        command.Parameters.AddWithValue("@Durum", 1);

                        // Sorguyu çalıştır
                        int affectedRows = command.ExecuteNonQuery();

                        return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube ödeme verisi başarıyla eklendi." });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Şube ödeme verisi eklenirken hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Şube ödeme verilerini toplu olarak uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("addSubeOdemeBulk")]
        public IHttpActionResult AddSubeOdemeBulk(List<AddSubeOdemeRequestModal> modalList)
        {
            if (modalList == null || !modalList.Any())
            {
                return BadRequest("Eklenecek veri bulunamadı.");
            }

            var ayar = db.Ayars.FirstOrDefault();

            if (ayar == null)
            {
                return BadRequest("Uzak sunucu bağlantı ayarları bulunamadı.");
            }

            var connectionString = $"Server={ayar.RemoteDbServerName};User ID={ayar.RemoteDbUserName};Password={ayar.RemoteDbPassword};Database={ayar.RemoteDbDatabaseName}";

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();

                    // Transaction başlat
                    using (var transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            int totalAffectedRows = 0;

                            foreach (var modal in modalList)
                            {
                                var query = @"INSERT INTO SubeOdeme 
                            (SubeKodu, FisId, AdisyonNo, FisTarihi, KullaniciAdi, OdemeYontemiAdi, Miktar, KayitTarihi, Durum) 
                            VALUES 
                            (@SubeKodu, @FisId, @AdisyonNo, @FisTarihi, @KullaniciAdi, @OdemeYontemiAdi, @Miktar, @KayitTarihi, @Durum)";

                                using (var command = new SqlCommand(query, sqlConnection, transaction))
                                {
                                    // Parametreleri ekle
                                    command.Parameters.AddWithValue("@SubeKodu", modal.SubeKodu);
                                    command.Parameters.AddWithValue("@FisId", modal.FisId);
                                    command.Parameters.AddWithValue("@AdisyonNo", modal.AdisyonNo);
                                    command.Parameters.AddWithValue("@FisTarihi", modal.FisTarihi);
                                    command.Parameters.AddWithValue("@KullaniciAdi", modal.KullaniciAdi);
                                    command.Parameters.AddWithValue("@OdemeYontemiAdi", modal.OdemeYontemiAdi);
                                    command.Parameters.AddWithValue("@Miktar", modal.Miktar); 
                                    command.Parameters.AddWithValue("@KayitTarihi", modal.Tarih);
                                    command.Parameters.AddWithValue("@Durum", 1);

                                    // Sorguyu çalıştır
                                    totalAffectedRows += command.ExecuteNonQuery();
                                }
                            }

                            // Transaction'ı commit et
                            transaction.Commit();

                            return Ok(new { Success = true, AffectedRows = totalAffectedRows, Message = $"{modalList.Count} adet şube ödeme verisi başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            // Hata durumunda transaction'ı geri al
                            transaction.Rollback();

                            return BadRequest($"Şube ödeme verileri eklenirken hata oluştu: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Veritabanı bağlantısı sırasında hata oluştu: {ex.Message}");
                }
            }
        }
    }
}