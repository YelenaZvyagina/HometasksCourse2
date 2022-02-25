module Hometask1.Tasks

let factorial number =
    if number < 0 then failwith "Attempt to calculate the factorial of negative number"
    let rec _inner number acc =
        if number = 0 then acc else _inner (number - 1) (acc * number)
    _inner number 1
    
let fibonacci n =
    let rec _inner n acc1 acc2 =
        if n = 0 then acc1
        elif n < 0 then failwith "It's unreal to find Fibonacci number if N is below zero"
        else _inner (n - 1) acc2 (acc1 + acc2)
    _inner n 0 1
    
let reverseList list =
    let rec _inner list newList =  
        match list with
        | [] -> []
        | [x] -> [x] @ newList
        | h :: t -> _inner t ([h] @ newList)
    _inner list []
    
let findInList number list =
    let rec _inner number list acc =
        match list with
        | [x] -> if x = number then acc else failwith "No number found sorry"
        | h :: t -> if h = number then acc else _inner number t (acc+1)
    _inner number list 0

let rec twoPowN n =
    if n = 1 then 2
    else
        match n%2 with
        | 0 -> twoPowN (n/2) * twoPowN (n/2)
        | 1 -> twoPowN (n/2) * twoPowN (n/2) * 2

// returns list [2^n ... 2^(n+m)]
let listOfPowers n m =
    let accStart = twoPowN n
    let rec _inner m list acc =
        match m with
        | 0 -> list
        | _ -> _inner (m-1) (list @ [acc*2]) (acc*2)
    _inner (m-n) [accStart] accStart