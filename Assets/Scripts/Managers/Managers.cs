using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance = null;
    static Managers Instance { get { Init(); return _instance; } }
    
    private GameManager _game = new GameManager();
    private CursorManager _cursor = new CursorManager();
    private SoundManager _sound = new SoundManager();
    private ParticleManager _particle = new ParticleManager();
    private InputManager _input = new InputManager();
    
    public static GameManager Game => Instance._game;
    public static CursorManager Cursor => Instance._cursor;
    public static SoundManager Sound => Instance._sound;
    public static ParticleManager Particle => Instance._particle;
    public static InputManager Input => Instance._input;
    
    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (_instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(obj);
            _instance = obj.GetComponent<Managers>();
            
            Input.Init();
            Cursor.Init();
            Game.Init();
            Sound.Init();
            Particle.Init();
        }
    }

    private void Update()
    {
        Input.Update();
        Game.Update();
    }
}
