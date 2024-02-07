using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance = null;
    static Managers Instance { get { Init(); return _instance; } }
    
    private GameManager _game = new GameManager();
    private CursorManager _cursor = new CursorManager();
    private SoundManager _sound = new SoundManager();
    private ParticleManager _particle = new ParticleManager();
    
    public static GameManager Game => Instance._game;
    public static CursorManager Cursor => Instance._cursor;
    public static SoundManager Sound => Instance._sound;
    public static ParticleManager Particle => Instance._particle;
    
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

            Cursor.Init();
            Game.Init();
            Sound.Init();
            Particle.Init();
        }
    }

    private void Update()
    {
        Game.Update();
    }
}
