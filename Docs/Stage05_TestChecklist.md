# 第 5 阶段测试清单：UI 与流程闭环

## 准备

1. 打开 Unity 工程 `slgLearnProject/`。
2. 等待 Unity 编译完成。
3. 点击菜单：`SLG Learn > Build Stage 05 UI Flow Scene`。
4. Unity 会生成并打开场景：`Assets/_Project/Scenes/Stage05_UIFlow.unity`。

## 运行测试

- 点击 Play 后，小队应自动前进。
- 屏幕左上角应显示小队人数。
- 屏幕左上角应显示 Boss 血量。
- 经过倍率门后，小队人数 UI 应同步变化。
- 攻击 Boss 时，Boss 血量 UI 应下降。
- Boss 被击败后，应弹出结果面板并显示 `VICTORY`。
- 小队人数归零后，应弹出结果面板并显示 `DEFEAT`。
- 点击 `Restart` 后，应重新加载当前场景。

## 当前可接受问题

- UI 视觉是临时样式，主要用于验证流程。
- 还没有开始界面，进入场景后直接开战。
- 还没有暂停、下一关、正式结算奖励。
- Boss 血量暂时使用文字，后续可以改成血条。

## 通过标准

- HUD 能实时反映小队人数和 Boss 血量。
- 胜利、失败、重开流程都可用。
- 运行过程中没有明显报错。

