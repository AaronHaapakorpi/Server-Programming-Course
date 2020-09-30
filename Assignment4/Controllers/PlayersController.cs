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
    [Route("players")]
    public class PlayersController:ControllerBase {
        private readonly IRepository repository;

        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger,IRepository rep){
            _logger = logger;
            repository = rep;
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public Task<Player> Get(Guid id){
            return repository.Get(id);
            
        }
        [HttpGet]
        [Route("GetAll")]
        public Task<Player[]> GetAll(){
           return repository.GetAll();
            
        }
        //testing adding with httppost, usage -> { "Name":"name of player here"}
        [HttpPost]
        [Route("Create")]
        public Task<Player> Create(NewPlayer player){
            return repository.Create(new Player{Name = player.Name});
        }
        [HttpGet]
        [Route("Modify/{id}")]
        public Task<Player> Modify(Guid id,[FromBody] ModifiedPlayer player){
            Console.WriteLine(player.Score);
            return repository.Modify(id,player);
        }

        //testing deleting with route parameters, could also use a httppost to send the id to delete
        [HttpGet]
        [Route("Delete/{id}")]
        public Task<Player> Delete(Guid id){
            return repository.Delete(id);
        
        }

        [HttpGet]

        [Route("")]
        public async Task<Player[]> GetWithScoreOver(int minScore){
            return await repository.GetWithScoreOver(minScore);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<Player> GetWithName(string name){
            return await repository.GetWithName(name);
        }
        [HttpPost]
        [Route("UpdateName/{id}")]
         public async Task<Player> UpdateName(Guid id, string name){
             return await repository.UpdateName(id,name);
         }
        [HttpGet]
        [Route("Top")]
         public async Task<Player[]> GetSorted(){
             return await repository.GetSorted(10);
         }

         [HttpGet]
         [Route("MostCommon")]

         public async Task<int> GetMostCommonLevel(){
             return await repository.GetMostCommonLevel();
         }

    }
}
