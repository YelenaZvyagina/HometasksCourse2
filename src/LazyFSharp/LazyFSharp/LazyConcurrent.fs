module LazyFSharp.LazyConcurrent

open System
open LazyFSharp.ILazy

type LazyConcurrent<'a> (supplier : unit -> 'a) =
    let mutable lazyObject = None
    let object = Object()
    
    interface ILazy<'a> with 
        member this.Get() =
            lock object (fun () ->
                match lazyObject with
                | Some x -> x
                | None -> supplier() 
            ) 
            