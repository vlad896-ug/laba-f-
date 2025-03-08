open System

let rec readNatural (text: string) =
    printf "%s" text
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n > 0 ->
        if input = n.ToString() then
            n
        else
            printfn "Ошибка: число не должно начинаться с нуля. Попробуйте снова."
            readNatural text
    | (true, _) ->
        printfn "Ошибка: число должно быть натуральным (больше нуля). Попробуйте снова."
        readNatural text
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readNatural text

let createSequence () =
    seq {
        let text = "Введите количество элементов для добавления в последовательность: "
        let number = readNatural text
        if number > 0 then
            for i in 1 .. number do
                let prompt = sprintf "Введите %d-ю строку последовательности: " i
                printf "%s" prompt
                let element = Console.ReadLine()
                yield element 
    }

let main () =
    let sequence = createSequence () //|> Seq.cache
    printfn "Исходная последовательность строк:"
    //sequence |> Seq.iter (printfn "%s")
    let totalLength = sequence |> Seq.fold (fun acc (str: string) -> acc + str.Length) 0
    printfn "Суммарная длина строк в последовательности: %d" totalLength
main ()