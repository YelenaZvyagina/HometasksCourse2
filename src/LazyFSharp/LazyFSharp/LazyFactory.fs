module LazyFSharp.LazyFactory

open LazyFSharp.ILazy
open LazyFSharp.LazyConcurrent
open LazyFSharp.LazyLockFree

type LazyFactory =
    static member CreateSingleThreadedLazy (supplier : unit -> 'a) =
        new LazyFSharp.Lazy.Lazy<'a> (supplier) :> ILazy<'a>
        
    static member CreateConcurrentLazy (supplier : unit -> 'a) =
        new LazyConcurrent<'a> (supplier) :> ILazy<'a>
        
    static member CreateLockFreeLazy (supplier : unit -> 'a) =
        new LazyLockFree<'a> (supplier) :> ILazy<'a>