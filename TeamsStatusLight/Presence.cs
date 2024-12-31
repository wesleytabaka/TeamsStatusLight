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

        public Presence(string activity, string availability)
        {
            this.activity = activity;
            this.availability = availability;
        }

        ~Presence () { }

        public void setPresence(string activity, string availability)
        {
            this.activity = activity;
            this.availability = availability;
        }

        public void setPresence(IPresence presence)
        {
            this.activity = presence.activity;
            this.availability = presence.availability;
        }

        public Presence getPresence() {
            return this;
        }

        IPresence IPresence.getPresence()
        {
            throw new NotImplementedException();
        }
        public bool Equals(IPresence other) {
            return this.activity == other.activity && this.availability == other.availability;
        }
    }
}
