using System.Drawing;
using System.Text;

public class LexicalAnalyzer
{
    private InputOutput io;
    public static List<(int Code, int Line)> symbolCodes { get; private set; } = new();
    private static int MaxInt = 32767;
    public const int CharConstCode = 84;
    public const int IntConstCode = 15;
    public const int FloatConstCode = 16;
    public const int Identificator = 2;
    public static Dictionary<string, int> Keywords = new()
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
        { "with", 37 },
        {"boolean", 150 }
    };

    public static Dictionary<char, int> singleSymbols = new Dictionary<char, int>
        {
            { '+', 70 }, { '-', 71 }, { '*', 21 }, { '/', 60 },
            { '=', 16 }, { ',', 20 }, { ';', 14 }, { ':', 5 },
            { '.', 61 }, { '(', 9 }, { ')', 4 }, { '[', 11 }, { ']', 12 },
            { '{', 63 }, { '}', 64 }, { '<', 65 }, { '>', 66 }, { '^', 62 }
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
           /* else if (ch == '(')
            {
                CheckPairedSymbol1();
            }*/
            else if (ch == '[')
            {
                CheckPairedSymbol2();
            }
            else if (ch == '{')
            {
                CheckPairedSymbol3();
            }
            else
            {
                ProcessSymbol();
            }
        }
        Console.WriteLine("Коды символов:");
        var codes = new StringBuilder();
        foreach (var (code, _) in symbolCodes)  // "_" означает, что номер строки не используется
        {
            codes.Append(code).Append(' ');
        }
        Console.WriteLine(codes.ToString().Trim());  // Trim() убирает лишний пробел в конце
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
        int code = Keywords.TryGetValue(name, out int keywordCode) ? keywordCode : 2;
        symbolCodes.Add((code, pos.LineNumber));
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
                    symbolCodes.Add((FloatConstCode, pos.LineNumber));
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
                symbolCodes.Add((FloatConstCode, pos.LineNumber));
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
            symbolCodes.Add((isReal ? FloatConstCode : IntConstCode, pos.LineNumber));
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

            symbolCodes.Add((FloatConstCode, pos.LineNumber));
        }
        else
        {
            if (!int.TryParse(numberStr, out int intVal) || intVal > MaxInt)
            {
                io.Error(203, pos);
            }
            symbolCodes.Add((IntConstCode, pos.LineNumber));
        }
    }

    private void ProcessCharConst()
    {
        TextPosition pos = io.CurrentPosition;
        io.NextChar();
        bool foundClosingQuote = false;
        bool errorAlreadyReported = false;
        if (io.CurrentChar == '\'')
        {
            io.Error(206, pos);
            io.NextChar();
            symbolCodes.Add((CharConstCode, pos.LineNumber));
            return;
        }
        while (io.CurrentChar != '\0' && io.CurrentChar != '\n')
        {
            if (io.CurrentChar == '\'')
            {
                foundClosingQuote = true;
                io.NextChar(); // пропускаем закрывающую кавычку
                break;
            }

            if ((io.CurrentChar == ';' || io.CurrentChar == ')') && !foundClosingQuote)
            {
                io.Error(208, pos);
                errorAlreadyReported = true;
                break;
            }

            io.NextChar();
        }

        if (!foundClosingQuote && !errorAlreadyReported)
        {
            io.Error(208, pos);
        }

        symbolCodes.Add((CharConstCode, pos.LineNumber));
    }

    private void CheckPairedSymbol1()
    {
        TextPosition pos = io.CurrentPosition;
        io.NextChar();
        bool foundClosingQuote = false;
        bool errorAlreadyReported = false;

        while (io.CurrentChar != '\0' && io.CurrentChar != '\n')
        {
            if (io.CurrentChar == ')')
            {
                foundClosingQuote = true;
                io.NextChar(); // пропускаем закрывающую кавычку
                break;
            }

            if (io.CurrentChar == ';' && !foundClosingQuote)
            {
                io.Error(208, pos);
                errorAlreadyReported = true;
                break;
            }

            io.NextChar();
        }

        if (!foundClosingQuote && !errorAlreadyReported)
        {
            io.Error(208, pos);
        }

        symbolCodes.Add((CharConstCode, pos.LineNumber));
    }

    private void CheckPairedSymbol2()
    {
        TextPosition pos = io.CurrentPosition;
        io.NextChar();
        bool foundClosingQuote = false;
        bool errorAlreadyReported = false;

        while (io.CurrentChar != '\0' && io.CurrentChar != '\n')
        {
            if (io.CurrentChar == ']')
            {
                foundClosingQuote = true;
                io.NextChar(); // пропускаем закрывающую кавычку
                break;
            }

            if (io.CurrentChar == ';' && !foundClosingQuote)
            {
                io.Error(208, pos);
                errorAlreadyReported = true;
                break;
            }

            io.NextChar();
        }

        if (!foundClosingQuote && !errorAlreadyReported)
        {
            io.Error(208, pos);
        }

        symbolCodes.Add((CharConstCode, pos.LineNumber));
    }

    private void CheckPairedSymbol3()
    {
        TextPosition pos = io.CurrentPosition;
        io.NextChar(); // пропускаем '{'

        while (io.CurrentChar != '\0')
        {
            if (io.CurrentChar == '}')
            {
                io.NextChar(); // пропускаем '}'
                return;
            }
            io.NextChar();
        }

        // Закрывающая фигурная скобка не найдена до конца файла
        Console.WriteLine("Ошибка: дошли до конца файла без закрывающей фигурной скобки");
        io.Error(208, pos);
    }


    private void ProcessSymbol()
    {
        char ch = io.CurrentChar;
        TextPosition pos = io.CurrentPosition;
        switch (ch)
        {
            case ':':
                io.NextChar();
                if (io.CurrentChar == '=')
                {
                    symbolCodes.Add((51, pos.LineNumber)); io.NextChar(); return;
                }
                else
                {
                    symbolCodes.Add((5, pos.LineNumber)); io.NextChar(); return;
                }
            case '<':
                io.NextChar();
                if (io.CurrentChar == '=')
                {
                    symbolCodes.Add((67, pos.LineNumber)); io.NextChar(); return;
                }
                else if (io.CurrentChar == '>')
                {
                    symbolCodes.Add((69, pos.LineNumber)); io.NextChar(); return;
                }
                else
                {
                    symbolCodes.Add((65, pos.LineNumber)); io.NextChar(); return;
                }
            case '>':
                io.NextChar();
                if (io.CurrentChar == '=')
                {
                    symbolCodes.Add((68, pos.LineNumber)); io.NextChar(); return;
                }
                else
                {
                    symbolCodes.Add((66, pos.LineNumber)); io.NextChar(); return;
                }
            case '.':
                io.NextChar();
                if (io.CurrentChar == '.')
                {
                    symbolCodes.Add((74, pos.LineNumber)); io.NextChar(); return;
                }
                else
                {
                    symbolCodes.Add((61, pos.LineNumber)); io.NextChar(); return;
                }

        }

        if (singleSymbols.ContainsKey(ch))
        {
            symbolCodes.Add((singleSymbols[ch], pos.LineNumber));
            io.NextChar();
        }
        else
        {
            io.Error(6, pos);
            io.NextChar();
        }
    }
}