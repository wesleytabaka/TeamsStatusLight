namespace TeamsStatusLight
{
    internal interface IIndicator
    {
        void SetIndicator(IIndicatorInstruction instruction);
        void Write(string input);
    }
}
