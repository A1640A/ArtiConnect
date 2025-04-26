using ArtiConnect.DataAccess;
using ArtiConnect.Entities;
using Microsoft.Owin.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtiConnect
{
    public partial class frmAna : Form
    {
        private IDisposable _webApp;
        private string _baseUrl;
        private AppDbContext _dbContext;
        private TaskSchedulerManager _startupManager;
        bool isLoading = false;
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private bool _startMinimized = false;

        public frmAna(bool startMinimized = false)
        {
            InitializeComponent();
            SetupTrayIcon();

            _startMinimized = startMinimized;

            // Veritabanı dosya yolunu ayarla
            string dbFolderPath = Application.StartupPath;
            if (!Directory.Exists(dbFolderPath))
                Directory.CreateDirectory(dbFolderPath);

            AppDomain.CurrentDomain.SetData("DataDirectory", dbFolderPath);

            _dbContext = new AppDbContext();

            // Veritabanını oluştur/güncelle
            _dbContext.Database.Initialize(false);

            LoadSettings();
        }

        private void SetupTrayIcon()
        { 
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Göster", null, OnShowClick);
            trayMenu.Items.Add("Çıkış", null, OnExitClick);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "ArtiConnect"; 
            trayIcon.Icon = this.Icon; 
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;

            trayIcon.DoubleClick += OnTrayIconDoubleClick;
        }

        private void OnTrayIconDoubleClick(object sender, EventArgs e)
        { 
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void OnShowClick(object sender, EventArgs e)
        { 
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
        }

        private void OnExitClick(object sender, EventArgs e)
        { 
            Application.Exit();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Bildirim simgesini temizle
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }

            base.OnFormClosing(e);
        }

        private void LoadSettings()
        {
            try
            {
                var portSetting = _dbContext.Ayars.FirstOrDefault();
                txtPort.Text = portSetting.Port;
                tbSunucu.Text = portSetting.RemoteDbServerName;
                tbKullaniciAdi.Text = portSetting.RemoteDbUserName;
                tbSifre.Text = portSetting.RemoteDbPassword;
                tbVeritabani.Text = portSetting.RemoteDbDatabaseName;

                UpdateStatus("Hazır");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Hata: Ayarlar yüklenemedi");
            }
        }

        private void frmAna_Load(object sender, EventArgs e)
        {
            isLoading = true;
            try
            {
                string appPath = Application.ExecutablePath;
                string appName = Application.ProductName;

                _startupManager = new TaskSchedulerManager(appName, appPath);

                // CheckBox'ın durumunu ayarla
                chkStartWithWindows.Checked = _startupManager.IsStartupEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Başlangıç ayarları yüklenirken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkStartWithWindows.Enabled = false;
            }
            isLoading = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_webApp == null)
            {
                try
                {
                    string port = txtPort.Text;
                    string allInterfacesUrl = $"http://+:{port}";
                    string localhostUrl = $"http://localhost:{port}";

                    // Başlatma seçeneklerini ayarla
                    var options = new StartOptions();
                    options.Urls.Add(allInterfacesUrl);

                    _baseUrl = localhostUrl;

                    // Ayarı güncelle
                    var portSetting = _dbContext.Ayars.FirstOrDefault();
                    if (portSetting != null)
                    {
                        portSetting.Port = port;
                        _dbContext.SaveChanges();
                    }

                    // Web API'yi başlat
                    _webApp = WebApp.Start<Api.Startup>(options);

                    // UI'ı güncelle
                    btnStart.Text = "Durdur";
                    txtPort.Enabled = false;
                    btnOpenSwagger.Enabled = true;

                    var ipAddresses = GetAllLocalIPAddresses();
                    string ipList = string.Join(", ", ipAddresses);

                    UpdateStatus($"API tüm arayüzlerde ({ipList}) port {port} üzerinde çalışıyor");

                    LogApiEvent("API Başlatıldı", $"API tüm arayüzlerde ({ipList}) port {port} üzerinde çalışıyor");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"API başlatılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateStatus("Hata: API başlatılamadı");
                }
            }
            else
            {
                StopWebApi();
            }
        }

        private List<string> GetAllLocalIPAddresses()
        {
            var ipAddresses = new List<string>();

            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddresses.Add(ip.ToString());
                    }
                }
            }
            catch
            {
                ipAddresses.Add("127.0.0.1");
            }

            return ipAddresses;
        }

        private string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }

                return "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        private void StopWebApi()
        {
            if (_webApp != null)
            {
                try
                {
                    _webApp.Dispose();
                    _webApp = null;
                     
                    btnStart.Text = "Başlat";
                    txtPort.Enabled = true;
                    btnOpenSwagger.Enabled = false;

                    UpdateStatus("API durduruldu");
                     
                    LogApiEvent("API Durduruldu", "API servisi durduruldu");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"API durdurulurken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpenSwagger_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start($"{_baseUrl}/swagger");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Swagger açılırken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogApiEvent(string eventName, string description)
        {
            try
            {
                _dbContext.ApiLogs.Add(new ApiLog
                {
                    Timestamp = DateTime.Now,
                    Endpoint = eventName,
                    Method = "SYSTEM",
                    RequestData = description,
                    ResponseData = "",
                    StatusCode = 200
                });
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Log kaydedilirken hata: {ex.Message}");
            }
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = message;
            statusStrip.Items["statusLabel"].Text = message;
        }

        private void frmAna_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopWebApi();

            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        private void btKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                var connectionString = $"Server={tbSunucu.Text};User ID={tbKullaniciAdi.Text};Password={tbSifre.Text};Database={tbVeritabani.Text}";
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                sqlConnection.Close();

                var setting = _dbContext.Ayars.FirstOrDefault();
                if (setting != null)
                {
                    setting.RemoteDbServerName = tbSunucu.Text;
                    setting.RemoteDbUserName = tbKullaniciAdi.Text;
                    setting.RemoteDbPassword = tbSifre.Text;
                    setting.RemoteDbDatabaseName = tbVeritabani.Text;
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void chkStartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            if (_startupManager == null) return;

            try
            {
                bool success;

                if (chkStartWithWindows.Checked)
                {
                    success = _startupManager.EnableStartup(true);

                    if (!success)
                    {
                        MessageBox.Show("Başlangıçta çalıştırma özelliği etkinleştirilemedi.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        chkStartWithWindows.Checked = false;
                    }
                    else
                    {
                        if (isLoading == false)
                            MessageBox.Show("Uygulama Windows başlangıcında çalışacak şekilde ayarlandı.",
                                "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            btnStart_Click(null, null);
                    }
                }
                else
                {
                    success = _startupManager.DisableStartup();
                    if (!success)
                    {
                        MessageBox.Show("Başlangıçta çalıştırma özelliği devre dışı bırakılamadı.",
                            "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        chkStartWithWindows.Checked = true;
                    }
                    else
                    {
                        if (isLoading == false)
                            MessageBox.Show("Uygulamanın Windows başlangıcında çalışması devre dışı bırakıldı.",
                            "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                if (isLoading == false)
                    MessageBox.Show($"Başlangıç ayarları değiştirilirken hata oluştu: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

                chkStartWithWindows.CheckedChanged -= chkStartWithWindows_CheckedChanged;
                chkStartWithWindows.Checked = _startupManager.IsStartupEnabled();
                chkStartWithWindows.CheckedChanged += chkStartWithWindows_CheckedChanged;
            }
        }

        private void frmAna_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            { 
                Hide();
                 
                //trayIcon.ShowBalloonTip(
                //    3000,
                //    Application.ProductName,
                //    "Uygulama arka planda çalışıyor. Açmak için simgeye çift tıklayın.",
                //    ToolTipIcon.Info
                //);
            }
        }
    }
}