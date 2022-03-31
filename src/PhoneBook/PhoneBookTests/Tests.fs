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
                
            testCase "tests for file functions" <| fun _ ->
                let testingBook = []
                let addedRecord = addRecord "Nastya" 12345678789L testingBook
                let filepath = "..\PhoneBookTests\pb.txt"
                let savd = savePhoneBookToFile testingBook filepath
                let read = getPhoneBookFromFile filepath
                Expect.equal read.[1].name "Nastya" ""
                Expect.sequenceEqual testingBook read ""
       ]
    
