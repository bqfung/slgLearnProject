# 技术难点学习文档

本文档用于记录 Last War: Survival 学习项目中的技术难点、实现思路、常见坑点和面试可讲内容。

## 1. 小队移动系统

### 目标

实现玩家小队自动向前移动，并允许玩家通过左右拖拽控制横向位置。

### 技术点

- 输入系统：鼠标、触摸、键盘调试输入。
- 移动方式：Transform、CharacterController、Rigidbody 的取舍。
- 边界限制：限制玩家横向移动范围。
- 摄像机跟随：LateUpdate 中跟随目标，减少抖动。

### 难点

- 移动端触摸输入和编辑器鼠标输入兼容。
- 前进速度与横向速度手感调节。
- 横向移动不能穿出赛道。
- 摄像机跟随不能明显抖动。

### 面试可讲点

- 为什么角色移动常放在 Update，而物理相关移动常放在 FixedUpdate。
- LateUpdate 适合处理摄像机跟随的原因。
- Transform 移动和 Rigidbody 移动的区别。

### 当前实现记录

- `SquadController` 负责小队整体移动。
- 小队每帧自动向世界 Z 轴正方向前进。
- 横向输入同时支持键盘 A/D、方向键、鼠标拖拽和触摸拖拽。
- 横向位置通过 `horizontalLimit` 限制，避免移出赛道。
- 当前使用 Transform 位移，原因是第 1 阶段重点是跑酷控制手感，不涉及复杂物理碰撞。
- `CameraFollow` 在 LateUpdate 中跟随小队，减少摄像机和玩家移动的时序抖动。
- `StageOneSceneBuilder` 是编辑器工具，用于自动生成测试场景，避免每次手动创建对象和挂脚本。

## 2. 小队阵型系统

### 目标

根据当前人数动态生成和排列士兵。

### 技术点

- 阵型坐标计算。
- 士兵对象池。
- 人数增加和减少。
- 阵型刷新动画。

### 难点

- 人数频繁变化时避免反复 Instantiate 和 Destroy。
- 阵型要既整齐又不拥挤。
- 减员时选择哪些士兵移除。

### 面试可讲点

- 对象池如何减少 GC 和实例化开销。
- 数据结构如何管理当前存活士兵。
- 阵型刷新如何避免瞬间跳变。

### 当前实现记录

- `SquadManager` 管理当前小队成员列表。
- `AddMembers`、`RemoveMembers`、`SetCount` 提供统一人数变化接口，后续倍率门可以直接调用。
- 当前阵型按固定列数排列，行内居中，人数变化后重新计算目标位置。
- 士兵位置使用 Lerp 平滑移动到目标位置，避免人数变化时瞬间跳变。
- 如果没有配置士兵 Prefab，会自动创建胶囊体作为临时士兵，方便早期测试。
- `SquadDebugInput` 用于开发期测试，按 E 增加人数，按 Q 减少人数。

## 3. 倍率门系统

### 目标

玩家通过门时，小队人数根据门的规则变化。

### 技术点

- Trigger 碰撞检测。
- 门数据配置。
- UI 文本显示。
- 触发后禁用重复触发。

### 难点

- 避免一个门被多个士兵重复触发。
- 加法、减法、乘法等规则统一抽象。
- 负数或人数为 0 的边界处理。

### 面试可讲点

- Collider 和 Trigger 的区别。
- 如何设计可扩展的门规则。
- 为什么要把门数值做成配置。

### 当前实现记录

- `GateOperation` 定义门的运算类型：加法、减法、乘法。
- `SquadGate` 负责检测小队通过，并调用 `SquadManager` 的人数接口。
- 门内部用 `hasTriggered` 防止多个士兵重复触发同一个门。
- 门触发后会关闭自身渲染和碰撞，作为临时反馈。
- `StageTwoSceneBuilder` 是编辑器工具，用于自动生成包含多组倍率门的测试场景。

### 当前设计取舍

- 门不直接管理士兵对象，只调用 `SquadManager`，这样倍率门逻辑和阵型管理保持解耦。
- 加、减、乘统一通过枚举表达，后续可以扩展为除法、固定人数、随机奖励等规则。
- 当前数值写在场景工具中，后续第 6 阶段会迁移到 ScriptableObject 或关卡配置中。

## 4. 自动索敌与攻击

### 目标

小兵自动寻找合适目标并按攻击频率输出伤害。

### 技术点

- 目标搜索范围。
- 攻击冷却。
- 射线、子弹、范围检测。
- 目标优先级。

### 难点

