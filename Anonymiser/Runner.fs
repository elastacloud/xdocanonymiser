module Runner

open System.IO
open System.Xml.Linq
open XDocAnonymiser

let private anonymiser replacements = XDocAnonymiser.anonymiseDocument replacements HashGenerators.hashSha256 DataAccess.sqlPersist DataAccess.sqlTryGetHash

let parseDocuments folder replacements =
    let outputFolder = Path.Combine(folder, "anonymised")
    if not (Directory.Exists outputFolder) then (Directory.CreateDirectory outputFolder |> ignore)
    let replacements = replacements |> List.map ElementType.ofRawPath
    Directory.GetFiles(folder, "*.xml")
    |> Array.map(fun filename ->
        printfn "Parsing '%s'" filename
        filename, XDocument.Load filename)
    |> Array.map(fun (filename, doc) -> filename, anonymiser replacements doc)
    |> Array.iter(fun (filename, newDoc) ->
        let newFilename = Path.GetFileNameWithoutExtension filename + "-anonymised.xml"
        let newFilename = Path.Combine(outputFolder, newFilename)
        printfn "Saving '%s' to '%s'" filename newFilename
        newDoc.Save newFilename)