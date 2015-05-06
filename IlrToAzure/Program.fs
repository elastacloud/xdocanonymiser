open IlrTools
open System
open System.Configuration
open System.Linq
open System.Xml.Linq
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Auth
open Microsoft.WindowsAzure.Storage.Blob
open Newtonsoft.Json

let storageAccount = 
    let storageAccountName = ConfigurationManager.AppSettings.["storageAccountName"]
    let storageAccountKey = ConfigurationManager.AppSettings.["storageAccountKey"]
    let accountCreds = new StorageCredentials(storageAccountName, storageAccountKey)
    CloudStorageAccount(accountCreds, true)

[<EntryPoint>]
let main argv = 
    let xdoc = argv.[0]
                    |> XDocument.Load
    let provider = xdoc
                    |> getLearningProvider 

    printfn "Working on provider %s (UKPRN)" provider

    let learnersJson = xdoc
                        |> learners
                        |> Seq.map(fun x -> JsonConvert.SerializeXNode(x, Formatting.None))

    printfn "There are %d learners" (learnersJson.Count())
    
    let blobClient = storageAccount.CreateCloudBlobClient()
    let container = blobClient.GetContainerReference ConfigurationManager.AppSettings.["storageAccountContainer"]
    let blobRef = container.GetBlockBlobReference (sprintf "%s/%d/%d/%d/input.json" provider System.DateTime.Now.Year System.DateTime.Now.Month System.DateTime.Now.Day)
    let uploadText = String.Join(Environment.NewLine, learnersJson)
    blobRef.UploadText uploadText

    printf "Completed. Press ENTER to conclude."

    System.Console.ReadLine() |> ignore

    0 // return an integer exit code
