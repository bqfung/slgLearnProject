# 第 9 阶段测试清单：部队训练最小闭环

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 点击菜单：`SLG Learn > Build Stage 08 SLG Base Scene`。
3. 点击菜单：`SLG Learn > Clear Stage 08 SLG Save`，清空旧测试存档。
4. 打开生成的场景：`Assets/_Project/Scenes/Stage08_SlgBase.unity`。
5. 点击 Play。

## 运行测试

- UI 右侧应显示 `Total Power`。
- UI 右侧应显示 `Reserve Power` 和 `March Power`。
- UI 右侧应显示 `Infantry` 和 `Archer` 两种兵种。
- 每个兵种应显示库存数量、编队数量、单兵 Power、训练 Cost、`Train` 按钮、`+` 按钮和 `-` 按钮。
- 资源足够时，点击 `Train` 应消耗资源并增加训练订单。
- 训练中，兵种状态应显示 `Training` 数量。
- 倒计时结束后，对应兵种数量应增加。
- `Reserve Power` 应随库存部队数量增加而增加。
- 资源不足时，`Train` 按钮应不可点击。
- 当库存数量大于 0 时，点击 `+` 应把 1 个士兵加入编队，库存数量减少，编队数量增加。
- 当编队数量大于 0 时，点击 `-` 应把 1 个士兵移回库存，编队数量减少，库存数量增加。
- `March Power` 应随编队数量变化。
- 停止 Play 后再次 Play，兵种数量和未完成训练订单应恢复。
- 停止 Play 后再次 Play，编队数量应恢复。
- 停止 Play 等待一段时间后再次 Play，训练订单应按离线时间推进。

## 配置测试

- 首次生成场景时，应自动创建以下配置资产：
  - `Assets/_Project/ScriptableObjects/Stage09_Infantry.asset`
  - `Assets/_Project/ScriptableObjects/Stage09_Archer.asset`
- 修改 `Power` 后重新进入 Play，`Total Power` 计算应变化。
- 修改 `Training Duration Seconds` 后重新进入 Play，训练时间应变化。
- 修改 `Training Costs` 后重新进入 Play，UI 中显示的 Cost 应变化。

## 当前可接受问题

- 当前只实现最小编队界面，还没有出征目标选择和战斗结算。
- 当前编队界面是最小版，只支持单个士兵加入/移出。
- 当前训练订单是简化队列展示，只显示每个兵种正在训练的数量。
- 当前没有训练速度加成、兵营等级限制或训练容量。
- 当前没有兵种克制和出征战斗结算。

## 通过标准

- 可以消耗资源训练士兵。
- 训练完成后兵种库存增加。
- 库存战力随兵种库存和配置 Power 变化。
- 编队战力随加入编队的兵种和数量变化。
- 训练状态可以保存，并支持基础离线推进。
- 编队状态可以保存并恢复。
