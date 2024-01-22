유니티로 제작한 마비노기 모작 작품입니다.

구현된 기능 (최종 수정 2023-12-21)

1. 플레이어 및 적 FSM
2. 플레이어 이동 / FreeLook 카메라를 이용한 3인칭 카메라 이동
3. 그리드 인벤토리 및 아이템 / 장비 착용(최초 착용 시 객체화, 그 이후는 Dictionary에 넣어 오브젝트 풀링으로 관리), 해제 등
4. Unity SendMessage와 C# Collection을 사용한 퀘스트 시스템
5. 옵저버 패턴으로 게임 매니저 구현(마우스 커서 변경), 사운드 매니저 구현

// In English

It is copy practice of Mabinogi(Game in Nexon co.) in Unity

Implemented Feature (Last update on 2023-12-21)

1. Player/Enemy FSM
2. Player Moving / 3rd Person Cam by FreeLook Camera(Cinemachine)
3. Grid Inventory and Item / Equipment equipping(Instancing at the first time, and make Dictionary Container for managing by Object pooling), unequipping etc.
4. Quest System by Uniny SendMessage and C# collection
5. Game Managers is made by Observer pattern(For changing mouse cursor, sounds etc.)
