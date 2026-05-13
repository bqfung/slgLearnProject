# Last War: Survival 学习项目开发计划

## 项目目标

本项目用于通过制作一款参考 Last War: Survival 核心玩法的 Unity Demo，提升中级 Unity 开发能力，并沉淀可用于笔试、面试和作品集展示的项目经验。

项目重点不是完整复刻商业游戏，而是完成一个高质量、可运行、可讲清楚技术细节的核心玩法 Demo。

## 最终交付目标

- 一个可运行的 Unity Demo。
- 一条完整战斗流程：开始战斗、移动、倍率门、自动射击、敌人波次、Boss、胜利或失败结算。
- 一套清晰的工程结构。
- 一份技术难点学习文档。
- 一份可用于面试讲解的项目总结。

## 项目信息

- 本地 Unity 工程目录：`slgLearnProject/`
- GitHub 仓库：https://github.com/bqfung/slgLearnProject.git

## 核心玩法范围

第一版只实现以下核心内容：

- 玩家小队自动向前移动。
- 玩家通过左右拖拽控制小队横向移动。
- 通过倍率门改变小队人数。
- 小兵自动攻击前方敌人。
- 敌人具有生命值、受击、死亡逻辑。
- 关卡中包含普通敌人和终点 Boss。
- 战斗结束后显示胜利或失败界面。

暂不纳入第一版的内容：

- 商城、广告、内购。
- 复杂基地经营。
- 抽卡系统。
- 联网功能。
- 完整商业化数值系统。

## 阶段计划

### 第 0 阶段：项目初始化

目标：建立 Unity 项目、基础目录和开发规范。

任务：

- 创建 Unity 3D 项目。
- 建立基础目录结构。
- 创建初始场景。
- 设置 Git 忽略规则。
- 创建基础 README。

验收标准：

- Unity 项目可以正常打开和运行。
- 场景中有基础地面、摄像机和灯光。
- 项目目录结构清晰。

状态：基本完成

### 第 1 阶段：玩家移动与小队系统

目标：完成最小可操作角色控制。

任务：

- 实现玩家小队自动向前移动。
- 实现左右拖拽或键盘横向移动。
- 实现小队成员生成。
- 实现基础阵型排列。
- 实现人数增加和减少接口。

验收标准：

- 运行后小队自动前进。
- 玩家可以控制小队左右移动。
- 可以通过代码或测试按钮改变小队人数。
- 小队人数变化后阵型自动刷新。

状态：进行中

### 第 2 阶段：倍率门系统

目标：实现跑酷关卡中的关键选择机制。

任务：

- 创建倍率门对象。
- 支持加法门，例如 +5。
- 支持乘法门，例如 x2。
- 支持减法门，例如 -3。
- 玩家经过门后更新小队人数。
- 门触发后禁用重复触发。

验收标准：

- 小队经过不同门时人数正确变化。
- 门只能触发一次。
- 门上显示对应数值。

状态：进行中

### 第 3 阶段：自动战斗系统

目标：让小兵可以自动寻找敌人并攻击。

任务：

- 实现敌人基础生命值组件。
- 实现小兵自动索敌。
- 实现武器攻击频率。
- 实现子弹或射线攻击。
- 实现敌人受击和死亡。
- 加入对象池管理子弹和特效。

验收标准：

- 小兵可以自动攻击前方敌人。
- 敌人生命值归零后死亡。
- 多个小兵攻击时不会产生明显性能问题。

状态：进行中

### 第 4 阶段：敌人波次与 Boss

目标：形成完整关卡战斗节奏。

任务：

- 实现敌人生成点。
- 实现敌人波次配置。
- 实现 Boss 血量和攻击。
- 实现玩家小队受伤和减员。
- 实现胜利和失败判断。

验收标准：

- 关卡中能出现多波敌人。
- 终点 Boss 可以被击败。
- 小队全灭时失败。
- Boss 被击败时胜利。

状态：进行中

### 第 5 阶段：UI 与流程闭环

目标：完成可玩的完整体验。

任务：

- 创建开始界面。
- 创建战斗界面。
- 显示小队人数、Boss 血条、关卡进度。
- 创建胜利界面。
- 创建失败界面。
- 支持重新开始。

验收标准：

- 从开始到结算流程完整。
- UI 状态随游戏流程切换。
- 失败或胜利后可以重新开始。

状态：进行中

### 第 6 阶段：数据驱动与工程整理

目标：把 Demo 从“能跑”整理成“可维护、可讲解”的项目。

任务：

