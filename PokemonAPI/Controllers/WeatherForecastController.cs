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
        [HttpPost("CreateFolder")]
        public ActionResult CreateFolder([FromQuery] string folderPath, [FromQuery] string folderName)
        {
            try
            {
                // Check if the folder path exists
                if (!Directory.Exists(folderPath))
                {
                    return BadRequest("The specified folder path does not exist.");
                }

                // Construct the full path for the new folder
                string newFolderPath = Path.Combine(folderPath, folderName);

                // Check if the folder already exists
                if (Directory.Exists(newFolderPath))
                {
                    return BadRequest("A folder with the same name already exists.");
                }

                // Create the new folder
                Directory.CreateDirectory(newFolderPath);

                return Ok($"Folder '{folderName}' created successfully at '{folderPath}'.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // FetchFiles and DownloadFile endpoints are unchanged...
    }

    // Models for response...
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
        [HttpDelete("DeleteFile")]
        public ActionResult DeleteFile([FromQuery] string filePath)
        {
            try
            {
                // Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return BadRequest("The specified file does not exist.");
                }

                // Delete the file
                System.IO.File.Delete(filePath);

                return Ok($"File '{filePath}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // Other endpoints like FetchFiles, DownloadFile, and CreateFolder remain unchanged...
    }

    // Models for response...
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
