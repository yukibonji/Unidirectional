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
    | Add
    | Remove


type NumericUpDownWindow = XAML<"NumericUpDown/NumericUpDownWindow.xaml">
type NumericUpDownWindowView(root : NumericUpDownWindow) = 
    inherit View<NumericUpDownListEvents, NumericUpDownListModel, Window>(root)
    override this.EventStreams = [
        root.AddButton.Click |> Observable.map (fun _ -> Add)
        root.RemoveButton.Click |> Observable.map (fun _ -> Remove)
        ]

    override this.SetBindings model =
        Binding.OfExpression 
            <@
                root.Items.ItemsSource <- model.List
            @>



type NumericUpDownWindowController() =
    inherit Controller<NumericUpDownListEvents, NumericUpDownListModel>()

    let rec remove i l =
        match i, l with
        | 0, x::xs -> xs
        | i, x::xs -> x::remove (i - 1) xs
        | i, [] -> []

    let add (m:NumericUpDownListModel) =
        m.List <- (List.append m.List [NumericUpDownModel.Create()])

    let remove (m:NumericUpDownListModel) =
        m.List <- remove (List.length m.List - 1) m.List

    override this.InitModel model =
        model.List <- []

    override this.Dispatcher = function
    | Add -> Sync (fun m -> add m)
    | Remove -> Sync (fun m -> remove m)