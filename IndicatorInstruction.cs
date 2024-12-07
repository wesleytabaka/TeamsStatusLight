using Microsoft.Extensions.Configuration;

namespace TeamsStatusLight
{
    internal class IndicatorInstruction : IIndicatorInstruction
    {
        int _r;
        int _g;
        int _b;
        Effect _effect;
        int _effectRate;
        Transition _transition;
        int _transitionDuration;

        int IIndicatorInstruction.r { get => _r; set => _r = value; }
        int IIndicatorInstruction.g { get => _g; set => _g = value; }
        int IIndicatorInstruction.b { get => _b; set => _b = value; }
        Effect IIndicatorInstruction.effect { get => _effect; set => _effect = value; }
        int IIndicatorInstruction.effectRate { get => _effectRate; set => _effectRate = value; }
        Transition IIndicatorInstruction.transition { get => _transition; set => _transition = value; }
        int IIndicatorInstruction.transitionDuration { get => _transitionDuration; set => _transitionDuration = value; }

        public IndicatorInstruction(int r, int g, int b, Effect effect, int effectRate, Transition transition, int transitionDuration)
        {
            this._r = r;
            this._g = g;
            this._b = b;
            this._effect = effect;
            this._effectRate = effectRate;
            this._transition = transition;
            this._transitionDuration = transitionDuration;
        }
        public IndicatorInstruction(int r, int g, int b)
        {
            this._r = r;
            this._g = g;
            this._b = b;
            this._effect = Effect.SOLID;
            this._effectRate = 0;
            this._transition = Transition.NOW;
            this._transitionDuration = 0;
        }

        public IndicatorInstruction()
        {
            this._r = 0;
            this._g = 0;
            this._b = 0;
            this._effect = Effect.SOLID;
            this._effectRate = 0;
            this._transition = Transition.NOW;
            this._transitionDuration = 0;
        }
        public static Dictionary<string, IndicatorInstruction> DeserializeIndicatorInstructionsFromConfig(IConfigurationSection config) {
            Dictionary<string, IndicatorInstruction> output = new Dictionary<string, IndicatorInstruction>();

            foreach (var item in config.GetChildren())
            {
                output.Add(item.Key, new IndicatorInstruction(Int32.Parse(item.GetSection("red").Value), Int32.Parse(item.GetSection("green").Value), Int32.Parse(item.GetSection("blue").Value), (Effect) Enum.Parse(typeof(Effect), item.GetSection("effect").Value), Int32.Parse(item.GetSection("effectRate").Value), (Transition) Enum.Parse(typeof(Transition), item.GetSection("transition").Value), Int32.Parse(item.GetSection("transitionDuration").Value)));
            }
            return output;
        }
    }
}
