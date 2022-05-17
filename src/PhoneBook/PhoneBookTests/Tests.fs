module PhoneBookTests.Tests

open NUnit.Framework
open PhoneBook.PhoneBook
open FsUnit

let testingBook =
    [
        PhoneBookRecord("nnick", 12345678789L);
        PhoneBookRecord("Nastya", 15255555555L);
        PhoneBookRecord("tidid", 18885976836L);
    ]

[<Test>]
let testForAdding () =
    let book = addRecord "kavin" 89567893654L []
    Assert.AreEqual ([PhoneBookRecord("kavin", 89567893654L)], book)
     
[<Test>]
let testFindName () =
    let result = findNameByPhone 12345678789L testingBook
    Assert.AreEqual (["nnick"], result)
    
[<Test>]
let testFindPhone () =
    let result = findPhoneByName "Nastya" testingBook
    Assert.AreEqual ([15255555555L], result)
    
[<Test>]
let saveAndGetFromFileTest () =
    savePhoneBookToFile testingBook "test.txt"
    let result = getPhoneBookFromFile "test.txt"
    Assert.AreEqual (result, testingBook)