- 多个小兵频繁搜索敌人可能导致性能压力。
- 敌人死亡后目标引用失效。
- 攻击频率、射程、伤害要可配置。

### 面试可讲点

- 如何优化大量单位的索敌逻辑。
- Update 中做搜索为什么可能有性能问题。
- 事件驱动和轮询的区别。

### 当前实现记录

- `SoldierWeapon` 挂在每个士兵身上，按攻击间隔寻找目标并发射子弹。
- 目标搜索使用 `Physics.OverlapSphereNonAlloc`，避免每次搜索都分配新数组。
- 当前优先攻击前方攻击范围内距离最近的敌人。
- `EnemyHealth` 负责敌人的血量、受击和死亡。
- 普通敌人死亡后通过 `EnemyPool` 回收，Boss 死亡后用于触发胜利判断。

### 当前设计取舍

- 当前攻击已经升级为对象池子弹，命中后再造成伤害。
- 每个士兵独立索敌，逻辑清晰但规模变大后会有性能压力；后续可以改成小队级目标分发、分帧搜索或空间分区。
- 当前只检查目标是否在前方，暂未处理遮挡、目标权重和多目标技能。

## 5. 子弹与对象池

### 目标

通过对象池复用子弹、伤害数字和特效。

### 技术点

- 泛型对象池。
- 预加载。
- 取出、回收、重置状态。
- 生命周期管理。

### 难点

- 回收对象时必须重置状态。
- 子弹命中、超时、场景切换时都要正确回收。
- 池容量过小或过大都会有问题。

### 面试可讲点

- Instantiate 和 Destroy 的性能成本。
- 对象池如何减少 GC。
- 对象池可能带来的状态残留问题。

### 当前实现记录

- `BulletPool` 是运行时子弹对象池，首次攻击时会自动创建。
- `BulletPool` 会预热一批子弹，并在发射时从队列中取出。
- `Bullet` 负责追踪目标、移动、命中、超时和回收。
- `SoldierWeapon` 不再直接对敌人扣血，而是在攻击冷却结束后向目标发射子弹。
- 子弹命中敌人后调用 `EnemyHealth.TakeDamage`，然后回收到池中。
- `DamageNumberPool` 负责复用伤害数字。
- `HitEffectPool` 负责复用命中闪光效果。
- 子弹命中后会同时触发伤害数字和命中特效，二者各自到期后回收。
- `EnemyPool` 负责复用普通敌人。
- `PooledEnemy` 监听 `EnemyHealth.Died` 事件，并在普通敌人死亡时通知对象池回收。
- `EnemySpawner` 现在通过 `EnemyPool` 生成普通敌人，Boss 仍独立创建。
- `ComponentPool<T>` 抽出了对象池的通用逻辑：预热、取出、回收、超过配置容量时警告。
- `BulletPool`、`DamageNumberPool`、`HitEffectPool`、`EnemyPool` 都继承 `ComponentPool<T>`。
- `ComponentPool<T>` 暴露 `TotalCreated`、`AvailableCount`、`ActiveCount`、`PeakActiveCount`、`ExpansionCount`，供调试面板读取。
- `PoolDebugOverlay` 会显示各对象池的 active/free/total/peak/grow 数量，用于运行时观察池复用情况。
- `PoolDebugOverlay` 支持按 F3 隐藏或显示。

### 当前设计取舍

- 对象池当前覆盖子弹、伤害数字和命中特效，仍保持在战斗反馈范围内。
- 普通敌人也已经接入对象池，死亡后不再只做隐藏，而是回到 `EnemyPool`。
- 子弹和命中特效使用运行时球体，伤害数字使用 TextMesh，后续都可以替换为正式 Prefab。
- `BulletPool` 当前是简化版全局池，适合 Demo；大型项目中可以按场景、玩法模块或资源类型管理多个池。
- Boss 暂不池化，因为它通常是关卡关键对象，生命周期与普通敌人波次不同。
- 通用池不关心具体对象怎么创建和怎么播放，只管理池生命周期，避免抽象层过度了解业务。

### 面试可讲点补充

- 对象池不是为了让单个对象更快，而是为了避免频繁 `Instantiate` / `Destroy` 造成 CPU 峰值和 GC 压力。
- 回收对象时必须重置运行时状态，例如目标引用、生命周期、位置、激活状态。
- 对象池容量需要结合峰值并发量设置，太小会频繁扩容，太大则浪费内存。
- 反馈类对象特别适合池化，因为它们数量多、生命周期短、创建销毁频繁。
- 敌人池化时要特别注意状态重置，例如血量、目标、移动速度、攻击冷却、父节点和激活状态。
- 当前 `EnemyMeleeAttacker.Configure` 会重置攻击冷却，避免池化敌人复用后继承旧状态。
- 抽通用池时要避免“为了复用而复用”：如果通用基类塞入太多业务参数，反而会让每个对象池更难维护。
- 对象池调试面板能帮助判断池是否在复用：如果 total 长期上涨，说明池容量不够或回收逻辑有问题；如果 grow 大于 0，说明运行时已经超过了预设池容量。

