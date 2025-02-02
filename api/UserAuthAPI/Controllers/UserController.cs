using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using UserAuthAPI.Data;
using UserAuthAPI.Models;

using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;

namespace UserAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        // Login endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username && u.UserPassword == user.UserPassword);

            if (existingUser == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful.");
        }

        // Endpoint to generate and return a PDF with stamped ID Card details
        [HttpPost("idCardDetails")]
        public IActionResult GenerateIdCardPdf([FromBody] IdCardDetailsRequest idCardDetails)
        {
            if (idCardDetails == null)
                return BadRequest("Invalid ID Card details provided.");


            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "assets", "student-id.pdf");
            if (!System.IO.File.Exists(templatePath))
            {
                return NotFound("Template PDF not found.");
            }

            byte[] pdfBytes;
            using (var templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            using (var outputStream = new MemoryStream())
            {

                var reader = new PdfReader(templateStream);
                var writer = new PdfWriter(outputStream);
                using (var pdfDoc = new PdfDocument(reader, writer))
                {

                    var page = pdfDoc.GetFirstPage();
                    var canvas = new PdfCanvas(page);

                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    canvas.BeginText();
                    canvas.SetFontAndSize(font, 6); 
                    canvas.SetFillColor(ColorConstants.BLACK);


                    canvas.MoveText(80, 69);

                    canvas.ShowText(idCardDetails.AdmissionNo);
                    canvas.MoveText(0, -9); 
                    canvas.ShowText(idCardDetails.ParentName);
                    canvas.MoveText(0, -9); 
                    canvas.ShowText(idCardDetails.PhoneNo);
                    canvas.EndText();
                }
                pdfBytes = outputStream.ToArray();
            }

            return File(pdfBytes, "application/pdf", "StudentID.pdf");
        }
    }
}
