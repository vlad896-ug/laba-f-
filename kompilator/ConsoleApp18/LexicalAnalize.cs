using System.Text;

public class LexicalAnalyzer
{
    private InputOutput io;
    private List<int> symbolCodes = new();
    private static int MaxInt = 32767;
    private const int CharConstCode = 84;
    private const int IntConstCode = 15;
    private const int FloatConstCode = 16;
    private static Dictionary<string, int> Keywords = new()
    {
        { "program", 122 },
        { "var", 105 },
        { "const", 116 },
        { "begin", 113 },
        { "end", 104 },
        { "if", 56 },
        { "then", 52 },
        { "else", 32 },
        { "while", 114 },
        { "do", 54 },
        { "array", 115 },
        { "of", 101 },
        { "to", 103 },
        { "real", 13 },
        { "integer", 17 },
        { "char", 18 },
        { "in", 100 },
        { "or", 102 },
        { "div", 106 },
        { "and", 107 },
        { "not", 108 },
        { "for", 109 },
        { "mod", 110 },
        { "nil", 111 },
        { "set", 112 },
        { "label", 117 },
        { "downto", 118 },
        { "packed", 119 },
        { "record", 120 },
        { "repeat", 121 },
        { "function", 123 },
        { "procedure", 124 },
        { "case", 31 },
        { "file", 57 },
        { "goto", 33 },
        { "type", 34 },
        { "until", 53 },
        { "with", 37 }
    };

    public LexicalAnalyzer(InputOutput io)
    {
        this.io = io;
    }
    public void Analyze()
    {
        while (io.CurrentChar != '\0')
        {
            char ch = io.CurrentChar;

            if (char.IsWhiteSpace(ch))
            {
                io.NextChar();
                continue;
            }

            if (char.IsLetter(ch))
            {
                ProcessIdentifier();
            }
            else if (char.IsDigit(ch))
            {
                ProcessNumber();
            }
            else if (ch == '\'')
            {
                ProcessCharConst();
            }
            else
            {
                ProcessSymbol();
            }
        }
        Console.WriteLine("Коды символов:");
        Console.WriteLine(string.Join(" ", symbolCodes));
    }

    private void ProcessIdentifier()
    {
        StringBuilder sb = new();
        TextPosition pos = io.CurrentPosition;
        while (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '_')
        {
            sb.Append(io.CurrentChar);
            io.NextChar();
        }
        string name = sb.ToString().ToLower();
        bool isKeyword = Keywords.ContainsKey(name);
        int code = isKeyword ? Keywords[name] : 2;
        if (char.IsWhiteSpace(io.CurrentChar)&&!isKeyword)
        {
            int savedLine = io.CurrentPosition.LineNumber;
            int savedChar = io.CurrentPosition.CharNumber;
            io.NextChar();
            if (char.IsLetter(io.CurrentChar))
            {
                while (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '_')
                {
                    sb.Append(io.CurrentChar);
                    io.NextChar();
                }
                io.Error(207, pos);
                io.CurrentPosition = new TextPosition(savedLine, savedChar + 1);
                io.CurrentChar = ' ';
                return;
            }
            else
            {
                io.CurrentPosition = new TextPosition(savedLine, savedChar);
            }
        }
        symbolCodes.Add(code);
    }

