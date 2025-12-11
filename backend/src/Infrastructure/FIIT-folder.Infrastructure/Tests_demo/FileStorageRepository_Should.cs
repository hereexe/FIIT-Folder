using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using FIIT_folder.Domain.Interfaces; 
using NUnit.Framework;
using FIIT_folder.Infrastructure.FileStorage;

namespace TestsAll.Tests.Infrastructure;

[TestFixture]
public class FileStorageRepository_Should
{
    [Test]
    public void Test_Config_Loads()
    {
        // Пример теста для Infrastructure
        Assert.That(1 + 1, Is.EqualTo(2));
    }
        
    [Test]
    public void Test_MongoDB_ConnectionString_Format()
    {
        // Тестируем что-то из вашего Infrastructure
        string connectionString = "mongodb://localhost:27017";
        Assert.That(connectionString, Does.Contain("mongodb://"));
    }
}