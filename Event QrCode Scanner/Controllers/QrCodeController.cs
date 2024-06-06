using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Event_QrCode_Scanner.DbContext;
using Event_QrCode_Scanner.Helper;
using Event_QrCode_Scanner.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Text.RegularExpressions;

namespace Event_QrCode_Scanner.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly ILogger<QrCodeController> _logger;
        private readonly Konteksti _context;
        //private readonly GoogleSheetsService _googleSheetsService;

        public QrCodeController(ILogger<QrCodeController> logger, Konteksti context)
        {
            _logger = logger;
            _context = context;
            //_googleSheetsService = googleSheetsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ruan QR kodin ne databaze nese nuk eshte prezent
        [HttpPost]
        public IActionResult SaveQRCodeData(string qrData)
        {
            if (!qrData.ToLower().Contains("-1111-") || !qrData.ToLower().Contains("-2222-"))
            {
                return StatusCode(503);
            }

            // Kerko ne bazen e te dhenave per te pare nese ekziston nje QR kod i njejte
            var existingQrCodeData = _context.QrCodeData.FirstOrDefault(q => q.QrCode_Data == qrData);

            // Nese ekziston, kthe nje pergjigje gabimi "Bad Request"
            if (existingQrCodeData != null)
            {
                return StatusCode(504);
            }

            try
            {
                // Krijo nje objekt te ri `QrCodeData` per te ruajtur te dhenat e QR kodit dhe kohen e skanimit
                var newQrCodeData = new QrCodeData
                {
                    QrCode_Data = qrData,
                    Koha_skanimit = DateTime.Now
                };

                // Shto te dhenat e reja ne bazen e te dhenave
                _context.QrCodeData.Add(newQrCodeData);
                _context.SaveChanges(); // Ruaj ndryshimet ne bazen e te dhenave

                // Kthe nje pergjigje "OK" me nje mesazh suksesi
                return Ok("QR Kodi eshte ruajtur ne databaze.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Ndodhi nje gabim ju lutem provoni me vone");
            }
        }

        // merr si liste nga databaza per ti shfaqur 
        [HttpGet]
        public IActionResult GetScannedQRCodeData()
        {
            var qrCodeDataList = _context.QrCodeData.ToList();
            return Json(qrCodeDataList);
        }

        // perditson excel file me te dhenat nga databaza
        [HttpPost]
        public IActionResult UpdateSpreadsheet()
        {
            try
            {
                string path;
                string fileName = "\\Konferenca Dev Ops 2024 (Responses).xlsx";

                GetAndSaveDownloadsPath(out path);
                string fullPath = Path.Combine(path + fileName);

                var userIds = _context.QrCodeData.Select(q => q.QrCode_Data).ToList();
                bool nderrimetBera = false; // Kontrollo per ndryshime

                DownloadTheExcelFile(path, fileName);

                using (var workbook = new XLWorkbook(fullPath))
                {
                    var worksheet = workbook.Worksheet(1); // sheet 1
                    var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // kalo rreshtin header 

                    foreach (var row in rows)
                    {
                        string userId = row.Cell(1).GetValue<string>(); // merr vleren prej kolones A (userID)
                        string status = row.Cell(6).GetValue<string>(); // merr vleren prej kolones F (status)

                        if (string.IsNullOrEmpty(status))
                        {
                            row.Cell(6).SetValue("Regjistruar"); // nese nuk ka vlere statusi by default "Regjistruar"
                            nderrimetBera = true; // Flag change
                        }
                        else if (status != "Pjesmarres" && userIds.Contains(userId))
                        {
                            row.Cell(6).SetValue("Pjesmarres"); // jepi vleren "Pjesmarres" nese userID eshte gjet edhe nuk eshte i ndrruar
                            nderrimetBera = true; // Flag change
                        }
                    }
                    workbook.Save();
                }

                if (!nderrimetBera)
                {
                    // nese nuk ka ndrrime shfaq SweetAlert info
                    return Content("<script>Swal.fire('Info', 'Lista eshte e perditsuar', 'info').then(() => window.location.href = '/');</script>", "text/html");
                }
                return Ok("Lista u validua me sukses");
            }            
            catch (Exception)
            {
                // kthe error nese file nuk gjindet
                return StatusCode(500, new { message = "Deshtim i validimit ju lutem provoni me vone!" });
            }
        }

        public void DownloadTheExcelFile(string path, string fileName)
        {
            string exportUrl = "https://docs.google.com/spreadsheets/d/1QkJ54EMfBbD1JkKlScLazhXBH4WJNAkjBVur-MoKfhc/export?format=xlsx";
            string downloadedFilePath = Path.Combine(path + fileName);

            using (var client = new WebClient())
            {                
                if (FileHelper.FileExists(downloadedFilePath))
                {
                    FileHelper.DeleteFile(downloadedFilePath);
                }

                client.DownloadFile(exportUrl, downloadedFilePath);
            } 
        }

        // Merr shtegun e folderit "Downloads" te perdoruesit
        public static void GetAndSaveDownloadsPath(out string path)
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        }
    }
}
