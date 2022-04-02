module PhoneBook.PhoneBook

open System
open System.IO

[<Struct>]
type PhoneBookRecord =
    val name: string
    val phoneNumber: int64
    new (n, p) = {name = n; phoneNumber = p}
    
let addRecord name phone phoneBook = PhoneBookRecord(name, phone) :: phoneBook
        
let findPhoneByName name phoneBook =
    phoneBook
    |> List.filter (fun (x : PhoneBookRecord) -> (x.name = name))
    |> List.map ( fun (x : PhoneBookRecord) -> x.phoneNumber)

let findNameByPhone phone phoneBook =
    phoneBook
    |> List.filter (fun (x : PhoneBookRecord) -> (x.phoneNumber = phone))
    |> List.map ( fun (x : PhoneBookRecord) -> x.name)
    
let printAllPhoneBook phoneBook = List.iter(fun (x : PhoneBookRecord) -> printfn $"Name %A{x.name}. Phone %A{string x.phoneNumber}") phoneBook

let savePhoneBookToFile phoneBook filepath =
    let dataToWriteInFile = List.map (fun (x : PhoneBookRecord) -> (String.Format("{0} {1}", x.name, (x.phoneNumber |> string)))) phoneBook
    File.WriteAllLines(filepath, dataToWriteInFile)

let getPhoneBookFromFile filepath =
    let getDataFromString (s : string) =
        let split = s.Split(' ')
        if split.Length <> 2 then failwith $"Incorrect data format in a file, \"name phoneNumber\" expected, {s} got "
        PhoneBookRecord(split.[0], (split.[1] |> int64))
    File.ReadAllLines filepath |> Seq.toList |> List.map getDataFromString