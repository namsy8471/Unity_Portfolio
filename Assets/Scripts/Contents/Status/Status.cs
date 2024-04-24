namespace Contents.Status
{
    public class Status
    {
        // 기초 스테이터스
        private float _hp;
        private float _maxHp;
        private float _mp;
        private float _maxMp;
        private float _stamina;
        private float _maxStamina;

        private float _maxAtkCount;
        
        public virtual float Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                if (_hp >= MaxHp) _hp = MaxHp;
            }
        }

        public float MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                Hp = value;
            }
        }

        public virtual float Mp
        {
            get => _mp;
            set
            {
                _mp = value;
                if (_mp >= MaxMp) _mp = MaxMp;
            }
        }

        public float MaxMp
        {
            get => _maxMp;
            set
            {
                _maxMp = value;
                Mp = value;
            }
        }

        public virtual float Stamina
        {
            get => _stamina;
            set
            {
                _stamina = value;
                if (_stamina >= MaxStamina) _stamina = MaxStamina;
            }
        }

        public float MaxStamina
        {
            get => _maxStamina;
            set
            {
                _maxStamina = value;
                Stamina = value;
            }
        }

        // 전투 스테이터스

        public float Str { get; set; }

        public float Int { get; set; }

        public float Dex { get; set; }

        public float Will { get; set; }

        public float Luck { get; set; }

        public float MinDmg { get; set; }

        public float MaxDmg { get; set; }

        public float AtkRange { get; set; }

        public float AtkSpeed { get; set; }

        public float MaxAtkCount
        {
            get => _maxAtkCount;
            set
            {
                _maxAtkCount = value;
                DownGaugeToHit = (100 / _maxAtkCount) + 1;
            }
        }

        public float Def { get; set; }

        public float DownGaugeFromHit { get; set; }

        public float DownGaugeToHit { get; private set; }

        public float MoveSpeed { get; set; }

        public Status() : this(100, 50, 50, 20, 10, 10, 10, 10) {}
        public Status(int maxHp, int maxMp, int maxStamina,
            float str, float dex, float @int, float will, float luck)
        {
            MaxHp = maxHp;
            MaxMp = maxMp;
            MaxStamina = maxStamina;
            
            Str = str;
            Dex = dex;
            Int = @int;
            Will = will;
            Luck = luck;

            MinDmg = (Str / 4);
            MaxDmg = (Str / 2.5f);

            AtkRange = 2.5f;
            AtkSpeed = 0.4f;
            MaxAtkCount = 3;

            MoveSpeed = 600f;
        }
    }
}
