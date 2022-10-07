using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TestPV.Models;

namespace TestPV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeathersController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetInfoWeather()
        {
            InfoWeather.Root weatherList = new InfoWeather.Root();
            using (var http = new HttpClient())
            {
                using (var respone = await http.GetAsync("http://api.openweathermap.org/data/2.5/group?id=1580578,1581129,1581297,1581188,1587923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a"))
                {
                    string apiRes = await respone.Content.ReadAsStringAsync();
                    weatherList = JsonConvert.DeserializeObject<InfoWeather.Root>(apiRes);
                }
            }
            var dataWeathers = new List<DataWeather>();
            foreach(var item in weatherList.list)
            {
                dataWeathers.Add(new DataWeather
                {
                    cityId = item.id,
                    cityName = item.name,
                    weatherMain = item.weather[0].main,
                    weatherDescription = item.weather[0].description,
                    weatherIcon = " http://openweathermap.org/img/wn/" + item.weather[0].icon + "@2x.png",
                    mainTemp = item.main.temp,
                    mainHumidity = item.main.humidity
                });
            }
            return Ok(new
            {
                data= dataWeathers,
                message= "Current weather information of cities",
                statusCode=200
            });
        }
    }
}
