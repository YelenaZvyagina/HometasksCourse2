module Hometask2.Tasks

// Functions that calculate the amount of even numbers in the list using map, fold and filter system functions for lists
let calculateEvenMap list = list |> List.map (fun x -> abs (x + 1) % 2) |> List.sum 
let calculateEvenFilter list = list |> List.filter (fun x -> x % 2 = 0) |> List.length
let calculateEvenFold list = List.fold (fun acc elem -> acc + (elem + 1) % 2 ) 0 list

type BinaryTree<'t> =
    | None
    | Node of 't * BinaryTree<'t> * BinaryTree<'t>

let rec binaryTreeMap<'t> (tree : BinaryTree<'t>) (f : 't -> 't) =
    match tree with
    | None -> None
    | Node (value, left, right) -> Node ( f value, binaryTreeMap left f, binaryTreeMap right f)
    
type ArithmeticExpression<'t> =
    | Sum
    | Subtraction
    | Division
    | Multiplication 
    
type ArithmeticTree<'t> =
    | Leaf of 't
    | Node of ArithmeticTree<'t> * ArithmeticExpression<'t> * ArithmeticTree<'t>
      
// Function that calculates the result of arithmetic expression, represented as a tree
let rec calculateArithmeticTree tree =
    match tree with
    | Leaf value -> value
    | Node (operand1, expression, operand2) ->
        let part1 = calculateArithmeticTree operand1
        let part2 = calculateArithmeticTree operand2
        match expression with
        | Sum -> part1 + part2
        | Subtraction -> part1 - part2
        | Division -> part1 / part2
        | Multiplication -> part1 * part2
   
let isPrime number =
    match number with
    | _ -> seq {2 .. int(sqrt(float number))}
    |> Seq.forall (fun x -> (number % x <> 0))
   
// Generates infinite sequence of prime numbers        
let generatePrimes = Seq.initInfinite (fun i -> i + 1) |> Seq.filter isPrime