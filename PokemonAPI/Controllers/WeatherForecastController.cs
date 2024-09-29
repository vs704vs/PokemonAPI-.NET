using Microsoft.AspNetCore.Mvc;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}












namespace GPTool.File_API.Controllers
{
    [ApiController]
    [Route("api/docs")]
    public class DocsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocsController(IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _config = config;
            _webHostEnvironment = webHostEnvironment;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("FetchFiles")]
        public ActionResult<IEnumerable<FileDetails>> FetchFiles([FromQuery] string folderPath)
        {
            try
            {
                // Check if folder exists
                if (!Directory.Exists(folderPath))
                {
                    return BadRequest("Folder path does not exist.");
                }

                // Fetch files and directories from the specified folder
                var files = Directory.GetFiles(folderPath)
                    .Select(file => new FileDetails
                    {
                        FileName = Path.GetFileName(file),
                        FileType = Path.GetExtension(file),
                        Size = new FileInfo(file).Length,
                        DateModified = System.IO.File.GetLastWriteTime(file)
                    });

                var folders = Directory.GetDirectories(folderPath)
                    .Select(folder => new FolderDetails
                    {
                        FolderName = Path.GetFileName(folder),
                        DateModified = Directory.GetLastWriteTime(folder)
                    });

                // Combine files and folder details
                var folderContents = new FolderContents
                {
                    Files = files.ToList(),
                    Folders = folders.ToList()
                };

                return Ok(folderContents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromQuery] string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest("File does not exist.");
                }

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                var contentType = "application/octet-stream";
                return File(memory, contentType, Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }

    // Models for response
    public class FileDetails
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; } // Size in bytes
        public DateTime DateModified { get; set; }
    }

    public class FolderDetails
    {
        public string FolderName { get; set; }
        public DateTime DateModified { get; set; }
    }

    public class FolderContents
    {
        public List<FileDetails> Files { get; set; }
        public List<FolderDetails> Folders { get; set; }
    }
}
