module Main

open System
open System.Windows
open FsXaml
open FSharp.Desktop.UI

type App = XAML<"App.xaml">

open NumericUpDown

[<STAThread>]
[<EntryPoint>]
let main argv = 
    let model = NumericUpDownModel.Create()
    let app = App()
    let view = NumericUpDownView(MainWindow())
    let mvc = Mvc(model, view, Controller.Create numericUpDownEventHandler)
    use __ = mvc.Start()
    app.Run(view.Root) 


