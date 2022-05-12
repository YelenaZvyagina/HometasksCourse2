module FsLazyTests

open LazyFSharp
open NUnit.Framework
open System.Threading
open FsUnit

[<Test>]
let singleThreadedLazyTest () =
    let mutable count = 0
    let myLazy = LazyFactory.LazyFactory.CreateSingleThreadedLazy (fun () -> Interlocked.Increment(ref count))
    myLazy.Get() |> ignore
    myLazy.Get() |> ignore
    myLazy.Get() |> should equal 1

[<Test>]
let concurrentLazyTest () =
    let mutable count = 0
    let myLazy = LazyFactory.LazyFactory.CreateConcurrentLazy (fun () -> Interlocked.Increment(ref count))
    myLazy.Get() |> ignore
    myLazy.Get() |> ignore
    let result () = myLazy.Get() |> should equal 1 
    let threads = Array.init 10000 (fun _ -> Thread(result))
    for thread in threads do
        thread.Start()
    for thread in threads do
        thread.Join()

[<Test>]
let lockFreeLazyTest () =
    let mutable count = 0
    let myLazy = LazyFactory.LazyFactory.CreateLockFreeLazy (fun () -> Interlocked.Increment(ref count))
    myLazy.Get() |> ignore
    myLazy.Get() |> ignore
    let result () = myLazy.Get() |> should equal 1 
    let threads = Array.init 10000 (fun _ -> Thread(result))
    for thread in threads do
        thread.Start()
    for thread in threads do
        thread.Join()
        