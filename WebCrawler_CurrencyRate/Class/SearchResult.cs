using System;
using System.Collections.Generic;
using System.Text;

namespace GetCurrencyRate.Class
{
    class SearchResult
    {
        public string BankName { get; set; }

        public List<RateDetail> rateDetails;

        public void Log()
        {
            Console.WriteLine("截至"+DateTime.Now);
            Console.WriteLine(BankName+" 匯率:\n--------------------");
            for (int i = 0; i < rateDetails.Count; i++)
            {
                var rateDetail = rateDetails[i];
                Console.WriteLine("貨幣:\t" + rateDetail.Currency+"("+rateDetail.CurrencyCode+")");
                Console.WriteLine("現金買入:\t"+ rateDetail.CashBuying);
                Console.WriteLine("現金賣出:\t" + rateDetail.CashSelling);
                Console.WriteLine("即期買入:\t" + rateDetail.SpotBuying);
                Console.WriteLine("即期賣出:\t" + rateDetail.SpotSelling);
                Console.WriteLine("\n");
            }
            Console.WriteLine("--------------------\n");
        }
    }

    class RateDetail
    {

        public string Currency { get; set; }

        public string CurrencyCode { get; set; }

        public string CashBuying { get; set; }

        public string CashSelling { get; set; }

        public string SpotBuying { get; set; }

        public string SpotSelling { get; set; }

    }
}
