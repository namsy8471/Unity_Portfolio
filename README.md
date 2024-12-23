English content is written below!

日本語の内容が以下に記載されています!

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

내가 가장 주의를 기울인 부분

프로그램 작성 시, 각 계층 구조를 최대한 준수하며 구현하려고 노력했습니다. 이를 통해 코드의 가독성을 높이는 것뿐만 아니라, 수정 및 확장 시의 효율성도 향상시킬 수 있다고 생각했습니다.

가장 어려웠던 점

이 프로젝트를 시작했을 때, 다양한 디자인 패턴에 대한 충분한 이해가 부족했습니다. 그 결과, 부적절한 **FSM(유한 상태 머신)**을 사용해 코드 전체를 다시 작성해야 하는 상황을 겪었습니다. 이를 통해 설계의 중요성을 다시 한번 실감했습니다.

가장 공들인 부분

마비노기의 스킬 시스템과 전투 시스템입니다. 이 게임은 비교적 마이너한 편이라 참고할 수 있는 기존 자료가 거의 없었고, 모든 것을 처음부터 구현해야 했습니다. 따라서 직관적이고 매끄러운 시스템을 구축하기 위해 많은 시간과 노력을 기울였습니다.


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

Q: What were I careful about when creating the program?

When writing the program, I focused on adhering to the hierarchical structure as much as possible. I believe this not only improves code readability but also enhances efficiency during modifications and extensions.

Q: What did you find difficult when creating the program?

At the start of this project, I did not have a thorough understanding of various design patterns. As a result, I used an inappropriate FSM (Finite State Machine), which forced me to rewrite the entire code. This experience highlighted the importance of proper design.

Q: What points do you want us to particularly focus on in your program?

The skill system and combat system of Mabinogi. Since this game is relatively niche, there were few existing references, requiring me to implement everything from scratch. Therefore, I devoted considerable time and effort to building a smooth and intuitive system.

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

一番注意した事

プログラムを作成する際、各階層構造を可能な限り守りながら実装することを心掛けました。これにより、コードの可読性を向上させるだけでなく、修正や拡張時の効率化も図ることができると考えています。

大変だった所

このプロジェクトを開始した当初、さまざまなデザインパターンについて十分に理解していませんでした。その結果、不適切なFSM（有限状態マシン）を使用してしまい、コードを全面的に作り直す羽目になった経験があります。この失敗を通じて、設計の重要性を改めて実感しました。

力をいれて作った部分

マビノギのスキルシステムと戦闘システムです。このゲームは比較的マイナーであるため、既存の参考になる部分が少なく、自分で一から実装する必要がありました。そのため、多くの時間と労力をかけてスムーズで直感的なシステムを構築することを目指しました。

Video URL: https://www.youtube.com/watch?v=xcyLgep3kXs

実行ファイルは以下のリンクからダウンロードできます: https://drive.google.com/file/d/1l2EyKP8pFqGlTZ6mFsJGTs0biJued0EW/view?usp=sharing
