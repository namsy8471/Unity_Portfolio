# Unity Mabinogi Recreated Project  
유니티로 제작한 **마비노기 모작 프로젝트**  
Unityで制作したマビノギ模倣作品  
A fan recreation of the MMORPG **Mabinogi**, built from scratch in Unity.

---

## Preview / プレビュー / 미리보기
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

2. **그리드 인벤토리 시스템 / グリッドインベントリシステム / Grid Inventory System**  
　→ 아이템 및 장비 착용/해제 구현 (최초 착용 시 객체화 후 Dictionary 기반 오브젝트 풀링 관리)  

3. **플레이어 스킬 / プレイヤースキル / Player Skills**  
　→ Smash, Defence, CounterAttack 스킬 구현  

4. **옵저버 패턴 기반 매니저 / オブザーバーパターンのマネージャー / Observer Pattern Managers**  
　→ 마우스 커서 변경을 담당하는 Game Manager, 사운드를 제어하는 Sound Manager 구현  

---

![image](https://github.com/user-attachments/assets/94e72b97-a113-4a35-ac1c-f77a2e688696)
게임 시스템 내 다양한 매니저와 서브시스템  
ゲームシステム内のさまざまなマネージャーとサブシステム  
Various Managers and Subsystems within the Game System

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
마비노기의 **스킬 및 전투 시스템**을 가장 중점적으로 구현했습니다. 기존 참고 자료가 거의 없어 **직접 설계·제작**했습니다.  
マビノギの**スキルシステムと戦闘システム**に最も力を入れました。参考資料が少なかったため、すべてを**自分で設計・実装**しました。  
The **skill and combat systems** were my main focus. Since there were few existing references, I **implemented everything from scratch** to create a smooth and intuitive gameplay experience.

---

## 실행 파일 / 実行ファイル / Executable
[Download from Google Drive](https://drive.google.com/file/d/1l2EyKP8pFqGlTZ6mFsJGTs0biJued0EW/view?usp=sharing)

---

## Summary / まとめ / 요약
이 프로젝트를 통해 FSM, 옵저버, 오브젝트 풀링 등 **RPG 핵심 패턴**을 직접 설계하며 실무적인 설계 감각을 익혔습니다.  
FSM、オブザーバー、オブジェクトプーリングなどの**RPGの基本パターン**を設計・実装し、実践的な設計力を養いました。  
This project deepened my understanding of **RPG design patterns** like FSM, Observer, and Object Pooling, while improving my hands-on design intuition.
