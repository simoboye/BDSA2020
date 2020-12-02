using System;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class StudentRepositoryTests : SqlLiteContext
    {
        private readonly IStudentRepository repository;

        private readonly Guid id1 = new Guid("290c1a5f-3790-4bcb-89dc-6a4c3de155d1");
        private readonly Guid id2 = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");

        public StudentRepositoryTests() : base()
        {
            repository = new StudentRepository(Context);
        }

        [Fact]
        public async Task GetStudentsAsync_returns_list_of_students()
        {
            // Act
            var actual = await repository.GetStudentsAsync();

            // Assert
            Assert.Equal(4, actual.Count);
        }

        [Fact]
        public async Task GetStudent_returns_the_requested_student() 
        {
            var id = id1;
            var actual = await repository.GetStudentAsync(id);

            var expected = new StudentDetailsDTO
            { 
                Id = id,
                Degree = Degree.Bachelor, 
                KeywordNames = new [] { "Testing", "C#" },
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere",
                PlacementDescriptionIds = new [] { 1, 2 }
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Degree, actual.Degree);
            Assert.Equal(expected.PlacementDescriptionIds, actual.PlacementDescriptionIds);
            Assert.Equal(expected.KeywordNames, actual.KeywordNames);
        }

        [Fact]
        public async Task GetStudent_returns_ArgumentException_on_not_found_student() 
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetStudentAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task CreateStudent_returns_the_id_of_created_student()
        {
            var id = Guid.NewGuid();
            var student = new CreateStudentDTO
            { 
                Id = id,
                Degree = Degree.Bachelor,
                KeywordNames = new [] { "Testing" },
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere" 
            };

            var actual = await repository.CreateStudentAsync(student);

            Assert.Equal(student.Id, actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_true() 
        {
            var entity = await Context.Students.FirstOrDefaultAsync();
            var actual = await repository.DeleteStudentAsync(entity.Id);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DeleteStudentAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateStudent_returns_true_on_updated()
        {
            var studentToUpdate = await Context.Students.FirstAsync();
            var dto = new UpdateStudentDTO
            {
                Id = studentToUpdate.Id,
                KeywordNames = new [] { "Testing", "C#" },
                Degree = Degree.Other,
                MinSalary = 1,
                MinWorkingHours = 1,
                MaxWorkingHours = 2,
                Agreement = true,
                Location = "København"
            };
            var actual = await repository.UpdateStudentAsync(dto);

            Assert.True(actual);
            Assert.Equal(dto.Id, studentToUpdate.Id);
            Assert.Equal(dto.KeywordNames, studentToUpdate.Keywords.Select(k => k.Keyword.Name).ToList());
        }

        [Fact]
        public async Task UpdateStudent_returns_ArgumentException_on_not_found()
        {
            var studentToUpdate = new UpdateStudentDTO { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UpdateStudentAsync(studentToUpdate));
        }

        [Fact]
        public async Task SavePlacementDescription_adds_Id_to_saved_list()
        {
            var studentToLike = await Context.Students.FindAsync(id2);
            var descriptionToBeLiked = await Context.PlacementDescriptions.FindAsync(1);

            var actual = await repository.SavePlacementDescription(studentToLike.Id, descriptionToBeLiked.Id);

            Assert.True(actual);
        }

        [Fact]
        public async Task SavePlacementDescription_throws_ArgumentException_on_not_found_student_or_description()
        {
            var studentToLike = await Context.Students.FindAsync(id2);
            var descriptionToBeLiked = await Context.PlacementDescriptions.FindAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => repository.SavePlacementDescription(Guid.NewGuid(), descriptionToBeLiked.Id));
            await Assert.ThrowsAsync<ArgumentException>(() => repository.SavePlacementDescription(studentToLike.Id, 100));
        }

        [Fact]
        public async Task UnsavePlacementDescription_removes_Id_from_saved_list()
        {
            var studentToLike = await Context.Students.FindAsync(id1);
            var descriptionToBeLiked = await Context.PlacementDescriptions.FindAsync(1);

            var actual = await repository.UnSavePlacementDescription(studentToLike.Id, descriptionToBeLiked.Id);

            Assert.True(actual);
        }

        [Fact]
        public async Task UnSavePlacementDescription_throws_ArgumentException_on_not_found_student_or_description()
        {
            var studentToLike = await Context.Students.FindAsync(id1);
            var descriptionToBeLiked = await Context.PlacementDescriptions.FindAsync(1);

            await Assert.ThrowsAsync<ArgumentException>(() => repository.UnSavePlacementDescription(Guid.NewGuid(), descriptionToBeLiked.Id));
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UnSavePlacementDescription(studentToLike.Id, 100));
        }
    }
}
