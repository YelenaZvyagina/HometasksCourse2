namespace Workflows

open System

// Workflow that computes arithmetical with numbers written as strings
type StringComputingBuilder() =
    member this.Bind(x : string, f) =
        try
            int x |> f
        with
        | :? FormatException -> None 
    member this.Return(x) = Some x
    
// Workflow that computes mathematical expressions with given accuracy     
type ComputingWithAccuracyBuilder(accuracy : int) =
    member this.Bind(x, f) = f x
    member this.Return(x : float) = Math.Round(x, accuracy)
