module Main

open System
open System.Windows
open System.Windows.Input
open System.Windows.Data
open FsXaml
open FSharp.Desktop.UI

type App = XAML<"App.xaml">
type MainWindow = XAML<"MainWindow.xaml">

[<AbstractClass>]
type NumericUpDownModel() = 
    inherit Model()

    abstract Value: int with get, set

type NumericUpDownEvents = Up | Down

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

let eventHandler event (model: NumericUpDownModel) =
    match event with
    | Up -> model.Value <- model.Value + 1
    | Down -> model.Value <- model.Value - 1

let controller = Controller.Create eventHandler

[<STAThread>]
[<EntryPoint>]
let main argv = 
    let model = NumericUpDownModel.Create()
    let app = App()
    let view = NumericUpDownView(MainWindow())
    let mvc = Mvc(model, view, controller)
    use __ = mvc.Start()
    app.Run(view.Root) 


