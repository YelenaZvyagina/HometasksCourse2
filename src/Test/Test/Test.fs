namespace Test

module Test =
    
    let getRepeatd number = Seq.init number (fun _ -> number)
    let s = Seq.initInfinite getRepeatd |> Seq.concat


    let squareOfStars number =        
        let fullString = List.init number (fun _ -> '*')
        let boardString = List.init number (fun x -> if x = 0 || x = number - 1 then '*' else ' ')
        
        let printString list =
            List.iter (printf "%A") list
            printf "\n"
        
        let rec squareInner  accStrings number =
            match accStrings with
            | 1 -> printString fullString
            | x when x > 1 && x < number ->
                printString boardString
                squareInner (accStrings - 1) number
            | number ->
                printString fullString
                squareInner (accStrings - 1) number
        squareInner number number
    
    squareOfStars 4    
        
                