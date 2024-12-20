유니티로 제작한 마비노기 모작 작품입니다.

![image](https://github.com/user-attachments/assets/ea3646b1-b3d9-47dd-a594-d310dda066c3)
![image](https://github.com/user-attachments/assets/9f7c6941-374e-43c7-963b-997295ebfe3d)
![image](https://github.com/user-attachments/assets/be5f8598-ace4-4877-a6f3-1bd6355a3b10)

개발인수: 1명

동작환경: Window 11

기간: 4개월

목표: 제일 오래했던 PC게임인 마비노기를 모작해보면서 전체적인 RPG게임의 시스템 설계 및 디자인 패턴 연습

구현된 기능

1. 플레이어 Control, 적 AI을 FSM(State Pattern) 구현
2. 그리드 인벤토리 및 아이템 / 장비 착용(최초 착용 시 객체화, 그 이후는 Dictionary에 넣어 오브젝트 풀링으로 관리), 해제 등
3. Smash, Defence, CounterAttack 과 같은 PlayerSkill 구현
4. 옵저버 패턴으로 게임 매니저 구현(마우스 커서 변경), 사운드 매니저 구현

![image](https://github.com/user-attachments/assets/94e72b97-a113-4a35-ac1c-f77a2e688696)
게임 시스템 내의 다양한 매니저와 하위 시스템

Video URL: https://www.youtube.com/watch?v=xcyLgep3kXs

// In English

Recreated Mabinogi Project in Unity

Development Personnel: 1 Person

Operation Enviroment：Windows 11

Duration: 4 months

Development Motivation:
By recreating Mabinogi, the PC game I played the longest, I aimed to practice implementing the fundamental systems of RPG games and study design patterns.

Implemented Features

1. Player Control and Enemy AI implemented using FSM (State Pattern).
2. Grid Inventory System with item/equipment equipping (objects are instantiated on first equip, then managed using a Dictionary for object pooling) and unequipping.
3. Player Skills: Implemented abilities such as Smash, Defence, and CounterAttack.
4. Observer Pattern:
- Game Manager for managing mouse cursor changes.
- Sound Manager for sound effects and background music.

![image](https://github.com/user-attachments/assets/69fd2254-5cf2-4e61-a011-a594e1f7dcae)
Various Managers and Subsystems within the Game System

Video URL: https://www.youtube.com/watch?v=xcyLgep3kXs

// 日本語Ver.

Unityで制作したマビノギ模倣作品

開発人数: 1名

動作環境：Windows 11

機関：４ヶ月

開発動機:
最も長くプレイしたPCゲーム「マビノギ」を模倣することで、RPGゲームの基礎的なシステムの実装とデザインパターンの習得を目指しました。

実装された機能

1. プレイヤー操作および敵AI をFSM（ステートパターン）で実装。
2. グリッドインベントリシステム：アイテム/装備の装着機能（初回装着時にオブジェクト化し、その後はDictionaryを使いオブジェクトプーリングで管理）および装備解除機能。
3. プレイヤースキル：Smash、Defence、CounterAttack の実装。
4. オブザーバーパターン:
- ゲームマネージャーによるマウスカーソル変更機能。
- サウンドマネージャーによるサウンドおよびBGM管理。

![image](https://github.com/user-attachments/assets/5fe8653d-657f-4944-8989-22e50a210fbd)
ゲームシステム内のさまざまなマネージャーとサブシステム

Video URL: https://www.youtube.com/watch?v=xcyLgep3kXs
