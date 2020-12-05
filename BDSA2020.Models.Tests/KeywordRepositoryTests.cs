using System.Threading.Tasks;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class KeywordRepositoryTests : SqlLiteContext
    {
        private readonly IKeywordRepository repository;

        public KeywordRepositoryTests() : base()
        {
            repository = new KeywordRepository(Context);
        }

        [Fact]
        public async Task GetCompaniesAsync_returns_list_of_companies()
        {
            var expected = new [] 
            {
                "Testing",
                "C#",
                "Java",
                "FullStack",
                "Frontend",
                "Backend",
                "UML",
                "DevOps",
                "Communication",
                "JavaScript"
            };

            var actual = await repository.GetKeywordsAsync();

            Assert.Equal(expected, actual);
        }
    }
}
