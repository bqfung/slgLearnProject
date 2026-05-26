# SLG 学习项目开发计划

## 项目目标

本项目用于通过制作一款 SLG 核心系统 Demo，提升中级 Unity 开发能力，并沉淀可用于笔试、面试和作品集展示的项目经验。

项目定位是：基地建设 + 资源生产 + 建筑升级 + 科技研究 + 部队训练 + 编队出征 + 战斗结算。

项目重点不是完整复刻商业 SLG，而是完成一个高质量、可运行、可讲清楚技术细节的 SLG 核心循环 Demo。当前已有的战斗、对象池、数据驱动和编辑器工具会作为可复用资产保留，但后续主线转向 SLG 局外系统。

## 最终交付目标

- 一个可运行的 Unity Demo。
- 一条完整 SLG 循环：基地产出资源、升级建筑、研究科技、训练部队、编队出征、战斗结算、回到基地继续成长。
- 一套基地系统：建筑解锁、建筑升级、资源产出和容量。
- 一套部队系统：兵种配置、训练、消耗、编队和战斗力计算。
- 一套科技/成长系统：科技树、升级消耗、效果加成。
- 一套简化战斗系统：用自动战斗或战报模拟承接编队出征结果。
- 一套清晰的工程结构。
- 一份技术难点学习文档。
- 一份可用于面试讲解的项目总结。

## 项目信息

- 本地 Unity 工程目录：`slgLearnProject/`
- GitHub 仓库：https://github.com/bqfung/slgLearnProject.git

## 核心玩法范围

第一版实现以下核心内容：

- 基地主界面。
- 资源系统：粮食、木材/钢材、金币等至少 2 种资源。
- 建筑系统：主基地、兵营、资源建筑、科技建筑。
- 建筑升级：消耗资源、需要时间或简化倒计时、升级后提升产出或解锁功能。
- 科技系统：研究科技并提供属性加成。
- 部队系统：训练士兵、维护兵力数量。
- 编队系统：选择兵种和数量形成出征队伍。
- 关卡/据点系统：选择一个敌方据点出征。
- 战斗结算：根据双方战力或简化自动战斗得到胜负、损耗和奖励。
- 本地存档：资源、建筑等级、科技等级、部队数量可保存。

暂不纳入第一版的内容：

- 商城、广告、内购。
- 完整商业 SLG 大地图。
- 抽卡系统。
- 联网功能。
- 联盟、PVP、跨服等多人系统。
- 完整商业化数值系统。
- 复杂美术资源和正式商业 UI。

## 功能对齐清单

### 基地与资源

- 基地主界面。
- 资源产出、消耗、容量。
- 建筑建造和升级。
- 升级倒计时或简化完成机制。
- 建筑效果：产出提升、容量提升、解锁兵种、解锁科技。

### 部队与战斗

- 兵种配置：步兵、射手、骑兵/车辆等。
- 训练士兵。
- 编队出征。
- 简化自动战斗或战报。
- 战斗损耗。
- 胜利奖励和失败返还。

### 科技与成长

- 科技树。
- 科技研究消耗资源和时间。
- 科技加成影响产出、训练速度、兵种属性、行军或战斗力。
- 章节/关卡推进。
- 本地存档。

### 工程与作品集亮点

- ScriptableObject 数据驱动建筑、资源、科技、兵种、关卡和战斗公式。
- 对象池和调试面板。
- 编辑器工具：配置校验、关卡/数值检查、配置规则。
- 模块划分：Core、Base、Resource、Building、Technology、Troop、Battle、Map、UI、Data、Save。
- 可讲解的扩展点：建筑效果、资源流转、科技加成、兵种克制、战斗结算、存档版本兼容。

## 需求护栏

为了避免开发过程中偏离 SLG 主需求，后续每次开发前都先检查本轮功能是否服务以下至少一项：

- 资源生产、消耗、容量或离线收益。
- 建筑建造、升级、解锁或建筑效果。
- 科技研究、前置条件或全局加成。
- 部队训练、库存、编队或战斗力计算。
- 地图据点、出征、战斗结算、奖励或损耗。
- 存档、版本兼容或长期成长。
- 建筑、资源、科技、兵种、地图或战斗公式的配置校验。

暂不作为主线扩展的内容：

- 跑酷玩法。
- 倍率门。
- Roguelike 三选一。
- 塔防或割草玩法。
- 复杂动作战斗、子弹类型堆叠或 Boss 技能深挖。
- PVP、联盟、聊天、抽卡、商城、广告和商业化系统。

仓库根目录新增 `AGENTS.md`，作为后续开发的项目方向约束。每次继续实现前，应先说明本轮属于哪个阶段、服务哪条 SLG 主循环。

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

目标：已完成旧方向的最小可操作角色控制；后续作为历史实现保留，不再作为主线继续扩展。

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

状态：已暂停，部分代码可作为战斗表现或部队展示参考。

### 第 2 阶段：倍率门系统

目标：已完成旧方向的跑酷选择机制；后续不再作为主线核心。

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

状态：已暂停，相关配置和编辑器校验经验可复用。

### 第 3 阶段：自动战斗系统

目标：已完成旧战斗验证；后续作为 SLG 出征战斗或战报表现的可复用基础。

任务：

- 实现敌人基础生命值组件。
- 实现士兵自动索敌。
- 实现武器攻击频率。
- 实现子弹或射线攻击。
- 实现敌人受击和死亡。
- 加入对象池管理子弹和特效。

验收标准：

- 士兵可以自动攻击前方敌人。
- 敌人生命值归零后死亡。
- 多个小兵攻击时不会产生明显性能问题。

状态：已暂停，后续会改造成 SLG 出征战斗或简化战报的一部分。

### 第 4 阶段：敌人波次与 Boss

目标：已完成旧战斗关卡验证；后续作为 SLG 战斗结算表现的可选基础。

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

状态：已暂停，后续优先做 SLG 局外核心。

### 第 5 阶段：UI 与流程闭环

目标：已完成旧战斗流程闭环；后续 UI 主线转向基地与局外系统。

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

状态：已暂停，后续新增基地、资源、建筑和部队 UI。

