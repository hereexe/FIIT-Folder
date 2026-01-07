using System.ComponentModel;

namespace FIIT_folder.Domain.Enums;

public enum MaterialType
{
    [Description("Экзамены")]
    Exam,
    
    [Description("Коллоквиумы")]
    Colloquium,
    
    [Description("Зачёты")]
    Pass,
    
    [Description("Контрольные работы")]
    ControlWork,
    
    [Description("Компьютерные практикумы")]
    ComputerPractice,

    [Description("Контесты")]
    Contest
}
