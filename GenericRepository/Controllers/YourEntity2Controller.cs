using GenericRepository.Data;
using GenericRepository.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace GenericRepository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YourEntity2Controller : ControllerBase
    {

        private readonly ILogger<YourEntity2Controller> _logger;
        private IUnitOfWorkFactory _unitOfWorkFactory;
        public YourEntity2Controller(ILogger<YourEntity2Controller> logger, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet(Name = "GetYourEntity2")]
        public async Task<IEnumerable<YourEntity2>> Get()
        {
            using var uow = _unitOfWorkFactory.CreateUnitOfWork();
            var insertTask1 = uow.Repository<YourEntity2>().InsertAsync(new YourEntity2() { Prop1 = DateTime.Now.ToString(), });
            var insertTask2 = uow.Repository<YourEntity2>().InsertAsync(new YourEntity2() { Prop1 = DateTime.Now.ToString(), });
            var insertTask3 = uow.Repository<YourEntity2>().InsertAsync(new YourEntity2() { Prop1 = DateTime.Now.ToString(), });
            await Task.WhenAll(insertTask1, insertTask2, insertTask3);
            uow.Commit();

            using var uow1 = _unitOfWorkFactory.CreateUnitOfWork();
            var itemList = await uow1.Repository<YourEntity2>().GetAllAsync();
            var item = itemList.First();
            item.Prop1 = "updated";
            await uow1.Repository<YourEntity2>().UpdateAsync(item);
            uow1.Commit();

            using var uow2 = _unitOfWorkFactory.CreateUnitOfWork();
            return await uow2.Repository<YourEntity2>().GetAllAsync();
        }
    }
}
