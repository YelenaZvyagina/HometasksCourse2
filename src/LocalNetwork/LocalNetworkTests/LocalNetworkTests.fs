module LocalNetworkTests

open LocalNetwork.LocalNetwork
open NUnit.Framework
open FsUnit

[<Test>]
let EverybodyInfects() =
    let computers = [| Computer(Linux, "aa", 1); Computer(MacOS, "bb", 2); Computer(Windows, "сс", 0) |]
    let viruses = [| Virus([| computers[1] |], (function  _ -> 0.8), (fun () -> 0.5)) |]
    let connectionMatrix = Array2D.create 3 3 true
    let net = PlagueInc(viruses, computers, connectionMatrix)
    net.Play()
    viruses[0].InfectedComputers.Count |> should equal 3

    
[<Test>]    
let NobodyInfects() =
    let computers = [| Computer(Linux, "aa", 1); Computer(MacOS, "bb", 2); Computer(Windows, "сс", 0) |]
    let viruses = [| Virus([| computers[1] |], (function  _ -> 0.0), (fun () -> 1.0)) |]
    let connectionMatrix = Array2D.create 3 3 true
    let net = PlagueInc(viruses, computers, connectionMatrix)
    net.Play()
    viruses[0].InfectedComputers.Count |> should equal 1
    