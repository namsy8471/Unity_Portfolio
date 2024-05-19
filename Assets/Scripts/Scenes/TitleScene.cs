using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class TitleScene : MonoBehaviour
    {
        public TextMeshProUGUI pressAnyKeyText;
        private bool _alphaController;

        void Update()
        {
            if (Input.anyKey)
                SceneManager.LoadScene("GameScene");
            
            if (_alphaController)
            {
                pressAnyKeyText.alpha -= Time.deltaTime;
                if (pressAnyKeyText.alpha <= 0)
                    _alphaController = false;
            }
            else
            {
                pressAnyKeyText.alpha += Time.deltaTime;
                if (pressAnyKeyText.alpha >= 1)
                    _alphaController = true;
            }
        }
    }
}
