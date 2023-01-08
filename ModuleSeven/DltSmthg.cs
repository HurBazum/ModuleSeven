namespace DeleteLib
{
    //класс для удаления неточного ввода
    //запомнит позицию курсора до начала
    //ввода, после, при неточном вводе,
    //встанет на эту позицию и вставит
    //туда строку состоящую из пробелов
    //и равную той, которая неудовлетворяет
    //тому, что нужно программе. И вновь
    //установит курсор на изначальную позицию.
    public static class DltSmthg
    {
        public static void _setCursorPosition(out int xPositionOfCursorBeforeEnter, out int yPositionOfCursorBeforeEnter)
        {
            xPositionOfCursorBeforeEnter = Console.CursorLeft;
            yPositionOfCursorBeforeEnter = Console.CursorTop;
        }
        public static void deleteWrongEnter(int xPositionOfCursorBeforeEnter, int yPositionOfCursorBeforeEnter, string? s)
        {
            string s0 = "";
            for (int i = 0; i < s.Length; i++)
            {
                s0 += " ";
            }
            Console.SetCursorPosition(xPositionOfCursorBeforeEnter, yPositionOfCursorBeforeEnter);
            Console.Write(s0);
            Console.SetCursorPosition(xPositionOfCursorBeforeEnter, yPositionOfCursorBeforeEnter);
        }
    }
}