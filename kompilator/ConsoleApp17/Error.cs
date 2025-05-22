public class Error
{
    public TextPosition Position { get; set; }
    public int Code { get; set; }
    public Error(int code, TextPosition position)
    {
        Code = code;
        Position = position;
    }
    public string GetDescription()
    {
        return Code switch
        {
            3 => "Должно идти служебное слово Program",
            4 => "Должен идти символ ')'",
            7 => "Недопустимый символ диапазона '..'",
            10 => "Символ '$' запрещен",
            52 => "Должно идти слово then(после if)",
            _ => "неизвестная ошибка"
        };
    }
}