### 第 6 阶段：数据驱动与工程整理

目标：把 Demo 从“能跑”整理成“可维护、可讲解”的项目，并适配 SLG 主线。

任务：

- 用 ScriptableObject 配置建筑、资源、科技、兵种、关卡和战斗公式。
- 整理 GameManager、BaseManager、ResourceManager、BuildingManager、TroopManager、BattleManager 模块职责。
- 引入事件系统解耦 UI 和战斗逻辑。
- 统一对象池接口。
- 整理命名和目录。

验收标准：

- 主要数值可以通过配置修改。
- 新增敌人或关卡不需要大改代码。
- 项目结构适合面试讲解。

状态：进行中，已有对象池和数据驱动基础可复用。

### 第 7 阶段：作品集亮点系统

目标：做出一个能体现技术深度的亮点。当前已完成旧关卡配置工具基础版，后续优先回到 SLG 核心系统。

候选方向：

- 简易关卡编辑器。
- 科技树系统。
- 建筑升级系统。
- 战斗数值模拟器。
- 战斗数值模拟器。
- 移动端性能优化报告。

建议优先级：

1. 建筑升级和资源循环。
2. 科技树系统。
3. 部队训练和编队系统。
4. 战斗数值模拟器。

验收标准：

- 至少完成一个亮点系统。
- 能说明设计思路、扩展方式和遇到的问题。

状态：进行中，编辑器工具暂告一段落。

### 第 8 阶段：SLG 基地与资源最小闭环

目标：建立 SLG 的局外核心：资源产出、资源消耗、建筑升级。

任务：

- 新增 `ResourceConfig` 和资源库存。
- 新增 `BuildingConfig` 和建筑运行时状态。
- 实现主基地、农场/伐木场、兵营、研究所。
- 实现资源随时间产出。
- 实现建筑升级消耗资源。
- 实现建筑升级后提升产出或解锁功能。
- 新增基地主界面，显示资源和建筑列表。

验收标准：

- 资源会随时间增长。
- 点击建筑可以查看等级和升级消耗。
- 资源足够时可以升级建筑。
- 建筑升级后效果生效。
- 基地界面能形成最小成长循环。

状态：已完成最小闭环，后续可继续扩展建筑解锁、建筑队列和容量效果。

### 第 9 阶段：部队训练与编队

目标：实现 SLG 中最核心的“造兵”和“带兵出征”。

任务：

- 新增 `TroopConfig`。
- 实现步兵、射手、骑兵/车辆等基础兵种。
- 实现兵营训练士兵。
- 训练消耗资源和时间。
- 实现部队库存。
- 实现编队界面，选择兵种和数量。
- 计算编队战斗力。

验收标准：

- 至少有 2 种兵种。
- 可以消耗资源训练士兵。
- 可以把士兵加入出征编队。
- 编队战斗力会随兵种和数量变化。

状态：已完成最小闭环，后续可继续扩展训练队列容量、批量编队和兵种限制。

### 第 10 阶段：地图据点与战斗结算

目标：实现 SLG 的出征目标和战斗闭环。

任务：

- 新增地图/关卡据点配置。
- 每个据点有敌方兵力、推荐战力和奖励。
- 编队出征到据点。
- 通过战力公式或简化自动战斗计算胜负。
- 结算奖励和兵力损耗。
- 解锁下一关或下一片区域。

验收标准：

- 可以选择据点出征。
- 战斗会产生胜负、奖励和损耗。
- 奖励能回到基地资源循环。
- 据点可以按进度解锁。

状态：已完成最小闭环，并已支持据点前置条件、分支解锁和轻量章节地图节点；后续可继续扩展驻军配置、行军时间、战斗公式和表现层。

### 第 11 阶段：科技树与全局加成

目标：实现 SLG 长线成长和策略选择。

任务：

- 新增 `TechnologyConfig`。
- 实现科技树节点。
- 科技研究消耗资源和时间。
- 科技提供资源产出、训练速度、兵种攻击/生命等加成。
- 科技前置条件和建筑等级条件。

验收标准：

- 可以研究科技。
- 科技会影响资源、训练或战斗数值。
- 科技前置条件生效。

状态：已完成最小闭环，并已支持前置科技和建筑等级条件；后续可继续扩展正式树状 UI 和更多效果类型。

### 第 12 阶段：存档、离线收益与数值校验

目标：补齐 SLG Demo 的长期运行基础。

任务：

- 本地存档资源、建筑、科技、部队和地图进度。
- 处理存档版本号。
- 计算离线资源收益。
- 增加资源上限。
- 增加配置校验：建筑消耗、科技前置、兵种数值、关卡推荐战力。

验收标准：

- 重启游戏后进度保留。
- 离线一段时间后资源收益正确。
- 配置错误能被工具提示。

状态：进行中，已完成存档缺字段容错、离线科技推进和 Stage 08-11 默认配置校验。

### 第 13 阶段：优化、打包与面试材料

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
      Base/
      Resource/
      Building/
      Technology/
      Troop/
      Battle/
      Map/
      Save/
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

下一步继续第 10 阶段核心玩法：增加章节地图标题、区域奖励和章节完成条件。

优先实现顺序：

1. 增加章节地图标题、区域奖励和章节完成条件。
2. 增加兵种克制或阵型加成，让战斗公式具备策略选择。
3. 增加一键扫荡入口，复用重复挑战奖励但跳过手动编队出征。
4. 增加行军队列上限和队列解锁条件。
5. 再回头补配置校验窗口搜索过滤、离线收益报告和迁移测试样例。

## 开发记录

### 2026-05-15：项目方向调整为 SLG 核心系统

- 完成功能：明确项目目标为 SLG 学习 Demo，主线调整为基地建设、资源生产、建筑升级、科技研究、部队训练、编队出征和战斗结算。
- 修改的主要文件：
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 设计结果：跑酷移动、倍率门和旧战斗作为历史验证内容保留，后续主线切到 SLG 局外系统。
- 核心对齐：先做基地资源最小闭环，再做部队训练和编队，再做地图据点和战斗结算，最后补科技树、存档和离线收益。
- 后续待优化内容：开始第 8 阶段，新增资源配置、建筑配置、资源产出和建筑升级 UI。

