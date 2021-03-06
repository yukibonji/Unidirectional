﻿module Main

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
    let mvc = Mvc(model, view, controller)
    use __ = mvc.Start()
    App().Run(view.Root)