## 6. 敌人波次系统

### 目标

根据关卡配置生成敌人波次。

### 技术点

- Wave 配置。
- SpawnPoint。
- 敌人类型配置。
- 生成时机。

### 难点

- 波次触发方式：距离触发、时间触发、区域触发。
- 如何让关卡内容方便调试。
- 敌人数量较多时的性能控制。

### 面试可讲点

- 数据驱动关卡设计。
- ScriptableObject 在关卡配置中的使用。
- 如何避免关卡逻辑写死在代码里。

### 当前实现记录

- 第 4 阶段先通过 `StageFourSceneBuilder` 在场景中预摆放多组敌人，验证波次节奏。
- 普通敌人挂载 `EnemyHealth` 和 `EnemyMeleeAttacker`。
- `EnemyMeleeAttacker` 会向小队移动，进入攻击距离后按频率调用 `SquadManager.RemoveMembers`。
- 当前波次还不是运行时生成，后续会迁移到 `EnemySpawner` 和关卡配置。

## 7. Boss 战

### 目标

终点出现 Boss，Boss 有独立血量和攻击逻辑。

### 技术点

- Boss 状态机。
- Boss 血条 UI。
- 攻击范围和攻击间隔。
- 胜利条件判断。

### 难点

- Boss 攻击与玩家减员之间的节奏。
- Boss 死亡后流程切换。
- Boss UI 与战斗逻辑解耦。

### 面试可讲点

- 状态机适合解决什么问题。
- UI 如何监听战斗事件。
- 胜负判断应由哪个模块负责。

### 当前实现记录

- Boss 当前复用 `EnemyHealth` 和 `EnemyMeleeAttacker`，通过更高血量、更大体型、更高攻击伤害形成差异。
- `GameOutcomeController` 监听小队人数和 Boss 状态。
- 小队人数为 0 时失败，Boss 死亡时胜利。
- 胜负结果暂时通过世界空间 `TextMesh` 和日志输出反馈。

### 当前设计取舍

- Boss 先复用普通敌人组件，减少早期系统复杂度。
- 胜负判断独立放在 `GameOutcomeController`，避免让 UI、Boss 或小队管理器承担过多职责。
- 后续 UI 阶段会把胜负结果从世界空间文字迁移到正式结算面板。

## 8. UI 流程管理

### 目标

完成开始、战斗、胜利、失败等界面切换。

### 技术点

- UI 面板管理。
- 游戏状态机。
- 按钮事件。
- 血条、人数、进度显示。

### 难点

- UI 不应直接控制大量战斗逻辑。
- 场景重启时状态清理。
- UI 更新频率控制。

### 面试可讲点

- MVC、MVP、MVVM 在 Unity UI 中的应用思路。
- 事件系统如何降低耦合。
- UI 和业务逻辑如何分层。

### 当前实现记录

- `BattleHudController` 负责战斗 HUD 和胜负面板。
- HUD 通过轮询显示小队人数和 Boss 血量，适合当前简单 Demo。
- `GameOutcomeController` 在胜负触发时派发 `Finished` 事件。
- `BattleHudController` 监听胜负事件，打开结果面板并显示胜利或失败。
- `Restart` 按钮通过 `SceneManager.LoadScene` 重新加载当前场景。

### 当前设计取舍

- 小队人数和 Boss 血量使用轮询刷新，代码简单；后续如 UI 较复杂，可改为事件驱动。
- 胜负判断仍放在流程控制组件中，UI 只负责展示结果。
- 第 5 阶段暂时跳过开始界面，优先完成战斗到结算的闭环。

## 9. 数据驱动设计

### 目标

通过配置管理角色、敌人、武器、关卡数据。

### 技术点

- ScriptableObject。
- 配置引用。
- 编辑器可视化。
- 运行时读取配置。

### 难点

- 配置之间的引用关系要清晰。
- 修改配置后要避免影响运行时临时状态。
- 区分静态配置和动态存档数据。

### 面试可讲点

- ScriptableObject 的优点和注意事项。
- 配置数据和运行时数据为什么要分离。
- 数据驱动如何提升可扩展性。

### 当前实现记录