    private void ProcessNumber()
    {
        StringBuilder numberBuilder = new();
        TextPosition pos = io.CurrentPosition;
        while (char.IsDigit(io.CurrentChar))
        {
            numberBuilder.Append(io.CurrentChar);
            io.NextChar();
        }
        bool isReal = false;
        if (io.CurrentChar == '.')
        {
            if (io.CurrentLine.Length > io.CurrentPosition.CharNumber &&
                io.CurrentLine[io.CurrentPosition.CharNumber] == '.')
            {
                // Это оператор ".." — не часть числа
            }
            else
            {
                isReal = true;
                numberBuilder.Append('.');
                io.NextChar();
                if (!char.IsDigit(io.CurrentChar))
                {
                    io.Error(205, pos);
                    while (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '_' || io.CurrentChar == '+' || io.CurrentChar == '-')
                    {
                        io.NextChar();
                    }
                    symbolCodes.Add(FloatConstCode);
                    return;
                }
                while (char.IsDigit(io.CurrentChar))
                {
                    numberBuilder.Append(io.CurrentChar);
                    io.NextChar();
                }
            }
        }

        if (char.ToLower(io.CurrentChar) == 'e')
        {
            isReal = true;
            numberBuilder.Append(io.CurrentChar);
            io.NextChar();
            if (io.CurrentChar == '+' || io.CurrentChar == '-')
            {
                numberBuilder.Append(io.CurrentChar);
                io.NextChar();
            }
            if (!char.IsDigit(io.CurrentChar))
            {
                io.Error(205, pos);
                while (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '_' || io.CurrentChar == '+' || io.CurrentChar == '-')
                {
                    io.NextChar();
                }
                symbolCodes.Add(FloatConstCode);
                return;
            }
            while (char.IsDigit(io.CurrentChar))
            {
                numberBuilder.Append(io.CurrentChar);
                io.NextChar();
            }
        }

        if (char.IsLetter(io.CurrentChar) || io.CurrentChar == '_')
        {
            io.Error(204, pos);
            while (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '_')
            {
                io.NextChar();
            }
            symbolCodes.Add(isReal ? FloatConstCode : IntConstCode);
            return;
        }

        string numberStr = numberBuilder.ToString();

        if (isReal)
        {
            if (!double.TryParse(numberStr, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out double realVal))
            {
                io.Error(205, pos);
            }
            else if (double.IsInfinity(realVal))
            {
                io.Error(209, pos);
            }

            symbolCodes.Add(FloatConstCode);
        }
        else
        {
            if (!int.TryParse(numberStr, out int intVal) || intVal > MaxInt)
            {
                io.Error(203, pos);
            }
            symbolCodes.Add(IntConstCode);
        }
    }

    private void ProcessCharConst()
    {
        TextPosition pos = io.CurrentPosition;
        io.NextChar();
        if (io.CurrentChar == '\'')
        {
            io.Error(206, pos);
            io.NextChar();
            symbolCodes.Add(CharConstCode);
            return;
        }
        if (io.CurrentChar == '\0' || io.CurrentChar == '\n')
        {
            io.Error(208, pos); 
            return;
        }
        io.NextChar();
        if (io.CurrentChar == '\'')
        {
            io.NextChar();
            symbolCodes.Add(CharConstCode);
            return;
        }
        if (char.IsLetterOrDigit(io.CurrentChar) || io.CurrentChar == '\'')
        {
            io.Error(210, pos); 
        }
        else
        {
            io.Error(208, pos); 
        }
        while (io.CurrentChar != '\0' && io.CurrentChar != '\'' && io.CurrentChar != '\n')
        {
            io.NextChar();
        }
        if (io.CurrentChar == '\'')
        {
            io.NextChar(); 
        }
        symbolCodes.Add(CharConstCode);
    }

    private void ProcessSymbol()
    {
        char ch = io.CurrentChar;
        TextPosition pos = io.CurrentPosition;
        char next = io.PeekChar();
        string next2 = $"{ch}{next}";
        switch (next2)
        {
            case ":=":
                symbolCodes.Add(51); io.NextChar(); io.NextChar(); io.NextChar(); return;
            case "..":
                symbolCodes.Add(74); io.NextChar(); io.NextChar(); io.NextChar(); return;
            case "<=":
                symbolCodes.Add(67); io.NextChar(); io.NextChar(); io.NextChar(); return;
            case "<>":
                symbolCodes.Add(69); io.NextChar(); io.NextChar(); io.NextChar(); return;
            case ">=":
                symbolCodes.Add(68); io.NextChar(); io.NextChar(); io.NextChar(); return;
        }
        Dictionary<char, int> singleSymbols = new Dictionary<char, int>
        {
            { '+', 70 }, { '-', 71 }, { '*', 21 }, { '/', 60 },
            { '=', 16 }, { ',', 20 }, { ';', 14 }, { ':', 5 },
            { '.', 61 }, { '(', 9 }, { ')', 4 }, { '[', 11 }, { ']', 12 },
            { '{', 63 }, { '}', 64 }, { '<', 65 }, { '>', 66 }, { '^', 62 }
        };
        if (singleSymbols.ContainsKey(ch))
        {
            symbolCodes.Add(singleSymbols[ch]);
            io.NextChar();
        }
        else
        {
            io.Error(6, pos);
            io.NextChar();
        }
    }
}
