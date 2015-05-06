#r "System.Xml"
#r "System.Xml.Linq"
#r @"..\packages\Newtonsoft.Json.6.0.8\lib\net45\Newtonsoft.Json.dll"

#load "IlrTools.fs"

open System.Linq
open System.Xml.Linq
open IlrTools
open Newtonsoft.Json

let xdoc = @"C:\tmp\Andy\ILR-A-10001919-1415-20150429-114818-01.xml"
            |> XDocument.Load

xdoc
|> getLearningProvider 
|> printfn "%O"

let learnersJson = xdoc
                    |> learners
                    |> Seq.map(fun x -> JsonConvert.SerializeXNode(x, Formatting.None))