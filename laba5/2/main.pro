% Copyright
% Нахождение суммы чётных элементов списка.

implement main
    open core, console

class predicates
    readList : (integer N, integer* List [out]).
    sumEven : (integer* List, integer Sum [out]).
    printList : (integer* List).

clauses
    run() :-
        init(),
        write("Enter the number of list items (natural number): "),
        Nstring = readLine(),
        N = toTerm(integer, Nstring),
        write("Enter the elements of the list of integers:\n"),
        readList(N, List),
        write("List: [ "),
        printList(List), % Выводим список
        nl,
        sumEven(List, Sum),
        write("Sum of even elements: ", Sum),
        nl,
        _ = readLine().

    readList(0, []) :-
        !.
    readList(N, [H | T]) :-
        Hstring = readLine(),
        H = toTerm(integer, Hstring),
        N1 = N - 1,
        readList(N1, T).

    sumEven([], 0) :-
        !.
    sumEven([H | T], Sum) :-
        H mod 2 = 0,
        !,
        sumEven(T, Sum1),
        Sum = Sum1 + H.
    sumEven([_ | T], Sum) :-
        sumEven(T, Sum).

    printList([]) :-
        !.
    printList([H]) :-
        !,
        write(H),
        write("] ").
    printList([H | T]) :-
        write(H),
        write(", "),
        printList(T).

end implement main

goal
    console::runUtf8(main::run).
