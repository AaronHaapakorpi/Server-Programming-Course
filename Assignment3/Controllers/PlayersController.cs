using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;
namespace Assignment3.Controllers
{
    
    [ApiController]
    [Route("api")]
    public class PlayersController:ControllerBase {
        private readonly IRepository repository;

        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger,IRepository rep){
            _logger = logger;
            repository = rep;
        }
        [HttpGet]
        [Route("Get")]
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
    }
}
