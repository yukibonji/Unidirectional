module NumericUpDown

open System.Windows
open System.Windows.Input
open System.Windows.Data

open FsXaml
open FSharp.Desktop.UI




[<AbstractClass>]
type NumericUpDownModel() = 
    inherit Model()

    abstract Value: int with get, set


type NumericUpDownEvents = Up | Down


type MainWindow = XAML<"NumericUpDown.xaml">
type NumericUpDownView(root : MainWindow) = 
    inherit View<NumericUpDownEvents, NumericUpDownModel, Window>(root) 
    override this.EventStreams = [        
        root.upButton.Click |> Observable.map (fun _ -> Up)
        root.downButton.Click |> Observable.map (fun _ -> Down)

        root.input.KeyUp |> Observable.choose (fun args -> 
            match args.Key with 
            | Key.Up -> Some Up  
            | Key.Down -> Some Down
            | _ ->  None
        )

        root.input.MouseWheel |> Observable.map (fun args -> if args.Delta > 0 then Up else Down)
    ]

    override this.SetBindings model =           
        Binding.OfExpression 
            <@
                //'coerce' means "use WPF default conversions"
                root.input.Text <- coerce model.Value
            @> 


let numericUpDownEventHandler event (model: NumericUpDownModel) =
    match event with
    | Up -> model.Value <- model.Value + 1
    | Down -> model.Value <- model.Value - 1

