using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsManager
{
    // This Manager manage whole graphics elements like UI, Image, and Cursor etc.
    // 이 매니저는 UI, 이미지, 커서 등의 모든 그래픽 요소를 담당합니다.
    
    private readonly CursorManager _cursor = new CursorManager();
    private readonly UIManager _ui = new UIManager();
    private readonly VisualManager _visual = new VisualManager();

    public CursorManager Cursor => _cursor;
    public UIManager UI => _ui;
    public VisualManager Visual => _visual;

    public void Init()
    {
        Cursor.Init();
        UI.Init();
        Visual.Init();
    }
    

    public void Update()
    {
        Cursor.Update();
    }
}
