module LazyFSharp.LazyLockFree

open System.Threading
open LazyFSharp.ILazy

type LazyLockFree<'a> (supplier : unit -> 'a) =
    let mutable lazyObject = None
    interface ILazy<'a> with 
        member this.Get() =
            let computed = Some( supplier() )
            Interlocked.CompareExchange(&lazyObject, computed, None) |> ignore
            lazyObject.Value
         