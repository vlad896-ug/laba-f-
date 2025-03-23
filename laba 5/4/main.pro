implement main
    open core, console

domains
    role = string.
    person = string.

class facts
    fact1 : (role).
    checkRole : (person, role).

class predicates
    esli : (role, role, role, integer) nondeterm.
    esli1 : (role, role, integer) nondeterm.
    uniqueRoles : (role, role, role, role, role, role, role, role) determ.

clauses
    fact1("Bio"). % Биолог
    fact1("Gid"). % Гидролог
    fact1("Sin"). % Синоптик
    fact1("Rad"). % Радист
    fact1("Mex"). % Механик
    fact1("Vra"). % Врач
    fact1("net").

    checkRole("E", "Bio").
    checkRole("G", "Bio").
    checkRole("B", "Gid").
    checkRole("F", "Gid").
    checkRole("F", "Sin").
    checkRole("G", "Sin").
    checkRole("C", "Rad").
    checkRole("D", "Rad").
    checkRole("C", "Mex").
    checkRole("H", "Mex").
    checkRole("A", "Vra").
    checkRole("D", "Vra").

    uniqueRoles(RoleA, RoleB, RoleC, RoleD, RoleE, RoleF, RoleG, RoleH) :-
        if RoleA = "net" then 0 else 1 end if + if RoleB = "net" then 0 else 1 end if + if RoleC = "net" then 0 else 1 end if
                + if RoleD = "net" then 0 else 1 end if + if RoleE = "net" then 0 else 1 end if + if RoleF = "net" then 0 else 1 end if
                + if RoleG = "net" then 0 else 1 end if + if RoleH = "net" then 0 else 1 end if
            = 6,
        if RoleE = 'Bio' then 1 else 0 end if + if RoleG = 'Bio' then 1 else 0 end if = 1,
        if RoleB = 'Gid' then 1 else 0 end if + if RoleF = 'Gid' then 1 else 0 end if = 1,
        if RoleF = 'Sin' then 1 else 0 end if + if RoleG = 'Sin' then 1 else 0 end if = 1,
        if RoleC = 'Rad' then 1 else 0 end if + if RoleD = 'Rad' then 1 else 0 end if = 1,
        if RoleC = 'Mex' then 1 else 0 end if + if RoleH = 'Mex' then 1 else 0 end if = 1,
        if RoleA = 'Vra' then 1 else 0 end if + if RoleD = 'Vra' then 1 else 0 end if = 1.

    run() :-
        fact1(RoleA),
        fact1(RoleB),
        fact1(RoleC),
        fact1(RoleD),
        fact1(RoleE),
        fact1(RoleF),
        fact1(RoleG),
        fact1(RoleH),
        (checkRole("A", RoleA) or RoleA = "net"),
        (checkRole("B", RoleB) or RoleB = "net"),
        (checkRole("C", RoleC) or RoleC = "net"),
        (checkRole("D", RoleD) or RoleD = "net"),
        (checkRole("E", RoleE) or RoleE = "net"),
        (checkRole("F", RoleF) or RoleF = "net"),
        (checkRole("G", RoleG) or RoleG = "net"),
        (checkRole("H", RoleH) or RoleH = "net"),
        uniqueRoles(RoleA, RoleB, RoleC, RoleD, RoleE, RoleF, RoleG, RoleH),
        esli(RoleD, RoleC, RoleH, 1),
        esli1(RoleF, RoleB, 2),
        esli1(RoleC, RoleG, 3),
        esli1(RoleA, RoleB, 4),
        write("A(", RoleA, "), B(", RoleB, "), C(", RoleC, "), D(", RoleD, "), E(", RoleE, "), F(", RoleF, "), G(", RoleG, "), H(", RoleH, ")"),
        nl,
        fail
        or
        succeed.

    esli("net", "net", "net", _) :-
        !.
    esli(RoleD, RoleC, RoleH, 1) :-
        (RoleD = "net" or not(RoleD = "net") and not(RoleC = "net") and not(RoleH = "net")),
        esli("net", "net", "net", 1).

    esli1(RoleF, RoleB, 2) :-
        (RoleF = "net" or not(RoleF = "net") and not(RoleB = "net")),
        esli("net", "net", "net", 2).

    esli1(RoleC, RoleG, 3) :-
        (RoleC = "net" and not(RoleG = "net") or not(RoleC = "net") and RoleG = "net"),
        esli("net", "net", "net", 3).

    esli1(RoleA, RoleB, 4) :-
        (not(RoleA = "net") and RoleB = "net" or RoleA = "net" and not(RoleB = "net")),
        esli("net", "net", "net", 4).

end implement main

goal
    console::runUtf8(main::run).
