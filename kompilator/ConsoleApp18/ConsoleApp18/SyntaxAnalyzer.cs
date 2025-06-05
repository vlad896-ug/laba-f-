using System;
using System.Collections.Generic;

public class SyntaxAnalyzer
{
    private readonly List<(int Code, int Line)> tokens;
    private int position = 0;

    public static int ErrorCount { get; private set; } = 0;

    public SyntaxAnalyzer(List<(int Code, int Line)> tokens)
    {
        this.tokens = tokens;
    }

    public void Analyze()
    {
        Expect(LexicalAnalyzer.Keywords["program"], "Программа должна начинаться с ключевого слова 'program'.");
        Expect(LexicalAnalyzer.Identificator, "Ожидается идентификатор после 'program'.");
        Expect(LexicalAnalyzer.singleSymbols[';'], "Ожидается ';' после заголовка программы.");

        if (Peek() == LexicalAnalyzer.Keywords["type"])
        {
            position++;
            if (!Match(LexicalAnalyzer.Identificator)) ReportError(CurrentLine(), "Ожидается идентификатор после 'type'.");
            if (!Match(LexicalAnalyzer.singleSymbols['='])) ReportError(CurrentLine(), "Ожидается '=' после идентификатора типа.");

            if (Peek() == LexicalAnalyzer.singleSymbols['('])
            {
                position++;
                if (Peek() != LexicalAnalyzer.Identificator) ReportError(CurrentLine(), "Ожидается идентификатор.");
                while (Peek() == LexicalAnalyzer.Identificator)
                {
                    position++;
                    if (Peek() == LexicalAnalyzer.singleSymbols[')']) break;
                    if (!Match(LexicalAnalyzer.singleSymbols[','])) ReportError(CurrentLine(), "Ожидается ','.");
                }
                if (!Match(LexicalAnalyzer.singleSymbols[')'])) ReportError(CurrentLine(), "Ожидается ).");
                if (!Match(LexicalAnalyzer.singleSymbols[';'])) ReportError(CurrentLine(), "Ожидается ';' после объявления типа.");
            }
            else if (Peek() == LexicalAnalyzer.Identificator)
            {
                ReportError(CurrentLine(), "Неизвестный тип.");
                while (Peek() != LexicalAnalyzer.singleSymbols[';']) position++;
            }
            else
            {
                if (!Match(LexicalAnalyzer.IntConstCode)) ReportError(CurrentLine(), "Ожидается константа начала диапазона.");
                if (!Match(74)) ReportError(CurrentLine(), "Ожидается '..' в определении диапазона.");
                if (!Match(LexicalAnalyzer.IntConstCode)) ReportError(CurrentLine(), "Ожидается константа конца диапазона.");
                if (!Match(LexicalAnalyzer.singleSymbols[';'])) ReportError(CurrentLine(), "Ожидается ';' после объявления типа.");
            }
        }
        else if (Match(LexicalAnalyzer.Keywords["var"]))
        {
            ParseVarDeclarations();
            Expect(LexicalAnalyzer.Keywords["begin"], "Ожидается 'begin' перед телом программы.");
            ParseStatements();
            if (!Match(LexicalAnalyzer.Keywords["end"])) ReportError(CurrentLine(), "Ожидается 'end'.");
            Expect(LexicalAnalyzer.singleSymbols['.'], "Ожидается '.' в конце программы.");
        }
        else if (Peek() == LexicalAnalyzer.Keywords["begin"])
        {
            Expect(LexicalAnalyzer.Keywords["begin"], "Ожидается 'begin' перед телом программы.");
            ParseStatements();
            if (!Match(LexicalAnalyzer.Keywords["end"])) ReportError(CurrentLine(), "Ожидается 'end'.");
            Expect(LexicalAnalyzer.singleSymbols['.'], "Ожидается '.' в конце программы.");
        }
        else
        {
            Expect(LexicalAnalyzer.Keywords["begin"], "Ожидается 'begin' или 'var' или 'type' перед телом программы.");
        }
    }

    private void ParseVarDeclarations()
    {
        do
        {
            ParseIdentifierList();
            Expect(LexicalAnalyzer.singleSymbols[':'], "Ожидается ':' после списка переменных");

            if (!Match(LexicalAnalyzer.Keywords["integer"]) && !Match(LexicalAnalyzer.Keywords["real"]) && !Match(LexicalAnalyzer.Keywords["char"]) && !Match(LexicalAnalyzer.Keywords["boolean"]))
            {
                ReportError(CurrentLine(), "Ожидается тип переменной (integer, real, char, boolean).");
                position++;
            }

            Expect(LexicalAnalyzer.singleSymbols[';'], "Ожидается ';' после объявления переменных");

        } while (Peek() == LexicalAnalyzer.Identificator);
    }

    private void ParseIdentifierList()
    {
        Expect(LexicalAnalyzer.Identificator, "Ожидается идентификатор переменной");
        while (Match(LexicalAnalyzer.singleSymbols[',']))
        {
            Expect(LexicalAnalyzer.Identificator, "Ожидается идентификатор после запятой");
        }
    }

    private void ParseStatements()
    {
        int prevPosition = -1;

        while (Peek() != LexicalAnalyzer.Keywords["end"])
        {
            if (position == prevPosition)
            {
                ReportError(CurrentLine(), "Невозможно продолжить разбор операторов, пропуск токена.");
                position++;
                if (position >= tokens.Count) break;
            }
            prevPosition = position;
            ParseStatement();
        }
    }

