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
    client.GetStringAsync url |> Async.AwaitTask |> Async.Catch
    
let getSizes pages =
    pages
    |> Seq.map (fun page ->
        match page with
        | Choice1Of2 (x : string) -> Some x.Length
        | Choice2Of2 (_ : exn) -> None)
      
let crawl url =
    async {
        let client = new HttpClient()
        let! page = fetchAsync url client
        let html =
            match page with
            | Choice1Of2 result -> Some result
            | Choice2Of2 (_ : exn) -> None
        match html with
        | Some value ->
            let links = extractLinks value
            let! pages =
                links
                |> Seq.map (fun link -> fetchAsync link client)
                |> Async.Parallel
            return getSizes pages |> Seq.zip links
        | None -> return Seq.empty
    }