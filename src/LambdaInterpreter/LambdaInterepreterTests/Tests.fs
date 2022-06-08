module LambdaInterepreterTests

open NUnit.Framework
open LambdaInterpreter
open FsUnit

[<Test>]
let test1() =
    let term = Application(Variable "a", Variable "a")
    betaReduction term |> should equal term

[<Test>]
let test2() =
    let term = Application(LambdaAbstraction ("a", Variable "a"), LambdaAbstraction("b", Variable "b"))
    betaReduction term |> should equal (LambdaAbstraction("b", Variable "b")) 

[<Test>]
let test3() =
    let term = Application(Variable "aa", Variable "b")
    substitute "b" term (Variable "aaa") |> should equal (Application (Variable "aa", Variable "aaa"))

[<Test>]    
let test4() =
    let term = LambdaAbstraction("a", Variable "b")
    substitute "b" term (Variable "a") |> should equal (LambdaAbstraction("b_new", Variable "a"))
