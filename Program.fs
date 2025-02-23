open System
let rec readNatural () =
    printf "Введите натуральное число: "
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n > 0 -> n
    | (true, _) ->
        printfn "Ошибка: число должно быть натуральным (больше нуля). Попробуйте снова."
        readNatural ()
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readNatural ()

let Count n =
    let rec kol (n: int) (a: int) =
        if n = 0 then a
        else kol (n / 10) (a + 1)
    kol n 0

let number = readNatural ()
let result = Count number
printfn "Количество цифр в данном натуральном числе: %d" result