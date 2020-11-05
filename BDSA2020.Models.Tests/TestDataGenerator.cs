using BDSA2020.Entities;

namespace BDSA2020.Models.Tests
{
    public static class TestDataGenerator
    {
        public static void GenerateTestData(this Context context)
        {
            context.Students.AddRange(
                    new Student { Degree = Degree.Bachelor },
                    new Student { Degree = Degree.Master },
                    new Student { Degree = Degree.PhD },
                    new Student { Degree = Degree.Other }
                );
            context.SaveChanges();
        }
    }
}