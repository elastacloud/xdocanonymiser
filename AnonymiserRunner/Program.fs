open System.IO

[<EntryPoint>]
let main argv = 
    match argv with
    | [| xmlFolder; replacementsFile |] -> 
        let replacements = File.ReadAllLines replacementsFile |> List.ofArray
        Runner.parseDocuments xmlFolder replacements
        1
    | _ ->
        printfn "Expected two arguments - folder contains all XML documents and a file contains replacements."
        0