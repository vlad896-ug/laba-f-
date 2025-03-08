open System
open System.IO

[<EntryPoint>]
let main argv =
    let directory = "E:\\labafile"
    let rec getFiles d =
        seq {
            yield! Directory.EnumerateFiles(d) 
        }
    let files = getFiles directory
    let sortedFiles = files |> Seq.sort
    match Seq.tryHead sortedFiles with
    | Some firstFile ->
        printfn "Первый файл по алфавиту: %s" (Path.GetFileName firstFile)
    | None ->
        printfn "В каталоге '%s' нет файлов." directory
    0 