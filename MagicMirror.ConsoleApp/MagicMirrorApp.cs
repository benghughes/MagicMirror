using Acme.Generic.Helpers;
using AutoMapper;
using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.ConsoleApp.Models;
using System;
using System.Threading.Tasks;

namespace MagicMirror.ConsoleApp
{
    class MagicMirrorApp
    {
        //private UserInformation _userInformation;

        //Services
        //private readonly MainViewModel _model;
        private readonly IWeatherService _weatherService;
        private readonly ITrafficService _trafficService;
        private readonly IMapper _mapper;

        public MagicMirrorApp(IWeatherService weatherService, ITrafficService trafficService, IMapper mapper)
        {
            _weatherService = weatherService;
            _trafficService = trafficService;
            _mapper = mapper;
            //_model = new MainViewModel();
        }

        private async Task<WeatherModel>GetWeatherModelAsync(string city)
        {
            if(string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentNullException(nameof(city));
            }

            WeatherModel model = await _weatherService.GetWeatherModelAsync(city);
            return model;
        }

        private async Task<TrafficModel>GetTrafficModelAsync(string origin, string destination)
        {
            if (string.IsNullOrWhiteSpace(origin))
            {
                throw new ArgumentNullException(nameof(origin));
            }

            if (string.IsNullOrWhiteSpace(destination))
            {
                throw new ArgumentNullException(nameof(destination));
            }

            TrafficModel model = await _trafficService.GetTrafficModelAsync(origin, destination);
            return model;
        }

        public async Task RunAsync()
        {
            UserInformation information = GetInformation();

            try
            {
                MainViewModel model = await GenerateViewModel(information);
                GenerateOutput(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            finally
            {
                Console.ReadLine();
            }
        }

        private async Task<MainViewModel> GenerateViewModel(UserInformation information)
        {
            var model = new MainViewModel();
            WeatherModel weatherModel;
            TrafficModel trafficModel;

            try
            {
                weatherModel = await GetWeatherModelAsync(information.Town);
                trafficModel = await GetTrafficModelAsync($"{information.Address}, {information.Town}",
                   information.WorkAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred, Displaying offline data");
                Console.WriteLine(ex.ToString());

                weatherModel = GetOfflineWeatherData();
                trafficModel = GetOfflineTrafficData();
            }

            //ToDO: Map models to ViewModel
            model = _mapper.Map(weatherModel, model);
            model = _mapper.Map(trafficModel, model);
            model.UserName = information.Name;

            return model;
        }

        private UserInformation GetInformation()
        {
            Console.WriteLine("Please enter your name:");
            string name = Console.ReadLine();

            Console.WriteLine("Please enter your street and number:");
            string address = Console.ReadLine();

            Console.WriteLine("Please enter your zipcode:");
            string zipcode = Console.ReadLine();

            Console.WriteLine("Please enter your town:");
            string town = Console.ReadLine();

            Console.WriteLine("Please enter your work address:");
            string workAddress = Console.ReadLine();

            var result = new UserInformation
            {
                Name = name,
                Address = address,
                Zipcode = zipcode,
                Town = town,
                WorkAddress = workAddress
            };

            return result;
        }

        private WeatherModel GetOfflineWeatherData()
        {
            return new WeatherModel
            {
                Location = "Fayetteville",
                Sunrise = "7:01",
                Sunset = "7:49",
                Temperature = 65,
                WeatherType = "Sunny",
                TemperatureUom = Business.Enums.TemperatureUom.Fahrenheit
            };
        }

        private TrafficModel GetOfflineTrafficData()
        {
            return new TrafficModel
            {
                Duration = 4,
                Distance = 1,
                DistanceUom = Business.Enums.DistanceUom.Imperial,
                Destination = "University of Arkansas"
            };
        }

        private void GenerateOutput(MainViewModel model)
        {
            Console.WriteLine($"Good {DateTimeHelper.GetTimeOfDay()} {model.UserName}");
            Console.WriteLine($"The current time is {DateTime.Now.ToShortTimeString()} and the outside weather is {model.WeatherType}.");
            Console.WriteLine($"Current topside temperature is {model.Temperature} degrees {model.TemperatureUom}.");
            Console.WriteLine($"The sun has risen at {model.Sunrise} and will set at approximately {model.Sunset}.");
            Console.WriteLine($"Your trip to work is about {model.Distance}{model.DistanceUom} long and will take about {model.TravelTime} minutes. " +
                $"If you leave now, you should arrive at approximately {model.TimeOfArrival }.");
            Console.WriteLine("Thank you, and have a very safe and productive day!");
        }

       
    }
}
