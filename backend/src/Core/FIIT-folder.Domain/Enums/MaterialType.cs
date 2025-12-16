using System.ComponentModel;

namespace FIIT_folder.Domain.Enums;

public enum MaterialType
{
    [Description("Экзамен")]
    Exam,
    
    [Description("Коллоквиум")]
    Colloquium,
    
    [Description("Зачёт")]
    Pass,
    
    [Description("Контрольная работа")]
    ControlWork,
    
    [Description("Компьютерный практикум")]
    ComputerPractice
}