### 2026-05-15：第 8 阶段基地与资源最小闭环

- 完成功能：新增资源配置、建筑配置、资源库存、建筑运行时状态、基地运行时和临时基地 UI。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/ResourceConfig.cs`
  - `Assets/_Project/Scripts/Data/BuildingConfig.cs`
  - `Assets/_Project/Scripts/Data/BuildingLevelConfig.cs`
  - `Assets/_Project/Scripts/Data/ResourceAmount.cs`
  - `Assets/_Project/Scripts/Data/BuildingProduction.cs`
  - `Assets/_Project/Scripts/Resource/ResourceInventory.cs`
  - `Assets/_Project/Scripts/Building/BuildingRuntimeState.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiInstaller.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage08_TestChecklist.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene`，进入 Play 后观察资源产出并点击建筑升级。
- 设计结果：`ResourceInventory` 只负责资源数量，`BuildingRuntimeState` 只负责单个建筑状态，`SlgBaseRuntime` 负责资源产出和升级流程，UI 只做展示和按钮调用。
- 学习价值：这是 SLG 最小闭环的起点，核心不是战斗表现，而是资源流转和成长反馈。
- 后续待优化内容：加入存档、离线收益、建筑解锁、资源容量建筑效果和建筑升级队列。

### 2026-05-15：第 8 阶段本地存档与离线收益

- 完成功能：新增 `PlayerPrefs + JsonUtility` 本地存档，保存资源数量、建筑等级、升级剩余时间和保存时间。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Resource/ResourceInventory.cs`
  - `Assets/_Project/Scripts/Building/BuildingRuntimeState.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage08_TestChecklist.md`
- 使用方式：运行第 8 阶段场景，升级建筑或等待资源增长，停止 Play 后再次 Play 验证状态恢复；可点击 `SLG Learn > Clear Stage 08 SLG Save` 清空测试存档。
- 设计结果：运行时变化先标记为 dirty，再按固定间隔自动保存，避免资源每帧变化时频繁写入。
- 离线收益：读取存档时根据当前时间和保存时间计算离线秒数，按建筑生产结算资源，并推进建筑升级倒计时。
- 学习价值：SLG 的长期成长必须有存档和离线收益，这是区分“单局 Demo”和“SLG 循环”的关键。
- 后续待优化内容：改为文件存档、增加存档版本迁移、支持更精确的离线升级分段结算。

### 2026-05-15：第 9 阶段部队训练最小闭环

- 完成功能：新增兵种配置、部队库存、训练订单、训练倒计时、训练存档和基础部队 UI。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/TroopConfig.cs`
  - `Assets/_Project/Scripts/Troop/TroopInventory.cs`
  - `Assets/_Project/Scripts/Troop/TroopTrainingOrder.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage09_TestChecklist.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 后进入 Play，在右侧部队面板点击 `Train` 训练 Infantry 或 Archer。
- 设计结果：`TroopConfig` 定义兵种和训练成本，`TroopInventory` 保存库存，`TroopTrainingOrder` 表示一条训练倒计时，`SlgBaseRuntime` 统一推进训练并在完成时增加库存。
- 学习价值：这是 SLG “造兵”循环的起点，资源不再只用于建筑，也可以转化为部队和战斗力。
- 后续待优化内容：新增编队界面、训练队列容量、兵营等级限制、训练速度加成和出征战斗结算。

### 2026-05-15：第 9 阶段编队最小闭环

- 完成功能：新增出征编队运行时状态，支持把库存士兵加入编队或从编队移回库存。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Troop/MarchFormation.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Docs/Stage09_TestChecklist.md`
- 使用方式：训练完成后，在右侧兵种面板点击 `+` 将 1 个士兵加入编队，点击 `-` 将 1 个士兵移回库存。
- 设计结果：`TroopInventory` 表示基地库存，`MarchFormation` 表示当前出征编队，二者职责分离；编队数量也会进入存档。
- 学习价值：SLG 的“有兵”和“带出去的兵”不是一回事，库存与编队分离能为后续出征、损耗和返回打基础。
- 后续待优化内容：新增地图据点、出征按钮、战斗结算、损耗回写和一键调整数量。

### 2026-05-15：第 10 阶段据点进度与顺序解锁

- 完成功能：在地图据点与战斗结算基础上，新增据点攻克状态、顺序解锁、已攻克据点禁止重复挑战，以及攻克状态存档。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：先攻克 `Outpost`，胜利后 `Outpost` 会显示 `Cleared`，`Raider Camp` 从 `Locked` 变为 `Unlocked`。
- 设计结果：据点数组顺序就是当前阶段的地图推进顺序；第一个据点默认解锁，后续据点依赖前一个据点已攻克。
- 学习价值：这是 SLG “关卡推进/章节解锁”的最小实现，先用线性顺序保证闭环清楚，后续再升级成章节、区域或多分支地图。
- 后续待优化内容：新增章节奖励、可视化大地图、多分支解锁条件、首通奖励和可重复扫荡奖励的区分。

### 2026-05-15：第 11 阶段科技树与全局加成

- 完成功能：新增科技配置、科技运行时状态、科技研究倒计时、研究消耗、科技存档和科技 UI。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/TroopConfig.cs`
  - `Assets/_Project/Scripts/Troop/TroopTrainingOrder.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage11_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 后进入 Play，在右侧科技面板点击 `Research`。
- 设计结果：`Agriculture` 提升资源产出，`Military Drill` 缩短训练时间并提升部队战力；科技等级和研究剩余时间会进入存档。
- 学习价值：科技系统把资源、训练、战力和地图推进连成长期成长线，是 SLG 区别于单局玩法的重要系统。
- 后续待优化内容：新增科技前置条件、建筑等级条件、正式科技树 UI、效果校验和更丰富的加成类型。

### 2026-05-15：第 12 阶段存档容错、离线科技与配置校验

- 完成功能：新增存档版本号常量、存档缺字段容错、离线科技研究推进，以及 SLG 配置校验菜单。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Validate All SLG Configs` 校验 `Assets/_Project/ScriptableObjects` 目录下的 SLG 配置。
- 设计结果：读取旧存档时会补齐空列表，避免新增 `strongholds`、`technologies` 等字段后旧存档空引用；离线结算会推进资源、建筑、训练和科技。
- 学习价值：SLG 系统会持续加字段，存档容错和配置校验是长期项目稳定性的基本盘。
- 后续待优化内容：新增正式存档迁移表、校验 Inspector 面板、配置定位跳转、离线分段结算和自动化测试。

