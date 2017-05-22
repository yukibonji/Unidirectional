module Main

open System
open System.Windows
open FsXaml
open FSharp.Desktop.UI

type App = XAML<"App.xaml">

open Controls.NumericUpDown
open NumericUpDownWindow

[<STAThread>]
[<EntryPoint>]
let main argv = 
    
    let model = NumericUpDownListModel.Create()
    let window = NumericUpDownWindow()
    let view = NumericUpDownWindowView(window)
    let controller = NumericUpDownWindowController()
    let controlController = Controller.Create NumericUpDownControlController.eventHandler
    let 
        mvc = Mvc(model, view, controller)  
            <+> (controlController, NumericUpDownControlView(window.UpDown1), fun (m:NumericUpDownListModel) -> m.List.[0])
            <+> (controlController, NumericUpDownControlView(window.UpDown2), fun (m:NumericUpDownListModel) -> m.List.[1])
    use __ = mvc.Start()
    App().Run(view.Root)


