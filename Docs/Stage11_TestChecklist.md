# 第 11 阶段测试清单：科技树与全局加成

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 点击菜单：`SLG Learn > Build Stage 08 SLG Base Scene`。
3. 点击菜单：`SLG Learn > Clear Stage 08 SLG Save`，清空旧测试存档。
4. 打开生成的场景：`Assets/_Project/Scenes/Stage08_SlgBase.unity`。
5. 点击 Play。

## 运行测试

- 右侧部队列表下方应显示科技列表。
- 科技列表应包含 `Agriculture` 和 `Military Drill`。
- 资源不足时，`Research` 按钮应不可点击。
- 前置条件不足时，科技应显示 `Locked`，`Research` 按钮应不可点击。
- 升级 `Headquarters` 到 2 级后，依赖主基地等级的科技应解锁。
- 资源足够时，点击 `Research` 会扣除资源并显示研究倒计时。
- 研究倒计时结束后，科技等级应提升。
- `Agriculture` 完成后，Food 产出速度应变快。
- `Military Drill` 完成后，兵种显示的训练时间应缩短。
- `Military Drill` 完成后，兵种显示的单兵 Power、Reserve Power 和 March Power 应提升。
- 研究中的科技退出 Play 后重新进入 Play，应能从存档恢复剩余时间。
- 已完成的科技退出 Play 后重新进入 Play，应能从存档恢复等级和加成。

## 配置测试

- 首次生成场景时，应自动创建以下配置资产：
  - `Assets/_Project/ScriptableObjects/Stage11_Agriculture.asset`
  - `Assets/_Project/ScriptableObjects/Stage11_MilitaryDrill.asset`
- 修改 `Research Duration Seconds` 后重新进入 Play，研究耗时应变化。
- 修改 `Research Costs` 后重新进入 Play，研究消耗应变化。
- 修改 `Technology Prerequisites` 后重新进入 Play，科技前置条件应变化。
- 修改 `Building Requirements` 后重新进入 Play，建筑等级条件应变化。
- 修改 `Effects` 后重新进入 Play，产出、训练或战力加成应变化。

## 当前可接受问题

- 当前科技已有前置条件，但 UI 仍是列表，不是正式树状视图。
- 当前科技效果只覆盖资源产出、训练速度和部队战力。
- 当前科技 UI 只是运行时列表，还不是正式科技树界面。
- 当前科技效果按已完成等级累加，后续需要数值表和配置校验工具。

## 通过标准

- 可以消耗资源研究科技。
- 科技研究有倒计时。
- 科技完成后能改变 SLG 核心数值。
- 科技前置条件和建筑等级条件能阻止/解锁研究。
- 科技等级和研究进度能存档恢复。
