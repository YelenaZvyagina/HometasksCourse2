module WorkflowsTests

open NUnit.Framework
open Workflows

[<Test>]
let testForRounding () =
     let rounding = ComputingWithAccuracyBuilder(3)
     let result = 
         rounding {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b
         }
     Assert.AreEqual(result, 0.048)

[<Test>]
let testForCorrectStrings () =
    let calculate = StringComputingBuilder()   
    let result = calculate {
        let! x = "1"
        let! y = "2"
        let z = x + y
        return z
    }
    Assert.AreEqual(result, Some(3))


[<Test>]
let testForIncorrectStrings () =
    let calculate = StringComputingBuilder()   
    let res = calculate {
        let! x = "1"
        let! y = "Ъ"
        let z = x + y
        return z
    }
    Assert.AreEqual(res, None)