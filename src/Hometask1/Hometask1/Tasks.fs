module Hometask1.Tasks

open System

let factorial number =
    if number < 0 then failwith "Attempt to calculate the factorial of negative number"
    let rec factorialInner number acc =
        if number = 0 then acc else factorialInner (number - 1) (acc * number)
    factorialInner number 1
    
let fibonacci n =
    let rec fibonacciInner n acc1 acc2 =
        if n = 0 then acc1
        elif n < 0 then failwith "It's unreal to find Fibonacci number if N is below zero"
        else fibonacciInner (n - 1) acc2 (acc1 + acc2)
    fibonacciInner n 0 1
    
let reverseList list =
    let rec reverseInner list newList =  
        match list with
        | [] -> newList
        | h :: t -> reverseInner t (h :: newList)
    reverseInner list []
    
let findInList number list =
    let rec findInner number list acc =
        match list with
        | [] -> failwith "Number cannot be found in empty list"
        | h :: t -> if h = number then acc else findInner number t (acc+1)
    findInner number list 0

// returns list [2^n ... 2^(n+m)]
let listOfPowers n m =
    let accStart = Math.Pow (2.0, float n)
    let rec powersInner m list acc =
        match m with
        | 0 -> reverseList list
        | _ -> powersInner (m-1) ( acc*2.0 :: list ) (acc*2.0)
    powersInner (m-n) [accStart] accStart