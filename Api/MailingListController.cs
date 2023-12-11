using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Models.Domain;
using Almondcove.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace almondcove.Api
{
    [ApiController]
    public class MailingListController : ControllerBase
    {
        private readonly ILogger<MailingListController> _logger;
        private readonly IMailingListRepository _mailRepo;
        public MailingListController(ILogger<MailingListController> logger,IMailingListRepository mailRepo)
        {
            _logger = logger;
            _mailRepo = mailRepo;
        }

        [HttpPost("/api/mailinglist/subscribe")]
        [IgnoreAntiforgeryToken]
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
