namespace TeamsStatusLight
{
    public interface IPresence
    {
        string activity { get; set; }
        string availability { get; set; }
        IPresence getPresence();
        void setPresence(string activity, string availability);
        void setPresence(IPresence presence);
        bool Equals(IPresence other);
    }
}
