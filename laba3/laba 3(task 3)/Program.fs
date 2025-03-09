open System
open System.IO

[<EntryPoint>]
let main argv =
    let rec getDirectoryPath () =
        printf "Введите путь к каталогу: "
        let directory = Console.ReadLine()
        if Directory.Exists(directory) then
            directory
        else
            printfn "Каталог '%s' не существует. Попробуйте снова." directory
            getDirectoryPath ()

    let directory = getDirectoryPath ()
    let rec getFiles d =
        seq {
            yield! Directory.EnumerateFiles(d)
        }
    let files = lazy (getFiles directory)
    let sortedFiles = files.Value |> Seq.sort
    match Seq.tryHead sortedFiles with
    | Some firstFile ->
        printfn "Первый файл по алфавиту: %s" (Path.GetFileName firstFile)
    | None ->
        printfn "В каталоге '%s' нет файлов." directory

    0