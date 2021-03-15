using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyMusicMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Config;

        public HomeController(ILogger<HomeController> logger, IConfiguration Config)
        {
            _logger = logger;
            _Config = Config;
        }

        public async Task<IActionResult> IndexAsync()
        {
            ListMusicViewModel listMusicViewModel = new ListMusicViewModel();
            List<Music> listMusic = new List<Music>();
            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "Music"))
                {
                    //Lire la réponse
                    string apiResponse = await respense.Content.ReadAsStringAsync();
                    //Déserialiser la réponse du requete: caster la réponse avec liste<Music>
                    listMusic = JsonConvert.DeserializeObject<List<Music>>(apiResponse);
                }
            }
            listMusicViewModel.ListMusic = listMusic;
            return View(listMusicViewModel);
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

        private string URLBase
        {
            get
            {
                return _Config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }
    }
}
