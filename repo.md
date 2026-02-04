# b1386_pre

## Repo简介

Fitness Windows Application 是一个基于 WPF 的健身追踪 Windows 应用程序，用于跟踪和管理用户的健身信息。

**主要功能：**
- 设置健身目标（跑步距离、饮水量等）
- 记录健身进度（跑步和饮水）
- 支持多种单位选择（距离单位：Miles/Kilometers，水单位：Ounces/Liters）
- 数据持久化到 JSON 文件
- 目标管理和进度追踪

**技术栈：**
- C# / .NET 9.0
- WPF (Windows Presentation Foundation)
- MVVM 架构模式
- Microsoft.Extensions.DependencyInjection (依赖注入)
- MaterialDesignThemes (UI 组件库)
- MahApps.Metro (UI 框架)

**项目结构：**
- `FitnessTracker/Models/` - 数据模型（Goal, FitnessProgress, RunningDistance, WaterContent）
- `FitnessTracker/Repositories/` - 数据持久化层（GoalRepository, FitnessProgressRepository）
- `FitnessTracker/Services/` - 业务逻辑层（GoalService, FitnessProgressService, WindowService）
- `FitnessTracker/ViewModels/` - 视图模型（HomeViewModel, SetGoalViewModel, FitnessProgressViewModel, MainViewModel）
- `FitnessTracker/Views/` - 视图层（Home.xaml, SetGoal.xaml, FitnessProgress.xaml）
- `FitnessTracker/App.xaml.cs` - 应用程序入口，配置依赖注入容器
- `SaveData/` - 数据存储目录（goals.json, fitness_progress.json）

**核心组件：**
- GoalService - 管理健身目标
- FitnessProgressService - 管理健身进度记录
- WindowService - 管理对话框窗口显示
- GoalRepository / FitnessProgressRepository - 数据持久化，使用 SemaphoreSlim 确保线程安全

## 题目Prompt

This is a fitness app where I need you to add a way for users to enter fitness progress (running and water) through a button on the home screen. I want users to be able to choose their units (coming from the models). And then this also should be saved to json files at "<application root>/SaveData/". Create whatever files you need to. This is going to be in a professional codebase so make sure that this adheres to best MVVM, DI, safety, and separation of concerns practices

## PR链接

https://github.com/ncepudlgc/b1386_pre/pull/3
