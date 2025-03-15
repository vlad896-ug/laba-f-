open System

type BinaryTree =
    | Empty
    | Node of string * BinaryTree * BinaryTree

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

let rec depth tree =
    match tree with
    | Empty -> 0
    | Node(_, left, right) -> 1 + max (depth left) (depth right)

let rec sravn (x: string) (y: string) (n: int) =
    if n >= x.Length && n >= y.Length then
        false
    elif n >= x.Length then
        true 
    elif n >= y.Length then
        false 
    elif x[n] < y[n] then
        true
    elif x[n] > y[n] then
        false
    else
        sravn x y (n + 1)

let rec insert value tree =
    match tree with
    | Empty -> Node(value, Empty, Empty)
    | Node(rootValue, left, right) ->
        if sravn value rootValue 0 then
            Node(rootValue, insert value left, right)
        else
            Node(rootValue, left, insert value right)

let readString prompt =
    printfn "%s" prompt
    System.Console.ReadLine()

let rec buildTree count tree =
    match count with
    | 0 -> tree
    | _ ->
        let value = readString "Введите следующий элемент дерева (строку):"
        buildTree (count - 1) (insert value tree)

let rec printTree (tree: BinaryTree) x y spacing =
    match tree with
    | Empty -> ()
    | Node(value, left, right) ->
        try
            System.Console.SetCursorPosition(x, y)
            printf "%s" value
        with
        | :? ArgumentOutOfRangeException ->
            ()

        let leftX = x - spacing
        let rightX = x + spacing
        let nextSpacing = max 1 (spacing / 2)
        if left <> Empty then
            try
                System.Console.SetCursorPosition(x - 1, y + 1)
                printf "/"
            with
            | :? ArgumentOutOfRangeException -> ()
            printTree left leftX (y + 2) nextSpacing

        if right <> Empty then
            try
                System.Console.SetCursorPosition(x + value.Length, y + 1)
                printf "\\"
            with
            | :? ArgumentOutOfRangeException -> ()
            printTree right rightX (y + 2) nextSpacing

let nextChar (c: char) : char =
    char (int c + 1)

let nextString (s: string) : string =
    s |> Seq.map nextChar |> Seq.toArray |> String

let rec mapTree (f: 'a -> 'b) (tree: BinaryTree) : BinaryTree =
    match tree with
    | Empty -> Empty
    | Node(value, left, right) ->
        Node(f value, mapTree f left, mapTree f right)

[<EntryPoint>]
let main argv =
    let count = readNatural "Введите количество элементов дерева: "
    let rootValue = readString "Введите корень дерева (строку):"
    let initialTree = Node(rootValue, Empty, Empty)
    let tree = buildTree (count - 1) initialTree
    let treeDepth = depth tree
    let consoleWidth = 
        try System.Console.WindowWidth 
        with _ -> 80
    let initialX = consoleWidth / 2
    let initialY = System.Console.CursorTop + 1
    let baseSpacing = 4
    let initialSpacing = baseSpacing * pown 2 (treeDepth - 2 |> max 0)
    printfn "\nИсходное дерево:"
    printTree tree initialX initialY (max 1 initialSpacing)
    try
        System.Console.SetCursorPosition(0, initialY + 2 * treeDepth)
    with
    | :? ArgumentOutOfRangeException ->
        printfn ""
    let newTree = mapTree nextString tree
    let newTreeDepth = depth newTree
    printfn "\nДерево после замены каждого символа на следующий:"
    let newY = System.Console.CursorTop + 1
    let newSpacing = baseSpacing * pown 2 (newTreeDepth - 2 |> max 0)
    printTree newTree initialX newY (max 1 newSpacing)
    try
        System.Console.SetCursorPosition(0, newY + 2 * newTreeDepth)
    with
    | :? ArgumentOutOfRangeException ->
        printfn ""
    0