### 2026-05-15：第 12 阶段配置校验扫描与资产路径定位

- 完成功能：配置校验从固定默认资产升级为扫描 `Assets/_Project/ScriptableObjects` 目录，并在 Error/Warning 中输出具体资产路径。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 使用方式：点击 `SLG Learn > Validate All SLG Configs`。
- 设计结果：新增配置资产后不需要改校验器路径，校验器会按资源、建筑、兵种、据点和科技类型自动收集资产。
- 学习价值：配置校验工具必须能跟随内容规模增长，扫描目录和输出资产路径是从 Demo 工具走向生产工具的重要一步。
- 后续待优化内容：增加点击定位、独立校验窗口、构建前自动校验和更细的科技前置条件校验。

### 2026-05-15：第 11/12 阶段科技前置条件与校验

- 完成功能：科技等级配置新增前置科技和建筑等级条件，运行时研究会检查条件，UI 会显示 Locked，配置校验会检查前置引用和等级范围。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/TroopConfig.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage11_TestChecklist.md`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 使用方式：在 `TechnologyConfig` 的等级配置中设置 `Technology Prerequisites` 或 `Building Requirements`。
- 设计结果：科技前置条件配置在等级上，能表达“1 级门槛低、2 级门槛高”的常见 SLG 科技树结构。
- 学习价值：科技树不是按钮集合，前置条件和配置校验才是它作为长期成长系统的关键。
- 后续待优化内容：正式科技树视图、点击定位配置问题、科技依赖环检测和构建前自动校验。

### 2026-05-15：第 12 阶段配置校验窗口

- 完成功能：新增 `SLG Learn > Config Validator` 编辑器窗口，可一键扫描 SLG 配置、统计 Error/Warning、显示资产路径并选中问题资产。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Config Validator` 打开窗口，再点击 `Scan All SLG Configs`。
- 设计结果：菜单校验和窗口校验共用 `RunSlgConfigValidation`，避免维护两套规则。
- 学习价值：配置校验从一次性 Console 输出升级为可交互工具，能更好地服务配置排查和作品集展示。
- 后续待优化内容：搜索过滤、按资产类型筛选、点击定位到 Inspector 字段、构建前自动校验。

### 2026-05-15：第 12 阶段存档迁移框架

- 完成功能：新增 `SlgSaveMigrator`，存档读取统一经过迁移入口，当前支持把旧版本缺失字段补齐到当前版本。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 使用方式：运行时读取存档会自动调用 `SlgSaveMigrator.MigrateToCurrent`。
- 设计结果：兼容逻辑集中到迁移器中，后续新增字段、字段改名或数据拆分时，可以按版本追加迁移方法。
- 学习价值：SLG 是长期养成游戏，存档版本演进是核心基础设施，不能散落在各个系统里临时兼容。
- 后续待优化内容：增加迁移测试样例、文件存档备份、字段改名迁移案例和异常存档恢复策略。

### 2026-05-18：第 12 阶段离线收益分段结算

- 完成功能：离线结算从一次性批处理改为按最近时间型事件分段，支持建筑升级、训练完成和科技完成后继续用新状态结算剩余离线时间。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 使用方式：正常读取存档即可触发，无需额外操作。
- 设计结果：离线期间建筑升级或科技研究中途完成后，后续离线资源产出会吃到新建筑等级或新科技加成。
- 学习价值：SLG 离线收益要按时间事件推进，否则会出现长期养成数值误差。
- 后续待优化内容：增加离线收益报告、离线分段自动化测试和更细的多队列训练结算。

### 2026-05-18：第 12 阶段构建前自动配置校验

- 完成功能：新增构建前处理器，Build 前自动执行 SLG 配置校验，存在 Error 时阻止构建，Warning 只输出日志。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：正常执行 Unity Build 即可自动触发。
- 设计结果：菜单校验、校验窗口、构建前校验共用同一套 `RunSlgConfigValidation` 规则。
- 学习价值：配置校验不应只依赖人工点击，构建前自动阻断是项目质量门禁的一部分。
- 后续待优化内容：增加可配置开关、CI 日志格式、错误白名单和更细粒度的问题定位。

### 2026-05-18：第 10 阶段据点前置条件与分支解锁

- 完成功能：据点配置新增前置据点 id 列表，运行时会按前置条件判断解锁状态，并新增 `Supply Depot` 作为 `Outpost` 后的第二条分支目标。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/StrongholdConfig.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 后进入 Play，初始只有 `Outpost` 可攻打；攻克 `Outpost` 后，`Raider Camp` 和 `Supply Depot` 会同时解锁。
- 设计结果：优先读取 `StrongholdConfig.PrerequisiteStrongholdIds` 作为显式解锁条件；如果旧配置没有填写前置条件，则保留按数组顺序解锁的兼容逻辑。
- 学习价值：SLG 地图推进不能长期依赖硬编码顺序，配置化前置条件是章节、区域、多分支路线和活动地图的基础。
- 后续待优化内容：章节地图 UI、节点连线、行军时间、首通/扫荡奖励拆分、据点驻军配置和更完整的战斗公式。

### 2026-05-18：第 10 阶段轻量章节地图节点

