# WebCrawler_ForeignExchangeRate
Get foreign  exchange rate from website of bank in Taiwan.

從台灣銀行、第一銀行、合作金庫抓取外幣匯率並儲存。


## Getting Started

### Example
```
BankCrawler crawler = new BankCrawler();

crawler.AddTarget(new TaiwanBank());  //新增台灣銀行
crawler.AddTarget(new FirstBank());  //新增第一銀行
crawler.AddTarget(new CooperativeBank());  //新增合作金庫
crawler.Start();  //開始抓外幣匯率資料
var result = crawler.GetSearchResult(); //回傳資料和command line顯示抓取資料
```

### BankCrawler
類別`BankCrawler`有3個public method。
* `AddTarget()` : 參數是`IBank`，把要查詢的銀行加入到`Crawler`的List
* `Start()` : 開始查詢
* `GetSearchResult()` : 回傳`List<SearchResult>`，內容包含銀行名稱，該銀行外匯頁面的幣別、現金買入、現金賣出、即期買入、即期賣出。

## Built With
* [Html Agility Pack (HAP)](https://github.com/zzzprojects/html-agility-pack) - 抓取網頁資訊


## Author
**TKaiC666** 
第一次實習的最後一份code。


## License
This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/TKaiC666/WebCrawler_ForeignExchangeRate/blob/master/LICENSE) file for details
