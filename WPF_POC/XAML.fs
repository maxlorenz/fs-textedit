module XAML

open System.IO
open System.Reflection
open System.Resources
open System.Windows
open System.Windows.Markup
open System.Xaml

let createFromRessource file =

    let assembly = Assembly.GetExecutingAssembly()
    let resource = assembly.GetManifestResourceStream(file)
    let stream = new StreamReader(resource)
    let text = stream.ReadToEnd()

    XamlReader.Parse(text) :?> Window

