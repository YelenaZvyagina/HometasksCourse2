module MiniCrawlerTests

open NUnit.Framework
open MiniCrawler.Crawler
open FsUnit

[<Test>]
let extractedLinksTest () =
    let expected = [("https://www.thecatsmeow.com/", Some(44298)); ("https://www.thecatsmeow.com/", Some(44298)); ("https://www.thecatsmeow.com/blog/", Some(86401)); 
                 ("https://www.thecatsmeow.com/sell-with-us", Some(18382)); ("https://www.thecatsmeow.com/about-us", Some(19077)); ("https://www.thecatsmeow.com/contact", Some(25087))]
    let result = crawl "https://www.thecatsmeow.com/" |> Async.RunSynchronously
    result |> should equal expected

[<Test>]
let noLinksTest () =
    let result = crawl "http://info.cern.ch/hypertext/WWW/TheProject.html" |> Async.RunSynchronously
    result |> should equal [] 