- `LevelConfig` 是关卡配置入口，包含赛道长度、宽度、倍率门、敌人波次和 Boss 配置。
- `GateConfig` 描述一组左右门的位置、运算类型和数值。
- `EnemyWaveConfig` 描述敌人波次的位置、数量、血量、速度、攻击范围、攻击频率和减员数量。
- `BossConfig` 描述 Boss 位置、血量、速度、攻击范围、攻击频率和减员数量。
- `StageSixSceneBuilder` 首次执行时会自动创建默认 `Stage06_LevelConfig.asset`，并创建挂载 `LevelBuilder` 的测试场景。
- `LevelBuilder` 在运行时读取 `LevelConfig`，生成赛道、小队、摄像机、倍率门、敌人波次、Boss 和 UI。

### 当前设计取舍

- 现在采用 ScriptableObject 作为静态关卡配置，适合 Unity 编辑器内调参与版本管理。
- 配置类只保存数据，不直接创建 GameObject，避免数据层和表现层耦合。
- 当前生成逻辑已经从 Editor 工具迁移到运行时 `LevelBuilder`，Editor 菜单只负责创建配置资产和测试场景。
- `LevelBuilder` 当前职责较多，后续可以按对象类型继续拆分，避免变成新的上帝类。

### 当前拆分记录

- `GateBuilder` 负责根据 `GateConfig` 创建左右倍率门、门面板和文字。
- `EnemySpawner` 负责根据 `EnemyWaveConfig` 创建普通敌人，并根据 `BossConfig` 创建 Boss。
- `RuntimePrimitiveFactory` 暂时集中处理运行时临时材质创建，避免颜色和 Shader 查找逻辑散落在多个类里。
- `RuntimeUiBuilder` 负责创建战斗 HUD、结果面板、重开按钮和 EventSystem。
- `EnvironmentBuilder` 负责运行时创建临时灯光、赛道和边界。
- `PlayerSquadFactory` 负责创建玩家小队对象并挂载小队相关组件。
- `RuntimeCameraFactory` 负责创建主摄像机并挂载跟随组件。
- `LevelBuilder` 现在主要负责关卡构建顺序：环境、小队、摄像机、倍率门、敌人、Boss、胜负控制和 UI 装配。
- `VisualConfig` 负责集中配置运行时临时表现，包括颜色、文字大小和基础尺寸。
- `RuntimePrimitiveFactory` 在 `LevelBuilder` 启动时接收 `VisualConfig`，运行时创建临时对象时统一读取这些表现参数。
- `VisualConfig` 现在也可以配置子弹、命中特效、敌人、Boss、士兵 Prefab。
- `RuntimePrimitiveFactory.InstantiatePrefabOrPrimitive` 负责“优先实例化 Prefab，未配置时回退到临时 Primitive”的逻辑。

### 面试可讲点补充

- 一个类开始变大时，不一定马上做复杂架构；可以先按变化原因拆分，例如门、敌人、UI 的变化原因不同。
- `LevelBuilder` 作为流程编排器，不应长期持有所有对象创建细节，否则会逐渐变成上帝类。
- 拆分后每个类更容易单独替换，例如后续可以把 `EnemySpawner` 改成对象池版本，而不影响倍率门逻辑。
- UI 创建拆到 `RuntimeUiBuilder` 后，后续可以从“代码创建 UI”平滑替换为“加载 UI Prefab”，不用改关卡生成流程。
- 环境、小队和摄像机拆出后，`LevelBuilder` 更接近流程编排器；后续替换正式 Prefab 时影响面更小。
- 表现配置先集中到 `VisualConfig`，再逐步迁移到正式 Prefab，是比一次性替换所有资源更稳的路径。
- Prefab 字段保持可选，可以在没有美术资源时继续运行 Demo，也可以逐步替换单个对象类型。

### 面试可讲点补充

- ScriptableObject 适合保存静态配置，但不适合直接保存玩家运行时存档。
- 配置数据和运行时状态要分离，例如敌人最大血量属于配置，当前血量属于运行时状态。
- 数据驱动的价值是减少硬编码，让策划或开发者可以通过配置快速调整关卡。

## 10. 性能优化

### 目标

让 Demo 在中低端移动设备上也能保持稳定。

### 技术点

- Profiler。
- GC Alloc。
- DrawCall。
- Batching。
- Overdraw。
- 粒子数量控制。

### 难点

- 大量单位和子弹同时存在时的 CPU 压力。
- UI 和特效带来的 Overdraw。
- 频繁字符串拼接、LINQ、临时 List 导致 GC。

### 面试可讲点

- Unity 中常见 GC 来源。
- 如何使用 Profiler 定位性能问题。
- 移动端渲染优化的常见方向。

