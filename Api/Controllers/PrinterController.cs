using Artı.Reports;
using ArtiConnect.Api.Modals.Printer;
using DevExpress.LookAndFeel;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using DevExpress.XtraCharts;

namespace ArtiConnect.Api.Controllers
{
    /// <summary>
    /// Controller for handling printer and report designer operations
    /// </summary>
    [ApiLogger]
    [RoutePrefix("api/printer")]
    public class PrinterController : ApiController
    {
        #region Windows API Definitions
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // ShowWindow commands
        private const int SW_MAXIMIZE = 3;
        #endregion

        private const string REPORTS_DIRECTORY = "Reports";
        private const int WINDOW_SEARCH_ATTEMPTS = 20;
        private const int WINDOW_SEARCH_DELAY_MS = 100;
        private const int WINDOW_SEARCH_TIMEOUT_SEC = 3;

        /// <summary>
        /// Opens a report in the DevExpress report designer
        /// </summary>
        /// <param name="xtraReport">The report to open in the designer</param>
        private void OpenDesigner(XtraReport xtraReport)
        {
            // Save the list of currently open windows
            var initialWindows = GetAllWindowHandles();

            var thread = new Thread(() =>
            {
                try
                {
                    var designTool = new ReportDesignTool(xtraReport);
                    var lookAndFeel = new UserLookAndFeel(null) { UseDefaultLookAndFeel = true };

                    // Start a background task to maximize the designer window
                    var maximizeTask = Task.Run(() =>
                    {
                        // Wait briefly for the window to start opening
                        Thread.Sleep(WINDOW_SEARCH_DELAY_MS);

                        // Try to find and maximize the designer window
                        for (int i = 0; i < WINDOW_SEARCH_ATTEMPTS; i++)
                        {
                            Thread.Sleep(WINDOW_SEARCH_DELAY_MS);

                            var newWindow = FindNewlyOpenedDesignerWindow(initialWindows);
                            if (newWindow != IntPtr.Zero)
                            {
                                ShowWindow(newWindow, SW_MAXIMIZE);
                                SetForegroundWindow(newWindow);
                                Debug.WriteLine($"Designer window maximized on attempt {i + 1}.");
                                break;
                            }
                        }
                    });

                    // Show the designer
                    designTool.ShowRibbonDesignerDialog(lookAndFeel);

                    // Wait for the maximize operation to complete
                    maximizeTask.Wait(TimeSpan.FromSeconds(WINDOW_SEARCH_TIMEOUT_SEC));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error opening designer: {ex.Message}");
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        /// <summary>
        /// Gets all visible window handles
        /// </summary>
        /// <returns>List of window handles</returns>
        private List<IntPtr> GetAllWindowHandles()
        {
            var handles = new List<IntPtr>();

            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    handles.Add(hWnd);
                }
                return true; // Continue enumeration
            }, IntPtr.Zero);

            return handles;
        }

        /// <summary>
        /// Finds the newly opened designer window
        /// </summary>
        /// <param name="initialWindows">List of windows before opening the designer</param>
        /// <returns>Handle to the designer window, or IntPtr.Zero if not found</returns>
        private IntPtr FindNewlyOpenedDesignerWindow(List<IntPtr> initialWindows)
        {
            var currentWindows = GetAllWindowHandles();
            var newWindows = currentWindows.Except(initialWindows).ToList();

            // Keywords that might appear in the designer window title
            string[] designerKeywords = new[]
            {
                "Report Designer",
                "Rapor Tasarımcısı",
                "XtraReport",
                "DevExpress",
                "Designer"
            };

            foreach (var window in newWindows)
            {
                int length = GetWindowTextLength(window);
                if (length > 0)
                {
                    StringBuilder builder = new StringBuilder(length + 1);
                    GetWindowText(window, builder, builder.Capacity);
                    string title = builder.ToString();

                    // Check if the window title contains any of the designer keywords
                    if (designerKeywords.Any(keyword => title.Contains(keyword)))
                    {
                        return window;
                    }
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Opens the report designer with the specified report
        /// </summary>
        /// <param name="fileName">The name of the report file to open</param>
        /// <returns>HTTP action result</returns>
        [HttpGet]
        [Route("openDesigner")]
        public IHttpActionResult OpenDesigner(string fileName)
        {
            try
            {
                var reportPath = Path.Combine(REPORTS_DIRECTORY, fileName);

                if (!File.Exists(reportPath))
                {
                    return BadRequest($"Report file not found: {fileName}");
                }

                var report = new XtraReport();
                report.LoadLayout(reportPath);
                report.DisplayName = $"{fileName}";

                OpenDesigner(report);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Gets a list of all report files in the Reports directory
        /// </summary>
        /// <returns>List of report file names</returns>
        [HttpGet]
        [Route("getReportFiles")]
        public IHttpActionResult GetReportFiles()
        {
            try
            {
                if (!Directory.Exists(REPORTS_DIRECTORY))
                {
                    return BadRequest($"Reports directory not found: {REPORTS_DIRECTORY}");
                }

                var files = Directory.GetFiles(REPORTS_DIRECTORY)
                    .Select(Path.GetFileName)
                    .ToList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Prints a label using the specified template and data
        /// </summary>
        /// <param name="request">Print label request containing all necessary information</param>
        /// <returns>HTTP action result</returns>
        [HttpPost]
        [Route("printLabel")]
        public IHttpActionResult PrintLabel(PrintLabelRequestModal request)
        {
            if (request == null)
            {
                return BadRequest("Print request cannot be null");
            }

            try
            {
                var reportPath = Path.Combine(REPORTS_DIRECTORY, request.FileName);

                if (!File.Exists(reportPath))
                {
                    return BadRequest($"Report template not found: {request.FileName}");
                }

                var report = new Etiket();
                report.LoadLayout(reportPath);
                report.DisplayName = $"Report Designer - {request.FileName}";

                // Set report data
                report.lbBarkod.Text = request.Barcode;
                report.lbUrunAdi.Text = request.ProductName;
                report.lbFiyat.Text = request.Price.ToString("n2");

                // Configure report settings
                report.CreateDocument();
                report.PrintingSystem.ShowMarginsWarning = false;
                report.PrintingSystem.ShowPrintStatusDialog = false;
                report.RequestParameters = false;

                // Configure print tool
                var printTool = new ReportPrintTool(report);
                printTool.AutoShowParametersPanel = false;
                printTool.PrintingSystem.ShowMarginsWarning = false;
                printTool.PrintingSystem.ShowPrintStatusDialog = false;

                // Set number of copies
                printTool.PrintingSystem.StartPrint += (sender, e) =>
                {
                    e.PrintDocument.PrinterSettings.Copies = (short)request.CopyNumber;
                };

                // Print to the specified printer
                if (!string.IsNullOrEmpty(request.PrinterName))
                {
                    printTool.PrintingSystem.PageSettings.PrinterName = request.PrinterName;
                    printTool.Print(request.PrinterName);
                }
                else
                {
                    // Print to default printer if no printer specified
                    printTool.Print();
                }

                return Ok(new { Success = true, Message = "Label printed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during printing: {ex.Message}");
            }
        }
    }
}