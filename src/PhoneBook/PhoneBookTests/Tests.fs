module PhoneBookTests.Tests

open Expecto
open PhoneBook.PhoneBook


[<Tests>]
let phoneBookTests =
    testList "Tests for phone book function"
        [
            testCase "Test for adding record function" <| fun _ ->
                let testingBook = []
                let addedRecord = addRecord "Nastya" 12345678789L testingBook
                Expect.isTrue (addedRecord.Length = 1) ""
                
            testCase "test for find name function" <| fun _ ->
                let testingBook = []
                let addedRecord = addRecord "Nastya" 12345678789L testingBook
                let foundName = findNameByPhone 12345678789L testingBook
                Expect.sequenceEqual foundName ["Nastya"] ""
                
            testCase "test for find phone function" <| fun _ ->
                let testingBook = []
                let addedRecord = addRecord "Nastya" 12345678789L testingBook
                let foundNumber = findPhoneByName "Nastya" testingBook
                Expect.sequenceEqual foundNumber [12345678789L] ""
                
            testCase "tests for file functions" <| fun _ ->
                let testingBook = []
                let addedRecord = addRecord "Nastya" 12345678789L testingBook
                let filepath = "..\PhoneBookTests\pb.txt"
                let savd = savePhoneBookToFile testingBook filepath
                let read = getPhoneBookFromFile filepath
                Expect.equal read.[1].name "Nastya" ""
                Expect.sequenceEqual testingBook read ""
       ]
    