## 11. 项目架构

### 目标

形成清晰、可维护、适合面试讲解的工程结构。

### 建议模块

- Core：流程、事件、对象池。
- Player：玩家控制、小队、士兵。
- Combat：攻击、子弹、伤害。
- Enemy：敌人、Boss、生成器。
- Level：关卡、倍率门、波次。
- UI：界面、血条、结算。
- Data：配置数据。

### 难点

- GameManager 容易变成上帝类。
- 模块之间引用过多会导致难以维护。
- 事件系统滥用后也会变得难以追踪。

### 面试可讲点

- 如何划分模块职责。
- 如何避免单例滥用。
- 事件解耦的优缺点。

## 当前学习重点

建议优先学习和实现：

1. 小队移动系统。
2. 小队阵型系统。
3. 倍率门系统。
4. 自动战斗系统。

这些内容完成后，项目就会有可玩的核心雏形。

## 12. 关卡编辑器

### 目标

通过 Unity Editor 扩展，让 `LevelConfig` 更容易校验、生成和调试，形成作品集亮点。

### 当前实现记录

- `LevelConfigValidator` 负责纯配置校验，不依赖 Inspector。
- `LevelConfigIssue` 用于描述单条校验结果，包含级别、消息和配置路径。
- `LevelValidationSettings` 保存关卡校验阈值，例如 Gate spacing、Enemy wave spacing。
- `LevelConfigEditor` 是 `LevelConfig` 的自定义 Inspector。
- Inspector 中提供 `Validate Config` 按钮。
- Inspector 中提供 `Generate Preview Scene For This Config` 按钮。
- Inspector 中提供 `Validation Settings` 引用，默认使用 `Stage07_LevelValidationSettings.asset`。
- 校验结果通过 `HelpBox` 显示在 Inspector 下方，Error 和 Warning 使用不同样式。
- 菜单 `SLG Learn > Build Stage 06 Data Driven Scene` 仍生成默认测试场景。
- Inspector 生成按钮会基于当前选中的 `LevelConfig` 生成 `Preview_<ConfigName>.unity`。
- 生成预览场景前会自动运行校验；如果存在 Error，会弹窗让开发者选择取消或继续生成。
- 校验结果会显示 `PropertyPath`，例如 `boss.position`、`gates.Array.data[0].z`。
- 校验器会检查关键内容之间的间距，发现节奏过密时给出 Warning。

### 当前校验规则

- Road length 和 road width 必须大于 0。
- Gate Z 不能为负数，不能超过赛道长度。
- Gate 左右数值必须大于 0。
- Enemy wave Z 不能为负数，不能超过赛道长度。
- Enemy wave 应按 Z 从小到大排序。
- Enemy wave count 和 health 必须大于 0。
- Boss 必须存在，血量必须大于 0。
- Boss 应放在最后一波敌人之后。
- VisualConfig 未配置时给出 Warning，因为运行时仍可使用代码回退表现。
- Gate 之间建议间距由 `LevelValidationSettings.MinGateSpacing` 控制，默认 8。
- Enemy wave 之间建议间距由 `LevelValidationSettings.MinWaveSpacing` 控制，默认 10。
- Gate 和 Enemy wave 之间建议间距由 `LevelValidationSettings.MinGateWaveSpacing` 控制，默认 6。
- Boss 与最后一波敌人的建议间距由 `LevelValidationSettings.MinBossAfterLastWaveSpacing` 控制，默认 12。

### 面试可讲点

- Editor 工具可以减少配置错误，提高关卡制作效率。
- 校验逻辑和 Inspector 展示分离，更方便后续复用到批量检查或构建前检查。
- ScriptableObject 适合做关卡配置，自定义 Inspector 适合做配置入口和轻量工具链。
- 工具链是作品集亮点，因为它体现了从“能玩”到“能生产内容”的工程意识。
- 让预览场景基于当前配置生成，比固定生成默认关卡更贴近真实关卡生产流程。
- 生成前校验是常见工具链设计：既要减少低级配置错误，也要允许开发者在需要时带着提醒继续调试。
- 校验结果分级很重要：Error 表示可能破坏关卡运行或数值逻辑，Warning 表示可运行但建议修正。
- `PropertyPath` 让校验结果具备可扩展性，后续可以用于点击定位、批量报告或构建前检查日志。
- 关卡节奏问题适合先做成 Warning，因为它更像设计建议；这样工具不会过度阻断调试流程。
- 校验阈值做成 ScriptableObject 后，工具规则可以被配置和复用，而不是散落在代码常量里。