- 完成功能：据点配置新增地图坐标，运行时 UI 从纯列表升级为节点卡片，并按前置关系绘制分支连线；节点和连线会随 Locked、Unlocked、Cleared 状态变色。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/StrongholdConfig.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 后进入 Play，观察 `Outpost` 分叉连接到 `Raider Camp` 和 `Supply Depot`；攻克 `Outpost` 后两个分支节点变为可攻打。
- 设计结果：`StrongholdConfig.MapPosition` 保存节点坐标，UI 根据据点前置关系自动生成连线，状态刷新仍统一走 `BaseHudController.RefreshStrongholdRow`。
- 学习价值：SLG 地图推进不只是列表数据，玩家需要看到“当前位置、下一步目标和分支路线”；节点坐标与前置关系分离后，后续可扩展章节地图、区域地图和活动地图。
- 后续待优化内容：据点驻军配置、行军时间、章节奖励、地图缩放/滚动、正式 UI Prefab 和更完整的战斗公式。

### 2026-05-18：第 10 阶段据点驻军配置

- 完成功能：据点配置新增驻军列表，支持配置敌方兵种 id 和数量；战斗结算从比较推荐战力升级为比较我方编队战力和据点敌军战力。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/StrongholdConfig.cs`
  - `Assets/_Project/Scripts/Battle/BattleReport.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 生成默认配置；`Outpost`、`Raider Camp`、`Supply Depot` 会自动写入不同驻军构成，UI 显示敌军战力和驻军数量。
- 设计结果：敌军战力由 `StrongholdConfig.Garrison` 按兵种基础战力累加；如果旧据点没有驻军配置，则回退使用 `Recommended Power`，避免旧配置失效。
- 学习价值：SLG 据点不应只是一个战力数字，驻军构成是后续兵种克制、侦查、行军目标和战斗模拟的基础。
- 后续待优化内容：首通/扫荡奖励拆分、行军队列、兵种克制、敌军损耗展示和更完整的战斗报告。

### 2026-05-18：第 10 阶段首通奖励与重复挑战奖励拆分

- 完成功能：据点新增重复挑战奖励；首次胜利发放首通奖励并记录攻克状态，之后重复挑战胜利只发放重复奖励。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/StrongholdConfig.cs`
  - `Assets/_Project/Scripts/Battle/BattleReport.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：首次攻克据点时战报显示 `First Clear` 并发放 `Rewards`；再次挑战已攻克据点时战报显示 `Repeat` 并发放 `Repeat Rewards`。
- 设计结果：保留旧字段 `Rewards` 作为首通奖励，新增 `RepeatRewards` 作为重复挑战奖励；已攻克据点仍可再次攻击，但不会再次发放首通奖励。
- 学习价值：SLG 地图奖励必须区分一次性推进奖励和可重复资源入口，否则会破坏资源经济和成长节奏。
- 后续待优化内容：行军队列、扫荡按钮、扫荡次数限制、重复奖励衰减、章节完成奖励和更完整的奖励预览 UI。

### 2026-05-18：第 10 阶段行军队列与行军时间

- 完成功能：点击据点 `Attack` 后不再立即结算，而是创建行军订单；编队离开基地，行军倒计时结束后再结算战斗。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Troop/MarchOrder.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveData.cs`
  - `Assets/_Project/Scripts/Save/SlgSaveSystem.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assembly-CSharp.csproj`
  - `Docs/Stage10_TestChecklist.md`
  - `Docs/Stage12_TestChecklist.md`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：训练士兵并加入编队，点击据点 `Attack`，目标节点会显示 `Marching X.Xs`；倒计时结束后生成战报、发放奖励并返还幸存士兵。
- 设计结果：`MarchOrder` 保存目标据点、出征兵种数量和剩余时间；存档版本升级到 3，新增 `marchOrders` 字段；离线分段结算会推进行军并在到达时结算。
- 学习价值：SLG 的出征不是一次按钮调用，而是一个时间队列；行军订单把“玩家资产离开基地、等待、到达结算、返还幸存部队”串成更真实的 SLG 流程。
- 后续待优化内容：行军队列上限、行军速度科技、召回/加速、行军中 UI 列表、队列解锁和扫荡入口。

### 2026-05-19：默认场景切换为 GameObject 可视化

- 完成功能：默认运行表现从屏幕调试 UI 改为世界 GameObject 可视化，生成基地地块、地图地块、建筑、兵营训练点、据点节点、分支连线和 3D 操作牌。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiInstaller.cs`
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Assembly-CSharp.csproj`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 使用方式：点击 `SLG Learn > Build Stage 08 SLG Base Scene` 重新生成场景后进入 Play；点击场景中的 `Upgrade`、`Train`、`+`、`-`、`Attack` 操作牌进行交互。
- 设计结果：保留 `SlgBaseRuntime` 等底层 SLG 逻辑，只替换表现层；新的 `WorldSlgVisualBuilder` 根据运行时数据生成可点击世界对象，并用 TextMesh 显示资源、章节、战报和状态。
- 学习价值：玩法系统和表现层解耦后，可以把难看的调试 UI 替换成游戏化场景，而不用推翻资源、建筑、部队、行军和战斗结算逻辑。
- 后续待优化内容：替换正式 Prefab、美术模型、建筑升级表现、行军路线动画、相机交互和更清晰的 3D 图标。

### 2026-05-15：第 10 阶段地图据点与战斗结算

- 完成功能：新增据点配置、战斗报告、出征结算、奖励发放、士兵损耗和战报 UI。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/StrongholdConfig.cs`
  - `Assets/_Project/Scripts/Battle/BattleReport.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Troop/MarchFormation.cs`
  - `Assets/_Project/Scripts/UI/BaseHudController.cs`
  - `Assets/_Project/Scripts/UI/RuntimeBaseUiBuilder.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Docs/Stage10_TestChecklist.md`
- 使用方式：训练士兵并加入编队后，点击据点列表中的 `Attack`。编队战力达到推荐战力则胜利，否则失败。
- 设计结果：`StrongholdConfig` 保存推荐战力、胜利/失败损耗率和奖励；`BattleReport` 保存最近一次战斗结果；`SlgBaseRuntime.TryAttackStronghold` 统一处理胜负、奖励、损耗和编队清空。
- 学习价值：这是 SLG “编队出征 -> 战斗结算 -> 奖励回基地”的最小闭环，战斗先用公式验证资源和部队流转。
- 后续待优化内容：新增兵种克制、战斗力公式配置和战斗过程表现。

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

### 2026-05-13：对象池调试面板

