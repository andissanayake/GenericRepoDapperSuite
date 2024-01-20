using FluentAssertions;
using GenericRepository.Data.Entity;

namespace GenericRepository.Data.Test
{
    public class UnitOfWorkTest : ContextFixture
    {
        [Fact]
        public async void CommitTest()
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
        public async void RollbackTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Rollback();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var data = await unitOfWork1.Repository<YourEntity1>().GetByIdAsync(item.Id);
            data.Should().BeNull(null);
        }

        [Fact]
        public async void MultipleCommitTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Commit();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var item1 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork1.Repository<YourEntity1>().InsertAsync(item1);
            unitOfWork1.Commit();

            using var unitOfWork2 = new UnitOfWork(GetDbConnection());
            var item2 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork2.Repository<YourEntity1>().InsertAsync(item2);
            unitOfWork2.Commit();

            using var unitOfWork3 = new UnitOfWork(GetDbConnection());
            var list = await unitOfWork3.Repository<YourEntity1>().GetAllAsync();
            list.First(i => i.Id == item.Id).Should().NotBeNull();
            list.First(i => i.Id == item1.Id).Should().NotBeNull();
            list.First(i => i.Id == item2.Id).Should().NotBeNull();
        }

        [Fact]
        public async void MultipleRollbackTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Rollback();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var item1 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork1.Repository<YourEntity1>().InsertAsync(item1);
            unitOfWork1.Rollback();

            using var unitOfWork2 = new UnitOfWork(GetDbConnection());
            var item2 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork2.Repository<YourEntity1>().InsertAsync(item2);
            unitOfWork2.Rollback();

            using var unitOfWork3 = new UnitOfWork(GetDbConnection());
            var list = await unitOfWork3.Repository<YourEntity1>().GetAllAsync();
            list.FirstOrDefault(i => i.Id == item.Id).Should().BeNull();
            list.FirstOrDefault(i => i.Id == item1.Id).Should().BeNull();
            list.FirstOrDefault(i => i.Id == item2.Id).Should().BeNull();
        }

        [Fact]
        public async void MultipleCommitRollbackTest()
        {
            using var unitOfWork = new UnitOfWork(GetDbConnection());
            var item = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork.Repository<YourEntity1>().InsertAsync(item);
            unitOfWork.Commit();

            using var unitOfWork1 = new UnitOfWork(GetDbConnection());
            var item1 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork1.Repository<YourEntity1>().InsertAsync(item1);
            unitOfWork1.Rollback();

            using var unitOfWork2 = new UnitOfWork(GetDbConnection());
            var item2 = new YourEntity1() { Prop1 = "p1", Prop2 = "p2" };
            await unitOfWork2.Repository<YourEntity1>().InsertAsync(item2);
            unitOfWork2.Commit();

            using var unitOfWork3 = new UnitOfWork(GetDbConnection());
            var list = await unitOfWork3.Repository<YourEntity1>().GetAllAsync();
            list.FirstOrDefault(i => i.Id == item.Id).Should().NotBeNull();
            list.FirstOrDefault(i => i.Id == item1.Id).Should().BeNull();
            list.FirstOrDefault(i => i.Id == item2.Id).Should().NotBeNull();
        }
    }
}
