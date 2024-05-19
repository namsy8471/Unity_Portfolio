using UnityEngine;
using UnityEngine.UI;

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

        private int _maxAtkCount;
        
        public GameObject Controller { get; protected set; }
        
        public virtual float Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                if (_hp >= MaxHp) _hp = MaxHp;
                
                Controller.GetComponent<Controller>().HpBar.GetComponentInChildren<Slider>().value = _hp / MaxHp;
            }
        }

        public virtual float MaxHp
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

        public virtual float MaxMp
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

        public virtual float MaxStamina
        {
            get => _maxStamina;
            set
            {
                _maxStamina = value;
                Stamina = value;
            }
        }

        // 전투 스테이터스

        public virtual int Str { get; set; }

        public virtual int Int { get; set; }

        public virtual int Dex { get; set; }

        public virtual int Will { get; set; }

        public virtual int Luck { get; set; }

        public virtual int MinDmg { get; set; }

        public virtual int MaxDmg { get; set; }

        public float AtkRange { get; set; }

        public float AtkSpeed { get; set; }

        public int MaxAtkCount
        {
            get => _maxAtkCount;
            set
            {
                _maxAtkCount = value;
                DownGaugeToHit = (100 / _maxAtkCount) + 1;
            }
        }

        public virtual int Def { get; set; }

        public float DownGaugeFromHit { get; set; }

        public float DownGaugeToHit { get; private set; }

        public float MoveSpeed { get; set; }

        public Status() {}

        public Status(GameObject go) : this(go, 100, 50, 50, 50, 10, 10, 10, 10)
        { }
        
        public Status(GameObject go, int maxHp, int maxMp, int maxStamina,
            int str, int dex, int @int, int will, int luck)
        {
            Controller = go;
            
            MaxHp = maxHp;
            MaxMp = maxMp;
            MaxStamina = maxStamina;
            
            Str = str;
            Dex = dex;
            Int = @int;
            Will = will;
            Luck = luck;

            MinDmg = (Str / 4);
            MaxDmg = (int)(Str / 2.5f);

            Def = 3;
            
            AtkRange = 2.5f;
            AtkSpeed = 0.4f;
            MaxAtkCount = 3;

            MoveSpeed = 600f;
        }

        public static Status operator +(Status s1, Status s2)
        {
            return new Status(s1.Controller, (int)(s1.MaxHp + s2.MaxHp), (int)(s1.MaxMp + s2.MaxMp), (int)(s1.MaxStamina + s2.MaxStamina),
                s1.Str + s2.Str, s1.Dex + s2.Dex, s1.Int + s2.Int, s1.Will + s2.Will, s1.Luck + s2.Luck);
        }

        public static Status operator -(Status s1, Status s2)
        {
            return new Status(s1.Controller, (int)(s1.MaxHp - s2.MaxHp), (int)(s1.MaxMp - s2.MaxMp),
                (int)(s1.MaxStamina - s2.MaxStamina),
                s1.Str - s2.Str, s1.Dex - s2.Dex, s1.Int - s2.Int, s1.Will - s2.Will, s1.Luck - s2.Luck);
        }
    }
}
