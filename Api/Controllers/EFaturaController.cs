using ArtiConnect.Api.Modals.Ticari1;
using ArtiConnect.DataAccess;
using ArtiConnect.Integrations.Ticari1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using static ArtiConnect.Integrations.Ticari1.Modals;

namespace ArtiConnect.Api.Controllers
{
    [ApiLogger]
    [RoutePrefix("api/eFatura")]
    public class EFaturaController : BaseApiController
    {
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// E-Fatura oluşturur
        /// </summary>
        [HttpPost]
        [Route("eFaturaOlustur")]
        public IHttpActionResult OlusturEFatura(EFaturaRequest request)
        {
            try
            {
                if (request == null || request.EFaturaModal == null)
                {
                    return BadRequest("Geçersiz fatura bilgileri.");
                }

                if (!request.EFaturaModal.FaturaSatirlari.Any())
                {
                    return BadRequest("Fatura satırları boş olamaz.");
                }

                var result = Ticari1Entegrasyon.EFaturaOlustur(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.SubeNo,
                    request.EFaturaModal
                );

                if (result == null)
                {
                    return BadRequest("E-Fatura oluşturulurken bir hata oluştu.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"E-Fatura oluşturulurken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// E-Fatura PDF'ini alır ve isteğe bağlı olarak yazdırır
        /// </summary>
        [HttpPost]
        [Route("getEfaturaPdf")]
        public IHttpActionResult GetEFaturaPdf(EFaturaPdfRequest request)
        {
            try
            { 
                var result = Ticari1Entegrasyon.GetEFaturaPdf(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.PdfModal,
                    request.YaziciAdi,
                    request.KopyaSayisi
                );

                return Ok(new { Success = result });
            }
            catch (Exception ex)
            {
                return BadRequest($"E-Fatura PDF'i alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// E-Arşiv faturasını iptal eder
        /// </summary>
        [HttpPost]
        [Route("iptalEArsiv")]
        public IHttpActionResult IptalEArsiv(EArsivIptalRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.FaturaKey))
                {
                    return BadRequest("Fatura Key bilgisi gereklidir.");
                }

                var (message, success) = Ticari1Entegrasyon.EArsivIptal(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.FaturaKey
                );

                return Ok(new { Message = message, Success = success });
            }
            catch (Exception ex)
            {
                return BadRequest($"E-Arşiv faturası iptal edilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Fatura XML'ini alır
        /// </summary>
        [HttpPost]
        [Route("getFaturaXml")]
        public IHttpActionResult GetFaturaXml(FaturaXmlRequest request)
        {
            try
            {
                var response = Ticari1Entegrasyon.GetFaturaXml(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.Vkn,
                    request.FaturaKey
                );

                if (response == null)
                {
                    return BadRequest("Fatura XML'i alınamadı.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Fatura XML'i alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli tarih aralığındaki faturaların XML'lerini alır
        /// </summary>
        [HttpPost]
        [Route("getFaturaXmlCollection")]
        public IHttpActionResult GetFaturaXmlCollection(FaturaXmlCollectionRequest request)
        {
            try
            {
                var response = Ticari1Entegrasyon.GetFaturaXmlColl(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.Vkn,
                    request.BaslangicTarihi,
                    request.BitisTarihi
                );

                if (response == null)
                {
                    return BadRequest("Fatura XML koleksiyonu alınamadı.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Fatura XML koleksiyonu alınırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirli tarih aralığındaki gelen faturaları listeler
        /// </summary>
        [HttpPost]
        [Route("getGelenFaturaList")]
        public IHttpActionResult GetGelenFaturaList(GelenFaturaListRequest request)
        {
            try
            {
                var response = Ticari1Entegrasyon.GetGelenFaturaList(
                    request.ApiKey,
                    request.SecretKey,
                    request.CustomerId,
                    request.BaslangicTarihi,
                    request.BitisTarihi
                );

                if (response == null || !response.Any())
                {
                    return Ok(new List<Ticari1Entegrasyon.Ticari1Fatura>());
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Gelen faturalar listelenirken hata oluştu: {ex.Message}");
            }
        }
    }
}