- 用 ScriptableObject 配置士兵、敌人、武器和关卡。
- 整理 GameManager、LevelManager、SquadManager、Combat 模块职责。
- 引入事件系统解耦 UI 和战斗逻辑。
- 统一对象池接口。
- 整理命名和目录。

验收标准：

- 主要数值可以通过配置修改。
- 新增敌人或关卡不需要大改代码。
- 项目结构适合面试讲解。

状态：进行中

### 第 7 阶段：作品集亮点系统

目标：做出一个能体现技术深度的亮点。

候选方向：

- 简易关卡编辑器。
- 技能系统。
- 阵型系统。
- 战斗数值模拟器。
- 移动端性能优化报告。

建议优先级：

1. 关卡编辑器。
2. 技能系统。
3. 性能优化报告。

验收标准：

- 至少完成一个亮点系统。
- 能说明设计思路、扩展方式和遇到的问题。

状态：待开始

### 第 8 阶段：优化、打包与面试材料

目标：把项目整理成可展示成果。

任务：

- 优化帧率和 GC。
- 检查对象池、碰撞、DrawCall、Overdraw。
- 录制演示视频。
- 编写 README。
- 整理面试讲解提纲。

验收标准：

- 项目可以稳定运行。
- 有完整项目说明。
- 能围绕项目回答常见 Unity 面试问题。

状态：待开始

## 推荐目录结构

```text
Assets/
  _Project/
    Art/
    Audio/
    Materials/
    Prefabs/
    Scenes/
    Scripts/
      Core/
      Player/
      Combat/
      Enemy/
      Level/
      UI/
      Data/
    ScriptableObjects/
    VFX/
Docs/
```

## 每次开发记录格式

每完成一个功能，在本文档中补充：

- 完成日期。
- 完成功能。
- 修改的主要文件。
- 遇到的问题。
- 后续待优化内容。

## 当前下一步

下一步在 Unity 中测试第 6 阶段：点击 `SLG Learn > Build Stage 06 Data Driven Scene` 生成数据驱动测试场景，并按照 `Docs/Stage06_TestChecklist.md` 验收。

## 开发记录

### 2026-05-11：项目初始化

- 完成功能：创建 Unity 工程后，补充基础项目目录、Unity Git 忽略规则、README、开发计划和技术难点文档。
- 修改的主要文件：
  - `.gitignore`
  - `README.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `Assets/_Project/`
- 遇到的问题：Unity 工程位于 `slgLearnProject/` 子目录，文档需要放入该目录，方便和 Unity 工程一起提交到 GitHub。
- 后续待优化内容：需要在 Unity 中打开项目，让 Unity 自动生成新增目录的 `.meta` 文件，并创建正式初始场景。

### 2026-05-11：第 1 阶段基础脚本

- 完成功能：实现小队自动前进、左右输入控制、动态人数增减、基础阵型排列和摄像机跟随。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Player/SquadController.cs`
  - `Assets/_Project/Scripts/Player/SquadManager.cs`
  - `Assets/_Project/Scripts/Player/SquadDebugInput.cs`
  - `Assets/_Project/Scripts/Core/CameraFollow.cs`
- 遇到的问题：当前脚本先使用运行时胶囊体作为临时士兵，方便无美术资源时测试；后续需要替换为正式士兵 Prefab。
- 后续待优化内容：需要在 Unity 中挂载脚本并调试移动手感、阵型间距和摄像机角度。

### 2026-05-11：第 1 阶段测试场景工具

- 完成功能：新增 Unity 编辑器菜单，一键创建第 1 阶段测试场景。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/StageOneSceneBuilder.cs`
  - `Docs/Stage01_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 01 Movement Scene`。
- 后续待优化内容：运行测试后根据手感调整 `SquadController`、`SquadManager` 和 `CameraFollow` 参数。

### 2026-05-11：第 2 阶段倍率门系统

- 完成功能：实现加法、减法、乘法倍率门，并新增一键生成倍率门测试场景的编辑器菜单。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Level/GateOperation.cs`
  - `Assets/_Project/Scripts/Level/SquadGate.cs`
  - `Assets/_Project/Scripts/Editor/StageTwoSceneBuilder.cs`
  - `Docs/Stage02_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 02 Gates Scene`。
- 遇到的问题：当前门通过触发器检测小队成员，为避免多个士兵重复触发，门内部使用 `hasTriggered` 做一次性保护。
- 后续待优化内容：加入门触发特效、音效和更清晰的人数变化反馈。

### 2026-05-11：第 3 阶段自动战斗基础版

