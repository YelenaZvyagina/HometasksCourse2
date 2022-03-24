module ParenthesisTests

open NUnit.Framework
open Parenthesis.Parentheses

// Tests for containsValidParenthesis function
[<Test>]
let CorrectStringTest1 () =
    let string = "(((__)))"
    Assert.IsTrue(containsValidParenthesis string) 
    
[<Test>]
let CorrectStringTest2 () =
    let string = "[dd{dd(d)}]"
    Assert.IsTrue(containsValidParenthesis string)
    
[<Test>]
let CorrectStringTest3 () =
    let string = "(ad { dfdf} fd)"
    Assert.IsTrue(containsValidParenthesis string)
    
[<Test>]
let CorrectStringTest4 () =
    let string = "asdfgh"
    Assert.IsTrue(containsValidParenthesis string)
    
[<Test>]
let WrongStringTest1 () =
    let string = "[{]}"
    Assert.IsFalse(containsValidParenthesis string)
    
[<Test>]
let WrongStringTest2 () =
    let string = "aa((((aaa)"
    Assert.IsFalse(containsValidParenthesis string)
    
[<Test>]
let WrongStringTest3 () =
    let string = "){}[]())ss)"
    Assert.IsFalse(containsValidParenthesis string)
    
[<Test>]
let WrongStringTest4 () =
    let string = "{((k}"
    Assert.IsFalse(containsValidParenthesis string)  