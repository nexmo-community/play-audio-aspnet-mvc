using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlayAudioMvc.Models;
using Microsoft.Extensions.Configuration;
using Nexmo.Api.Voice.Nccos.Endpoints;
using Nexmo.Api.Voice.Nccos;
using Nexmo.Api.Voice;
using Nexmo.Api.Request;

namespace PlayAudioMvc.Controllers
{
    public class HomeController : Controller
    {
        const string STREAM_URL = "https://nexmo-community.github.io/ncco-examples/assets/voice_api_audio_streaming.mp3";
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpGet("/webhooks/answer")]
        public string Answer()
        {            
            var streamAction = new StreamAction{
                StreamUrl = new string[]
                { 
                    STREAM_URL
                }
            };
            var ncco = new Ncco(streamAction);
            return ncco.ToString();
        }

        [HttpPost]
        public IActionResult MakePhoneCall(string toNumber, string fromNumber)
        {
            var appId = _config["APPLICATION_ID"];
            var privateKeyPath = _config["PRIVATE_KEY_PATH"];
            
            var streamAction = new StreamAction{ StreamUrl = new string[] { STREAM_URL }};
            var ncco = new Ncco(streamAction);

            var toEndpoint = new PhoneEndpoint{Number=toNumber};
            var fromEndpoint = new PhoneEndpoint{Number=fromNumber};

            var credentials = Credentials.FromAppIdAndPrivateKeyPath(appId, privateKeyPath);
            var client = new VoiceClient(credentials);
            var callRequest = new CallCommand { To = new []{toEndpoint}, From = fromEndpoint, Ncco= ncco};
            var call = client.CreateCall(callRequest);
            ViewBag.Uuid = call.Uuid;
            return View("Index");
        }
    }
}
