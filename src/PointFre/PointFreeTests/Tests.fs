module PointFreeTests
open Expecto

open PointFree.Functions

[<Tests>]
let TestsForPointFree =
    testList "Tests for point free" [
        testProperty "Comparing first and last steps" <| fun (list : List<int>) (x : int) ->
        Expect.equal (step1 x list) (step5 x list) "Results for first and last steps should be equal"
    ]
   
