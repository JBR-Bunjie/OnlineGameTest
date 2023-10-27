using System;

namespace Fracture.PhysicsDestroy {
    [Flags]
    public enum Anchor {
        None = 0,
        Left = 1,
        Right = 2,
        Bottom = 4,
        Top = 8,
        Front = 16,
        Back = 32
    }
}


/*
[Flags]属性主要用于提供元数据，指示枚举类型可以被视为一组标志。这对于在调试器中显示枚举值或使用ToString()方法将枚举值转换为字符串时非常有用。例如，如果您使用带有[Flags]属性的枚举类型调用ToString()方法，则结果将是一个由各个标志名称组成的逗号分隔列表。

例如，在您提供的示例中，如果您创建一个包含多个标志的Anchor值并调用其ToString()方法，则结果将是一个逗号分隔的标志名称列表：

Anchor anchor = Anchor.Left | Anchor.Right | Anchor.Top;
string result = anchor.ToString(); // "Left, Right, Top"

如果您删除了[Flags]属性并执行相同的操作，则结果将不同：
// Without the [Flags] attribute
Anchor anchor = Anchor.Left | Anchor.Right | Anchor.Top;
string result = anchor.ToString(); // "15"
*/