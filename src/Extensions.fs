module Extensions

open Elmish
open Elmish.React
open Feliz
open Fable.Core

[<Emit("setInterval($0, $1)")>]
let setInterval (f: unit -> unit) (n: int) : int = jsNative

[<Emit("clearInterval($0)")>]
let clearInterval (n: int) : unit = jsNative

let removeAtIndex index list =
    list
    |> List.mapi (fun i element -> (i <> index, element))
    |> List.filter fst
    |> List.map snd
