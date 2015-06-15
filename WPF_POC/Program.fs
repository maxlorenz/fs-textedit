// WPF-Example in F#

open System
open System.IO
open System.Windows
open System.Windows.Controls
open System.Windows.Markup
open Microsoft.Win32

type LastFile = None | Path of string
let mutable currentFile = None

let getPath file =
    match file with
        | Path path -> path
        | None      -> String.Empty

let filter = "Textdokumente (.txt)|*.txt|Alle Dateien|*.*"
let dialog_open = new OpenFileDialog(DefaultExt = ".txt", Filter = filter)
let dialog_save = new SaveFileDialog(DefaultExt = ".txt", Filter = filter)

let saveFile (txt: TextBox) path =
    File.WriteAllText(path, txt.Text)
    currentFile <- Path path

let openFile (txt: TextBox) path =
    txt.Text <- File.ReadAllText(path)
    currentFile <- Path path

let openFileWithDialog (txt: TextBox) = 
    if dialog_open.ShowDialog().Value then dialog_open.FileName |> openFile txt

let saveFileWithDialog (txt: TextBox) =
    if dialog_save.ShowDialog().Value then dialog_save.FileName |> saveFile txt

let Editor =
    let window = XAML.createFromRessource "Editor.xaml"

    let btnNew    = window.FindName("btnNew")    :?> MenuItem
    let btnOpen   = window.FindName("btnOpen")   :?> MenuItem
    let btnSave   = window.FindName("btnSave")   :?> MenuItem
    let btnSaveAs = window.FindName("btnSaveAs") :?> MenuItem
    let btnExit   = window.FindName("btnExit")   :?> MenuItem
    let txtMain   = window.FindName("txtMain")   :?> TextBox

    btnNew.Click.Add (fun _ -> txtMain.Text <- String.Empty; currentFile <- None)
    btnOpen.Click.Add(fun _ -> openFileWithDialog txtMain)
    btnSave.Click.Add(fun _ -> match currentFile with 
                                | None      -> saveFileWithDialog txtMain 
                                | Path path -> saveFile txtMain path)
    btnSaveAs.Click.Add(fun _ -> saveFileWithDialog txtMain)
    btnExit.Click.Add(fun _ -> window.Close())

    window

[<EntryPoint>] [<STAThread>]
let main(_) = (new Application()).Run(Editor)