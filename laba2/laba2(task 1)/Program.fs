open System

let rec readNatural (text : string) =
    printf "%s" text
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n > 0 -> n
    | (true, _) ->
        printfn "Ошибка: число должно быть натуральным (больше нуля). Попробуйте снова."
        readNatural text
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readNatural text

let rec add x list =
    match list with
    | [] -> [x]
    | head :: t -> head :: (add x t)

let rec loop n a N =
    if n = 0 then a
    else
        let rec readElement i =
            let prompt = sprintf "Введите %d-й элемент списка: " i
            readNatural prompt
        let h = readElement (N - n + 1)
        loop (n - 1) (add h a) N

let Random x min max =      
    let r = new Random()
    [ for i in 1 .. x -> r.Next(min, max + 1) ]

let rec ListCreate list1 =
    printfn "Выберите действие:"
    printfn "1 - Создать список вводом с клавиатуры"
    printfn "2 - Сгенерировать список рандомно"
    printf "Ваш выбор: "
    let a = Console.ReadLine()
    let text = "Введите количество элементов для добавления в список: "
    let text1 = "Введите нижнюю границу диапазона списка: "
    let text2 = "Введите верхнюю границу диапазона списка: "
    match a with
    | "1" -> 
        let number = readNatural text
        let newList1 = loop number list1 number
        newList1
    | "2" -> 
        let number = readNatural text
        let min = readNatural text1
        let max = readNatural text2
        let newList1 = Random number min max
        newList1
    | _ -> 
        printfn "Неверный выбор. Попробуйте снова."
        ListCreate list1

let maxDigit n =
    let rec loop n max =
        if n = 0 then max
        else
            let digit = n % 10
            let newMax = 
                if digit > max then digit 
                else max
            loop (n / 10) newMax
    loop n 0

let list1 = []
let list = ListCreate list1
printfn "Исходный список: %A" list
let maxDigitsList = List.map maxDigit list
printfn "Список из максимальных цифр: %A" maxDigitsList