using ClosedXML.Excel;
using Event_QrCode_Scanner.DbContext;
using Event_QrCode_Scanner.Models;
using Microsoft.AspNetCore.Mvc;

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
            var existingQrCodeData = _context.QrCodeData.FirstOrDefault(q => q.QrCode_Data == qrData);
            if (existingQrCodeData != null)
            {
                return BadRequest("Ky QR Kod eshte i perdorur.");
            }

            var newQrCodeData = new QrCodeData
            {
                QrCode_Data = qrData,
                Koha_skanimit = DateTime.Now
            };

            _context.QrCodeData.Add(newQrCodeData);
            _context.SaveChanges();

            return Ok("QR Kodi eshte ruar ne databaze.");
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
                string path = "C:\\Users\\PC\\Downloads\\Konferenca Dev Ops 2024 (Responses).xlsx";
                var userIds = _context.QrCodeData.Select(q => q.QrCode_Data).ToList();
                bool nderrimetBera = false; // Kontrollo per ndryshime

                using (var workbook = new XLWorkbook(path))
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
            catch (Exception ex)
            {
                // kthe error per sweetalert2
                return Content($"<script>Swal.fire('Error', 'Kishte problem ne perditsimin e listës', 'error');</script>", "text/html");
            }
        }


        /*
        public async Task<IActionResult> DownloadSpreadsheet()
        {
            var filePath = "path/to/save/spreadsheet.xlsx";
            await _googleSheetsService.DownloadSpreadsheetAsync(filePath);

            // kthen nje pergjigje file
            return PhysicalFile(filePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        */
    }
}
