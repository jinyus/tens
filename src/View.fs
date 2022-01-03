module View

let private renderButton dispatch index (number: int) =
    Html.button [ prop.style [ style.padding 20
                               style.fontSize 20 ]
                  prop.onClick (fun _ -> dispatch (Clicked index))
                  prop.text number ]

let private renderRunning state dispatch =
    let buttons =
        state.Numbers |> List.mapi (renderButton dispatch)

    Html.div [ yield! buttons
               // yield Html.div (sprintf "%A" state)
                ]

let private renderFinished points dispatch =
    let score =
        Html.div [ Html.h2 [ prop.text (sprintf "Final Score: %i" points) ] ]

    Html.div [ score
               Html.button [ prop.style [ style.padding 20
                                          style.fontSize 20 ]
                             prop.onClick (fun _ -> dispatch Restart)
                             prop.text "Restart" ] ]

let render (state: State) (dispatch: Msg -> unit) =
    match state with
    | Not_Started ->
        Html.button [ prop.style [ style.padding 20
                                   style.fontSize 20 ]
                      prop.onClick (fun _ -> dispatch Start)
                      prop.text "Start" ]

    | Running state -> renderRunning state dispatch

    | Finished points -> renderFinished points dispatch