- 完成功能：实现士兵自动索敌、按频率造成伤害、敌人血量和死亡，并新增自动战斗测试场景。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Combat/SoldierWeapon.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyHealth.cs`
  - `Assets/_Project/Scripts/Player/SquadManager.cs`
  - `Assets/_Project/Scripts/Editor/StageThreeSceneBuilder.cs`
  - `Docs/Stage03_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 03 Combat Scene`。
- 遇到的问题：当前攻击先采用即时伤害，没有子弹飞行和受击表现，便于优先验证战斗数值闭环。
- 后续待优化内容：加入子弹对象池、伤害反馈、敌人反击和 Boss 战。

### 2026-05-12：第 4 阶段敌人波次与 Boss 基础版

- 完成功能：实现敌人向小队移动、近战攻击减员、Boss 血量和攻击、胜利/失败基础判断，并新增 Boss 测试场景。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Enemy/EnemyMeleeAttacker.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyHealth.cs`
  - `Assets/_Project/Scripts/Core/GameOutcomeController.cs`
  - `Assets/_Project/Scripts/Editor/StageFourSceneBuilder.cs`
  - `Docs/Stage04_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 04 Boss Scene`。
- 遇到的问题：当前波次仍是场景预摆放，不是运行时配置生成；Boss 胜利反馈暂时使用世界空间文字。
- 后续待优化内容：加入 Boss 血条、UI 流程、运行时敌人生成器和更完整的暂停/重开流程。

### 2026-05-12：第 5 阶段 UI 与流程闭环基础版

- 完成功能：实现战斗 HUD、Boss 血量显示、胜负结果面板和重新开始按钮，并新增 UI 流程测试场景。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/BattleHudController.cs`
  - `Assets/_Project/Scripts/Core/GameOutcomeController.cs`
  - `Assets/_Project/Scripts/Editor/StageFiveSceneBuilder.cs`
  - `Docs/Stage05_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 05 UI Flow Scene`。
- 遇到的问题：当前 UI 以流程验证为主，视觉样式比较简单；开始界面暂未实现。
- 后续待优化内容：增加开始界面、Boss 血条、关卡进度条、结算奖励和正式 UI 样式。

### 2026-05-13：第 6 阶段数据驱动基础版

- 完成功能：新增 `LevelConfig` 配置资产结构和运行时 `LevelBuilder`，把倍率门、敌人波次、Boss 和赛道参数从场景生成器中抽离。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/GateConfig.cs`
  - `Assets/_Project/Scripts/Data/EnemyWaveConfig.cs`
  - `Assets/_Project/Scripts/Data/BossConfig.cs`
  - `Assets/_Project/Scripts/Data/LevelConfig.cs`
  - `Assets/_Project/Scripts/Level/LevelBuilder.cs`
  - `Assets/_Project/Scripts/Editor/StageSixSceneBuilder.cs`
  - `Docs/Stage06_TestChecklist.md`
- 使用方式：打开 Unity 后点击 `SLG Learn > Build Stage 06 Data Driven Scene`，首次执行会自动创建 `Assets/_Project/ScriptableObjects/Stage06_LevelConfig.asset`，场景在 Play 时由 `LevelBuilder` 运行时生成。
- 遇到的问题：`LevelBuilder` 暂时同时负责赛道、门、敌人、Boss 和 UI 创建，职责偏多。
- 后续待优化内容：继续拆分运行时生成逻辑，并减少各阶段场景生成器的重复代码。

### 2026-05-13：第 6 阶段生成逻辑拆分

- 完成功能：将 `LevelBuilder` 中的倍率门生成、敌人波次生成和 Boss 生成拆到独立模块。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Level/GateBuilder.cs`
  - `Assets/_Project/Scripts/Enemy/EnemySpawner.cs`
  - `Assets/_Project/Scripts/Level/RuntimePrimitiveFactory.cs`
  - `Assets/_Project/Scripts/Level/LevelBuilder.cs`
- 设计结果：`LevelBuilder` 主要负责编排关卡加载流程，`GateBuilder` 负责倍率门，`EnemySpawner` 负责普通敌人和 Boss，`RuntimePrimitiveFactory` 暂时负责运行时临时材质创建。
- 遇到的问题：UI、赛道、小队和摄像机生成仍在 `LevelBuilder` 中，后续还需要继续拆分。
- 后续待优化内容：继续新增 `RoadBuilder`、`PlayerSquadFactory`，让 `LevelBuilder` 更接近纯流程编排器。

### 2026-05-13：第 6 阶段 UI 生成拆分

- 完成功能：将战斗 HUD、结果面板、重开按钮和 EventSystem 的创建逻辑从 `LevelBuilder` 拆到 `RuntimeUiBuilder`。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/RuntimeUiBuilder.cs`
  - `Assets/_Project/Scripts/Level/LevelBuilder.cs`
