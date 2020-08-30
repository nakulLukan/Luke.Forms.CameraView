namespace Luke.Forms.CameraView
{
    public static class Common
    {
        public static Vector<int,int> GetAspectRatioCoeff(AspectRatioOptions aspecRatio)
        {
            if (aspecRatio == AspectRatioOptions.Default)
            {
                throw new CameraViewException("Cannot get coefficients for 'Default' aspect ratio");
            }

            var coeff = aspecRatio.ToString().Remove(0, 6).Split('_');
            return new Vector<int, int>(int.Parse(coeff[0]), int.Parse(coeff[1]));
        }
    }
}
