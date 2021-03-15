using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyMusicMVC.Models;
using MyMusicMVC.ViewModels;
using Newtonsoft.Json;

namespace MyMusicMVC.Controllers
{
    public class ComposerController : Controller
    {
        private readonly IConfiguration _Config;
        private string URLBase
        {
            get
            {
                return _Config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }
        public ComposerController(IConfiguration Config)
        {
            _Config = Config;
        }
        public async  Task<IActionResult> Index()
        {
            var composerViewModel = new ListComposerViewModel();
            List<Composer> composerList = new List<Composer>();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "Composer"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    composerList = JsonConvert.DeserializeObject<List<Composer>>(apiResponse);
                }
            }
            composerViewModel.ListComposer = composerList;
            return View(composerViewModel);
        }
    }
}