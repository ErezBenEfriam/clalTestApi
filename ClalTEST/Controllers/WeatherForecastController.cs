using ClalTEST.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClalTEST.Controllers
{
    [ApiController]
    [Route("WeatherClal")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly HttpClient client;


        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            client = new HttpClient();
            _logger = logger;
        }

        [HttpGet]
        [Route("clalWeather")]
        public async Task<IActionResult> clalWeather(string cityName)
        {
            //אין באמת סיבה להעברת עיר מכיוון שיש רק עיר אחת
            string url = @"https://api.weatherapi.com/v1/forecast.json?key=39f8ecaf506c4f76b3f55139222906&q=london&days=3&aqi=yes&alerts=yes";
            var response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<Root>(responseBody);
            var londonWeather = articles.current.temp_c;
            var conditiontext = articles.current.condition.text;
            return Ok($"the wather in london is: {londonWeather}  {conditiontext}");
        }
        [HttpGet]
        [Route("WeatherForThreeDays")]
        public async Task<IActionResult> WeatherForThreeDays()
        {
            string url = @"https://api.weatherapi.com/v1/forecast.json?key=39f8ecaf506c4f76b3f55139222906&q=london&days=3&aqi=yes&alerts=yes";
            var response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var articles = JsonConvert.DeserializeObject<Root>(responseBody);
            var Forecastdays = new List<Forecastday>();
            for (int i = 0; i < 3; i++)
            {
                Forecastdays.Add(articles.forecast.forecastday[i]);
            }
            var forecast = new Forecast();
            forecast.forecastday = Forecastdays;
            return Ok(forecast);
        }
        [HttpGet]
        [Route("RandomNumber")]
        public int RandomNumber(int minNumber, int MaxNumber)
        { 
            var ramdomNumber = DateTime.Now.Millisecond;
            if (minNumber < ramdomNumber && ramdomNumber < MaxNumber)
                return ramdomNumber;
            while(!(minNumber < ramdomNumber && ramdomNumber < MaxNumber))
            {
                if (minNumber >= ramdomNumber)
                {
                    ramdomNumber= increaseNumber(minNumber, ramdomNumber);
                }
                if (MaxNumber <= ramdomNumber)
                {
                    ramdomNumber= reduceNumber(MaxNumber, ramdomNumber);
                }
            }
            return ramdomNumber;
        }

        private int increaseNumber(int minNumber, int RandomNumber)
        {
            while (minNumber >= RandomNumber)
            {
                var randomtime = DateTime.Now.Millisecond;
                var str = randomtime.ToString()[0];
                int RandomAddition = int.Parse(str.ToString());
                RandomNumber += RandomAddition;
            }
            return RandomNumber;
        }
        private int reduceNumber(int maxNumBer, int RandomNumber)
        { 
            while (maxNumBer <= RandomNumber)
            {
                int RandomAddition = GetSmallRandomNumber();
                RandomNumber -= RandomAddition;

            }
            return RandomNumber;
        }
        private int GetSmallRandomNumber()
        {
            var randomtime = DateTime.Now.Millisecond;
            var str = randomtime.ToString()[0];
            int RandomAddition = int.Parse(str.ToString());
            return RandomAddition;
        }
    }
}