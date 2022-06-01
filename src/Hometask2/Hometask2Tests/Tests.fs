module Hometask2Tests

open Expecto
open Hometask2.Tasks

[<Tests>]
let calculateEvenTests =
    testList "Tests for comparing different realizations of function that counts even elements in the list"
        [
            testCase "Function using filter works correctly" <| fun _ ->
                let list = [1; 2; 9; 44; 222; 9; 0; 15; 66]
                Expect.equal (calculateEvenFilter list) 5 "CalculateEvenFilter works wrong"
            
            testProperty "Functions using filter and map act the same way" <| fun (list : List<int>) ->
                Expect.equal (calculateEvenMap list) (calculateEvenFilter list) "Results of filter and map functions should be equal"
                
            testProperty "Functions using filter and fold act the same way" <| fun (list : List<int>) ->
                Expect.equal (calculateEvenMap list) (calculateEvenFilter list) "Results of filter and fold functions should be equal" 
                
            testProperty "Functions using fold and map act the same way" <| fun (list : List<int>) ->
                Expect.equal (calculateEvenMap list) (calculateEvenFilter list) "Results of fold and map functions should be equal"
        ]
        
[<Tests>]
let binaryTreeMapTests =
    testList "Tests for map for binary tree correctness"
        [
            testCase "Map for binary tree doubling" <| fun _ ->
                let treeInitial = BinaryTree.Node(8, None, BinaryTree.Node(6, None, None))
                let treeExpected = BinaryTree.Node(16, None, BinaryTree.Node(12, None, None))
                let treeActual = binaryTreeMap treeInitial (fun x -> x * 2)
                Expect.equal treeExpected treeActual "Map for binary tree works incorrectly" 
            
            testCase "Map for binary tree toUpperCase" <| fun _ ->
                let treeInitial = BinaryTree.Node('a', None, BinaryTree.Node('b', None, None))
                let treeExpected = BinaryTree.Node('A', None, BinaryTree.Node('B', None, None))
                let treeActual = binaryTreeMap treeInitial System.Char.ToUpper
                Expect.equal  treeExpected treeActual "Map for binary tree works incorrectly"
                
            testCase "Map for binary tree adding 5" <| fun _ ->
                let treeInitial = BinaryTree.Node(1, BinaryTree.Node(6, None, None), BinaryTree.Node(8, None, None))
                let treeExpected = BinaryTree.Node(6, BinaryTree.Node(11, None, None), BinaryTree.Node(13, None, None))
                let treeActual = binaryTreeMap treeInitial (fun x -> x + 5)
                Expect.equal treeExpected treeActual "Map for binary tree works incorrectly" 
        ]
  
[<Tests>]
let arithmeticTreeTests =
    testList "Tests for function that calculates arithmetic expression represented as a tree"
        [
           testCase "Sub and mult and div: 25 * 1 - 10 / 2 = 20" <| fun _ ->
               let actual = calculateArithmeticTree (Node(Node(Leaf 25, ArithmeticExpression.Multiplication, Leaf 1), ArithmeticExpression.Subtraction, Node(Leaf 10, ArithmeticExpression.Division, Leaf 2)))
               Expect.equal actual 20 "Result of expression should be 20"
           
           testCase "Sum and mult: 10 + 5 * 2 = 20" <| fun _ ->
               let actual = calculateArithmeticTree (Node(Leaf 10, ArithmeticExpression.Sum, Node(Leaf 5, ArithmeticExpression.Multiplication, Leaf 2)))
               Expect.equal actual 20 "Result of expression should be 20"
        ]
        
[<Tests>]
let primeSequenceTests =
    testList "Tests for functions that generates infinite sequence of prime numbers"
        [
            testCase "First 15 prime numbers" <| fun _ ->
                let actual = seq { for i in 1 .. 10 do Seq.item i generatePrimes}
                let expected = seq { 2; 3; 5; 7; 11; 13; 17; 19; 23; 29 }
                Expect.sequenceEqual expected actual "First 10 prime elements got incorrectly"
        ]
        