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

        public ItemDataWeapon.AttackStyle AtkStyle { get; set; }

        public PlayerStatus()
        {
            MaxHp = 100;
            MaxMp = 50;
            MaxStamina = 50;

            Str = 20;
            Dex = 20;
            Int = 20;
            Will = 20;
            Luck = 20;

            AtkStyle = ItemDataWeapon.AttackStyle.Punch;
        
            MinDmg = 5;
            MaxDmg = 10;
        
            AtkRange = 2.5f;
            AtkSpeed = 0.7f;
        
            MaxAtkCount = 3;
            Def = 10;

            DownGaugeFromHit = 0;
        
            MoveSpeed = 200.0f;

        }
    }
}
