implement main
    open core, console

class predicates
    readList : (integer N, integer* List [out]).
    printList : (integer* List).
    union : (integer* A, integer* B, integer* C [out]).
    remove : (integer* A, integer* B, integer* C [out]).
    removeElement : (integer A, integer* B, integer* C [out]).
    intersection : (integer* A, integer* B, integer* C [out]).
    member : (integer A, integer* B) determ.

clauses
    run() :-
        init(),
        write("Let's prove De Morgan's law for the union: -(A U B)=(-A)^(-B) (all sets consist\n"),
        write("of natural numbers from 1 to 15 for the optimal search for the complement)\n"),
        write("Enter the number of list elements (natural number from 1 to 15) for set A: \n"),
        Nstring = readLine(),
        N = toTerm(integer, Nstring),
        write("Enter the list items (natural numbers from 1 to 15 without repeating elements):\n"),
        readList(N, List),
        write("Set A: [ "),
        printList(List),
        nl,
        write("Enter the number of list elements (natural number from 1 to 15) for set B:\n"),
        Mstring = readLine(),
        M = toTerm(integer, Mstring),
        write("Enter the list items (natural numbers from 1 to 15 without repeating elements):\n"),
        readList(M, List2),
        write("Set B: [ "),
        printList(List2),
        nl,
        union(List, List2, Z),
        write("1) A U B = [ "),
        printList(Z),
        nl,
        Universal = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15],
        remove(Z, Universal, ComplementAB),
        write("2) - (A U B) = [ "),
        printList(ComplementAB),
        nl,
        remove(List, Universal, ComplementA),
        write("3) - A = [ "),
        printList(ComplementA),
        nl,
        remove(List2, Universal, ComplementB),
        write("4) - B = [ "),
        printList(ComplementB),
        nl,
        intersection(ComplementA, ComplementB, Intersect),
        write("5) (-A) ^ (-B) = [ "),
        printList(Intersect),
        nl,
        if ComplementAB = Intersect then
            write("- (A U B) = (-A) ^ (-B) = [ "),
            printList(ComplementAB),
            nl,
            write("The theorem is proven!")
        else
            write("The theorem is not proven")
        end if,
        _ = readLine().

    readList(0, []) :-
        !.
    readList(N, [H | T]) :-
        Hstring = readLine(),
        H = toTerm(integer, Hstring),
        N1 = N - 1,
        readList(N1, T).

    printList([]) :-
        !,
        write("] ").
    printList([H]) :-
        !,
        write(H),
        write("] ").
    printList([H | T]) :-
        write(H),
        write(", "),
        printList(T).

    member(X, [X | _]) :-
        !.
    member(X, [_ | T]) :-
        member(X, T).

    union([], M, M).
    union([H | T], M, R) :-
        not(member(H, M)),
        !,
        union(T, [H | M], R).
    union([_ | T], M, R) :-
        union(T, M, R).

    remove([], B, B).
    remove([H | T], B, R) :-
        removeElement(H, B, B1),
        remove(T, B1, R).

    removeElement(_, [], []).
    removeElement(H, [H | T], R) :-
        !,
        removeElement(H, T, R).
    removeElement(H, [X | T], [X | R]) :-
        removeElement(H, T, R).

    intersection([], _, []).
    intersection([H | T], B, [H | R]) :-
        member(H, B),
        !,
        intersection(T, B, R).
    intersection([_ | T], B, R) :-
        intersection(T, B, R).

end implement main

goal
    console::runUtf8(main::run).
