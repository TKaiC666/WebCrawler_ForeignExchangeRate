using WebCrawler_ForeignExchangeRate.Class;

namespace WebCrawler_ForeignExchangeRate
{
    class Program
    {
        static void Main(string[] args)
        {
            BankCrawler crawler = new BankCrawler();

            crawler.AddTarget(new TaiwanBank());    //台灣銀行最好寫，讚美台灣銀行
            crawler.AddTarget(new FirstBank());
            crawler.AddTarget(new CooperativeBank());
            crawler.Start();
            var result = crawler.GetSearchResult();
        }
    }
}
