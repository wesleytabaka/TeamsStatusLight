namespace TeamsStatusLight
{
    class Presence : IPresence
    {
        public string activity { get; set; }
        public string availability { get; set; }

        public Presence() {
            activity = "Offline";
            availability = "Offline";
        }
        ~Presence () { }

        public void setPresence(string activity, string availability)
        {
            this.activity = activity;
            this.availability = availability;
        }

        public Presence getPresence() {
            return this;
        }

        IPresence IPresence.getPresence()
        {
            throw new NotImplementedException();
        }
    }
}
