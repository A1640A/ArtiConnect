using ArtiConnect.DataAccess;
using ArtiConnect.Entities;
using ArtiConnect.Managers;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Reflection;
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
        private BindingList<ApiLog> apiLogs = new BindingList<ApiLog>();

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
            SetupApiLogsDataGridView();

            ApiLogManager.LogAdded += OnApiLogAdded;
        }

        private void OnApiLogAdded(object sender, ApiLog log)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<ApiLog>(OnApiLogAdded), sender, log);
                return;
            }

            // Add the log to the UI
            apiLogs.Add(log);

            // Scroll to the latest log
            if (dgvApiLogs.Rows.Count > 0)
            {
                dgvApiLogs.FirstDisplayedScrollingRowIndex = dgvApiLogs.Rows.Count - 1;
                dgvApiLogs.ClearSelection();
                dgvApiLogs.Rows[dgvApiLogs.Rows.Count - 1].Selected = true;
            }
        }

        private void SetupApiLogsDataGridView()
        {
            // DataGridView'ı yapılandır
            dgvApiLogs.AutoGenerateColumns = false;

            // Sütunları ekle
            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Timestamp",
                HeaderText = "Zaman",
                DataPropertyName = "Timestamp",
                Width = 150
            });

            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Endpoint",
                HeaderText = "Endpoint",
                DataPropertyName = "Endpoint",
                Width = 150
            });

            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Method",
                HeaderText = "Metod",
                DataPropertyName = "Method",
                Width = 80
            });

            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StatusCode",
                HeaderText = "Durum Kodu",
                DataPropertyName = "StatusCode",
                Width = 80
            });

            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RequestData",
                HeaderText = "İstek Verisi",
                DataPropertyName = "RequestData",
                Width = 200
            });

            dgvApiLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ResponseData",
                HeaderText = "Yanıt Verisi",
                DataPropertyName = "ResponseData",
                Width = 200
            });

            // DataSource'u ayarla
            dgvApiLogs.DataSource = apiLogs;

            // Son eklenen kaydı görüntüle
            dgvApiLogs.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex == dgvApiLogs.Columns["StatusCode"].Index)
                {
                    int statusCode = Convert.ToInt32(e.Value);
                    if (statusCode >= 200 && statusCode < 300)
                    {
                        e.CellStyle.ForeColor = Color.Green;
                    }
                    else if (statusCode >= 400)
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                }
            };

            // Çift tıklama olayı ekle - detayları göstermek için
            dgvApiLogs.DoubleClick += DgvApiLogs_DoubleClick;
        }

        private void DgvApiLogs_DoubleClick(object sender, EventArgs e)
        {
            if (dgvApiLogs.CurrentRow != null)
            {
                var log = dgvApiLogs.CurrentRow.DataBoundItem as ApiLog;
                if (log != null)
                {
                    // Detay formunu göster
                    ShowApiLogDetails(log);
                }
            }
        }

        static string SafeBeautifyJson(string jsonString)
        {
            try
            {
                JToken parsedJson = JToken.Parse(jsonString);
                return parsedJson.ToString(Formatting.Indented);
            }
            catch (JsonReaderException ex)
            {
                Console.WriteLine($"Geçersiz JSON formatı: {ex.Message}");
                return jsonString; // Hata durumunda orijinal string'i döndür
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Beklenmeyen hata: {ex.Message}");
                return jsonString;
            }
        }

        private void ShowApiLogDetails(ApiLog log)
        {
            // Basit bir detay formu oluştur
            Form detailForm = new Form
            {
                Text = $"API İstek Detayı - {log.Endpoint}",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill
            };

            // Genel bilgiler sekmesi
            TabPage generalTab = new TabPage("Genel Bilgiler");
            TableLayoutPanel generalPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 4,
                Padding = new Padding(10)
            };

            generalPanel.Controls.Add(new Label { Text = "Zaman:", Dock = DockStyle.Fill }, 0, 0);
            generalPanel.Controls.Add(new Label { Text = log.Timestamp.ToString(), Dock = DockStyle.Fill }, 1, 0);

            generalPanel.Controls.Add(new Label { Text = "Endpoint:", Dock = DockStyle.Fill }, 0, 1);
            generalPanel.Controls.Add(new Label { Text = log.Endpoint, Dock = DockStyle.Fill }, 1, 1);

            generalPanel.Controls.Add(new Label { Text = "Metod:", Dock = DockStyle.Fill }, 0, 2);
            generalPanel.Controls.Add(new Label { Text = log.Method, Dock = DockStyle.Fill }, 1, 2);

            generalPanel.Controls.Add(new Label { Text = "Durum Kodu:", Dock = DockStyle.Fill }, 0, 3);
            generalPanel.Controls.Add(new Label { Text = log.StatusCode.ToString(), Dock = DockStyle.Fill }, 1, 3);

            generalTab.Controls.Add(generalPanel);

            // İstek verisi sekmesi
            TabPage requestTab = new TabPage("İstek Verisi");
            TextBox requestTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Text = SafeBeautifyJson(log.RequestData)
            };
            requestTab.Controls.Add(requestTextBox);

            // Yanıt verisi sekmesi
            TabPage responseTab = new TabPage("Yanıt Verisi");
            TextBox responseTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                Text = SafeBeautifyJson(log.ResponseData)
            };
            responseTextBox.Multiline = true;
            responseTextBox.ReadOnly = true;
            responseTab.Controls.Add(responseTextBox);

            // Sekmeleri tab control'e ekle
            tabControl.TabPages.Add(generalTab);
            tabControl.TabPages.Add(requestTab);
            tabControl.TabPages.Add(responseTab);

            // Tab control'ü forma ekle
            detailForm.Controls.Add(tabControl);

            // Formu göster
            detailForm.ShowDialog();
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
        void SetVersionTitle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += $" v.{versionInfo.FileVersion}";
        }

        void CheckForUpdates()
        {
            new frmGuncelle().ShowDialog();
        }

        private void frmAna_Load(object sender, EventArgs e)
        {
            SetVersionTitle();
            CheckForUpdates();

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

                    // Mevcut log kayıtlarını yükle
                    LoadApiLogs();
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

                // Mevcut log kayıtlarını yükle
                LoadApiLogs();
            }
        }

        // Veritabanından log kayıtlarını yükle
        private void LoadApiLogs()
        {
            try
            {
                // Mevcut logları temizle
                apiLogs.Clear();

                // Son 100 log kaydını al
                var logs = _dbContext.ApiLogs
                    .OrderByDescending(l => l.Timestamp)
                    .Take(100)
                    .ToList();

                // Kronolojik sıraya çevir
                logs.Reverse();

                // Logları listeye ekle
                foreach (var log in logs)
                {
                    apiLogs.Add(log);
                }

                // DataGridView'ı güncelle
                dgvApiLogs.Refresh();

                // Son satırı seç
                if (dgvApiLogs.Rows.Count > 0)
                {
                    dgvApiLogs.FirstDisplayedScrollingRowIndex = dgvApiLogs.Rows.Count - 1;
                    dgvApiLogs.ClearSelection();
                    dgvApiLogs.Rows[dgvApiLogs.Rows.Count - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Log kayıtları yüklenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Bildirim simgesini temizle
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }

            // API'yi durdur
            StopWebApi();

            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }

            base.OnFormClosing(e);
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Tüm API log kayıtları temizlenecek. Bu işlem geri alınamaz. Devam etmek istiyor musunuz?",
                    "Log Kayıtlarını Temizle",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Ekrandaki logları temizle
                    apiLogs.Clear();

                    // Veritabanındaki logları temizle
                    _dbContext.Database.ExecuteSqlCommand("DELETE FROM ApiLogs");
                    _dbContext.SaveChanges();

                    MessageBox.Show("Log kayıtları başarıyla temizlendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Log kayıtları temizlenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchLogs_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchText = txtSearchLogs.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchText))
                {
                    // Filtreyi kaldır, tüm logları göster
                    LoadApiLogs();
                    return;
                }

                // Filtrelenmiş logları al
                var filteredLogs = _dbContext.ApiLogs
                    .Where(l =>
                        l.Endpoint.ToLower().Contains(searchText) ||
                        l.Method.ToLower().Contains(searchText) ||
                        l.RequestData.ToLower().Contains(searchText) ||
                        l.ResponseData.ToLower().Contains(searchText) ||
                        l.StatusCode.ToString().Contains(searchText))
                    .OrderByDescending(l => l.Timestamp)
                    .Take(100)
                    .ToList();

                // Kronolojik sıraya çevir
                filteredLogs.Reverse();

                // Mevcut logları temizle ve filtrelenmiş logları ekle
                apiLogs.Clear();
                foreach (var log in filteredLogs)
                {
                    apiLogs.Add(log);
                }

                // DataGridView'ı güncelle
                dgvApiLogs.Refresh();

                // Son satırı seç
                if (dgvApiLogs.Rows.Count > 0)
                {
                    dgvApiLogs.FirstDisplayedScrollingRowIndex = dgvApiLogs.Rows.Count - 1;
                    dgvApiLogs.ClearSelection();
                    dgvApiLogs.Rows[dgvApiLogs.Rows.Count - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Log kayıtları filtrelenirken hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}