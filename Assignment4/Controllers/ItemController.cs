using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;


namespace Assignment4.Controllers
{
    
    [ApiController]
    [Route("players/{playerId}/items")]
    public class ItemController:ControllerBase {
        private readonly IRepository repository;

        private readonly ILogger<PlayersController> _logger;

        public ItemController(ILogger<PlayersController> logger,IRepository rep){
            _logger = logger;
            repository = rep;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<Item> Get(Guid playerId,Guid itemId){
            return await repository.GetItem(playerId,itemId);
            
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<Item[]> GetAll(Guid playerId){
           return await repository.GetAllItems(playerId);
            
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Item> Create(Guid playerId,[FromBody]NewItem newItem){
            var item = new Item(){
                Id = Guid.NewGuid(),
                ItemType = newItem.ItemType,
                Level = newItem.Level,
                CreationDate = DateTime.Now
            };
            return await repository.CreateItem(playerId,item);
        }

        //update what item? assignment function doesn't contain item id or item to update
        [HttpGet]
        [Route("Update")]
        public async Task<Item> Update(Guid playerId,[FromBody] ModifiedItem modifiedItem){
            Item item = new Item();
            item.Price = modifiedItem.Price;
            try{
                await repository.UpdateItem(playerId,item);
            }
            catch{
                throw new NotFoundException();
            }
            return item;
            
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Item> Delete(Guid playerId,Item item){
            return await repository.DeleteItem(playerId,item);
        }
    }
}
