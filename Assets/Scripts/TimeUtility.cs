namespace Yuuta.VRMGo
{
    
    public static class TimeUtility
    {
        public static string GetTimeString(int time)
            => $"{_AlignTimeNumber(time / 3600)}:{_AlignTimeNumber(time % 3600 / 60)}:{_AlignTimeNumber(time % 60)}";
        
        private static string _AlignTimeNumber(int number)
            => number.ToString().PadLeft(2, '0');
    }
}

