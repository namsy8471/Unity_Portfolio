using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance = null;
    static Managers Instance { get { Init(); return _instance; } }
    
    private InputManager _input = new InputManager();
    private SoundManager _sound = new SoundManager();
    private GraphicsManager _graphics = new GraphicsManager();
    private RayManager _ray = new RayManager();
    private GameManager _game = new GameManager();

    public static InputManager Input => Instance._input;
    public static SoundManager Sound => Instance._sound;
    public static GraphicsManager Graphics => Instance._graphics;
    public static RayManager Ray => Instance._ray;
    public static GameManager Game => Instance._game;
    
    public static GameObject ManagersGO { get; private set; }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (_instance == null)
        {
            ManagersGO = GameObject.Find("@Managers");
            if (ManagersGO == null)
            {
                ManagersGO = new GameObject { name = "@Managers" };
                ManagersGO.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(ManagersGO);
            _instance = ManagersGO.GetComponent<Managers>();
            
            Input.Init();
            Sound.Init();
            Graphics.Init();
            Ray.Init();
            
            Game.Init();
        }
    }

    private void Update()
    {
        Input.Update();
        Ray.Update();
        
        Game.Update();
    }
}
