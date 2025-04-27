using Squirrel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ArtiConnect
{
    public partial class frmGuncelle : Form
    {
        private const string GITHUB_REPO = "https://github.com/A1640A/ArtiConnect";
        private bool _updateAvailable = false;
        private bool _updateCompleted = false;

        public frmGuncelle()
        {
            InitializeComponent();
        }

        private void SetCaption(string caption)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(SetCaption), caption);
                return;
            }

            lbCaption.Text = caption;
            Application.DoEvents(); // UI'ın güncellenmesini sağlar
        }

        private void SetProgress(int progress, string caption)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int, string>(SetProgress), progress, caption);
                return;
            }

            // Progress değerini sınırla (0-100 arası)
            progress = Math.Max(0, Math.Min(100, progress));

            pbProgress.Value = progress;
            SetCaption(caption);
            Application.DoEvents(); // UI'ın güncellenmesini sağlar
        }

        private void RestartApplication()
        {
            try
            {
                string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                // Uygulama yolunu kontrol et
                if (!File.Exists(appPath))
                {
                    // Eğer mevcut yol bulunamazsa, alternatif yol dene
                    string appDir = Path.GetDirectoryName(Application.ExecutablePath);
                    string appName = Path.GetFileName(Application.ExecutablePath);
                    appPath = Path.Combine(appDir, appName);
                }

                // Yeni bir process başlat
                Process.Start(new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true
                });

                // Mevcut uygulamayı kapat
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Uygulama yeniden başlatılamadı: {ex.Message}", "Hata",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task CheckForUpdates()
        {
            try
            {
                // Güncelleme kontrolü başlangıcı
                SetCaption("Güncellemeler kontrol ediliyor...");
                SetProgress(0, "Güncellemeler kontrol ediliyor...");

                // Geçici log dosyası oluştur (hata ayıklama için)
                string logPath = Path.Combine(Path.GetTempPath(), "ArtiConnect_Update.log");
                File.WriteAllText(logPath, $"Güncelleme kontrolü başladı: {DateTime.Now}\n");

                // GitHub repo'dan güncelleme kontrolü
                using (var updateManager = await UpdateManager.GitHubUpdateManager(GITHUB_REPO))
                {
                    // Mevcut sürüm bilgisini logla
                    var currentVersion = updateManager.CurrentlyInstalledVersion();
                    File.AppendAllText(logPath, $"Mevcut sürüm: {(currentVersion != null ? currentVersion.Version.ToString() : "İlk kurulum")}\n");

                    // Güncelleme kontrolü
                    var updateInfo = await updateManager.CheckForUpdate(
                        ignoreDeltaUpdates: false,
                        progress: (percentage) => SetProgress(percentage, $"Güncellemeler kontrol ediliyor... (%{percentage})")
                    );

                    // Güncelleme sonuçlarını logla
                    File.AppendAllText(logPath, $"Kontrol tamamlandı. Uygulanacak sürüm sayısı: {updateInfo.ReleasesToApply.Count}\n");

                    if (updateInfo.ReleasesToApply.Count > 0)
                    {
                        this.WindowState = FormWindowState.Normal;
                        _updateAvailable = true;

                        // Kullanıcıya güncelleme hakkında bilgi ver
                        var latestVersion = updateInfo.FutureReleaseEntry.Version;
                        File.AppendAllText(logPath, $"Yeni sürüm bulundu: {latestVersion}\n");

                        var result = MessageBox.Show(
                            $"Yeni bir güncelleme mevcut (v{latestVersion}).\n\nŞimdi güncellemek istiyor musunuz?",
                            "Güncelleme Mevcut",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (result == DialogResult.Yes)
                        {
                            // Güncelleme indirme ve kurma
                            SetCaption("Güncelleme indiriliyor ve kuruluyor...");
                            SetProgress(0, "Güncelleme indiriliyor...");

                            File.AppendAllText(logPath, "Güncelleme indiriliyor...\n");

                            var newRelease = await updateManager.UpdateApp(
                                progress: (percentage) => SetProgress(percentage, $"Güncelleme indiriliyor... (%{percentage})")
                            );

                            _updateCompleted = true;
                            File.AppendAllText(logPath, $"Güncelleme tamamlandı. Yeni sürüm: {newRelease}\n");

                            SetCaption("Güncelleme tamamlandı. Uygulama yeniden başlatılacak...");
                            SetProgress(100, "Güncelleme tamamlandı");

                            // Kullanıcıya bilgi ver ve uygulamayı yeniden başlat
                            MessageBox.Show(
                                "Güncelleme başarıyla tamamlandı. Uygulama şimdi yeniden başlatılacak.",
                                "Güncelleme Tamamlandı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                            await Task.Delay(500);
                             
                            try
                            {
                                // Önce standart yöntemi dene
                                UpdateManager.RestartApp();
                            }
                            catch (Exception ex)
                            {
                                // Hata durumunda alternatif yöntemi kullan
                                RestartApplication();
                            }
                        }
                        else
                        {
                            File.AppendAllText(logPath, "Kullanıcı güncellemeyi reddetti.\n");
                            SetCaption("Güncelleme iptal edildi.");
                        }
                    }
                    else
                    {
                        File.AppendAllText(logPath, "Yeni güncelleme bulunamadı.\n");
                        SetCaption("Uygulamanız güncel.");
                        SetProgress(100, "Uygulamanız güncel");

                        // 2 saniye bekleyip formu kapat
                        await Task.Delay(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda detaylı log tut
                string logPath = Path.Combine(Path.GetTempPath(), "ArtiConnect_UpdateError.log");
                File.WriteAllText(logPath, $"Güncelleme hatası: {DateTime.Now}\n{ex.ToString()}\n\nInner Exception: {ex.InnerException?.ToString()}");

                MessageBox.Show(
                    $"Uygulama güncellenirken bir hata oluştu:\n\n{ex.Message}\n\nDetaylı hata bilgisi şu konumda kaydedildi:\n{logPath}",
                    "Güncelleme Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Hata durumunda manuel güncelleme seçeneği sun
                var result = MessageBox.Show(
                    "Manuel olarak güncelleme sayfasını açmak ister misiniz?",
                    "Manuel Güncelleme",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = $"{GITHUB_REPO}/releases/latest",
                        UseShellExecute = true
                    });
                }
            }
            finally
            {
                // Güncelleme yoksa veya kullanıcı reddettiyse formu kapat
                if (!_updateAvailable || !_updateCompleted)
                {
                    await Task.Delay(1000); // Kullanıcının mesajı okuması için kısa bir bekleme
                    Close();
                }
            }
        }

        private async void frmGuncelle_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde güncelleme kontrolünü başlat
            await CheckForUpdates();
        }

        // Kullanıcı formu kapatmaya çalışırsa güncelleme işlemini iptal etmek isteyip istemediğini sor
        private void frmGuncelle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_updateAvailable && !_updateCompleted)
            {
                var result = MessageBox.Show(
                    "Güncelleme işlemi devam ediyor. İptal etmek istediğinize emin misiniz?",
                    "Güncelleme İptal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        // Form tasarımında iptal butonu eklediyseniz bu metodu kullanabilirsiniz
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_updateAvailable && !_updateCompleted)
            {
                var result = MessageBox.Show(
                    "Güncelleme işlemini iptal etmek istediğinize emin misiniz?",
                    "Güncelleme İptal",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }
    }
}