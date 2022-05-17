module LazyFSharp.ILazy

type ILazy<'a> =
    abstract member Get: unit -> 'a