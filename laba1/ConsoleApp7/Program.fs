open System
let rec readNatural () =
    printf "Введите количество элементов для добавления в список: "
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n > 0 -> n
    | (true, _) ->
        printfn "Ошибка: число должно быть натуральным (больше нуля). Попробуйте снова."
        readNatural ()
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readNatural ()

let rec readPolo () =
    printf "Введите индекс элемента из списка: "
    let input = Console.ReadLine()
    match Int32.TryParse(input) with
    | (true, n) when n >= 0 -> n
    | (true, _) ->
        printfn "Ошибка: число должно быть целое, больше или равно нулю. Попробуйте снова."
        readPolo ()
    | (false, _) ->
        printfn "Ошибка: введено не число. Попробуйте снова."
        readPolo ()

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

let rec remove x list =
    match list with
    | [] -> []
    | head :: t ->
        if head = x then
            t
        else
            head :: (remove x t)

let rec find x list =
    match list with
    | [] -> None
    | head :: t ->
        if head = x then
            Some head
        else
            find x t

let rec concatLists list1 list2 =
    match list1 with
    | [] -> list2
    | head :: t -> head :: concatLists t list2

let rec concatList1 list1 list2 =
    let newList1 = concatLists list1 list2
    printfn "Список 1 после сцепки: %A" newList1
    newList1

let rec concatList2 list1 list2 =
    let newList2 = concatLists list2 list1
    printfn "Список 2 после сцепки: %A" newList2
    newList2

let rec element index list =
    match list with
    | [] -> None
    | head :: t ->
        if index = 0 then
            Some head
        else
            element (index - 1) t

let rec mainMenu list1 list2 =
    printfn "Выберите действие:"
    printfn "1 - Добавить элементы в список 1"
    printfn "2 - Добавить элементы в список 2"
    printfn "3 - Удалить элемент из списка 1"
    printfn "4 - Удалить элемент из списка 2"
    printfn "5 - Поиск элемента в списке 1"
    printfn "6 - Поиск элемента в списке 2"
    printfn "7 - Сцепить два списка (list1 + list2 -> list1)"
    printfn "8 - Сцепить два списка (list2 + list1 -> list2)"
    printfn "9 - Получить элемент по номеру из списка 1"
    printfn "10 - Получить элемент по номеру из списка 2"
    printfn "11 - Вывести два списка"
    printfn "12 - Завершить программу"
    printf "Ваш выбор: "
    let a = Console.ReadLine()
    match a with
    |"1" -> 
        let number = readNatural ()
        let newList1 = loop number list1 number
        printfn "Итоговый список 1: %A" newList1
        mainMenu newList1 list2
    |"2" -> 
        let number = readNatural ()
        let newList2 = loop number list2 number
        printfn "Итоговый список 2: %A" newList2
        mainMenu list1 newList2
    |"3" -> 
        printf "Введите элемент для удаления из первого списка: "
        let remove1 = Console.ReadLine() 
        let newList1 = remove remove1 list1
        printfn "Первый список после удаления: %A" newList1
        mainMenu newList1 list2
    |"4" -> 
        printf "Введите элемент для удаления из второго списка: "
        let remove2 = Console.ReadLine() 
        let newList2 = remove remove2 list2
        printfn "Второй список после удаления: %A" newList2
        mainMenu list1 newList2
    |"5" -> 
        printf "Введите элемент для поиска в первом списке: "
        let search1 = Console.ReadLine()  
        match find search1 list1 with
        | Some value -> printfn "Элемент %A найден в списке 1." value
        | None -> printfn "Элемент %A не найден в списке 1." search1
        mainMenu list1 list2
    |"6" -> 
        printf "Введите элемент для поиска во втором списке: "
        let search2 = Console.ReadLine() 
        match find search2 list2 with
        | Some value -> printfn "Элемент %A найден в списке 2." value
        | None -> printfn "Элемент %A не найден в списке 2." search2
        mainMenu list1 list2
    |"7" -> 
        let newList1 = concatList1 list1 list2
        mainMenu newList1 list2
    |"8" -> 
        let newList2 = concatList2 list1 list2
        mainMenu list1 newList2
    |"9" -> 
        let index = readPolo ()
        match element index list1 with
        | Some value -> printfn "Элемент с индексом %d в списке 1: %A" index value
        | None -> printfn "Элемент с индексом %d не найден в списке 1." index
        mainMenu list1 list2
    |"10" -> 
        let index = readPolo ()
        match element index list2 with
        | Some value -> printfn "Элемент с индексом %d в списке 2: %A" index value
        | None -> printfn "Элемент с индексом %d не найден в списке 2." index
        mainMenu list1 list2
    |"11" -> 
        printfn "Список 1: %A" list1
        printfn "Список 2: %A" list2
        mainMenu list1 list2
    |"12" ->
        printfn "Программа завершена."
    | _ -> 
        printfn "Неверный выбор. Попробуйте снова."
        mainMenu list1 list2

[<EntryPoint>]
let main argv =
    mainMenu [] []
    0