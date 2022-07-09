namespace CG_Kuerteil.Util
{
    public static class MathHelper
    {
        public static float CircleX(double angle, float radius = 1) => radius * (float)Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(angle));
        public static float CircleY(double angle, float radius = 1) => radius * (float)Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(angle));
        public static float Max(float[] values)
        {
            float max = 0;
            for (int i = 0; i < values.Length; i++)
                max = Math.Max(max, values[i]);
            return max;
        }
    }
}