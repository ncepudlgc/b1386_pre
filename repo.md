# FitnessTracker

## Repo简介

FitnessTracker 是一个基于 WPF 的 Windows 桌面应用程序，用于跟踪和管理用户的健身目标与进度。该应用采用 MVVM（Model-View-ViewModel）架构模式，使用依赖注入（DI）实现松耦合设计，遵循关注点分离的最佳实践。

### 主要功能

1. **目标设置（Goal Setting）**
   - 用户可以设置跑步和饮水目标
   - 支持多种单位选择（距离单位：英里、米、千米、英尺；水量单位：盎司、杯、升）
   - 自动单位转换功能

2. **进度记录（Progress Entry）**
   - 用户可以通过主屏幕按钮输入健身进度（跑步距离和饮水量）
   - 支持选择单位（来自模型定义的单位枚举）
   - 进度数据自动保存到 JSON 文件

3. **数据持久化**
   - 所有数据保存在 `<application root>/SaveData/` 目录下
   - 目标数据：`goals.json`
   - 进度数据：`fitness_progress.json`
   - 使用 JSON 格式存储，支持原子性写入操作

### 技术栈

- **框架**: .NET 9.0 (WPF)
- **架构模式**: MVVM (Model-View-ViewModel)
- **依赖注入**: Microsoft.Extensions.DependencyInjection
- **日志**: Microsoft.Extensions.Logging
- **UI 框架**: WPF (Windows Presentation Foundation)
- **测试框架**: xUnit

### 项目结构

```
FitnessTracker/
├── Models/                    # 数据模型
│   ├── Goal.cs               # 目标模型
│   ├── FitnessProgress.cs    # 进度模型
│   ├── RunningDistance.cs    # 跑步距离模型（含单位转换）
│   └── WaterContent.cs       # 饮水量模型（含单位转换）
├── Repositories/             # 数据访问层
│   ├── GoalRepository.cs     # 目标数据持久化
│   ├── IFitnessProgressRepository.cs
│   └── FitnessProgressRepository.cs  # 进度数据持久化
├── Services/                 # 业务逻辑层
│   ├── GoalService.cs        # 目标业务逻辑
│   ├── IFitnessProgressService.cs
│   ├── FitnessProgressService.cs   # 进度业务逻辑
│   └── WindowService.cs      # 窗口管理服务
├── ViewModels/               # 视图模型层
│   ├── MainViewModel.cs      # 主窗口视图模型
│   ├── HomeViewModel.cs      # 主页视图模型
│   ├── SetGoalViewModel.cs   # 设置目标视图模型
│   └── FitnessProgressViewModel.cs  # 进度输入视图模型
├── Views/                    # 视图层
│   ├── Home.xaml            # 主页视图
│   ├── SetGoal.xaml         # 设置目标对话框
│   └── FitnessProgress.xaml # 进度输入对话框
├── App.xaml.cs              # 应用程序入口，DI 配置
└── MainWindow.xaml          # 主窗口

FitnessTracker.Tests/         # 单元测试项目
├── GoalRepositoryTests.cs
└── GoalServiceTests.cs
```

### 设计原则

1. **MVVM 模式**
   - View：XAML 文件，负责 UI 展示
   - ViewModel：包含业务逻辑和状态管理，实现 INotifyPropertyChanged
   - Model：数据模型和业务实体

2. **依赖注入**
   - 使用 Microsoft.Extensions.DependencyInjection
   - 所有服务通过构造函数注入
   - 在 App.xaml.cs 中统一配置服务注册

3. **关注点分离**
   - Repository：负责数据持久化
   - Service：负责业务逻辑
   - ViewModel：负责视图状态和命令
   - View：仅负责 UI 展示

4. **安全性**
   - 使用 SemaphoreSlim 确保线程安全的数据访问
   - 原子性文件写入操作（临时文件 + 移动）
   - 输入验证和错误处理

5. **可测试性**
   - 接口抽象便于单元测试
   - 依赖注入支持 Mock 对象
   - 业务逻辑与 UI 分离

## 题目Prompt

This is a fitness app where I need you to add a way for users to enter fitness progress (running and water) through a button on the home screen. I want users to be able to choose their units (coming from the models). And then this also should be saved to json files at "<application root>/SaveData/". Create whatever files you need to. This is going to be in a professional codebase so make sure that this adheres to best MVVM, DI, safety, and separation of concerns practices

## PR链接

待创建
