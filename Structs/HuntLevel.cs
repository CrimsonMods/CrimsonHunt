namespace CrimsonHunt.Structs
{
    public struct HuntLevel
    { 
        public int Level { get; set; }
        public int ExpNeeded { get; set; }
        public HuntReward HuntReward { get; set; }
        public string Message { get; set; }

        public HuntLevel(int _level, int _expNeeded, HuntReward _reward, string _message)
        {
            Level = _level;
            ExpNeeded = _expNeeded;
            HuntReward = _reward;
            Message = _message;
        }

        public HuntLevel() { }
    }

    public struct HuntReward
    {
        public int EffectHash { get; set; }
        public int ItemHash { get; set; }
        public int ItemQuantity { get; set; }

        public HuntReward(int effectHash, int itemHash, int itemQuantity) { EffectHash = effectHash; ItemHash = itemHash; ItemQuantity = itemQuantity; }

        public HuntReward() { }
    }
}
