module LocalNetwork.LocalNetwork

open System

type OperatingSystem =
    | Windows
    | Linux
    | MacOS
    
type Computer (operatingSystem : OperatingSystem, computerName : string, isInfected : bool) = 
    member val name = computerName
    member val os = operatingSystem 
    member val isInfected = isInfected with get, set
    member val justInfected = false with get, set
    member val infectionChance = 
        match operatingSystem with
        | Windows -> 0.7
        | Linux -> 0.3
        | MacOS -> 0.5
             
type localNet (connections : List<string * string>, computers : Computer list, ?rand) =
    
    member val computers = computers
    member val connections = connections
    member val random =
        match rand with
        | Some value -> value 
        | None -> Random()
    member val infectedComputers = List.fold(fun acc (c : Computer) -> if c.isInfected then acc + 1 else acc ) 0 computers with get, set
    
    member this.Run =
        
        let shouldBeInfected (c : Computer) =
            let random = this.random
            let aa = random.NextDouble()
            c.infectionChance > random.NextDouble() && not c.justInfected
        
        let isInPair (item : 't, pair : 't * 't) = item = fst pair || item = snd pair

        let getConnected (item : 't, pair : 't * 't) = if item = fst pair then snd pair else fst pair
               
        let infectConnected (connectedName : string) =
            let connected = ((List.find (fun (c : Computer) -> c.name = connectedName)) this.computers)
            if shouldBeInfected connected && not connected.isInfected
            then
                this.infectedComputers <- this.infectedComputers + 1
                connected.justInfected <- true
                connected.isInfected <- true
            
        let findAndInfectByConnection (connection : string * string, comp : Computer) = 
            if isInPair (comp.name, connection)
            then
                let bro = getConnected (comp.name, connection)
                if not comp.justInfected then infectConnected bro
    
        let infectAllConnected (comp : Computer) = List.iter(fun (connection : string * string) -> findAndInfectByConnection(connection, comp) ) this.connections

        let stepOfInfection() =
            List.iter(fun (comp : Computer) -> if comp.isInfected then infectAllConnected comp) this.computers
            
        let getPrintString (comp : Computer) =
            let infectState = if comp.isInfected then "infected" else "still standing"
            $"Computer {comp.name} is {infectState}"
            
        let printNetState =
            List.iter(fun (comp : Computer) -> printfn $"{getPrintString comp}") this.computers
        
        let amountOfComputers = computers.Length
        
        let dropJustInfected = List.iter(fun (comp : Computer) -> comp.justInfected <- false) this.computers
                        
        let rec processNet() =
            match this.infectedComputers with
            | 0 -> printfn "Everyone gonna stay alive :)"
            | x when x > 0 && x < amountOfComputers ->
                stepOfInfection()
                dropJustInfected
                printNetState
                processNet()
            | amountOfComputers  -> printfn "Everyone is infected no one is safe :("
                
        processNet()