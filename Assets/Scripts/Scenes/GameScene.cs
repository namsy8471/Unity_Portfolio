using UnityEngine;

namespace Scenes
{
    public class GameScene : MonoBehaviour
    {
        private int TargetEnemyCount { get; set;}

        void Start()
        {
            TargetEnemyCount = 3;
            GetComponent<AudioSource>().loop = true;
            Managers.Sound.PlaySound(gameObject, "BGM");
        }

        public void EnemyKilled()
        {
            TargetEnemyCount--;
            
            if (TargetEnemyCount <= 0)
            {
                VictoryGame();
            }
        }

        private void VictoryGame()
        {
            Managers.Graphics.UI.WinCanvasActive();
        }
    }
}
