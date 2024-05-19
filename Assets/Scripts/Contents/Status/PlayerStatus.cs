using UnityEngine;

namespace Contents.Status
{
    public class PlayerStatus : Status
    {
        private int _exp;
        private int _gold;
        private int _level;
    
        public int Exp
        {
            get => _exp;
            set
            {
                _exp = value;
            }
        }

        public override float Hp
        {
            get => base.Hp;
            set
            {
                base.Hp = value;
            
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.HpBar, value);
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.HpBarInStatusWindow, value);
            }
        }
        
        public override float MaxHp
        {
            get => base.MaxHp;
            set
            {
                base.MaxHp = value;
                Hp = value;
                
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.HpBar, value);
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.HpBarInStatusWindow, value);
            }
        }

        public override float Mp
        {
            get => base.Mp;
            set
            {
                base.Mp = value;
            
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.MpBar, value);
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.MpBarInStatusWindow, value);
            }
        }
        
        public override float MaxMp
        {
            get => base.MaxMp;
            set
            {
                base.MaxMp = value;
                Mp = value;
                
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.MpBar, value);
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.MpBarInStatusWindow, value);
            }
        }

        public override float Stamina
        {
            get => base.Stamina;
            set
            {
                base.Stamina = value;
            
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.StaminaBar, value);
                Managers.Graphics.UI.ChangeSliderValue(Managers.Graphics.UI.StaminaBarInStatusWindow, value);
            }
        }
        
        public override float MaxStamina
        {
            get => base.MaxStamina;
            set
            {
                base.MaxStamina = value;
                Stamina = value;
                
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.StaminaBar, value);
                Managers.Graphics.UI.ChangeSliderMaxValue(Managers.Graphics.UI.StaminaBarInStatusWindow, value);
            }
        }

        public override int Str
        {
            get => base.Str;
            set
            {
                base.Str = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Str, value);
            }
        }
        
        public override int Int
        {
            get => base.Int;
            set
            {
                base.Int = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Int, value);
            }
        }
        
        public override int Dex
        {
            get => base.Dex;
            set
            {
                base.Dex = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Dex, value);
            }
        }
        
        public override int Will
        {
            get => base.Will;
            set
            {
                base.Will = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Will, value);
            }
        }
        
        public override int Luck
        {
            get => base.Luck;
            set
            {
                base.Luck = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Luck, value);
            }
        }
        
        public override int MinDmg
        {
            get => base.MinDmg;
            set => base.MinDmg = value + (Str / 4);
        }
        
        public override int MaxDmg
        {
            get => base.MaxDmg;
            set
            {
                base.MaxDmg = value + (int)(Str / 2.5f);
                
                Managers.Graphics.UI.ChangeTextForDamage
                    (Managers.Graphics.UI.Damage, MinDmg, MaxDmg);
            }
        }
        
        public override int Def
        {
            get => base.Def;
            set
            {
                base.Def = value;
                
                Managers.Graphics.UI.ChangeText(Managers.Graphics.UI.Def, value);
            }
        }
        
        public ItemDataWeapon.AttackStyle AtkStyle { get; set; }

        public PlayerStatus(GameObject go)
        {
            Controller = go;
            
            MaxHp = 100;
            MaxMp = 50;
            MaxStamina = 50;

            Str = 20;
            Dex = 20;
            Int = 20;
            Will = 20;
            Luck = 20;

            AtkStyle = ItemDataWeapon.AttackStyle.Punch;
        
            MinDmg = (Str / 4);
            MaxDmg = (int)(Str / 2.5f);
        
            AtkRange = 2.5f;
            AtkSpeed = 0.7f;
        
            MaxAtkCount = 3;
            Def = 0;

            DownGaugeFromHit = 0;
        
            MoveSpeed = 200.0f;
        }

        public static PlayerStatus operator +(PlayerStatus s1, Status s2)
        {
            s1.MaxHp += s2.MaxHp;
            s1.MaxMp += s1.MaxMp;
            s1.MaxStamina += s2.MaxStamina;

            s1.Str += s2.Str;
            s1.Dex += s2.Dex;
            s1.Int += s2.Int;
            s1.Will += s2.Will;
            s1.Luck += s2.Luck;

            s1.Def += s2.Def;

            s1.MinDmg = (s1.Str / 4);
            s1.MaxDmg = (int)(s1.Str / 2.5);
            
            return s1;
        }
    }
}
