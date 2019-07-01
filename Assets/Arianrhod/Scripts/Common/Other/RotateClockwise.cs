namespace Arianrhod
{
    public static class RotateClockwise
    {
        public static int[,] Rotate(int[,] g) {
            var rows = g.GetLength(0);
            var cols = g.GetLength(1);
            var t = new int[cols, rows];
            for (var i = 0; i < rows; i++) {
                for (var j = 0; j < cols; j++) {                
                    t[j, rows-i-1] = g[i, j];
                }
            }
            return t;
        }
    }
}