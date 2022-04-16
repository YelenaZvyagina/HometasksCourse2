module LocalNetworkTests

open LocalNetwork.LocalNetwork
open NUnit.Framework
open Foq

[<Test>]
let EverybodyInfects() =
    let computers1416 = [Computer(Linux, "Na$ty@_D@ngerous", true); Computer(Windows, "Hannuken", false); Computer(MacOS, "OneLegPirat", false)]
    let connections = [("Na$ty@_D@ngerous", "Hannuken"); ("Hannuken", "OneLegPirat")]
    let myNet = localNet(connections, computers1416)
    myNet.Run 
    Assert.AreEqual(3, myNet.infectedComputers)
    
[<Test>]    
let NobodyInfects() =
    let computers1416 = [Computer(Linux, "Na$ty@_D@ngerous", false); Computer(Windows, "Hannuken", false); Computer(MacOS, "OneLegPirat", false)]
    let connections = [("Na$ty@_D@ngerous", "Hannuken"); ("Hannuken", "OneLegPirat")]
    let myNet = localNet(connections, computers1416)
    myNet.Run 
    Assert.AreEqual(0, myNet.infectedComputers)
    
[<Test>]
let FoqTest() =
    let rand = Mock<System.Random>().Setup(fun x -> <@ x.NextDouble() @>).Returns(0.5).Create()
    let computers1416 = [Computer(Linux, "Na$ty@_D@ngerous", true); Computer(Windows, "Hannuken", true); Computer(MacOS, "OneLegPirat", true)]
    let connections = [("Na$ty@_D@ngerous", "Hannuken"); ("Hannuken", "OneLegPirat")]
    let myNet = localNet(connections, computers1416, rand)
    myNet.Run 
    Assert.AreEqual(3, myNet.infectedComputers)
    

