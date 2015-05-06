module IlrTools
open System.Linq
open System.Xml.Linq

let xName implicit =
    let xname = XName.Get(implicit, "http://www.theia.org.uk/ILR/2014-15/1")
    xname

let getLearningProvider (xdoc:XDocument) =
    let lp = xdoc.Descendants <| xName "LearningProvider"
    let ukprn = lp.First().Element <| xName "UKPRN"
    ukprn.Value

let learners (xdoc:XDocument) = 
    xdoc.Descendants <| xName "Learner"