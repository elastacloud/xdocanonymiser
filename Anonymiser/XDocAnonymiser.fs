module XDocAnonymiser

open System.Xml.Linq

type ElementType =
| Element
| Attribute
    static member ofXObject (xObject:XObject) =
        match xObject with
        | :? XElement -> Element
        | :? XAttribute -> Attribute
        | _ -> failwith "unknown!"
    static member ofRawPath (path:string) = if path.Contains "@" then (Attribute, path.Replace("@", "")) else (Element, path)

type Replacement = ElementType * string

let private getName (element:XObject) =
    match element with
    | :? XElement as element -> element.Name.LocalName
    | :? XAttribute as attribute -> attribute.Name.LocalName
    | _ -> failwith "Unknown!"

let private getAttributeNamePath elementName =
    getName >> sprintf "%s/%s" elementName

let rec private getPaths (path:string) (element:XElement) =
    let elementName = sprintf "%s/%s" path (getName element)
    let attributes = element.Attributes() |> Seq.map(fun att -> att :> XObject, getAttributeNamePath elementName att) |> Seq.toList
        
    let children = element.Elements() |> Seq.collect(getPaths elementName) |> Seq.toList
        
    (element :> XObject, elementName)
    :: attributes
    @ children

let private getValue (item:XObject) =
    match item with
    | :? XAttribute as att -> att.Value
    | :? XElement as element -> element.Value

let private setValue newValue (item:XObject) =
    match item with
    | :? XAttribute as att -> att.Value <- newValue
    | :? XElement as element -> element.Value <- newValue

/// Anonymises an XML Document
let anonymiseDocument replacements anonymiseValue persistHash tryLoadHash (doc:XDocument) =
    let doc = XDocument doc
    let replacements = Set replacements
    getPaths "" doc.Root
    |> List.filter(fun (element, path) -> (ElementType.ofXObject element, path) |> replacements.Contains)
    |> List.iter(fun (element, path) ->
        let currentValue = getValue element
        let hash =
            match tryLoadHash currentValue with
            | None -> 
                let hash = anonymiseValue currentValue
                persistHash(currentValue, hash)
                hash
            | Some hash -> hash
        element |> setValue hash) 
    doc