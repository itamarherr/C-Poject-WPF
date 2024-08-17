using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WeatherApp.Enum;
using WeatherApp.Models;
using WeatherApp.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
using Common;

namespace WeatherApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly WeatherServices _weatherServices;
    public ObservableCollection<ForecastItem> ForecastItems { get; } = new ObservableCollection<ForecastItem>();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _weatherServices = new WeatherServices(LogMessage);
    }
    private async void SearchWeatherButton_Click(object sender, RoutedEventArgs e)
    {
        string location = txtLocation.Text.Trim();
        if (string.IsNullOrEmpty(location))
        {
            MessageBox.Show("Please enter a location.");
            return;
        }
        await DisplayWeather(location);
      
        currentWeatherPanel.Visibility = Visibility.Visible;
        currentWeatherPanelImage.Visibility = Visibility.Visible;
        forecastPanel.Visibility = Visibility.Visible;
    }


    private async Task DisplayWeather(string location)
    {
        CurrentWeather currentWeather = await _weatherServices.GetCurrentWeatherAsync(location);
        DisplayCurrentWeatherInfo(currentWeather); 

        
        var forecast = await _weatherServices.GetForecastAsync(location);
        DisplayForecastInfo(forecast);  
    }

 
    private void ScaleDownImageForHighDensity(BitmapImage bitmapImage)
    {

        double targetWidth = bitmapImage.PixelWidth / 2;
        double targetHeight = bitmapImage.PixelHeight / 2;


        bitmapImage.DecodePixelWidth = (int)targetWidth;
        bitmapImage.DecodePixelHeight = (int)targetHeight;
    }



    private void DisplayCurrentWeatherInfo(CurrentWeather weatherData)
    {
        if (weatherData == null)
        {
            MessageBox.Show("Could not retrieve weather data.");
            return;
        }


        txtTemperature.Text = $"Temperature: {weatherData.Temperature} °C";
        txtDescription.Text = $"Description: {weatherData.Description}";
        txtHumidity.Text = $"Humidity: {weatherData.Humidity}%";
        txtWindSpeed.Text = $"Wind Speed: {weatherData.WindSpeed} m/s";
        txtCloud.Text = $"Cloud: {weatherData.Cloud}%";

        string iconUrl = $"http://openweathermap.org/img/wn/{weatherData.Icon}@2x.png";
        BitmapImage bitmap = new BitmapImage(new Uri(iconUrl, UriKind.Absolute));
        weatherIcon.Source = bitmap;
    }
    private void DisplayForecastInfo(List<ForecastItem> forecast)
    {
        ForecastItems.Clear();
        var filteredForecast = forecast
        .GroupBy(item => DateTime.ParseExact(item.FormattedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
        .Select(group => group.First())
        .ToList();
        foreach (var item in filteredForecast)
        {
            ForecastItems.Add(item); 
        }
    }
    //private static List<ForecastItem> GetOneItemPerDay(this List<ForecastItem> forecast)
    //{
    //    return forecast
    //        .GroupBy(item => item.FormattedDate) 
    //        .Select(group => group.First())
    //        .ToList();
    //}



    private void ClearText(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.Clear();
        }
    }
    private void LogMessage(string message)
    {
        Dispatcher.Invoke(() =>
        {
            debugTextBlock.Text += message + "\n";
        });
    }

}

