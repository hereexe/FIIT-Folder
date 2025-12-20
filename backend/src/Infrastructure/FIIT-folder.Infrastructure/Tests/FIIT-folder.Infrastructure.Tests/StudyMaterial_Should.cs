// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using FIIT_folder.Domain.Entities;
// using FIIT_folder.Domain.Enums;
// using FIIT_folder.Domain.Value_Object;
// using FIIT_folder.Infrastructure.FileStorage;
// using NUnit.Framework;
//
// namespace FIIT_folder.Infrastructure.Tests;
//
// [TestFixture]
// public class Basic_Should
// {
//     [Test]
//     public void TwoPlusTwoEqualsFour()
//     {
//         Assert.That(2 + 2, Is.EqualTo(4));
//     }
// }
//
// // [TestFixture]
// // public class StudyMaterialMongoDB_Should
// // {
// //     private StudyMaterialMongoDB mongo;
// //
// //     // [SetUp]
// //     // public void SetUp()
// //     // {
// //     //     mongo = mongo.Start();
// //     //     repository = new StudyMaterialMongoDB(
// //     //         mongo.ConnectionString,
// //     //         "StudyMaterialTestDb"
// //     //     );
// //     // }
// //     //
// //     // [TearDown]
// //     // public void TearDown()
// //     // {
// //     //     mongo.Dispose();
// //     // }
// // }    



using NUnit.Framework;

namespace FIIT_folder.Infrastructure.Tests;

[TestFixture]
public class StudyMaterial_Should
{
    [Test]
    public void TwoPlusTwoEqualsFour()
    {
        Assert.That(2 + 2, Is.EqualTo(4));
    }
}