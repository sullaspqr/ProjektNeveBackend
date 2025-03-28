using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx;
using System.Net;

namespace ProjektNeveBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FtpCheckFileExistsController : ControllerBase
    {
        [HttpGet("{fileName}")]

        public IActionResult WebCheckFileExists(string fileName)
        {

            string fileUrl = "http://images.balazska.nhely.hu/" + fileName;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.Send(new HttpRequestMessage(HttpMethod.Head, fileUrl));
                    if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return Ok(false);
                    }
                }
                catch
                {
                    return Ok(false);
                }
            }
        }
    }
}

