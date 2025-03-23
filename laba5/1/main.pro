% Copyright

implement main
    open core, console

class predicates
    divisors1 : (integer N).
    divisors : (integer N, integer C).

clauses
    run() :-
        init(),
        write("Enter the natural number n: "),
        nl,
        Nstring = readLine(),
        N = toTerm(integer, Nstring),
        nl,
        writef("Divisors of the number %:\n ", N),
        divisors1(N),
        nl,
        _ = readLine().

    divisors1(N) :-
        divisors(N, 1).

    divisors(N, C) :-
        C > N,
        !.

    divisors(N, C) :-
        N mod C = 0,
        !,
        if C < N then
            writef("%, ", C)
        else
            writef("%. ", C)
        end if,
        Next = C + 1,
        divisors(N, Next).

    divisors(N, C) :-
        Next = C + 1,
        divisors(N, Next).

end implement main

goal
    console::runUtf8(main::run).