- 设计结果：`LevelBuilder` 不再关心 UI 具体创建细节，只在 Boss 和胜负控制器创建后调用 `RuntimeUiBuilder.BuildBattleUi`。
- 遇到的问题：UI 仍是运行时临时创建，暂未使用正式 Prefab 或 UI 资源。
- 后续待优化内容：后续可以把 `RuntimeUiBuilder` 替换为加载 UI Prefab 的实现，保留 `BattleHudController` 的数据绑定方式。

### 2026-05-13：第 6 阶段环境与玩家生成拆分

- 完成功能：将环境、玩家小队和运行时摄像机创建从 `LevelBuilder` 拆出。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Level/EnvironmentBuilder.cs`
  - `Assets/_Project/Scripts/Player/PlayerSquadFactory.cs`
  - `Assets/_Project/Scripts/Core/RuntimeCameraFactory.cs`
  - `Assets/_Project/Scripts/Level/LevelBuilder.cs`
- 设计结果：`LevelBuilder` 当前主要负责按顺序调用环境、玩家、摄像机、倍率门、敌人、Boss、胜负控制和 UI 生成模块。
- 遇到的问题：这些 Builder/Factory 仍在直接创建临时 GameObject，后续需要逐步替换为 Prefab 或资源加载。
- 后续待优化内容：引入 Prefab 配置、对象池和更正式的运行时资源管理。

### 2026-05-13：子弹对象池与攻击表现

- 完成功能：将士兵攻击从即时扣血升级为发射子弹，子弹通过对象池复用，命中敌人后造成伤害并回收。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Combat/Bullet.cs`
  - `Assets/_Project/Scripts/Combat/BulletPool.cs`
  - `Assets/_Project/Scripts/Combat/SoldierWeapon.cs`
- 设计结果：`SoldierWeapon` 只负责索敌和发射，`Bullet` 负责飞行和命中，`BulletPool` 负责创建、取出和回收子弹。
- 遇到的问题：当前子弹仍使用运行时临时球体，缺少正式 Prefab、拖尾、命中特效和音效。
- 后续待优化内容：将子弹 Prefab、命中特效和伤害数字都纳入对象池，并增加池容量监控。

### 2026-05-13：命中反馈对象池

- 完成功能：新增伤害数字和命中特效对象池，子弹命中时显示伤害数字和短暂命中闪光。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Combat/DamageNumber.cs`
  - `Assets/_Project/Scripts/Combat/DamageNumberPool.cs`
  - `Assets/_Project/Scripts/Combat/HitEffect.cs`
  - `Assets/_Project/Scripts/Combat/HitEffectPool.cs`
  - `Assets/_Project/Scripts/Combat/Bullet.cs`
- 设计结果：子弹命中后只负责触发反馈，伤害数字和命中特效各自管理生命周期与回收。
- 遇到的问题：当前反馈表现仍是临时 TextMesh 和球体，没有美术资源。
- 后续待优化内容：替换正式伤害数字 Prefab、粒子特效 Prefab，并把对象池抽象为通用池。

### 2026-05-13：普通敌人对象池

- 完成功能：新增普通敌人对象池，敌人死亡后通过死亡事件回收到池中。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Enemy/EnemyHealth.cs`
  - `Assets/_Project/Scripts/Enemy/PooledEnemy.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemySpawner.cs`
- 设计结果：`EnemyHealth` 负责派发死亡事件，`PooledEnemy` 监听死亡事件并通知 `EnemyPool` 回收，`EnemySpawner` 通过 `EnemyPool` 创建普通敌人。
- 遇到的问题：Boss 暂未池化，因为 Boss 与胜负判断强绑定，生命周期和普通敌人不同。
- 后续待优化内容：抽象通用组件池，统一子弹、反馈、敌人对象池的重复逻辑。

### 2026-05-13：通用组件对象池抽象

- 完成功能：新增 `ComponentPool<T>`，统一对象池的预热、取出、回收和容量警告逻辑。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Core/ComponentPool.cs`
  - `Assets/_Project/Scripts/Combat/BulletPool.cs`
  - `Assets/_Project/Scripts/Combat/DamageNumberPool.cs`
  - `Assets/_Project/Scripts/Combat/HitEffectPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyPool.cs`
- 设计结果：通用池只处理生命周期共性，具体池只负责创建对象和暴露业务化 `Spawn` 方法。
- 遇到的问题：当前仍是每种池一个全局 `Shared`，适合 Demo，但大型项目中需要更统一的池管理器。
- 后续待优化内容：增加池状态调试信息，例如总创建数、可用数量、峰值数量。
