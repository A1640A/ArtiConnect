using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Api.Modals
{
    public class KullaniciRequestModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        public string AdSoyad { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Şifre en fazla 50 karakter olabilir.")]
        public string Sifre { get; set; }

        public string Gorev { get; set; }
        public string VarsayilanTelefon { get; set; }
        public string VarsayilanAdres { get; set; }

        // Tüm yetki alanları
        public bool SiparisiVerilenUrunSilme { get; set; }
        public bool IskontoYapma { get; set; }
        public bool AdisyonYazdirma { get; set; }
        public bool IkramUrunEkleme { get; set; }
        public bool AdisyonIptalEtme { get; set; }
        public bool UrunFiyatDegistirme { get; set; }
        public bool AdisyonKapatma { get; set; }
        public bool UrunTasima { get; set; }
        public bool MasayaSiparis { get; set; }
        public bool OnlineSatis { get; set; }
        public bool MasaTasima { get; set; }
        public bool MasaBirlestirme { get; set; }
        public bool AdisyonBolme { get; set; }
        public bool OzelMasaAcma { get; set; }
        public bool StokYonetimi { get; set; }
        public bool Rezervasyon { get; set; }
        public bool KapananAdisyon { get; set; }
        public bool Yonetim { get; set; }
        public bool MasaEkleCikar { get; set; }
        public bool UrunEkleCikarDuzenle { get; set; }
        public bool MusteriIslemleri { get; set; }
        public bool FirmaBilgileri { get; set; }
        public bool Raporlar { get; set; }
        public bool CariIslemler { get; set; }
        public bool PersonelIslemleri { get; set; }
        public bool KasaIslemleri { get; set; }
        public bool YazıcıAyarlari { get; set; }
        public bool HizliSatis { get; set; }
        public bool PaketSiparis { get; set; }
        public bool KasaOzeti { get; set; }
        public bool RaporDuzenleme { get; set; }
        public bool TumKasaOzetiniGor { get; set; }
        public bool OdemeAlma { get; set; }
        public bool UrunYonetimi { get; set; }
        public bool Muhasebe { get; set; }
        public bool Ayarlar { get; set; }
        public bool SipariştenSonraOtomatikKapat { get; set; }
        public bool YazdirilanAdisyonuDegistirme { get; set; }
        public bool YazdirVeKapat { get; set; }
        public bool KilitAcma { get; set; }
        public bool KasaAcButonYetkisi { get; set; }
        public bool NakitteKasayiAc { get; set; }
        public bool KrediKartindaKasayiAc { get; set; }
        public bool CarideKasayiAc { get; set; }
        public bool DigerOdemedeKasayiAc { get; set; }
        public int KullaniciIslemDurumu { get; set; }
        public bool AcikAdisyonaCevirme { get; set; }
        public bool AdisyonDuzenleme { get; set; }
        public bool KasiyerRaporu { get; set; }
        public string AcilisEkrani { get; set; }
        public bool EIrsaliye { get; set; }
        public string Kod { get; set; }
        public int ElTerminaliIslemDurumu { get; set; }
        public string ElTerminaliLisansi { get; set; }
        public bool BelgeIptal { get; set; }
        public bool FarkliGarsonunMasasiniDuzenleme { get; set; }
        public bool SayimYapma { get; set; }
        public bool AnaNoktaUrunTalebiGirme { get; set; }
        public bool GunSonuKasaGirisi { get; set; }
        public bool OdemeAlVeKilitle { get; set; }
        public bool Firmalar { get; set; }
        public bool FaturaGirisi { get; set; }
        public bool FirmaHareketleri { get; set; }

        // Teknik Servis Yetkileri
        public bool Satis_TeknikServis { get; set; }
        public bool FaturaGirisi_TeknikServis { get; set; }
        public bool StokIslem_TeknikServis { get; set; }
        public bool MasrafGirisi_TeknikServis { get; set; }
        public bool TeknikServis_TeknikServis { get; set; }
        public bool Firmalar_TeknikServis { get; set; }
        public bool GuncelStok_TeknikServis { get; set; }
        public bool KapananFisler_TeknikServis { get; set; }
        public bool GidenFaturalar_TeknikServis { get; set; }
        public bool TalepFormu_TeknikServis { get; set; }
        public bool UrunYonetimi_TeknikServis { get; set; }
        public bool Raporlar_TeknikServis { get; set; }
        public bool GelenFaturalar_TeknikServis { get; set; }
        public bool CariIslemler_TeknikServis { get; set; }
        public bool ServisIslemleri_TeknikServis { get; set; }
        public bool GunSonu_TeknikServis { get; set; }
        public bool Ayarlar_TeknikServis { get; set; }
        public bool KasiyerRaporu_TeknikServis { get; set; }
        public bool TeknikServisRaporlari_TeknikServis { get; set; }
        public bool Ciro_TeknikServis { get; set; }
        public bool CariListesi_TeknikServis { get; set; }
        public bool CariEkleme_TeknikServis { get; set; }
        public bool OdemeAlma_TeknikServis { get; set; }
        public bool Iskonto_TeknikServis { get; set; }
        public bool OnSatis_TeknikServis { get; set; }
        public bool SatisIptal_TeknikServis { get; set; }
        public bool KasaAc_TeknikServis { get; set; }
        public bool GecmisSatis_TeknikServis { get; set; }
        public bool FiyatGorme_TeknikServis { get; set; }
        public bool TeknikServisOdemeAlma_TeknikServis { get; set; }
        public bool TeknikServisGoruntuleme_TeknikServis { get; set; }
        public bool TeknikServisYazdirma_TeknikServis { get; set; }
        public bool TeknikServisDuzenleme_TeknikServis { get; set; }
        public bool TeknikServisGecmisGorme_TeknikServis { get; set; }
        public bool UrunEkleDuzenleSil_TeknikServis { get; set; }
        public bool GrupEkleDuzenleSil_TeknikServis { get; set; }
        public bool KullaniciAyarlari_TeknikServis { get; set; }
        public bool YaziciAyarlari_TeknikServis { get; set; }
        public bool CiktiTasarimlari_TeknikServis { get; set; }
        public bool KasaAcButonYetkisi_TeknikServis { get; set; }
        public bool NakitteKasaAc_TeknikServis { get; set; }
        public bool KrediKartindaKasaAc_TeknikServis { get; set; }
        public bool CarideKasaAc_TeknikServis { get; set; }
        public bool PersonelIslemleri_TeknikServis { get; set; }
        public bool TeknikServisTamamlama_TeknikServis { get; set; }
        public bool TeknikServisUstaAtama_TeknikServis { get; set; }
        public int ServisNo { get; set; }

        // Hurda Satış Yetkileri
        public bool Satis_HurdaSatis { get; set; }
        public bool AktifIslemler_HurdaSatis { get; set; }
        public bool GidenFaturalar_HurdaSatis { get; set; }
        public bool GelenFaturalar_HurdaSatis { get; set; }
        public bool FaturaGirisi_HurdaSatis { get; set; }
        public bool Firmalar_HurdaSatis { get; set; }
        public bool GuncelStok_HurdaSatis { get; set; }
        public bool UrunYonetimi_HurdaSatis { get; set; }
        public bool CariIslemler_HurdaSatis { get; set; }
        public bool StokIslemleri_HurdaSatis { get; set; }
        public bool MasrafGirisi_HurdaSatis { get; set; }
        public bool KapananFisler_HurdaSatis { get; set; }
        public bool Raporlar_HurdaSatis { get; set; }
        public bool Ayarlar_HurdaSatis { get; set; }
        public bool Kasa_HurdaSatis { get; set; }
        public bool KasaIslemleri_HurdaSatis { get; set; }
        public bool MiktariManuelGirme_HurdaSatis { get; set; }
        public bool DarayiManuelGirme_HurdaSatis { get; set; }
        public bool AdisyonIptal_HurdaSatis { get; set; }
        public bool AktifIslemUrunSilme_HurdaSatis { get; set; }

        public string Sirket { get; set; }

        // Şube ID'leri
        public List<int> SubeIds { get; set; }
    }
}