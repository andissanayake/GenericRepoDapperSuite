using Dapper;
using FluentAssertions;
using GenericRepository.Data.Entity;

namespace GenericRepository.Data.Test
{
    public class UnitOfWorkGenericRepositoryTest : ContextFixture
    {
        [Fact]
        public async void GetAllAsyncTest()
        {
            dbConnection.Open();
            dbConnection.Execute(@"insert into [dbo].[YourEntity1] select 'GetAllAsyncTest','p2','2024/1/1',null");
            dbConnection.Close();

            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var data = await unitOfWork.Repository<YourEntity1>().GetAllAsync();
            var item = data.FirstOrDefault(i => i.Prop1 == "GetAllAsyncTest");
            item.Should().NotBe(null);
        }

        [Fact]
        public async void GetByIdAsyncTest()
        {
            dbConnection.Open();
            dbConnection.Execute(@"insert into [dbo].[YourEntity1] select 'p1','p2','2024/1/1',null");
            dbConnection.Close();

            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var list = await unitOfWork.Repository<YourEntity1>().GetAllAsync();
            var data = await unitOfWork.Repository<YourEntity1>().GetByIdAsync(list.First().Id);
            data.Should().NotBeNull(null);
        }

        [Fact]
        public async void InsertAsyncTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Commit();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var data = await unitOfWork1.Repository<YourEntity1>().GetByIdAsync(item.Id);
            data.Should().NotBeNull(null);
        }

        [Fact]
        public async void UpdateAsyncTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Commit();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var updateKey = "updated xyz";
            item.Prop1 = updateKey;
            await unitOfWork1.Repository<YourEntity1>().UpdateAsync(item);
            unitOfWork1.Commit();

            using var unitOfWork2 = new UnitOfWork(GetDbConnection());
            var data = await unitOfWork2.Repository<YourEntity1>().GetByIdAsync(item.Id);
            data.Prop1.Should().Be(updateKey);
        }

        [Fact]
        public async void DeleteAsyncTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Commit();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            await unitOfWork1.Repository<YourEntity1>().DeleteAsync(item.Id);
            unitOfWork1.Commit();

            using var unitOfWork2 = new UnitOfWork(GetDbConnection());
            var data = await unitOfWork2.Repository<YourEntity1>().GetByIdAsync(item.Id);
            data.Should().Be(null);
        }
    }
}