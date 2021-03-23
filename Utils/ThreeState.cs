namespace ConsoleGraphics.Utils
{
    public struct ThreeState
    {
        public byte x;

        public ThreeState(byte initialState)
        {
            x = 0;
        }

        public void changeState()
        {
            x = (byte)((x + 1) % 3);
        }
    }
}
