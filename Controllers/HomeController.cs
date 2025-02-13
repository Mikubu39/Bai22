using BTVN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace BTVN.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _clientFactory;

        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        [HttpPost]
        public async Task<IActionResult> Index(CircleViewModel model)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7250/api/huy/Circle/cv_dt?rr={model.rr}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(json);
                model.DienTich = data.dien_tich;
                model.ChuVi = data.chu_vi;
                model.DuongKinh = data.duong_kinh;
            }

            return View(model);
        }
        [HttpGet("api/huy/Circle/cv_dt")]
        public IActionResult tinh_chuvi_dientich_duongkinh(double rr)
        {
            if (rr <= 0)
                return BadRequest("Bán kính phải dương");
            double cv = 3.14 * 2 * rr;
            double dt = 3.14 * rr * rr;
            double dk = 2 * rr;
            var json_str = new { dien_tich = dt, chu_vi = cv, duong_kinh = dk };
            return Ok(json_str);
        }
        public IActionResult Index()
        {
            return View(new CircleViewModel());
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
    }
}