- 完成功能：新增运行时对象池调试面板，显示子弹、伤害数字、命中特效和敌人池的 active/free/total 数量。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Core/ComponentPool.cs`
  - `Assets/_Project/Scripts/UI/PoolDebugOverlay.cs`
  - `Assets/_Project/Scripts/UI/RuntimeUiBuilder.cs`
- 设计结果：`ComponentPool<T>` 暴露只读统计信息，`PoolDebugOverlay` 定时读取各池 `Shared` 实例并显示。
- 遇到的问题：当前面板是开发期调试 UI，后续正式展示或打包时应可关闭。
- 后续待优化内容：增加开关配置、峰值统计和池扩容次数统计。

### 2026-05-14：对象池调试信息增强

- 完成功能：对象池调试面板新增峰值使用量和扩容次数，并支持按 F3 显示或隐藏。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Core/ComponentPool.cs`
  - `Assets/_Project/Scripts/UI/PoolDebugOverlay.cs`
  - `Docs/Stage06_TestChecklist.md`
  - `Docs/TechnicalNotes.md`
- 设计结果：`ComponentPool<T>` 记录 `PeakActiveCount` 和 `ExpansionCount`，调试面板显示 active/free/total/peak/grow。
- 遇到的问题：当前开关按键写在组件默认值中，后续可以迁移到配置或调试菜单。
- 后续待优化内容：增加对象池统计重置按钮，并在超容量扩容时给出更明显的 UI 提示。

### 2026-05-14：运行时表现配置

- 完成功能：新增 `VisualConfig`，将子弹、伤害数字、命中特效、敌人、Boss、士兵、倍率门和基础 UI 的颜色/尺寸参数集中配置。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/VisualConfig.cs`
  - `Assets/_Project/Scripts/Data/LevelConfig.cs`
  - `Assets/_Project/Scripts/Level/RuntimePrimitiveFactory.cs`
  - `Assets/_Project/Scripts/Editor/StageSixSceneBuilder.cs`
  - `Assets/_Project/Scripts/Combat/BulletPool.cs`
  - `Assets/_Project/Scripts/Combat/DamageNumberPool.cs`
  - `Assets/_Project/Scripts/Combat/HitEffect.cs`
  - `Assets/_Project/Scripts/Combat/HitEffectPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemySpawner.cs`
  - `Assets/_Project/Scripts/Level/GateBuilder.cs`
  - `Assets/_Project/Scripts/Player/SquadManager.cs`
  - `Assets/_Project/Scripts/UI/RuntimeUiBuilder.cs`
- 使用方式：点击 `SLG Learn > Build Stage 06 Data Driven Scene` 后会自动创建 `Assets/_Project/ScriptableObjects/Stage06_VisualConfig.asset`，修改配置后重新进入 Play 可验证表现变化。
- 设计结果：运行时对象仍可自动生成，但表现参数开始从配置读取，为后续替换正式 Prefab 做准备。
- 后续待优化内容：将临时球体、胶囊体、TextMesh 替换为配置化 Prefab。

### 2026-05-14：Prefab 配置入口

- 完成功能：`VisualConfig` 新增子弹、命中特效、敌人、Boss、士兵 Prefab 引用，运行时创建逻辑优先使用配置 Prefab，未配置时回退到临时几何体。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/VisualConfig.cs`
  - `Assets/_Project/Scripts/Level/RuntimePrimitiveFactory.cs`
  - `Assets/_Project/Scripts/Combat/BulletPool.cs`
  - `Assets/_Project/Scripts/Combat/HitEffectPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemyPool.cs`
  - `Assets/_Project/Scripts/Enemy/EnemySpawner.cs`
  - `Assets/_Project/Scripts/Player/SquadManager.cs`
- 设计结果：资源替换路径已经打通，后续只要把 Prefab 拖到 `Stage06_VisualConfig.asset` 上即可替换运行时表现。
- 遇到的问题：伤害数字、倍率门和 UI 仍是代码创建，暂未 Prefab 化。
- 后续待优化内容：继续将伤害数字、倍率门、HUD/结果面板改为 Prefab 引用。

### 2026-05-14：第 7 阶段关卡编辑器基础版

- 完成功能：新增 `LevelConfig` 自定义 Inspector 和关卡配置校验器。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelConfigValidator.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 使用方式：选中 `Stage06_LevelConfig.asset`，在 Inspector 中点击 `Validate Config` 或 `Generate Preview Scene For This Config`。
- 设计结果：校验逻辑和 Inspector 展示分离，`LevelConfigValidator` 负责规则，`LevelConfigEditor` 负责按钮和展示。
- 当前校验规则：道路长度/宽度、倍率门位置和值、敌人波次位置/数量/血量、Boss 位置/血量、VisualConfig 是否配置。
- 后续待优化内容：增加赛道可视化预览、门/波次重叠检测、关卡难度估算。

### 2026-05-14：按当前配置生成预览场景

- 完成功能：`LevelConfigEditor` 的生成按钮改为使用当前选中的 `LevelConfig`，并生成独立预览场景。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/StageSixSceneBuilder.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 设计结果：菜单仍可生成默认 `Stage06_DataDriven` 场景，Inspector 按钮则生成 `Preview_<ConfigName>.unity`，方便多个关卡配置独立预览。
- 后续待优化内容：区分阻断性错误和普通提醒，并加入更明确的错误定位。

### 2026-05-14：预览生成前自动校验

- 完成功能：点击 `Generate Preview Scene For This Config` 时先自动运行配置校验。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 设计结果：如果配置存在问题，Inspector 会显示校验结果，并弹窗让开发者选择取消或继续生成。
- 学习价值：这是编辑器工具常见闭环，避免错误配置直接进入预览，同时保留“带警告继续验证”的灵活性。
- 后续待优化内容：为问题增加定位信息，让开发者更快找到对应配置项。

### 2026-05-14：关卡校验结果分级

