namespace TeamsStatusLight
{
    public interface IIndicatorInstruction
    {
        int r {  get; set; }
        int g { get; set; }
        int b { get; set; }
        Effect effect { get; set; }
        int effectRate { get; set; }
        Transition transition { get; set; }
        int transitionDuration { get; set; }
    }
}
