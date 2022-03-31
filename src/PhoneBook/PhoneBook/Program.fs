module Phonebook.Main

open System
open System.IO
open PhoneBook
open PhoneBook

let printStart =
    printfn "Hello! I'm a phone book and I can store names and phones.
    Please comply the following format - name can contain letters from a-z, A-Z (Ivanov),
    phone number can contain numbers from 0-9 (88005553535). 
    Commands: 
    Add a new record to a phone book: --add {your_name} {your_phone_number} 
    Find phone for given name: --findphone {your_name} 
    Find name for given phone: --findname {your_phone} 
    Print all records from phone book: --printall 
    Save current records from phone book to a file: --saveto {path_to_file} 
    Get records with phones and numbers from file: --openfile {path_to_file}
    Quit the phone book: --quit"

let checkFilePath filePath = File.Exists filePath || Directory.Exists filePath

let printAll list = List.iter (fun x -> printfn $"%A{x}") list

let rec program book =
    Console.WriteLine("Please enter your command")
    let input = Console.ReadLine().Split(' ')
    let command = input.[0]
    match command with
    | "--add" ->
        let newBook = addRecord input.[1]  (int64 input.[2]) book
        Console.WriteLine("Record added")
        program newBook
    | "--findphone" ->
        printfn "Phones for this name found"
        printAll (findPhoneByName input.[1] book)
        program book
    | "--findname" ->
        printfn "Names for this phone found"
        printAll (findPhoneByName input.[1] book)
        program book
    | "--printall" ->
        printAllPhoneBook book
        program book
    | "--saveto" ->
        let path = input.[0]
        if checkFilePath path
        then
            savePhoneBookToFile book path
            printfn "Phone book was saved"
        else printfn "Please check the file path and try again"
        program book
    | "--openfile" ->
        let path = input.[0]
        if checkFilePath path
        then
            let newBook = getPhoneBookFromFile path
            printfn "Phone book was successfully downloaded"
            program newBook
        else
            printfn "Please check the file path and try again"
        program book
    | "--quit" -> printfn("Have a good day see you! :)")
    | _ ->
        printStart
        program book

printStart
program []