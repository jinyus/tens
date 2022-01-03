module App

open Elmish
open Elmish.React
open Fable.Core


module Controller =
    open Extensions

    let private random = new System.Random()

    let private startGame =
        let start dispatch =
            let tickId =
                setInterval (fun () -> dispatch (Tick(random.Next(1, 9)))) 1000

            GameStarted tickId |> dispatch

        Cmd.ofSub start

    let private stopTicking id =
        let stop _ =
            clearInterval id
            ()

        Cmd.ofSub stop

    let private (|ReachedExactlyTen|NotTenAndClickNumberExceeded|StillGood|) clicked =
        if clicked |> List.sum = 10 then
            ReachedExactlyTen
        elif clicked |> List.length = 3 then
            NotTenAndClickNumberExceeded
        else
            StillGood

    let private (|MaxNumbersExceeded|WithinNumberLimit|) (numbers: int list) =
        if numbers |> List.length = 11 then
            MaxNumbersExceeded
        else
            WithinNumberLimit

    let private finishedGame state =
        Finished state.Points, stopTicking state.TickId

    let private withoutCommands state = state, Cmd.none

    let private handleClick state index number =
        let newState =
            { state with
                Clicked = state.Clicked @ [ number ]
                Numbers = state.Numbers |> removeAtIndex index }

        match newState.Clicked with
        | ReachedExactlyTen ->
            Running
                { newState with
                    Clicked = []
                    Points = state.Points + 1 }
            |> withoutCommands

        | NotTenAndClickNumberExceeded -> finishedGame state

        | StillGood -> Running newState |> withoutCommands

    let private initializedGame tickId =
        Running
            { Clicked = []
              Numbers = []
              Points = 0
              TickId = tickId }

    let private withHandledClickOn index state =
        state.Numbers
        |> List.item index
        |> handleClick state index


    let private addNewNumber number state =
        let state =
            { state with Numbers = state.Numbers @ [ number ] }

        match state.Numbers with
        | MaxNumbersExceeded -> finishedGame state

        | WithinNumberLimit -> Running state |> withoutCommands

    let init () = Not_Started, Cmd.none

    let update (msg: Msg) (state: State) =
        match state, msg with
        | Running state, Clicked index -> state |> withHandledClickOn index

        | Running state, Tick number -> state |> addNewNumber number

        | Not_Started, Start -> state, startGame

        | Finished _, Restart -> Not_Started, startGame

        | Not_Started, GameStarted tickId -> initializedGame tickId |> withoutCommands

        | _ -> state |> withoutCommands

Program.mkProgram Controller.init Controller.update View.render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "elmish-app"
|> Program.run
