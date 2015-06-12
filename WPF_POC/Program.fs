// WPF-Example in F#

open System
open System.IO
open System.Resources
open System.Reflection
open System.Windows
open System.Windows.Controls
open System.Windows.Markup
open System.Xml
open Microsoft.Win32

let NO_FILE = String.Empty
let mutable currentFile = NO_FILE

let getWindow xaml =

    let assembly = Assembly.GetExecutingAssembly()
    let resource = assembly.GetManifestResourceStream(xaml)
    let stream = new StreamReader(resource)
    let text = stream.ReadToEnd()

    XamlReader.Parse(text) :?> Window

let dialog_open = new OpenFileDialog(DefaultExt = ".txt", Filter = "Textdokumente (.txt)|*.txt")
let dialog_save = new SaveFileDialog(DefaultExt = ".txt", Filter = "Textdokumente (.txt)|*.txt")

let saveFile (txt: TextBox) =
    File.WriteAllText(currentFile, txt.Text)

let openFileWithDialog (txt: TextBox) = 
    if dialog_open.ShowDialog().Value then
        txt.Text <- File.ReadAllText(dialog_open.FileName)
        currentFile <- dialog_open.FileName

let saveFileWithDialog (txt: TextBox) =
    if dialog_save.ShowDialog().Value then 
        currentFile <- dialog_save.FileName
        saveFile txt

let Window =
    let temp = getWindow "Window1.xaml"

    let btnNew  = temp.FindName("btnNew")  :?> MenuItem
    let btnOpen = temp.FindName("btnOpen") :?> MenuItem
    let btnSave = temp.FindName("btnSave") :?> MenuItem
    let btnExit = temp.FindName("btnExit") :?> MenuItem

    let txtMain = temp.FindName("txtMain") :?> TextBox

    btnNew.Click.Add (fun _ -> txtMain.Text <- String.Empty; currentFile <- NO_FILE)
    btnOpen.Click.Add(fun _ -> openFileWithDialog txtMain)
    btnSave.Click.Add(fun _ -> if currentFile = NO_FILE then saveFileWithDialog txtMain else saveFile txtMain)
    btnExit.Click.Add(fun _ -> temp.Close())

    temp

let app = new Application()

let main = app.Run(Window) |> ignore

[<STAThread>]
do main