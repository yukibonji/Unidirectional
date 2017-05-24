﻿namespace Controls.NumericUpDown

open System.Windows
open System.Windows.Input
open System.Windows.Data
open System.Windows.Controls

open FsXaml
open FSharp.Desktop.UI




[<AbstractClass>]
type NumericUpDownModel() = 
    inherit Model()

    abstract Value: int with get, set


type NumericUpDownEvents = 
    | Up 
    | Down 
    | Edit of string


type NumericUpDownControl = XAML<"NumericUpDownControl.xaml">


type NumericUpDownControlView(root : NumericUpDownControl) = 
    inherit PartialView<NumericUpDownEvents, NumericUpDownModel, Control>(root) 
    override this.EventStreams = [        
        root.upButton.Click |> Observable.map (fun _ -> Up)
        root.downButton.Click |> Observable.map (fun _ -> Down)

        root.input.KeyUp |> Observable.choose (fun args -> 
            match args.Key with 
            | Key.Up -> Some Up  
            | Key.Down -> Some Down
            | _ ->  None
        )
        root.input.TextChanged  |> Observable.map (fun args -> Edit root.input.Text)
        root.input.MouseWheel   |> Observable.map (fun args -> if args.Delta > 0 then Up else Down)
    ]

    override this.SetBindings model =
        Binding.OfExpression 
            <@
                //'coerce' means "use WPF default conversions"
                root.input.Text <- coerce model.Value |> BindingOptions.OneWay
            @>

module NumericUpDownControlController =
    let eventHandler (event:NumericUpDownEvents) (model: NumericUpDownModel) =
        match event with
        | Up -> model.Value <- model.Value + 1
        | Down -> model.Value <- model.Value - 1
        | Edit str -> 
            match System.Int32.TryParse str with
            | true, i -> model.Value <- i
            | false, _ -> model.Value <-model.Value
                

