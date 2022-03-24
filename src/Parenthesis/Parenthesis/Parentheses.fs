module Parenthesis.Parentheses

let parenthesis = Map [ (')', '('); ('}', '{'); (']', '[') ]
let containsValue v = Map.fold (fun state _ value -> state || value.Equals(v) ) false parenthesis

// checks whether the string contains valid parentheses sequence
let containsValidParenthesis string =
    let listFromGivenString = Seq.toList string
    let rec inner list (stack: char list) =
        match list with
        | [] -> stack.IsEmpty
        | head :: tail ->
            if containsValue head then inner tail (head :: stack)
            elif parenthesis.ContainsKey head
            then
                match stack.Length with
                | 0 -> false
                | _ -> if stack.Head.Equals(parenthesis.[head]) then inner tail stack.Tail else inner tail stack
            else inner tail stack
    inner listFromGivenString []