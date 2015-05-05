#r @"System.Xml.Linq.dll"
#r @"..\packages\FSharp.Data.SqlClient.1.6.2\lib\net40\FSharp.Data.SqlClient.dll"
#load "XDocAnonymiser.fs"
#load "DataAccess.fs"

open DataAccess

let anonymise (item:string) = item.Length.ToString()

let replacements =
    [ "Message/Header/CollectionDetails/Collection" ]
    |> List.map XDocAnonymiser.ElementType.ofRawPath

let doc = System.Xml.Linq.XDocument.Load(@"C:\tmp\ilr\ILR-A-99999999-1415-20140601-164401-03.xml")
let updatedDoc = XDocAnonymiser.anonymiseDocument replacements anonymise DataAccess.sqlPersist DataAccess.sqlTryGetHash doc
updatedDoc.Save (@"C:\Users\Isaac\Desktop\XmlSamples\sample2.xml")

