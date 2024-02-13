using almondcove.Interefaces.Repositories;
using almondcove.Models.Domain;
using almondcove.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class MailingListController(ILogger<MailingListController> logger, IMailingListRepository mailRepo) : ControllerBase
    {
        private readonly ILogger<MailingListController> _logger = logger;
        private readonly IMailingListRepository _mailRepo = mailRepo;

        [EnableRateLimiting(policyName:"fixed")]
        [HttpPost("/api/mailinglist/subscribe")]
        
        public async Task<IActionResult> PostMail(MailDTO mailDTO)
        {
            try
            {
                var mail = MapToMailEntity(mailDTO);
                var (Success, Message) = await _mailRepo.PostMail(mail);

                _logger.LogInformation("email addition result {result} and message:{message}", Success, Message);

                return Success ? Ok("Mail submitted") : BadRequest(Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while processing email submission: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        private static Mail MapToMailEntity(MailDTO mailDTO)
        {
            return new Mail
            {
                Email = mailDTO.EMail,
                Origin = mailDTO.Origin,
                DateAdded = DateTime.Now,
            };
        }
    }
}
