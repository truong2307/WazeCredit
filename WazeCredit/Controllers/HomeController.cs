using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Data;
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
        private readonly WazeForecastSettings _wazeForecastOptions;
        private readonly ICreditValidator _creditValidator;
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public CreditApplication _creditApplication { get; set; }

        public HomeController(IMarketForecaster marketForecaster
            , IOptions<WazeForecastSettings> wazeForecastOptions
            , ICreditValidator creditValidator
            , ApplicationDbContext db)
        {
            homeVM = new HomeVM();
            _marketForecaster = marketForecaster;
            _wazeForecastOptions = wazeForecastOptions.Value;
            _creditValidator = creditValidator;
            _db = db;
        }

        public IActionResult CreditApplication()
        {
            _creditApplication = new CreditApplication();
            return View(_creditApplication);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPost()
        {
            if (ModelState.IsValid)
            {
                var (validationPassed, errorMessages) = await _creditValidator.PassAllValidator(_creditApplication);

                CreditResult creditResult = new CreditResult
                {
                    ErrorList = errorMessages,
                    CreditID = 0,
                    Success = validationPassed
                };
                if (validationPassed)
                {
                    _db.Add(_creditApplication);
                    _db.SaveChanges();
                    creditResult.CreditID = _creditApplication.Id;

                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
                else
                {
                    return RedirectToAction(nameof(CreditResult), creditResult);
                }
            }
            return View(_creditApplication);
        }

        public IActionResult CreditResult(CreditResult creditResult)
        {
            return View(creditResult);
        }

        /// <summary>
        /// The FromServices Attribute enables injecting a service directly into an action method 
        /// without using constructor injection
        /// </summary>
        /// <param name="sendGridOptions"></param>
        /// <param name="stripeOptions"></param>
        /// <param name="twilloOptions"></param>
        /// <returns></returns>
        public IActionResult AllConfigSettings(
            [FromServices] IOptions<SendGridSettings> sendGridOptions,
            [FromServices] IOptions<StripeSettings> stripeOptions,
            [FromServices] IOptions<TwilloSettings> twilloOptions
            )
        {
            List<string> messages = new List<string>();
            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);
            messages.Add($"Stripe Publishable Key: " + stripeOptions.Value.PublishableKey);
            messages.Add($"Stripe Secret Key: " + stripeOptions.Value.SecretKey);
            messages.Add($"Send Grid Key: " + sendGridOptions.Value.SendGridKey);
            messages.Add($"Twillo Phone: " + twilloOptions.Value.PhoneNumber);
            messages.Add($"Twillo SID: " + twilloOptions.Value.AccountSid);
            messages.Add($"Twillo Token: " + twilloOptions.Value.AuthToken);

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