- 完成功能：`LevelConfigValidator` 的结果从字符串升级为 `LevelConfigIssue`，支持 Error 和 Warning 两种级别。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelConfigValidator.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 设计结果：道路、倍率门、敌人波次、Boss 等会影响关卡可运行性的配置问题显示为 Error；VisualConfig 缺失显示为 Warning。
- 工具行为：生成预览场景前只有存在 Error 时才弹出确认框，只有 Warning 时允许直接生成。
- 学习价值：编辑器工具不只是发现问题，还要按风险分级，减少不必要的打断。
- 后续待优化内容：基于定位信息增加点击跳转或高亮对应 Inspector 字段。

### 2026-05-14：关卡校验问题定位

- 完成功能：`LevelConfigIssue` 新增 `PropertyPath`，校验结果会显示到具体配置字段。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelConfigValidator.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 设计结果：常见问题会显示类似 `boss.position`、`gates.Array.data[0].z`、`enemyWaves.Array.data[0].count` 的路径。
- 学习价值：配置工具要尽量缩短“发现问题到修正问题”的距离，定位信息比单纯报错更接近真实生产工具。
- 后续待优化内容：基于 `PropertyPath` 增加点击定位或高亮对应 Inspector 字段。

### 2026-05-14：关卡节奏间距校验

- 完成功能：新增门与门、敌人波次与波次、门与敌人波次、Boss 与最后一波敌人的距离检查。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelConfigValidator.cs`
  - `Docs/Stage07_TestChecklist.md`
  - `Docs/TechnicalNotes.md`
- 当前阈值：
  - Gate spacing：至少 8。
  - Enemy wave spacing：至少 10。
  - Gate-wave spacing：至少 6。
  - Boss after last wave spacing：至少 12。
- 设计结果：间距问题显示为 Warning，因为它通常不阻断运行，但会影响关卡节奏和可读性。
- 学习价值：关卡工具不只检查“能不能跑”，也应该辅助设计体验，帮助提前发现节奏问题。
- 后续待优化内容：为不同关卡类型准备多套阈值配置，并支持快速切换。

### 2026-05-14：校验阈值配置资产

- 完成功能：新增编辑器用 `LevelValidationSettings`，将关卡节奏间距阈值从代码常量迁移到 ScriptableObject 配置。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Editor/LevelValidationSettings.cs`
  - `Assets/_Project/ScriptableObjects/Stage07_LevelValidationSettings.asset`
  - `Assets/_Project/Scripts/Editor/LevelValidationSettingsProvider.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigValidator.cs`
  - `Assets/_Project/Scripts/Editor/LevelConfigEditor.cs`
  - `Docs/Stage07_TestChecklist.md`
- 使用方式：选中 `Stage07_LevelValidationSettings.asset` 修改间距阈值，再回到 `LevelConfig` Inspector 点击 `Validate Config`。
- 设计结果：`LevelConfigEditor` 默认加载校验配置，`LevelConfigValidator` 接收配置参数后按配置阈值生成 Warning。
- 学习价值：把工具规则数据化，能让不同关卡类型使用不同校验标准，也减少改代码带来的风险；编辑器专用配置放在 Editor 目录，边界更清楚。
- 后续待优化内容：给不同难度或章节准备多套校验配置，并支持在 Inspector 中快速切换。

### 2026-05-19：世界场景去文字化与胶囊体状态表现

- 完成功能：把默认运行场景的 SLG 表现从“大量 TextMesh 调试文字”改为“GameObject 状态可视化”。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
  - `README.md`
- 表现规则：
  - 建筑用立方体表示，等级越高体块越高；升级中、满级和普通状态用不同颜色区分。
  - 兵营/兵种用圆柱平台表示，预备兵用蓝色胶囊体，出征编队用绿色胶囊体。
  - 据点用球体节点表示，锁定、可攻打、已攻克分别用不同颜色。
  - 据点敌军强度用红色胶囊体数量表示，不再把敌军、奖励、状态都堆成大段文字。
  - 操作牌只保留短符号：`U` 升级、`T` 训练、`+` 加入编队、`-` 移出编队、`A` 攻击。
- 设计结果：运行场景更像一个可观察的 SLG 沙盘，文字只承担短标签和概览信息，不再遮挡核心玩法对象。
- 顺手修正：不可用按钮现在不只会变灰，也会真正禁止点击，避免资源不足时仍能触发操作。
- 后续待优化内容：把临时 Primitive 替换为 Prefab，增加悬停详情面板，让详细数值按需出现，而不是常驻在场景里。

### 2026-05-19：选中详情面板

- 完成功能：新增右侧选中详情面板，点击建筑、兵种训练点或据点后显示对应的 SLG 决策数据。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 详情内容：
  - 建筑：等级、产出、升级状态、升级消耗和升级时间。
  - 兵种：预备数量、编队数量、单兵战力、训练时间、训练消耗和训练队列数量。
  - 据点：锁定/可攻打/已攻克/行军状态、敌军战力、我方编队战力、驻军、奖励、前置条件和损耗率。
- 设计结果：3D 场景只显示短标签、形状和颜色，详细数值移到“选中后查看”的屏幕面板中，避免再次出现满屏文字。
- 后续待优化内容：把详情面板升级为正式 UI Prefab，增加操作按钮、选中高亮、移动端适配和科技详情入口。

### 2026-05-20：选中高亮与详情操作按钮

- 完成功能：点击建筑、兵种训练点或据点后，会在对象脚下显示黄色选中标记，并在详情面板底部显示可执行操作。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 操作入口：
  - 建筑详情：`Upgrade`。
  - 兵种详情：`Train`、`Add`、`Remove`。
  - 据点详情：`Attack`。
- 交互规则：详情按钮会根据资源、库存、编队、锁定、行军等状态自动启用或禁用。
- 设计结果：玩家可以先在 3D 沙盘中选择目标，再在右侧详情面板中完成决策操作，交互更接近正式 SLG。
- 后续待优化内容：把运行时 UI 迁移为正式 Prefab，增加科技详情入口、选中音效/动画和移动端布局。

### 2026-05-20：科技系统可视化入口

- 完成功能：在基地沙盘中新增科技节点，点击后可以查看科技等级、研究消耗、研究时间、前置条件和科技效果，并可直接执行研究。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 表现规则：
  - 科技节点使用紫色球体。
  - 条件未满足显示为灰色。
  - 研究中显示为黄色，并显示剩余秒数。
  - 满级显示为绿色。
  - 被选中时同样显示黄色选中标记。
