using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class WeatherServices
{
    private Action<string> _logAction;

    public WeatherServices(Action<string> logAction)
    {
        _logAction = logAction;
    }
    public const string apiKey = "5cabb36f9f1aa4a44f5b499373493b25";
    public const string CurrentWeatherApiUrl = "http://api.openweathermap.org/data/2.5/weather";
    public const string ForecastWeatherApiUrl = "http://api.openweathermap.org/data/2.5/forecast";

    public async Task<CurrentWeather> GetCurrentWeatherAsync(string location)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = $"{CurrentWeatherApiUrl}?q={location}&appid={apiKey}&units=metric";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var weatherData = JsonConvert.DeserializeObject<CurrentWeatherjsonModel>(responseBody);
                return new CurrentWeather
                {
                    Temperature = weatherData.Main.Temp,
                    Description = weatherData.Weather[0].Description,
                    Humidity = weatherData.Main.Humidity,
                    WindSpeed = weatherData.Wind.Speed,
                    Cloud = weatherData.Clouds.All,
                    Icon = weatherData.Weather[0].Icon
                };
            } catch (Exception ex)
            {
                MessageBox.Show("The location you typed does not exist. Check your spelling and type again");
                return null;
            }
        }
    }

    public async Task<List<ForecastItem>> GetForecastAsync(string location)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = $"{ForecastWeatherApiUrl}?q={location}&appid={apiKey}&units=metric";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                //_logAction("Raw API Response:");
                //_logAction(responseBody);

                var forecastData = JsonConvert.DeserializeObject<ForecastJsonModel>(responseBody);


                if (forecastData == null || forecastData.List == null)
                {
                    throw new Exception("Failed to deserialize forecast data");
                }

                List<ForecastItem> forecastItems = new List<ForecastItem>();

                foreach (var item in forecastData.List)
                {
                    if (item == null)
                    {
                        throw new Exception("item is null");

                        continue;
                    }

                    if (string.IsNullOrEmpty(item.DtTxt))
                    {
                        throw new Exception("DtTxt is null or empty for item: {JsonConvert.SerializeObject(item)}");

                        continue;
                    }
                    try
                    {
                        DateTime date = DateTime.ParseExact(item.DtTxt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        string formattedDate = date.ToString("dd/MM/yyyy");

                        forecastItems.Add(new ForecastItem
                        {
                            DateTime = date,
                            FormattedDate = formattedDate,
                            Temperature = $"{item.Main.Temp} °C",
                            Description = item.Weather[0].Description,
                            IconUrl = $"http://openweathermap.org/img/wn/{item.Weather[0].Icon}.png"
                        });
                    }
                    catch (Exception ex)
                    {
                        //_logAction($"Error processing forecast item: {ex.Message}");
                        //_logAction($"Problematic item: {JsonConvert.SerializeObject(item)}");
                    }


                }
                return forecastItems;
            }catch(HttpRequestException ex)
            {
                MessageBox.Show("The location you typed does not exist. Check your spelling and type again");
                return null;
            }
        }
    }



    private DateTime ConvertFromUnixTimestamp(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        return dateTimeOffset.LocalDateTime;
    }
}
