module HashGenerators

open System.Text
open System.Security.Cryptography
open System

let hashOnLength (item:string) = item.Length.ToString()

let private provider = SHA256.Create()
let hashSha256 (item:string) =
    item
    |> Encoding.UTF8.GetBytes
    |> provider.ComputeHash
    |> Convert.ToBase64String