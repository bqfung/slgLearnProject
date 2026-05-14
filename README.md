# slgLearnProject

一个用于学习和作品集展示的 Unity 项目，目标是制作一款参考 Last War: Survival 核心玩法的跑酷自动战斗 Demo。

## 学习目标

- 完成小队移动、倍率门、自动战斗、敌人波次、Boss 和结算流程。
- 练习 Unity 中级开发常见能力：对象池、数据驱动、事件系统、UI 流程、性能优化。
- 沉淀可用于笔试、面试和作品集展示的项目经验。

## 当前阶段

当前处于第 6 阶段：数据驱动与工程整理。

已完成：

- 创建 Unity 工程。
- 建立基础项目目录。
- 添加 Unity Git 忽略规则。
- 记录开发计划和技术难点文档。
- 实现小队移动、阵型管理和调试增减人数。
- 实现加法、减法、乘法倍率门。
- 实现士兵自动索敌、敌人血量和死亡。
- 实现敌人近战减员、Boss 和基础胜负判断。
- 实现战斗 HUD、胜负结果面板和重新开始按钮。
- 新增 `LevelConfig` 和运行时 `LevelBuilder`，把关卡门、敌人波次和 Boss 参数改为配置驱动。
- 新增子弹、伤害数字、命中特效、普通敌人对象池，士兵攻击从即时伤害升级为子弹命中。
- 新增通用 `ComponentPool<T>`，统一对象池基础生命周期。
- 新增对象池调试面板，用于观察 active/free/total/peak/grow 数量，支持 F3 显示或隐藏。
- 新增 `VisualConfig`，集中配置运行时临时表现颜色和尺寸。
- `VisualConfig` 支持可选 Prefab 引用，可逐步替换子弹、命中特效、敌人、Boss、士兵表现。
- 添加第 1、2、3、4、5、6 阶段测试场景生成工具。

下一步：

- 在 Unity 中生成并测试 `Stage06_DataDriven` 场景。
- 继续引入 Prefab 配置、对象池和更正式的运行时资源管理。

## 文档

- `Docs/DevelopmentPlan.md`：分阶段开发计划。
- `Docs/TechnicalNotes.md`：技术难点和面试学习笔记。

## GitHub

仓库地址：https://github.com/bqfung/slgLearnProject.git
