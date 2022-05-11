module MiniCrawlerTests

open System.Net.Http
open NUnit.Framework
open MiniCrawler.Crawler

    
[<Test>]
let extractedLinksTest () =
     let client = new HttpClient()
     let actual = fetchAsync "https://www.thecatsmeow.com/" client |> Async.RunSynchronously |> extractLinks 
     let expected = ["https://www.thecatsmeow.com/"; "https://www.thecatsmeow.com/"; "https://www.thecatsmeow.com/blog/"; 
                     "https://www.thecatsmeow.com/sell-with-us"; "https://www.thecatsmeow.com/about-us"; "https://www.thecatsmeow.com/contact"]
     Assert.AreEqual (expected, actual)
     let size = getSize actual.Head client
     Assert.AreEqual (size, 44298)
    

[<Test>]
let noLinksTest () =
    let client = new HttpClient()
    let actual = fetchAsync "http://info.cern.ch/hypertext/WWW/TheProject.html" client |> Async.RunSynchronously |> extractLinks  
    Assert.AreEqual (0, actual.Length)