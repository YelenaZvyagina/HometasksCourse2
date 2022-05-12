module LazyFSharp.Lazy

open LazyFSharp.ILazy

type Lazy<'a>(supplier : unit -> 'a) =
    let mutable lazyObject = None
    interface ILazy<'a> with 
        member this.Get() =
            match lazyObject with
            | Some x -> x
            | None ->
                lazyObject <- Some (supplier())
                supplier()
            
