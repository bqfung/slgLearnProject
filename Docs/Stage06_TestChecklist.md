# 第 6 阶段测试清单：数据驱动与工程整理

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 等待 Unity 编译完成。
3. 点击菜单：`SLG Learn > Build Stage 06 Data Driven Scene`。
4. Unity 会自动创建配置资产：`Assets/_Project/ScriptableObjects/Stage06_LevelConfig.asset`。
5. Unity 会自动创建表现配置资产：`Assets/_Project/ScriptableObjects/Stage06_VisualConfig.asset`。
6. Unity 会生成并打开场景：`Assets/_Project/Scenes/Stage06_DataDriven.unity`。
7. 场景中应只有一个核心对象：`LevelBuilder`。

## 运行测试

- 点击 Play 后，`LevelBuilder` 应自动生成赛道、小队、摄像机、倍率门、敌人波次、Boss 和 UI。
- 倍率门、敌人波次、Boss 应按 `Stage06_LevelConfig` 中的配置生成。
- 修改 `Stage06_LevelConfig` 中的门数值后，重新进入 Play，场景中的门应跟着变化。
- 修改敌人波次数量、血量或速度后，重新进入 Play，场景中的敌人应跟着变化。
- 修改 Boss 血量后，重新进入 Play，Boss 血量 UI 应显示新数值。
- 胜利、失败、重开流程应保持可用。
- 修改 `Stage06_VisualConfig` 中的子弹、敌人、Boss 或倍率门颜色后，重新进入 Play，运行时表现应跟着变化。
- 在 `Stage06_VisualConfig` 中配置子弹、命中特效、敌人、Boss 或士兵 Prefab 后，重新进入 Play，应优先使用配置 Prefab。
- 未配置 Prefab 时，运行时仍应回退到临时球体或胶囊体，不应报错。
- 士兵攻击时应能看到黄色子弹飞向敌人。
- 子弹命中或目标死亡后应消失，并由对象池回收。
- 子弹命中时应显示红色伤害数字。
- 子弹命中时应出现短暂橙色命中闪光。
- 普通敌人死亡后应消失，并回收到 `EnemyPool`。
- 屏幕右上角应显示对象池调试信息：Bullets、Damage、Hit FX、Enemies。
- 战斗过程中对象池调试信息的 active/free/total/peak/grow 数值应随对象取出和回收变化。
- 按 F3 应能隐藏或显示对象池调试面板。

## 当前可接受问题

- 目前运行时生成的对象仍然使用临时方块、胶囊体和基础 UI。
- 当前所有运行时对象仍然使用临时 GameObject 创建，后续可以替换为 Prefab 和资源加载。
- 子弹、命中特效、敌人、Boss、士兵已经支持可选 Prefab 配置。
- 子弹、伤害数字、命中特效和普通敌人对象池已经接入，但表现仍是临时资源。
- 对象池调试面板是开发期 UI，后续正式展示时应提供开关。
- 配置资产的 Inspector 还没有自定义编辑器，暂时使用 Unity 默认列表编辑。

## 通过标准

- `LevelConfig` 能控制关卡中的门、敌人波次和 Boss。
- 修改配置后重新进入 Play，内容能正确更新。
- 原有战斗和 UI 流程不被破坏。
