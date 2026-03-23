using ArtiConnect.Api.Modals;
using ArtiConnect.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/subeGunSonuKasasi")]
    public class SubeGunSonuKasasiController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// Şube gün sonu kasa verilerini uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddSubeGunSonuKasasi(SubeGunSonuKasasi model)
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

                    string anaNoktaVeritabani = "ANANOKTA"; // allVariables.Ayar.AnaNoktaVeritabani yerine
                    // SQL sorgusu oluştur
                    var query = $@"INSERT INTO SubeGunSonuKasasi
                        (SubeKodu, Nakit, KrediKarti, KayitTarihi, GuncellemeTarihi, Durum)
                        VALUES
                        (@SubeKodu, @Nakit, @KrediKarti, GETDATE(), GETDATE(), 1)";

                    using (var command = new SqlCommand(query, sqlConnection))
                    {
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@SubeKodu", model.SubeKodu);
                        command.Parameters.AddWithValue("@Nakit", model.Nakit);
                        command.Parameters.AddWithValue("@KrediKarti", model.KrediKarti);

                        // Sorguyu çalıştır
                        int affectedRows = command.ExecuteNonQuery();
                        return Ok(new { Success = true, AffectedRows = affectedRows, Message = "Şube gün sonu kasa verisi başarıyla eklendi." });
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Şube gün sonu kasa verisi eklenirken hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Şube gün sonu kasa verilerini toplu olarak uzak sunucuya ekler
        /// </summary>
        [HttpPost]
        [Route("addBulk")]
        public IHttpActionResult AddSubeGunSonuKasasiBulk(List<SubeGunSonuKasasi> modelList)
        {
            if (modelList == null || !modelList.Any())
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
                            foreach (var model in modelList)
                            {
                                var query = $@"INSERT INTO SubeGunSonuKasasi
                                    (SubeKodu, Nakit, KrediKarti, KayitTarihi, GuncellemeTarihi, Durum)
                                    VALUES
                                    (@SubeKodu, @Nakit, @KrediKarti, GETDATE(), GETDATE(), 1)";

                                using (var command = new SqlCommand(query, sqlConnection, transaction))
                                {
                                    // Parametreleri ekle
                                    command.Parameters.AddWithValue("@SubeKodu", model.SubeKodu);
                                    command.Parameters.AddWithValue("@Nakit", model.Nakit);
                                    command.Parameters.AddWithValue("@KrediKarti", model.KrediKarti);

                                    // Sorguyu çalıştır
                                    totalAffectedRows += command.ExecuteNonQuery();
                                }
                            }

                            // Transaction'ı commit et
                            transaction.Commit();
                            return Ok(new { Success = true, AffectedRows = totalAffectedRows, Message = $"{modelList.Count} adet şube gün sonu kasa verisi başarıyla eklendi." });
                        }
                        catch (Exception ex)
                        {
                            // Hata durumunda transaction'ı geri al
                            transaction.Rollback();
                            return BadRequest($"Şube gün sonu kasa verileri eklenirken hata oluştu: {ex.Message}");
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
        /// Örnek olarak verilen metodu kullanarak şube gün sonu kasa verisini ekler
        /// </summary>
        [HttpPost]
        [Route("addWithCustomMethod")]
        public IHttpActionResult AddWithCustomMethod(SubeGunSonuKasasi model)
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

                    // Verilen özel metodu kullanarak veriyi ekle
                    SubeGunSonuKasasiEkle(sqlConnection, model);

                    return Ok(new { Success = true, Message = "Şube gün sonu kasa verisi özel metot kullanılarak başarıyla eklendi." });
                }
                catch (Exception ex)
                {
                    return BadRequest($"Şube gün sonu kasa verisi eklenirken hata oluştu: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Verilen özel metot - Şube gün sonu kasa verilerini ekler
        /// </summary>
        private void SubeGunSonuKasasiEkle(SqlConnection sqlConnection, SubeGunSonuKasasi subeGunSonuKasasi)
        {
            // Not: allVariables yapısına erişim olmadığı için, bu kısmı düzenlemeniz gerekebilir
            // Örnek olarak hardcoded değerlerle gösteriyorum
            string firmaAdi = subeGunSonuKasasi.SubeKodu; // allVariables.lisans.FirmaAdi yerine

            var sqlCommand = new SqlCommand($@"
INSERT INTO SubeGunSonuKasasi
(
    SubeKodu,
    Nakit,
    KrediKarti,
    KayitTarihi,
    GuncellemeTarihi,
    Durum
)
VALUES
(
    '{firmaAdi}',          
    {subeGunSonuKasasi.Nakit},
    {subeGunSonuKasasi.KrediKarti},            
    GETDATE(),
    GETDATE(),
    1
)");
            sqlCommand.CommandTimeout = 0;
            sqlCommand.Connection = sqlConnection;
            sqlCommand.ExecuteNonQuery();
        }
    }
}