namespace Luke.Forms.CameraView
{
    public struct Vector<T, U>
    {
        public T X { get; set; }
        public U Y { get; set; }

        public Vector(T x,U y)
        {
            X = x;
            Y = y;
        }
    }
}
