module DataAccess

open System.Collections.Generic

let inMemoryUpsert() =
    let store = Dictionary<_,_>()
    store,
    fun (key, value) ->
        if store.ContainsKey key then ()
        else store.[key] <- value

open FSharp.Data

[<Literal>]
let private conn = "Data Source=(localdb)\ProjectsV12;Initial Catalog=AnonymiseDb;Integrated Security=True;Connect Timeout=30;Encrypt=False"
type private HashExists = SqlCommandProvider<"SELECT GeneratedHash FROM anonymiser.Lookup WHERE SourceKey = @SourceKey", conn, SingleRow = true>
type private HashInsert = SqlCommandProvider<"INSERT INTO anonymiser.Lookup VALUES (@key, @hash)", conn>

let sqlPersist = HashInsert.Create().Execute >> ignore
let sqlTryGetHash = HashExists.Create().Execute
