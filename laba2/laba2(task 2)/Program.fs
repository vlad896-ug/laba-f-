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
        printf "Введите %d-й элемент списка: " (N - n+1)
        let h = Console.ReadLine()
        loop (n - 1) (add h a) N

let RandomStrings (count: int) (maxLength: int) : string list =
    let random = Random()
    let chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:',.<>?/`~ \"'\\" 
    
    let RandomString (length: int) =
        let randomChars = 
            [| for _ in 1..length -> chars.[random.Next(chars.Length)] |]
        String(randomChars)
    
    [ for _ in 1..count -> 
        let length = random.Next(1, maxLength + 1) 
        RandomString length ]

let rec ListCreate list1 =
    printfn "Выберите действие:"
    printfn "1 - Создать список вводом с клавиатуры"
    printfn "2 - Сгенерировать список рандомно"
    printf "Ваш выбор: "
    let a = Console.ReadLine()
    let text = "Введите количество элементов для добавления в список: "
    let text1 = "Введите максимальную длину строки в списке: "
    match a with
    | "1" -> 
        let number = readNatural text
        let newList1 = loop number list1 number
        newList1
    | "2" -> 
        let number = readNatural text
        let max = readNatural text1
        let newList1 = RandomStrings number max
        newList1
    | _ -> 
        printfn "Неверный выбор. Попробуйте снова."
        ListCreate list1

let list1 = []
let list = ListCreate list1
printfn "Исходный список: %A" list

let totalLength = List.fold (fun acc (str: string) -> acc + str.Length) 0 list
printfn "Суммарная длина строк в списке: %d" totalLength
