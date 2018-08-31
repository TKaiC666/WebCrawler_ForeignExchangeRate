using GetCurrencyRate.Enum;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace GetCurrencyRate.Class
{
    /// <summary>
    /// Get and Store foreign exchange rate data from web.
    /// </summary>
    class BankCrawler
    {
        #region Fields

        private string _targetURL;
        private HtmlWeb _web;
        private List<IBank> _banks;
        private List<SearchResult> _searchResults;

        #endregion

        #region Ctor

        public BankCrawler()
        {
            _web = new HtmlWeb();
            _banks = new List<IBank>();
            _searchResults = new List<SearchResult>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add Bank into crawler list
        /// </summary>
        /// <param name="targetBank">bank you want to crawl</param>
        public void AddTarget(IBank targetBank)
        {
            _banks.Add(targetBank);
        }

        private void searchTaiwanBank(int ResultIndex)
        {
            //把全部tr儲存起來,每個tr包含一種貨幣的匯率資料
            var currencyList = _web.Load(_targetURL).DocumentNode.SelectNodes("//tbody/tr");
            _searchResults.Add(new SearchResult
            {
                BankName = "台灣銀行",
                rateDetails = new List<RateDetail>(),
            });

            for (int i = 0; i < currencyList.Count; i++)
            {
                string currency = currencyList[i].SelectSingleNode("td[@data-table='幣別']/div/*[3]").InnerText.Trim();
                string[] splitCurrencyString = null;
                splitCurrencyString = currency.Split("(");
                currency = splitCurrencyString[0];
                string currencyCode = splitCurrencyString[1].Replace(")", String.Empty);
                string cashBuying = currencyList[i].SelectSingleNode("td[@data-table='本行現金買入']").InnerText;
                string cashSelling= currencyList[i].SelectSingleNode("td[@data-table='本行現金賣出']").InnerText;
                string spotBuying = currencyList[i].SelectSingleNode("td[@data-table='本行即期買入']").InnerText;
                string spotSelling = currencyList[i].SelectSingleNode("td[@data-table='本行即期賣出']").InnerText;
                _searchResults[ResultIndex].rateDetails.Add(new RateDetail
                {
                    Currency = currency,
                    CurrencyCode = currencyCode,
                    CashBuying = cashBuying,
                    CashSelling = cashSelling,
                    SpotBuying = spotBuying,
                    SpotSelling = spotSelling
                });
            }
        }

        private void searchFirstBank(int ResultIndex)
        {
            //一樣的做法，但是第一銀行的現金和即期分開成兩個tr
            //只有現金或即期的貨幣就只會有一個tr。
            var currencyList = _web.Load(_targetURL).DocumentNode.SelectNodes("//table[@id='table1']//tr");
            _searchResults.Add(new SearchResult
            {
                BankName = "第一銀行",
                rateDetails = new List<RateDetail>(),
            });

            //第一銀行的第1個tr是title,略過不做。
            for (int i = 1; i < currencyList.Count; i++)
            {
                string cashBuying = "-";
                string cashSelling = "-";
                string spotBuying = "-";
                string spotSelling = "-";

                //小發現，這裡的編排是即期先，再來才是現金。
                //這是匯率一定會有即期，現金則不一定。
                string currency = currencyList[i].SelectSingleNode("td[1]").InnerText.Trim().Replace("&nbsp;", String.Empty);
                string nextCurrency;
                if (i + 1 == currencyList.Count)
                {
                    nextCurrency = null;
                }
                else
                {
                    nextCurrency = currencyList[i + 1].SelectSingleNode("td[1]").InnerText.Trim().Replace("&nbsp;", String.Empty); //下一個tr的幣種
                }
                
                //如果下一行也是該幣種，代表有現金和即期匯率。
                //沒有的話只有即期匯率。
                if (currency == nextCurrency)
                {
                    spotBuying = currencyList[i].SelectSingleNode("td[3]").InnerText.Trim();
                    spotSelling = currencyList[i].SelectSingleNode("td[4]").InnerText.Trim();
                    cashBuying = currencyList[i+1].SelectSingleNode("td[3]").InnerText.Trim();
                    cashSelling = currencyList[i+1].SelectSingleNode("td[4]").InnerText.Trim();
                    i++;
                }
                else
                {
                    spotBuying = currencyList[i].SelectSingleNode("td[3]").InnerText.Trim();
                    spotSelling = currencyList[i].SelectSingleNode("td[4]").InnerText.Trim();
                }

                string[] splitCurrencyString = null;
                splitCurrencyString = currency.Split("(");
                currency = splitCurrencyString[0];
                string currencyCode;
                if (splitCurrencyString.Length == 1) currencyCode = currency;
                else currencyCode= splitCurrencyString[1].Replace(")", String.Empty);

                _searchResults[ResultIndex].rateDetails.Add(new RateDetail
                {
                    Currency = currency,
                    CurrencyCode = currencyCode,
                    CashBuying = cashBuying,
                    CashSelling = cashSelling,
                    SpotBuying = spotBuying,
                    SpotSelling = spotSelling
                });
            }
        }

        private void searchCooperativeBank(int ResultIndex)
        {
            var currencyList = _web.Load(_targetURL).DocumentNode.SelectNodes("//table[@id='ctl00_PlaceHolderEmptyMain_PlaceHolderMain_fecurrentid_gvResult']/tr");
            _searchResults.Add(new SearchResult
            {
                BankName = "合作金庫銀行",
                rateDetails = new List<RateDetail>(),
            });

            //一樣是一個幣別，分2個tr。不過是用買和賣來區分
            //買入先，賣出後。
            //第1個tr跳過
            for (int i = 1; i < currencyList.Count; i+=2)
            {
                string currency = currencyList[i].SelectSingleNode("td[1]").InnerText.Trim().Replace("&nbsp;", String.Empty);
                string currencyCode = currencyList[i+1].SelectSingleNode("td[1]").InnerText.Trim().Replace("&nbsp;", String.Empty);
                string spotBuying = currencyList[i].SelectSingleNode("td[3]").InnerText.Trim();
                string cashBuying = currencyList[i].SelectSingleNode("td[4]").InnerText.Trim();
                string spotSelling = currencyList[i+1].SelectSingleNode("td[3]").InnerText.Trim();
                string cashSelling = currencyList[i+1].SelectSingleNode("td[4]").InnerText.Trim();

                if (String.IsNullOrEmpty(cashBuying) && String.IsNullOrEmpty(cashSelling))
                {
                    cashBuying = "-";
                    cashSelling = "-";
                }

                _searchResults[ResultIndex].rateDetails.Add(new RateDetail
                {
                    Currency = currency,
                    CurrencyCode = currencyCode,
                    CashBuying = cashBuying,
                    CashSelling = cashSelling,
                    SpotBuying = spotBuying,
                    SpotSelling = spotSelling
                });
            }
        }

        /// <summary>
        /// Start crawling foreign exchange rate data.
        /// </summary>
        public void Start()
        {
            var bankNum = _banks.Count;
            for (int i =0;i< bankNum;i++)
            {
                var bank = _banks[i];
                _targetURL = bank.GetRateUrl();
                switch (bank.GetBankBrand())
                {
                    case ListOfBank.TaiwanBank:
                        searchTaiwanBank(i);
                        break;
                    case ListOfBank.FirstBank:
                        searchFirstBank(i);
                        break;
                    case ListOfBank.CooperativeBank:
                        searchCooperativeBank(i);
                        break;
                }
            }
        }

        public List<SearchResult> GetSearchResult()
        {
            for (int i = 0; i < _searchResults.Count; i++)
            {
                _searchResults[i].Log();
            }
            return _searchResults;
        }

        #endregion
    }
}
