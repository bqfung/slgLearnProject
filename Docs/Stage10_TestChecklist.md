# 第 10 阶段测试清单：地图据点与战斗结算

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 点击菜单：`SLG Learn > Build Stage 08 SLG Base Scene`。
3. 点击菜单：`SLG Learn > Clear Stage 08 SLG Save`，清空旧测试存档。
4. 打开生成的场景：`Assets/_Project/Scenes/Stage08_SlgBase.unity`。
5. 点击 Play。

## 运行测试

- 场景中应显示基地地块和地图地块，而不是大面积屏幕调试 UI。
- 基地地块中应显示建筑块、兵营训练点和 3D 操作牌。
- 地图地块中应显示 `Outpost`、`Raider Camp` 和 `Supply Depot` 据点节点。
- `Outpost` 到 `Raider Camp`、`Supply Depot` 之间应显示世界中的连线。
- 地图区域应显示章节标题、完成进度和章节完成奖励。
- 每个据点节点旁应显示敌军战力、状态和可点击 `Attack` 操作牌。
- 每个据点应显示敌军战力、驻军构成、首通/重复奖励和前置条件。
- `Outpost` 初始应显示 `Unlocked`，节点颜色应为可攻打状态。
- `Raider Camp` 初始应显示 `Locked`，并显示前置条件 `Req: outpost`，节点颜色应为锁定状态。
- `Supply Depot` 初始应显示 `Locked`，并显示前置条件 `Req: outpost`，节点颜色应为锁定状态。
- 未加入任何编队时，`Attack` 按钮应不可点击。
- 训练士兵并点击 `+` 加入编队后，`Attack` 按钮应可点击。
- 点击 `Attack` 后应创建行军订单，当前编队应清空，`March Power` 应归零。
- 行军期间目标据点应显示 `Marching X.Xs`，且该据点 `Attack` 按钮应不可点击。
- 行军倒计时结束前，不应立即发放奖励或产生战报结算。
- 编队战力达到据点敌军战力时，行军结束后应显示 Victory 战报。
- 战报中应显示我方战力和敌军战力，例如 `Power 20 vs 20`。
- 首次 Victory 后应获得据点首通奖励资源。
- 首次 Victory 后战报应显示 `First Clear`。
- Victory 后该据点应显示 `Cleared`，节点颜色应变为已攻克状态。
- 已攻克据点在重新加入编队后，`Attack` 按钮应仍可点击。
- 重复挑战已攻克据点 Victory 后，应获得 `Repeat Rewards`，不应再次获得首通奖励。
- 重复挑战 Victory 后战报应显示 `Repeat`。
- 攻克 `Outpost` 后，`Raider Camp` 和 `Supply Depot` 应从 `Locked` 变为 `Unlocked`，分支连线应变为可推进状态。
- 攻克章节内全部据点后，章节状态应显示 `Complete | Reward Claimed`。
- 攻克章节内全部据点后，应自动获得章节完成奖励，且不能重复领取。
- 退出 Play 后重新进入 Play，已攻克据点状态应能从存档恢复。
- Victory 后部分出征士兵应按胜利损耗率损失，剩余士兵回到库存。
- 编队战力低于据点敌军战力时，点击 `Attack` 应显示 Defeat 战报。
- Defeat 后不应获得奖励，且应按失败损耗率损失更多士兵。
- 战斗结束后行军订单应移除。

## 配置测试

- 首次生成场景时，应自动创建以下配置资产：
  - `Assets/_Project/ScriptableObjects/Stage10_Outpost.asset`
  - `Assets/_Project/ScriptableObjects/Stage10_RaiderCamp.asset`
  - `Assets/_Project/ScriptableObjects/Stage13_SupplyDepot.asset`
  - `Assets/_Project/ScriptableObjects/Stage10_BorderlandsChapter.asset`
- 修改 `Recommended Power` 后重新进入 Play，胜负判定门槛应变化。
- 如果据点 `Garrison` 为空，战斗应回退使用 `Recommended Power` 作为敌军战力。
- 修改 `Garrison` 中的兵种 id 或数量后重新进入 Play，敌军战力和胜负门槛应变化。
- 修改 `Victory Loss Rate` 后重新进入 Play，胜利损耗应变化。
- 修改 `Defeat Loss Rate` 后重新进入 Play，失败损耗应变化。
- 修改 `Rewards` 后重新进入 Play，首通胜利奖励应变化。
- 修改 `Repeat Rewards` 后重新进入 Play，重复挑战胜利奖励应变化。
- 修改章节 `Completion Rewards` 后重新进入 Play，章节完成奖励应变化。
- 修改 `Prerequisite Stronghold Ids` 后重新进入 Play，据点锁定/解锁条件应变化。
- 修改 `Map Position` 后重新进入 Play，据点节点位置应变化。
- 临时把某个据点前置条件改成不存在的 id，再点击 `SLG Learn > Validate All SLG Configs`，应能看到 Error。
- 临时把某个据点前置条件改成自己，再点击 `SLG Learn > Validate All SLG Configs`，应能看到 Error。
- 临时把某个据点驻军兵种 id 改成不存在的 id，再点击 `SLG Learn > Validate All SLG Configs`，应能看到 Error。
- 临时把某个据点驻军数量改成 0，再点击 `SLG Learn > Validate All SLG Configs`，应能看到 Error。
- 临时把章节引用的据点 id 改成不存在的 id，再点击 `SLG Learn > Validate All SLG Configs`，应能看到 Error。

## 当前可接受问题

- 当前已经从纯列表升级为轻量章节地图节点，但还不是完整大地图视图。
- 当前战斗是战报公式，没有播放部队战斗过程。
- 当前胜负公式比较 `March Power >= Enemy Power`，敌军战力来自驻军配置或推荐战力兜底。
- 当前已经支持据点前置条件、简单分支解锁、节点连线、基础驻军、重复挑战奖励、行军倒计时和章节完成奖励，但还没有兵种克制和一键扫荡。

## 通过标准

- 可以从编队出征到据点。
- 战斗能根据我方编队战力和据点驻军战力产生胜负。
- 首次胜利能获得首通奖励。
- 重复挑战胜利能获得重复奖励，不能重复获得首通奖励。
- 胜利能标记据点已攻克，并按配置前置条件解锁后续分支据点。
- 全部章节据点攻克后能自动发放一次章节奖励。
- 地图节点和连线能反映 Locked、Unlocked、Cleared 状态。
- 已攻克状态能存档恢复。
- 行军订单能倒计时，并在到达后结算战斗。
- 战斗能造成士兵损耗。
- 战斗后编队清空，剩余士兵返回库存。
