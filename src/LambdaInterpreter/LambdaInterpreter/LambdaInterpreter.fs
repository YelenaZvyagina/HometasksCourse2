module LambdaInterpreter

type Term =
    | Variable of string
    | Application of Term * Term
    | LambdaAbstraction of string * Term

let rec getFreeVariables term =
    match term with
    | Variable var -> Set.singleton var
    | Application (term1, term2) -> Set.union (getFreeVariables term1) (getFreeVariables term2)
    | LambdaAbstraction (var, term) -> Set.difference (getFreeVariables term) (Set.singleton var)
    
let getFreeName vars =
    Set.maxElement(vars) + "_new"

let rec substitute variable term newTerm =
    match term with
    | Variable var -> if (variable = var) then newTerm else term
    | Application(term1, term2) -> Application (substitute variable term1 newTerm, substitute variable term2 newTerm)
    | LambdaAbstraction (var, term1) ->
        if var = variable then term
        elif (getFreeVariables term1).Contains(variable) || (getFreeVariables newTerm).Contains(var)
        then
            let freeVariables = Set.union (getFreeVariables term1) (getFreeVariables newTerm)
            let renamedVar = getFreeName freeVariables
            let renamedTerm = substitute var term1 (Variable renamedVar)
            LambdaAbstraction(renamedVar, substitute variable renamedTerm newTerm)
        else 
            LambdaAbstraction(var, substitute variable term1 newTerm)
            
let rec betaReduction term =
    match term with
    | Variable _ -> term
    | Application (LambdaAbstraction (var, term1), term2) -> betaReduction (substitute var term1 term2)
    | Application (term1, term2) ->
        let calcTerm1 = betaReduction term1
        match calcTerm1 with
        | LambdaAbstraction(var, term3) -> betaReduction(substitute var term3 term2)
        | _ -> Application(calcTerm1, betaReduction term2)                
    | LambdaAbstraction (var, term1) -> LambdaAbstraction(var, betaReduction term1)