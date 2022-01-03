[<AutoOpen>]
module AppState

type Running =
    { Clicked: int list
      Numbers: int list
      Points: int
      TickId: int }

type State =
    | Not_Started
    | Running of Running
    | Finished of int

type Msg =
    | Tick of int
    | GameStarted of int
    | Start
    | Restart
    | Clicked of int
