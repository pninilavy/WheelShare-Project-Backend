using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Threading.Tasks;

namespace WheelShare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // GET: api/email
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/email/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/email
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmailRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ToEmail))
            {
                return BadRequest("Invalid email request.");
            }

            bool isSent = await _emailService.SendEmailAsync(
                request.ToEmail,
                request.Subject,
                request.PlainTextContent,
                request.HtmlContent
            );

            if (isSent)
            {
                return Ok("Email sent successfully.");
            }
            else
            {
                return StatusCode(500, "Failed to send email.");
            }
        }

        // PUT api/email/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/email/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    // מחלקת מודל לבקשת המייל
    public class EmailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
