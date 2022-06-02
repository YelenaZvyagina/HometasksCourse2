module LocalNetwork.LocalNetwork

open System.Collections.Generic

type OS =
    | Windows
    | Linux
    | MacOS
    
type Computer (operatingSystem : OS, computerName : string, number : int) = 
    member val name = computerName
    member val number = number
    member val os = operatingSystem
    
let mutable connections = Array2D.zeroCreate 0 0
let mutable computers = Array.empty

type Virus(firstInfected: Computer[], infectionChance: OS -> float, random: unit -> float) =
    let infectedComputers = HashSet(firstInfected)

    let getNeighbours (computer : Computer) =
        let neighbours = HashSet()
        connections[computer.number, *]
        |> Array.iteri (fun i elem -> if elem then neighbours.Add computers[i] |> ignore)
        neighbours
    
    let calculateInfectionCandidates lastInfected =
        lastInfected
        |> Seq.map getNeighbours
        |> Seq.concat
        |> Seq.distinct
        |> Seq.filter (not << infectedComputers.Contains)
        |> Seq.filter (fun x -> infectionChance x.os > 0)

    let mutable infectionCandidates = calculateInfectionCandidates infectedComputers

    member x.InfectedComputers = infectedComputers

    member x.AbleToInfect = not (Seq.isEmpty infectionCandidates)

    member x.SpreadInfection() =
        let newInfected = List()

        for computer in infectionCandidates do
            if random() < infectionChance computer.os then
                newInfected.Add(computer)

        infectedComputers.UnionWith(newInfected)
        infectionCandidates <- calculateInfectionCandidates newInfected

type PlagueInc(viruses: Virus[], computersList : Computer[], connectionsMatrix : bool[,]) =
    let rec play viruses = 
        let viruses = viruses |> Seq.filter (fun (v: Virus) -> v.AbleToInfect)
        if Seq.isEmpty viruses then () else
        for virus in viruses do virus.SpreadInfection()
        play viruses
        
    member x.Play() =
        connections <- connectionsMatrix
        computers <- computersList
        play viruses