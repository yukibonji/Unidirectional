namespace NumericUpDownWindow


open FsXaml
open FSharp.Desktop.UI

open System.Windows
open System.Windows.Input
open System.Windows.Data

open Controls.NumericUpDown

[<AbstractClass>]
type NumericUpDownListModel() = 
    inherit Model()

    abstract List: List<NumericUpDownModel> with get, set


type NumericUpDownListEvents = 
    | First
    | Second


type NumericUpDownWindow = XAML<"NumericUpDown/NumericUpDownWindow.xaml">
type NumericUpDownWindowView(root : NumericUpDownWindow) = 
    inherit View<NumericUpDownListEvents, NumericUpDownListModel, Window>(root)
    override this.EventStreams = [        
        ]

    override this.SetBindings model =
        ()



type NumericUpDownWindowController() =
    inherit Controller<NumericUpDownListEvents, NumericUpDownListModel>()

    override this.InitModel model =
        model.List <- ([NumericUpDownModel.Create(); NumericUpDownModel.Create()] |> Seq.toList)

    override this.Dispatcher = function
        | First -> Sync (fun m -> ())
        | Second -> Sync (fun m -> ())
