using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecasterV2 : IMarketForecaster
    {
        public MarketResult GetMarketPrediction()
        {
            // Call api to do some complex calculations and current stock market forecast
            return new MarketResult
            {
                MarketCondition = MarketCondition.Volatile
            };
        }
    }
}
