module Parenthesis.Parentheses

open System.Collections.Generic

let parenthesis = Dictionary<char, char>()
parenthesis.Add(')', '(')
parenthesis.Add('}', '{')
parenthesis.Add(']', '[')                      
                     
// checks whether the string contains valid parentheses sequence
let containsValidParenthesis string =
    let listFromGivenString = Seq.toList string
    let rec inner list (stack: char list) =
        match list with
        | [] -> stack.IsEmpty
        | head :: tail ->
            if parenthesis.ContainsValue(head) then inner tail (head :: stack)
            elif parenthesis.ContainsKey head
            then
                match stack with
                | [] -> false
                | h :: t -> if h.Equals(parenthesis[head]) then inner tail t else inner tail stack
            else inner tail stack
    inner listFromGivenString []