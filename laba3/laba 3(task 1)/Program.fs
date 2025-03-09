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

let maxDigit n =
    let rec loop n max =
        if n = 0 then max
        else
            let digit = n % 10
            let newMax = if digit > max then digit else max
            loop (n / 10) newMax
    loop n 0

let createSequence () =
    seq {
        let text = "Введите количество элементов для добавления в последовательность: "
        let number = readNatural text
        if number > 0 then
            for i in 1 .. number do
                let prompt = sprintf "Введите %d-й элемент исходной последовательности: " i
                let element = readNatural prompt
                yield element 
    }

let main () =
    let lazySequence = Lazy<int seq>.Create (fun () -> createSequence ())
    let maxDigitsSequence = lazySequence.Value |> Seq.map maxDigit
    printfn "Последовательность из максимальных цифр (вычисляется после ввода каждого элемента исходной последовательности, лениво)"
    maxDigitsSequence |> Seq.iter (printfn "Элемент новой последовательности: %d")

main ()