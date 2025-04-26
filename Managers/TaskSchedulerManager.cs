using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;

public class TaskSchedulerManager
{
    private readonly string _appName;
    private readonly string _appPath;
    private readonly string _taskName;

    public TaskSchedulerManager(string appName, string appPath)
    {
        if (string.IsNullOrEmpty(appName))
            throw new ArgumentException("Uygulama adı boş olamaz.", nameof(appName));

        if (string.IsNullOrEmpty(appPath) || !File.Exists(appPath))
            throw new ArgumentException("Geçerli bir uygulama yolu belirtilmelidir.", nameof(appPath));

        _appName = appName;
        _appPath = appPath;
        _taskName = $"{_appName}StartupTask";
    }

    /// <summary>
    /// Uygulamanın başlangıçta çalışacak şekilde ayarlanıp ayarlanmadığını kontrol eder.
    /// </summary>
    public bool IsStartupEnabled()
    {
        try
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "schtasks";
                process.StartInfo.Arguments = $"/query /tn \"{_taskName}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                return process.ExitCode == 0 && output.Contains(_taskName);
            }
        }
        catch (Exception ex)
        {
            LogError($"Başlangıç durumu kontrol edilirken hata oluştu: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Uygulamayı Windows başlangıcında çalışacak şekilde ayarlar.
    /// </summary>
    public bool EnableStartup(bool runAsAdmin = false)
    {
        try
        {
            // Önce varsa mevcut görevi kaldır
            if (IsStartupEnabled())
                DisableStartup();

            // Temel komutu hazırla - /d parametresini kaldırdık
            string arguments = $"/create /tn \"{_taskName}\" /tr \"\\\"{_appPath}\\\"\" /sc onlogon /f";

            // Yönetici olarak çalıştırma seçeneği
            if (runAsAdmin)
                arguments += " /rl highest";

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "schtasks";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    LogError($"Görev oluşturma başarısız. Hata: {error}");
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            LogError($"Başlangıç ayarı etkinleştirilirken hata oluştu: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Uygulamanın Windows başlangıcında çalışmasını devre dışı bırakır.
    /// </summary>
    public bool DisableStartup()
    {
        try
        {
            if (!IsStartupEnabled())
                return true;

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "schtasks";
                process.StartInfo.Arguments = $"/delete /tn \"{_taskName}\" /f";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    LogError($"Görev silme başarısız. Hata: {error}");
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            LogError($"Başlangıç ayarı devre dışı bırakılırken hata oluştu: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Mevcut kullanıcının yönetici haklarına sahip olup olmadığını kontrol eder.
    /// </summary>
    public static bool IsRunningAsAdmin()
    {
        try
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gecikmeli başlatma için XML tabanlı görev tanımı kullanır
    /// </summary>
    public bool EnableStartupWithDelay(int delaySeconds = 30)
    {
        try
        {
            // Önce varsa mevcut görevi kaldır
            if (IsStartupEnabled())
                DisableStartup();

            string xmlPath = Path.GetTempFileName();
            string xml = $@"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.2"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Description>{_appName} uygulamasını Windows başlangıcında çalıştırır.</Description>
  </RegistrationInfo>
  <Triggers>
    <LogonTrigger>
      <Enabled>true</Enabled>
      <Delay>PT{delaySeconds}S</Delay>
    </LogonTrigger>
  </Triggers>
  <Principals>
    <Principal id=""Author"">
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>LeastPrivilege</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>false</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable>
    <IdleSettings>
      <StopOnIdleEnd>false</StopOnIdleEnd>
      <RestartOnIdle>false</RestartOnIdle>
    </IdleSettings>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>false</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>false</WakeToRun>
    <ExecutionTimeLimit>PT0S</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions Context=""Author"">
    <Exec>
      <Command>""{_appPath}""</Command>
    </Exec>
  </Actions>
</Task>";

            File.WriteAllText(xmlPath, xml);

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "schtasks";
                process.StartInfo.Arguments = $"/create /tn \"{_taskName}\" /xml \"{xmlPath}\" /f";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                File.Delete(xmlPath); // Geçici XML dosyasını sil

                if (process.ExitCode != 0)
                {
                    LogError($"XML ile görev oluşturma başarısız. Hata: {error}");
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            LogError($"Gecikmeli başlangıç ayarı etkinleştirilirken hata oluştu: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Hata mesajlarını loglar.
    /// </summary>
    private void LogError(string message)
    {
        // Bu kısmı kendi log sisteminize göre değiştirin
        Debug.WriteLine($"[TaskSchedulerManager Error] {message}");

        // Basit dosya loglama
        try
        {
            string logFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                _appName);

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            string logFile = Path.Combine(logFolder, "startup_manager.log");
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}{Environment.NewLine}";

            File.AppendAllText(logFile, logEntry);
        }
        catch
        {
            // Loglama sırasında hata oluşursa sessizce devam et
        }
    }
}