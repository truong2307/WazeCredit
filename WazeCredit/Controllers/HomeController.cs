﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;
        private readonly StripeSettings _stripeOptions;
        private readonly SendGridSettings _sendGridOptions;
        private readonly TwilloSettings _twilloOptions;
        private readonly WazeForecastSettings _wazeForecastOptions;

        public HomeController(IMarketForecaster marketForecaster
            , IOptions<SendGridSettings> sendGridOptions
            , IOptions<StripeSettings> stripeOptions
            , IOptions<TwilloSettings> twilloOptions
            , IOptions<WazeForecastSettings> wazeForecastOptions
            )
        {
            homeVM = new HomeVM();
            _marketForecaster = marketForecaster;
            _sendGridOptions = sendGridOptions.Value;
            _twilloOptions = twilloOptions.Value;
            _stripeOptions = stripeOptions.Value;
            _wazeForecastOptions = wazeForecastOptions.Value;
        }

        public IActionResult AllConfigSettings()
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe Publishable Key: " + _stripeOptions.PublishableKey);
            messages.Add($"Stripe Secret Key: " + _stripeOptions.SecretKey);
            messages.Add($"Send Grid Key: " + _sendGridOptions.SendGridKey);
            messages.Add($"Twillo Phone: " + _twilloOptions.PhoneNumber);
            messages.Add($"Twillo SID: " + _twilloOptions.AccountSid);
            messages.Add($"Twillo Token: " + _twilloOptions.AuthToken);

            return View(messages);
        }

        public IActionResult Index()
        {
            MarketResult currentMarket = _marketForecaster.GetMarketPrediction();

            switch (currentMarket.MarketCondition)
            {
                    case MarketCondition.StableDown:
                        homeVM.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                        break;
                    case MarketCondition.StableUp:
                        homeVM.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                        break;
                    case MarketCondition.Volatile:
                        homeVM.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                        break;
                    default:
                        homeVM.MarketForecast = "Apply for a credit card using our application!";
                        break;
            }

            return View(homeVM);    
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
