using WebCrawler_ForeignExchangeRate.Enum;

namespace WebCrawler_ForeignExchangeRate.Class
{
    public interface IBank
    {
        /// <summary>
        /// Get URL of currency exchange rate
        /// </summary>
        /// <returns></returns>
        string GetRateUrl();

        /// <summary>
        /// Get Bank's Brand
        /// </summary>
        /// <returns></returns>
        ListOfBank GetBankBrand();
    }

    public class TaiwanBank : IBank
    {
        public string GetRateUrl()
        {
            return "https://rate.bot.com.tw/xrt?Lang=zh-TW";
        }

        public ListOfBank GetBankBrand()
        {
            return ListOfBank.TaiwanBank;
        }
    }

    public class FirstBank : IBank
    {
        public string GetRateUrl()
        {
            return "https://ibank.firstbank.com.tw/NetBank/7/0201.html?sh=none";
        }

        public ListOfBank GetBankBrand()
        {
            return ListOfBank.FirstBank;
        }
    }

    public class CooperativeBank : IBank
    {
        public string GetRateUrl()
        {
            return "https://www.tcb-bank.com.tw/finance_info/Pages/foreign_spot_rate.aspx";
        }

        public ListOfBank GetBankBrand()
        {
            return ListOfBank.CooperativeBank;
        }
    }
}