- 设计结果：科技研究也进入了“3D 沙盘选择 -> 详情面板查看 -> 详情按钮操作”的核心交互闭环。
- 后续待优化内容：把科技节点连成树状前置关系，增加研究所建筑等级解锁提示，并把科技节点替换成正式 Prefab。

### 2026-05-20：科技前置连线与下一目标提示

- 完成功能：科技节点会根据配置中的科技前置关系绘制连线，场景顶部新增 `Next` 目标提示。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 科技连线规则：
  - 从科技配置的所有等级中收集 `TechnologyPrerequisites`。
  - 如果前置科技节点存在，则自动绘制连线。
  - 当前研究条件满足时连线变亮，条件未满足时连线变暗。
- 下一目标规则：
  - 行军中：提示等待行军。
  - 章节还有未攻克据点：优先提示解锁、编队、训练或攻打据点。
  - 章节目标完成后：提示可研究科技。
  - 没有可研究科技时：提示可升级建筑。
  - 都不可执行时：提示继续积累资源。
- 设计结果：玩家不只知道“当前选中了什么”，也能知道“为什么科技锁住”和“下一步该做什么”。
- 后续待优化内容：把目标提示扩展为任务列表，增加点击目标自动选中对象，并为科技树加入正式布局规则。

### 2026-05-20：下一目标点击跳转

- 完成功能：`Next` 提示旁新增 `Go` 操作牌，点击后会自动选中当前推荐目标。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 跳转规则：
  - 提示攻打或解锁据点时，`Go` 自动选中对应据点或阻塞前置据点。
  - 提示训练时，`Go` 自动选中可训练兵种；资源不足时选中第一个兵种供查看消耗。
  - 提示加入编队时，`Go` 自动选中有预备兵的兵种。
  - 提示研究科技时，`Go` 自动选中可研究科技。
  - 提示升级建筑时，`Go` 自动选中可升级建筑。
  - 没有明确可操作目标时，`Go` 自动禁用。
- 设计结果：`Next` 从静态提示升级为轻量任务引导，玩家可以一键定位当前最该处理的对象。
- 后续待优化内容：把单个 `Next` 扩展为 2-3 条任务列表，并支持任务奖励和完成反馈。

### 2026-05-20：任务列表与多目标跳转

- 完成功能：新增左上角 `Objectives` 面板，最多展示 3 条当前可处理目标，每条目标都有独立 `Go` 按钮。
- 修改的主要文件：
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 任务来源：
  - 章节推进目标：解锁/攻打据点、训练士兵、加入编队。
  - 成长目标：研究可用科技、升级可用建筑。
  - 等待状态：行军中显示等待行军。
- 交互规则：任务列表只负责定位对象，不自动执行升级、训练、研究或攻击，避免玩家误操作。
- 设计结果：`Next` 仍作为主目标摘要，`Objectives` 面板提供更多候选目标，形成任务系统雏形。
- 后续待优化内容：增加任务完成反馈、奖励领取、任务排序权重和点击任务后镜头平滑移动。

### 2026-05-20：不同兵种与克制关系

- 完成功能：兵种配置新增兵种类型、克制目标和克制倍率，默认示例兵种从 2 种扩展为 4 种。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Data/TroopConfig.cs`
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/Editor/StageEightSceneBuilder.cs`
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 默认兵种：
  - `Infantry`：步兵，克制骑兵。
  - `Archer`：射手，克制步兵。
  - `Cavalry`：骑兵，克制射手。
  - `Siege`：攻城，克制步兵，成本高、训练慢、单体战力高。
- 战斗变化：
  - 据点驻军会计算主力兵种类型。
  - 出征部队如果克制据点主力类型，结算战力会乘以兵种配置的克制倍率。
  - 据点详情中的我方战力改为显示“针对当前据点驻军计算后的编队战力”。
- 默认配置变化：
  - 新增 `Stage14_Cavalry.asset` 和 `Stage14_Siege.asset`。
  - `Supply Depot` 默认驻军改为射手和骑兵组合，让玩家有不同编队选择。
- 设计结果：部队系统从单纯堆战力，开始进入“根据敌军驻军选择兵种”的 SLG 决策。
- 后续待优化内容：把克制关系扩展为完整战斗矩阵，增加敌军侦查、推荐兵种和战斗预览。

### 2026-05-20：战斗预览与推荐兵种

- 完成功能：据点详情新增战斗预览，任务列表会在需要训练时优先推荐克制当前据点驻军的兵种。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 预览内容：
  - 敌军主力兵种类型。
  - 推荐训练/编队兵种。
  - 敌军战力。
  - 当前编队基础战力。
  - 克制修正后的编队战力。
  - 预计结果：`Expected Win`、`Risky`、`Not Enough Power`。
- 设计结果：兵种克制不再只是配置字段，而是变成了出征前可以看见、可以决策的战斗预览。
- 后续待优化内容：增加一键推荐编队、战斗损耗预估、胜率区间和完整兵种克制矩阵。

### 2026-05-20：一键推荐编队

- 完成功能：据点详情新增 `Recommend` 按钮，可以根据当前据点驻军和我方库存自动补充编队。
- 修改的主要文件：
  - `Assets/_Project/Scripts/Base/SlgBaseRuntime.cs`
  - `Assets/_Project/Scripts/UI/WorldSlgVisualBuilder.cs`
  - `Docs/DevelopmentPlan.md`
  - `Docs/TechnicalNotes.md`
- 推荐规则：
  - 优先使用克制据点主力兵种的预备兵。
  - 如果克制兵不足以达到敌军战力，再补充其他可用预备兵中修正后战力最高的兵种。
  - 推荐只移动库存到编队，不会自动出征。
  - 没有预备兵或据点不可攻打时，`Recommend` 禁用。
- 设计结果：玩家可以快速形成一个可用编队，同时仍然保留手动调整和最终出征确认。
- 后续待优化内容：增加编队上限、推荐数量预览、撤销推荐和按损耗最小化/战力最大化切换策略。
