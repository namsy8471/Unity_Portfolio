using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance = null;
    static Managers Instance { get { Init(); return _instance; } }
    
    private readonly InputManager _input = new InputManager();
    private readonly SoundManager _sound = new SoundManager();
    private readonly GraphicsManager _graphics = new GraphicsManager();
    private readonly RayManager _ray = new RayManager();
    private readonly GameManager _game = new GameManager();

    public static InputManager Input => Instance._input;
    public static SoundManager Sound => Instance._sound;
    public static GraphicsManager Graphics => Instance._graphics;
    public static RayManager Ray => Instance._ray;
    public static GameManager Game => Instance._game;
    
    public static GameObject ManagersGo { get; private set; }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (_instance == null)
        {
            ManagersGo = GameObject.Find("@Managers");
            if (ManagersGo == null)
            {
                ManagersGo = new GameObject { name = "@Managers" };
                ManagersGo.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(ManagersGo);
            _instance = ManagersGo.GetComponent<Managers>();
            
            Input.Init();
            Sound.Init();
            Ray.Init();
            Graphics.Init();
            
            Game.Init();
        }
    }

    private void Update()
    {
        Input.Update();
        Ray.Update();
        Graphics.Update();
        
        Game.Update();
    }
}
