using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using TgBotApp.Services.Telegram;

namespace TgBotApp.Controllers
{
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IUpdateService _updateService;

        public MessagesController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPost]
        public IActionResult Update([FromBody]Update update)
        {
            if (update == null)
                return Ok();

            _updateService.Update(update);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("App is starting!");
        }
    }
}
