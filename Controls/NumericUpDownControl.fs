namespace Controls.NumericUpDown

open System.Windows
open System.Windows.Input
open System.Windows.Data
open System.Windows.Controls

open FsXaml
open FSharp.Desktop.UI



[<AbstractClass>]
type NumericUpDownModel() = 
    inherit Model()
    abstract Value: double with get, set


type NumericUpDownEvents = 
    | Up 
    | Down 
    | Edit of string

type NumericUpDownControl = XAML<"NumericUpDownControl.xaml">

type NumericUpDownControlView(root : NumericUpDownControl) = 
    inherit PartialView<NumericUpDownEvents, NumericUpDownModel, Control>(root) 
    override this.EventStreams = [        
        root.UpButton.Click |> Observable.map (fun _ -> Up)
        root.DownButton.Click |> Observable.map (fun _ -> Down)

        root.Input.KeyUp |> Observable.choose (fun args -> 
            match args.Key with 
            | Key.Up -> Some Up  
            | Key.Down -> Some Down
            | _ ->  None
        )
        root.Input.TextChanged  |> Observable.map (fun args -> Edit root.Input.Text)
        root.Input.MouseWheel   |> Observable.map (fun args -> if args.Delta > 0 then Up else Down)
    ]

    override this.SetBindings model =
        Binding.OfExpression 
            <@
                root.Input.Text <- System.String.Format("{0:0.0}", model.Value) |> BindingOptions.OneWay
                //root.input.Text <- (coerce model.Value) |> BindingOptions.OneWay
            @>

module NumericUpDownControlController =
    let eventHandler (event:NumericUpDownEvents) (model: NumericUpDownModel) =
        match event with
        | Up -> model.Value <- model.Value + 1.0
        | Down -> model.Value <- model.Value - 1.0
        | Edit str -> 
            match System.Double.TryParse str with
            | true, d -> model.Value <- d
            | false, _ -> model.Value <-model.Value
                

