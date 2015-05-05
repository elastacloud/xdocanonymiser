#r @"System.Xml.Linq.dll"
#r @"..\packages\FSharp.Data.SqlClient.1.6.2\lib\net40\FSharp.Data.SqlClient.dll"
#load "XDocAnonymiser.fs"
#load "DataAccess.fs"

open DataAccess

let anonymise (item:string) = item.Length.ToString()

let replacements =
    [ "/Root/First/Child/Other"; "/Root/First/Other"; "/Root/First/Child/@Other" ]
    |> List.map XDocAnonymiser.ElementType.ofRawPath

let doc = System.Xml.Linq.XDocument.Load(@"C:\Users\Isaac\Desktop\XmlSamples\sample.xml")
let updatedDoc = XDocAnonymiser.anonymiseDocument replacements anonymise DataAccess.sqlPersist DataAccess.sqlTryGetHash doc
updatedDoc.Save (@"C:\Users\Isaac\Desktop\XmlSamples\sample2.xml")

