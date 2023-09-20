using Microsoft.AspNetCore.Mvc;
using PDF.Generator.API.Dtos;
using PDF.Generator.API.Helpers;

namespace pdf_generator.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private const string LocalDir = "./../pdfs/";
        [HttpPost]
        public async Task<IActionResult> ConvertPdf(ConverterDto dto){
            // Replacing file's extension to PDF
            var newFileName = dto.FileName.ReplaceFileExtensionWith("pdf");
            // Saving filename to temp directory
            var bytes = Convert.FromBase64String(dto.Content);
            var filePath = LocalDir + dto.FileName;
            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

            var cmd = "./convert.sh " + dto.FileName;
            var output = cmd.Bash();
            var newFilePath = LocalDir + newFileName;
            if (!System.IO.File.Exists(newFilePath)){
                var errorText = "PDF generation failed";
                Console.WriteLine("*** " + errorText);
                Console.WriteLine(output);
                return StatusCode(500, errorText);
            }
            Byte[] outputBytes = System.IO.File.ReadAllBytes(newFilePath);
            String file = Convert.ToBase64String(outputBytes);

            // Cleaning file system
            System.IO.File.Delete(filePath);
            System.IO.File.Delete(newFilePath);
            Console.WriteLine("Conversion completed: " + dto.FileName + " -> " + newFileName);
            return Ok(new {
                filename = newFileName,
                content = file
            });
        }
    }
}