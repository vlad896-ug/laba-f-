using System;
using System.Collections.Generic;
using System.IO;

public class InputOutput
{
    const int ERRMAX = 10;
    private List<string> sourceLines;
    private int currentLineIndex = -1;
    private string currentLine = "";
    private char currentChar;
    private TextPosition positionNow;
    private List<Error> errorList = new();
    private bool errorOverflow = false;
    public char CurrentChar
    {
        get { return currentChar; }
        set { currentChar = value; }
    }
    public TextPosition CurrentPosition
    {
        get { return positionNow; }
        set { positionNow = value; }
    }
    public int CurrentLineLength
    {
        get { return currentLine.Length; }
    }
    public string CurrentLine
    {
        get { return currentLine; }
    }
    public InputOutput(List<string> lines)
    {
        sourceLines = lines;
        ReadNextLine();
    }
    public int TotalErrors { get; private set; } = 0;
    public void NextChar()
    {
        if (positionNow.CharNumber >= currentLine.Length)
        {
            if (!string.IsNullOrEmpty(currentLine))
            {
                ListThisLine();
                if (errorList.Count > 0)
                    ListErrors();
            }

            ReadNextLine();
        }
        else
        {
            positionNow.CharNumber++;
        }

        if (positionNow.CharNumber <= currentLine.Length && currentLine.Length > 0)
        {
            currentChar = currentLine[positionNow.CharNumber - 1];
        }
        else
        {
            currentChar = '\0';
        }
    }


    private void ReadNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= sourceLines.Count)
        {
            currentLine = "";
            currentChar = '\0';
            return;
        }
        currentLine = sourceLines[currentLineIndex];
        positionNow.LineNumber = currentLineIndex + 1;
        positionNow.CharNumber = 1;

        if (currentLine.Length > 0)
        {
            currentChar = currentLine[0];
        }
        else
        {
            currentChar = '\n';
        }
        errorList.Clear();
        errorOverflow = false;
    }
    public void Error(int errorCode, TextPosition pos)
    {
        if (errorList.Count >= ERRMAX)
        {
            errorOverflow = true;
        }
        else
        {
            errorList.Add(new Error(errorCode, pos));
        }

        TotalErrors++;
    }
    private void ListThisLine()
    {
        Console.WriteLine($"{positionNow.LineNumber,4}: {currentLine}");
    }
    private void ListErrors()
    {
        foreach (var err in errorList)
        {
            string markerLine = new string(' ', err.Position.CharNumber + 5) + "^ ошибка код " + err.Code;
            string messageLine = new string('*', 6) + " " + err.GetDescription();

            Console.WriteLine(markerLine);
            Console.WriteLine(messageLine);
        }
        if (errorOverflow)
        {
            Console.WriteLine("** Превышено максимальное количество ошибок в строке!");
        }
    }

    public char PeekChar()
    {
        if (CurrentPosition.CharNumber + 1 < CurrentLine.Length)
        {
            return CurrentLine[CurrentPosition.CharNumber + 1];
        }
        return '\0';
    }

}