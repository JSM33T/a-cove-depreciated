using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using almondcove.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace almondcove.Controllers.Api
{
    [ApiController]
    public class MailingListController(
        ILogger<MailingListController> logger,
        IMailingListRepository mailRepo
    ) : ControllerBase
    {
        private readonly ILogger<MailingListController> _logger = logger;
        private readonly IMailingListRepository _mailRepo = mailRepo;

        [HttpPost("/api/mailinglist/subscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostMail(MailDTO mailDTO)
        {
            try
            {
                var mail = MapToMailEntity(mailDTO);
                var (Success, Message) = await _mailRepo.PostMail(mail);

                return Success ? Ok(Message) : BadRequest(Message);
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