    private void ParseStatement()
    {
        if (position >= tokens.Count) return;

        int startPosition = position;

        if (Peek() == LexicalAnalyzer.Identificator)
        {
            ParseAssignment();
            Match(LexicalAnalyzer.singleSymbols[';']);
        }
        else if (Peek() == LexicalAnalyzer.Keywords["if"])
        {
            int currentLine = CurrentLine();
            position++;
            while (position < tokens.Count && tokens[position].Line == currentLine && tokens[position].Code != LexicalAnalyzer.Keywords["then"])
                position++;

            if (!Match(LexicalAnalyzer.Keywords["then"]))
                ReportError(currentLine, "Ожидается 'then' после условия.");

            ParseStatement();
        }
        else if (Peek() == LexicalAnalyzer.Keywords["for"])
        {
            ParseFor();
        }
        else if (Peek() == LexicalAnalyzer.Keywords["case"])
        {
            int currentLine = CurrentLine();
            position++;
            while (position < tokens.Count && tokens[position].Line == currentLine && tokens[position].Code != LexicalAnalyzer.Keywords["of"])
                position++;

            if (!Match(LexicalAnalyzer.Keywords["of"]))
            {
                ReportError(currentLine, "Ожидается 'of' после условия.");
                while (position < tokens.Count && Peek() != LexicalAnalyzer.Keywords["end"])
                    position++;
            }
            else
            {
                ParseStatement(); 
            }
        }
        else
        {
            position++;
        }

        if (position == startPosition)
            position++;
    }

    private void ParseAssignment()
    {
        Expect(LexicalAnalyzer.Identificator, "Ожидается идентификатор переменной");
        Expect(51, "Ожидается ':=' в операторе присваивания");
        ParseExpression();
    }

    private void ParseExpression()
    {
        bool expectOperand = true;
        int openedParentheses = 0;

        while (position < tokens.Count)
        {
            int code = Peek();

            if (code == LexicalAnalyzer.singleSymbols[';'] || code == LexicalAnalyzer.Keywords["then"] || code == LexicalAnalyzer.singleSymbols['.'] || code == LexicalAnalyzer.Keywords["end"])
                break;

            if (code == LexicalAnalyzer.IntConstCode || code == LexicalAnalyzer.Identificator || code == LexicalAnalyzer.singleSymbols['='])
            {
                if (!expectOperand)
                {
                    ReportError(CurrentLine(), "Ожидался оператор между выражениями.");
                    return;
                }
                ParseFactor();
                expectOperand = false;
            }
            else if (code == LexicalAnalyzer.singleSymbols['-'] || code == LexicalAnalyzer.singleSymbols['+'] || code == LexicalAnalyzer.singleSymbols['*'])
            {
                if (expectOperand)
                {
                    ReportError(CurrentLine(), "Ожидалась константа или скобка после оператора.");
                    return;
                }
                position++;
                expectOperand = true;
            }
            else if (code == LexicalAnalyzer.singleSymbols['('])
            {
                position++;
                openedParentheses++;
                expectOperand = true;
            }
            else if (code == LexicalAnalyzer.singleSymbols[')'])
            {
                if (openedParentheses == 0)
                {
                    ReportError(CurrentLine(), "Лишняя закрывающая скобка.");
                }
                else
                {
                    openedParentheses--;
                }
                position++;
                expectOperand = false;
            }

            else
            {
                ReportError(CurrentLine(), "Недопустимый символ в выражении.");
                position++;
            }
        }

        if (openedParentheses > 0)
        {
            ReportError(CurrentLine(), "Ожидается ')' — не хватает закрывающей скобки.");
        }
    }

    private void ParseFactor()
    {
        int code = Peek();
        if (code == LexicalAnalyzer.IntConstCode || code == LexicalAnalyzer.Identificator || code == LexicalAnalyzer.singleSymbols['='])
        {
            position++;
        }
        else
        {
            ReportError(CurrentLine(), "Ожидается фактор (идентификатор или константа).");
            position++;
        }
    }

    private void ParseFor()
    {
        int currentLine = CurrentLine();
        position++;
        Expect(LexicalAnalyzer.Identificator, "Ожидается переменная цикла после 'for'.");
        Expect(51, "Ожидается ':=' в заголовке цикла for.");
        ParseExpression();

        if (!Match(LexicalAnalyzer.Keywords["to"]))
        {
            ReportError(currentLine, "Ожидается 'to' после выражения.");
            while (position < tokens.Count && tokens[position].Line == currentLine && Peek() != LexicalAnalyzer.singleSymbols[';'])
                position++;
            return;
        }

        ParseExpression();

        if (!Match(LexicalAnalyzer.Keywords["do"]))
        {
            ReportError(currentLine, "Ожидается 'do' после заголовка цикла.");
            while (position < tokens.Count && Peek() != LexicalAnalyzer.singleSymbols[';'])
                position++;
        }

        ParseStatement();
    }

    private bool Match(int expectedCode)
    {
        if (position < tokens.Count && tokens[position].Code == expectedCode)
        {
            position++;
            return true;
        }
        return false;
    }

    private bool Expect(int expectedCode, string errorMessage)
    {
        if (Match(expectedCode)) return true;
        ReportError(CurrentLine(), errorMessage);
        if (position < tokens.Count) position++;
        return false;
    }

    private int Peek()
    {
        if (position < tokens.Count) return tokens[position].Code;
        return -1;
    }

    private int CurrentLine()
    {
        if (position < tokens.Count) return tokens[position].Line;
        else if (tokens.Count > 0) return tokens[^1].Line;
        return -1;
    }

    private void ReportError(int line, string message)
    {
        if (line < 0) line = 0;
        Console.WriteLine($"Ошибка в строке {line}, {message}");
        ErrorCount++; 
    }
}
