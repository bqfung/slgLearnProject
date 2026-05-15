# 第 7 阶段测试清单：关卡编辑器亮点系统

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 等待 Unity 编译完成。
3. 点击菜单：`SLG Learn > Build Stage 06 Data Driven Scene`，确保默认配置资产已生成。
4. 在 Project 窗口选择：`Assets/_Project/ScriptableObjects/Stage06_LevelConfig.asset`。
5. 确认项目中存在：`Assets/_Project/ScriptableObjects/Stage07_LevelValidationSettings.asset`。

## Inspector 测试

- Inspector 中应显示 `Level Design Tools`。
- Inspector 中应显示 `Validation Settings` 引用，默认指向 `Stage07_LevelValidationSettings.asset`。
- 点击 `Validate Config` 后，应显示校验结果。
- 默认配置应尽量不出现阻断性问题。
- 临时把 Boss Z 改到最后一波敌人之前，再点击 `Validate Config`，应提示 Boss 应放在最后一波敌人之后。
- 临时把某个 Gate Z 改成负数，再点击 `Validate Config`，应提示 Gate 位置非法。
- Boss 位置、Gate 位置、敌人数量等阻断性问题应显示为 Error。
- 临时清空 VisualConfig，再点击 `Validate Config`，应提示 VisualConfig 未配置，并显示为 Warning。
- 校验提示中应包含配置定位路径，例如 `boss.position`、`gates.Array.data[0].z`、`enemyWaves.Array.data[0].count`。
- 临时把两个 Gate 的 Z 改得很近，再点击 `Validate Config`，应提示 Gate spacing 过近，并显示为 Warning。
- 临时修改 `Stage07_LevelValidationSettings.asset` 中的 `Min Gate Spacing`，再次校验时 Warning 文案中的推荐间距应随配置变化。
- 临时把两个 Enemy wave 的 Z 改得很近，再点击 `Validate Config`，应提示 wave spacing 过近，并显示为 Warning。
- 临时把某个 Gate 和 Enemy wave 的 Z 改得很近，再点击 `Validate Config`，应提示 gate-wave spacing 过近，并显示为 Warning。
- 临时把 Boss Z 调到最后一波敌人之后但距离很近，再点击 `Validate Config`，应提示 Boss spacing 过近，并显示为 Warning。
- 点击 `Generate Preview Scene For This Config` 后，会先自动校验当前配置。
- 如果配置存在 Error，应弹出确认窗口；点击 `Cancel` 不生成场景，点击 `Generate Anyway` 继续生成。
- 如果配置只有 Warning，应直接生成预览场景，不弹阻断确认。
- 配置无问题时，应根据当前选中的配置生成并打开预览场景。
- 预览场景路径应类似：`Assets/_Project/Scenes/Preview_Stage06_LevelConfig.unity`。

## 当前可接受问题

- 当前编辑器工具只在 Inspector 中显示按钮和校验结果，还不是可视化赛道编辑器。
- 校验规则仍是基础版，后续可以继续增加难度评估和数值预估。
- 预览场景会保存到 `Assets/_Project/Scenes/`，如果同名预览场景存在会被覆盖。
- VisualConfig 未配置属于提醒型问题，仍允许继续生成预览场景。

## 通过标准

- `LevelConfig` Inspector 中能看到工具按钮。
- 配置校验能发现明显错误。
- 校验结果能区分 Error 和 Warning。
- 校验结果能显示具体配置路径。
- 校验结果能提示明显的关卡节奏间距问题。
- 间距校验阈值可以通过 `Stage07_LevelValidationSettings.asset` 调整。
- 生成预览场景前会自动执行配置校验。
- 可以从 Inspector 直接基于当前 `LevelConfig` 生成数据驱动预览场景。
