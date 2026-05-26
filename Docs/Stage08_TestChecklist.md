# 第 8 阶段测试清单：SLG 基地与资源最小闭环

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 等待 Unity 编译完成。
3. 点击菜单：`SLG Learn > Build Stage 08 SLG Base Scene`。
4. 打开生成的场景：`Assets/_Project/Scenes/Stage08_SlgBase.unity`。
5. 点击 Play。

## 运行测试

- 画面左上角应显示 `SLG Base`。
- UI 中应显示 `Food` 和 `Wood` 两种资源。
- 资源数值应随时间增长，并且不超过容量。
- UI 中应显示 `Headquarters`、`Farm`、`Lumber Mill` 三个建筑。
- `Farm` 应持续生产 Food。
- `Lumber Mill` 应持续生产 Wood。
- 资源足够时，建筑的 `Upgrade` 按钮应可点击。
- 点击 `Upgrade` 后，资源应减少，建筑进入升级倒计时。
- 倒计时结束后，建筑等级应提升。
- 建筑升级后，生产速度应提升。
- 资源不足或建筑已满级时，`Upgrade` 按钮应不可点击。
- 停止 Play 后再次 Play，资源数量和建筑等级应从上次存档恢复。
- 停止 Play 等待一段时间后再次 Play，资源应获得基础离线收益。
- 点击菜单 `SLG Learn > Clear Stage 08 SLG Save` 后再次 Play，应回到默认初始资源和建筑等级。

## 配置测试

- 首次生成场景时，应自动创建以下配置资产：
  - `Assets/_Project/ScriptableObjects/Stage08_Food.asset`
  - `Assets/_Project/ScriptableObjects/Stage08_Wood.asset`
  - `Assets/_Project/ScriptableObjects/Stage08_Headquarters.asset`
  - `Assets/_Project/ScriptableObjects/Stage08_Farm.asset`
  - `Assets/_Project/ScriptableObjects/Stage08_LumberMill.asset`
- 修改资源容量后重新进入 Play，资源上限应变化。
- 修改建筑生产数值后重新进入 Play，资源增长速度应变化。
- 修改建筑升级消耗后重新进入 Play，UI 中显示的 Cost 应变化。

## 当前可接受问题

- 当前 UI 是运行时临时创建，视觉样式只用于功能验证。
- 当前建筑升级只支持单队列内的独立倒计时，尚未实现完整建筑队列。
- 当前存档使用 `PlayerPrefs + JsonUtility`，适合 Demo 学习，不是正式商业存档方案。
- 当前离线收益使用简化计算：按保存时的建筑等级结算资源，再推进升级倒计时。
- 当前还没有建筑解锁、科技前置和资源容量建筑效果。

## 通过标准

- 可以通过菜单生成第 8 阶段测试场景。
- Play 后可以看到资源随时间增长。
- 可以消耗资源升级建筑。
- 建筑升级后能改变资源产出。
- 停止并重新进入 Play 后，资源和建筑状态可以恢复。
- 离线一段时间后，资源会根据建筑产出增加。
- 基地与资源最小循环成立。
