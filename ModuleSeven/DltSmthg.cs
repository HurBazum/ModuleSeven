namespace DeleteLib
{
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
            Console.CursorLeft = xPositionOfCursorBeforeEnter;
            Console.CursorTop = yPositionOfCursorBeforeEnter;
            Console.Write(s0);
            Console.CursorLeft = xPositionOfCursorBeforeEnter;
            Console.CursorTop = yPositionOfCursorBeforeEnter;
        }
    }
}