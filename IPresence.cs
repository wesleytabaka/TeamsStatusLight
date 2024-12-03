namespace TeamsStatusLight
{
    public interface IPresence
    {
        string activity { get; set; }
        string availability { get; set; }
        IPresence getPresence();
        void setPresence(string activity, string availability);
    }
}
