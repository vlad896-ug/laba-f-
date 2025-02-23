open System
let rec readNatural () =
    printf "Введите количество элементов списка: "
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n > 0 -> n
    | (true, _) ->
        printfn "Ошибка: число должно быть натуральным (больше нуля). Попробуйте снова."
        readNatural ()
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readNatural ()

let rec readDouble index =
    printf "Введите %d-й элемент списка (дробная часть вводится через ','(если она есть)): " index
    let input = Console.ReadLine()
    match Double.TryParse(input) with
    | (true, n) -> n
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readDouble index

let generateList n =
    let rec loop i list =
        if i > n then
            list
        else
            let number = readDouble i
            let numberToInsert = if number = 0.0 then 0.0 else -number
            loop (i + 1) (list @ [numberToInsert])
    loop 1 []

let number = readNatural ()
let list1 = generateList number
printfn "Список из чисел, противоположных вводимым значениям: %A" list1
