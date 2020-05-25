using Microsoft.AspNetCore.Mvc;
using signalr.server.Services;
using System;
using System.Threading.Tasks;

namespace signalr.server.Controllers
{
    [Route("api/v1/performance-testing")]
    public class PerformanceController : ControllerBase
    {
        private readonly ICounterService _counterService;
        private readonly IMessageService _messageService;
        public PerformanceController(ICounterService counterService,
                                        IMessageService messageService)
        {
            _counterService = counterService;
            _messageService = messageService;
        }

        //start
        [Route("start")]
        [HttpPost]
        public async Task<IActionResult> StartAsync()
        {
            Console.WriteLine("Start testing server performances...");
            _counterService.Reset();

            for (int i = 0; i < 25; i++)
            {
                Console.WriteLine($"Sending messages of phase {i + 1}...");
                await _messageService.SendAllAsync(i + 1);
                await Task.Delay(200);
            }

            return Ok();
        }

        //result
        [Route("result")]
        [HttpGet]
        public IActionResult ResultAsync()
        {
            Console.WriteLine("Get testing result");            
            return Ok(new
            {
                NumConnections = _counterService.ConnectionIds.Count,
                NumMsgReceived = _counterService.MessagesReplied.Count
            });
        }
    }
}
