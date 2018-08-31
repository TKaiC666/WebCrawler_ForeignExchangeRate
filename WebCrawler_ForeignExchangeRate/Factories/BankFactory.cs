using WebCrawler_ForeignExchangeRate.Class;
using WebCrawler_ForeignExchangeRate.Enum;

namespace WebCrawler_ForeignExchangeRate.Factories
{
    static class BankFactory
    {
       static public void SetBank(ListOfBank bankBrand,IBank bank)
       {
            switch (bankBrand)
            {
                case ListOfBank.TaiwanBank:
                    bank = new TaiwanBank();
                    break;
                case ListOfBank.FirstBank:
                    bank = new FirstBank();
                    break;
                case ListOfBank.CooperativeBank:
                    bank =  new CooperativeBank();
                    break;
            }
       }
    }
}
