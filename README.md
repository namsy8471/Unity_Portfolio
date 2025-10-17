# Unity Mabinogi Recreated Project  
유니티로 제작한 **마비노기 모작 프로젝트**  
Unityで制作したマビノギ模倣作品  
A fan recreation of the MMORPG **Mabinogi**, built from scratch in Unity.

---

## 미리보기 / プレビュー / Preview
**Video URL (日本語説明付き)**  
[▶ https://www.youtube.com/watch?v=wVfeVCnwdKM](https://youtu.be/wVfeVCnwdKM?si=m30MyUjkE9UUaZrQ)

## In-Game Screenshots / ゲーム内スクリーンショット  

| ![Title Screen](https://github.com/user-attachments/assets/ea3646b1-b3d9-47dd-a594-d310dda066c3) | ![Character Info & Inventory](https://github.com/user-attachments/assets/9f7c6941-374e-43c7-963b-997295ebfe3d) |
|:---------------------------------------------------------------:|:---------------------------------------------------------------:|
| **Mabinogi Clone – Title Scene**  <br> *タイトルシーン / Title Scene* | **Character Info & Grid Inventory**  <br> *キャラクター情報とインベントリ / Character UI* |

| ![In-Game Battle](https://github.com/user-attachments/assets/be5f8598-ace4-4877-a6f3-1bd6355a3b10) | ![Managers](https://github.com/user-attachments/assets/94e72b97-a113-4a35-ac1c-f77a2e688696) |
|:---------------------------------------------------------------:|:---------------------------------------------------------------:|
| **Player Exploration Scene** <br> *探索シーン / Exploration Scene* | **Managers & Subsystems Overview**  <br> *マネージャーとサブシステム構成 / System Overview* |

---

## 프로젝트 개요 / プロジェクト概要 / Project Overview

| 항목 / 項目 / Item | 내용 / 内容 / Details |
|--------------------|----------------------|
| **개발 인원 / 人数 / Team** | 1명 (개인 제작) / 1名（個人制作） / 1 Person |
| **개발 환경 / 開発環境 / Environment** | Windows 11, Unity Engine |
| **개발 기간 / 開発期間 / Duration** | 2023/10 ~ 2023/12, 2024/03 ~ 2024/05 (4개월 / 4ヶ月 / 4 months) |
| **엔진 / エンジン / Engine** | Unity 2022 LTS |
| **목표 / 目的 / Goal** | RPG 시스템 설계 및 디자인 패턴 실습 / RPGシステム設計とデザインパターン学習 / Practicing RPG system design and design patterns |

---

## 구현 기능 / 実装された機能 / Implemented Features

1. **플레이어 조작 및 적 AI / プレイヤー操作と敵AI / Player & Enemy FSM**  
→ FSM(State Pattern)을 사용해 상태 기반 행동 전환 구현  
→ FSM（ステートパターン）を使用し、状態に応じて行動を切り替えるシステムを実装。  
→ Implemented **state-based behavior transitions** using the **FSM (State Pattern)**.  

2. **그리드 인벤토리 시스템 / グリッドインベントリシステム / Grid Inventory System**  
→ 아이템 및 장비 착용/해제 구현 (최초 착용 시 객체화 후 Dictionary 기반 오브젝트 풀링 관리)  
→ アイテムや装備の装着・解除機能を実装。初回装着時にオブジェクト化し、以降は **Dictionary ベースのオブジェクトプーリング** により管理。  
→ Implemented item/equipment **equip and unequip** functions, instantiating objects once and managing them through **Dictionary-based object pooling**.  

3. **플레이어 스킬 / プレイヤースキル / Player Skills**  
→ Smash, Defence, CounterAttack 스킬 구현  
→ 「Smash」「Defence」「CounterAttack」などのスキルを実装し、アニメーションおよび戦闘連携を再現。  
→ Implemented **Smash**, **Defence**, and **CounterAttack** skills with proper animation and combat linking.  

4. **옵저버 패턴 기반 매니저 / オブザーバーパターンのマネージャー / Observer Pattern Managers**  
  → **마우스 커서 변경을 담당하는 Game Manager**, **사운드를 제어하는 Sound Manager**,  
  **키보드·마우스 입력 이벤트를 제어하는 Input Manager**,  
  그리고 **한 번 습득한 장비를 풀링 관리하는 Pooling Manager** 등을 구현했습니다.  
  이 모든 매니저는 상호 독립적으로 동작하면서도, 옵저버 패턴을 통해 유기적으로 연결되어  
  시스템 전체의 유지보수성과 확장성을 높였습니다.  

　→ **マウスカーソルの切り替えを管理する Game Manager**、**サウンドとBGMを制御する Sound Manager**、  
　　**キーボードとマウスの入力イベントを統括する Input Manager**、  
　　そして **一度取得した装備をプーリングで管理する Pooling Manager** などを実装しました。  
　　これらのマネージャーはそれぞれ独立して動作しながら、オブザーバーパターンを介して連携し、  
　　システム全体の保守性と拡張性を向上させました。  

　→ Implemented the **Game Manager** (handles mouse cursor transitions),  
　　**Sound Manager** (controls sound and BGM),  
　　**Input Manager** (manages keyboard and mouse input events),  
　　and **Pooling Manager** (manages already-acquired equipment through pooling).  
　　Each manager operates independently yet communicates organically via the **Observer Pattern**,  
　　enhancing the overall **maintainability** and **scalability** of the system.


---

![image](https://github.com/user-attachments/assets/94e72b97-a113-4a35-ac1c-f77a2e688696)
게임 시스템 내 다양한 매니저와 서브시스템  
ゲームシステム内のさまざまなマネージャーとサブシステム  
Various Managers and Subsystems within the Game System

---

## 조작법 / 操作方法 / Controls

---

#### 기본 조작
- **이동**: 마우스 또는 **W / A / S / D** 키  
- **공격**: 적을 **왼쪽 마우스 클릭** 시 공격  
- **타게팅**: **왼쪽 Ctrl 키**로 적 선택 가능  

#### 화면 및 UI
- **스킬 창 열기**: **Z**  
- **스테이터스 창 열기**: **C**  
- **인벤토리 열기**: **I**

#### 상단 단축 슬롯 (Hotbar)
- 각 슬롯은 **F1 ~ F12** 키에 대응  
- **왼쪽 클릭**: 해당 스킬 사용  
- **오른쪽 클릭**: 슬롯에서 스킬 제거  

#### 基本操作
- **移動**：マウス または **W / A / S / D** キー  
- **攻撃**：敵を **左クリック** すると攻撃できます  
- **ターゲティング**：**左Ctrl** キーを押すと敵をターゲットできます  

#### 画面・UI操作
- **スキル画面を開く**：**Z**  
- **ステータス画面を開く**：**C**  
- **インベントリを開く**：**I**

#### ホットバー（画面上部の四角いボックス）
- 各ボックスは **F1〜F12** キーに対応しています  
- **左クリック**：指定されたスキルを使用  
- **右クリック**：ボックスからスキルを削除  

#### Basic Controls
- **Move**: Mouse or **W / A / S / D** keys  
- **Attack**: **Left-click** on an enemy to attack  
- **Targeting**: Hold **Left Ctrl** to target enemies  

#### UI & Screens
- **Open Skill Window**: **Z**  
- **Open Status Window**: **C**  
- **Open Inventory**: **I**

#### Hotbar (Top Square Slots)
- Each slot corresponds to **F1–F12** keys  
- **Left-click**: Use the assigned skill  
- **Right-click**: Remove the skill from the slot  

---

## 내가 신경 쓴 부분 / 一番注意した事 / What I Focused On
프로그램 작성 시 각 **계층 구조를 준수**하려고 노력했습니다. 이를 통해 코드의 가독성과 수정·확장 효율성을 높였습니다.  
各階層構造を可能な限り守ることを意識し、可読性と拡張性を高めました。  
I focused on maintaining a clear **hierarchical structure**, improving code readability and extensibility.

---

## 어려웠던 점 / 大変だった所 / Challenges Faced
프로젝트 초기에는 디자인 패턴에 대한 이해가 부족해 부적절한 FSM을 적용했고, 결국 전체 코드를 다시 작성해야 했습니다. 이를 통해 **설계의 중요성**을 깨달았습니다.  
プロジェクト初期にデザインパターンの理解が不十分で、不適切なFSMを使ってしまい、コードを全面的に書き直しました。これにより**設計の重要性**を学びました。  
Initially, I misused the FSM structure due to a lack of understanding of design patterns and had to **rewrite the entire codebase** — a valuable lesson in software design.

---

## 공들인 부분 / 力を入れた部分 / Points of Emphasis

### 1. 스킬 및 전투 시스템 구현 / スキル・戦闘システムの実装 / Skill & Combat System Implementation

마비노기의 **스킬 및 전투 시스템**을 가장 중점적으로 구현했습니다.  
기존 참고 자료가 거의 없어 **직접 설계·제작**했습니다.  
스킬 간의 상호작용, 충돌 판정, 타이밍 처리 등을 세밀하게 조정하여  
**직관적이고 부드러운 전투 시스템**을 구현하는 데 주력했습니다.  

マビノギの**スキルシステムと戦闘システム**に最も力を入れました。  
参考資料が少なかったため、すべてを**自分で設計・実装**しました。  
スキル間の連携、当たり判定、タイミング処理などを丁寧に調整し、  
**直感的でスムーズな戦闘システム**を実現しました。  

The **skill and combat systems** were my main focus.  
Since there were few existing references, I **implemented everything from scratch**.  
I carefully tuned skill interactions, hit detection, and timing logic  
to create a **smooth and intuitive combat experience**.  

---

### 2️. 입력 관리(Input Manager) 시스템의 설계 / Input Manager システムの設計 / Input Manager System Design

입력 관리(**Input Manager**) 시스템의 설계에 주력했습니다.  
클래스마다 개별적으로 `if (Input.GetKeyDown(...))` 조건을 작성하지 않고,  
**이벤트 구독형 입력 처리(Event-driven Input Handling)** 방식을 설계했습니다.  
`Input Manager`가 키 입력을 중앙에서 감지하고,  
해당 이벤트를 구독한 클래스에 전달하도록 구현했습니다.  
이 방식으로 각 클래스의 입력 로직이 단순해지고,  
코드의 **유지보수성**(Maintainability)과 **확장성**(Scalability)이 크게 향상되었습니다.  

**Input Manager システムの設計に力を入れました。**  
各クラスごとに `if (Input.GetKeyDown(...))` を記述するのではなく、  
**イベント購読型の入力管理方式** を設計しました。  
`Input Manager` が入力を一元的に検知し、購読しているクラスにイベントを通知する仕組みを実装しました。  
この方法により、クラスごとの重複コードを削減し、  
コードの **保守性（メンテナンス性）** と **拡張性** を大幅に向上させました。  

I focused on designing the **Input Manager system**.  
Instead of writing individual `if (Input.GetKeyDown(...))` checks inside each class,  
I implemented an **event-driven input management system**.  
The `Input Manager` centrally detects key inputs and triggers events subscribed by relevant classes.  
This approach simplified input logic in each class and significantly improved  
the overall **maintainability** and **scalability** of the project.  


---

## 실행 파일 / 実行ファイル / Executable
link: https://drive.google.com/file/d/1_4GDz1bjWsPTDqZaba_qnxX-vgrB0fOF/view?usp=drive_link

---

## 요약 / まとめ / Summary
이 프로젝트를 통해 FSM, 옵저버, 오브젝트 풀링 등 **RPG 핵심 패턴**을 직접 설계하며 실무적인 설계 감각을 익혔습니다.  
FSM、オブザーバー、オブジェクトプーリングなどの**RPGの基本パターン**を設計・実装し、実践的な設計力を養いました。  
This project deepened my understanding of **RPG design patterns** like FSM, Observer, and Object Pooling, while improving my hands-on design intuition.
