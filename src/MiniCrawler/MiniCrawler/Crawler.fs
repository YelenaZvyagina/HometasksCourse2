module MiniCrawler.Crawler

open System.Net.Http
open System.Text.RegularExpressions
open Microsoft.FSharp.Control

let extractLinks html =
    let pattern = "<a href=\"(https://\S+)\">"
    let links =
        [
            for x in Regex(pattern).Matches(html) do
                yield x.Groups.[1].Value
        ]
    links

let fetchAsync (url : string) (client : HttpClient) =
    client.GetStringAsync url |> Async.AwaitTask
    
let getSize (url : string) (client : HttpClient) =
    let page = fetchAsync url client |> Async.RunSynchronously
    page.Length
    
let printLinks links client =
      List.iter (fun x ->  printfn $"%A{x}  %A{getSize x client}") links     
      
let crawl url =
    task {
        let client = new HttpClient()
        let html = fetchAsync url client |> Async.RunSynchronously
        let links = extractLinks html 
        printLinks links client
    }