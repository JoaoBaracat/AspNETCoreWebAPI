using Microsoft.AspNetCore.Mvc;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        public List<Aluno> Alunos = new List<Aluno>(){
            new Aluno() {
                Id = 1,
                Nome = "Marcos",
                Sobrenome = "Almeida",
                Telefone = "1351132135"
            },
            new Aluno() {
                Id = 2,
                Nome = "Marta",
                Sobrenome = "Kent",
                Telefone = "43567657"
            },
            new Aluno() {
                Id = 3,
                Nome = "Laura",
                Sobrenome = "Maria",
                Telefone = "21343432"
            }
            
        };

        public AlunoController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }


        // [HttpGet("{id:int}")]
        // public IActionResult GetById(int id)
        // {
        //     var aluno = Alunos.Where(x=> x.Id == id).FirstOrDefault();
        //     if (aluno == null){
        //         return BadRequest("Aluno não encontrado");
        //     }
        //     return Ok(aluno);
        // }

        //querystring
        ///api/Aluno/byId?id=1
        // [HttpGet("byId")]
        // public IActionResult GetById(int id)
        // {
        //     var aluno = Alunos.Where(x=> x.Id == id).FirstOrDefault();
        //     if (aluno == null){
        //         return BadRequest("Aluno não encontrado");
        //     }
        //     return Ok(aluno);
        // }


        ///api/Aluno/byId/1
        [HttpGet("byId/{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = Alunos.Where(x=> x.Id == id).FirstOrDefault();
            if (aluno == null){
                return BadRequest("Aluno não encontrado");
            }
            return Ok(aluno);
        }

        //query string >> ex: api/Aluno/byName?nome=Laura&sobrenome=Maria
        [HttpGet("byName")]        
        public IActionResult GetByName(string nome, string sobrenome)
        {
            var aluno = Alunos.Where(x=> 
                x.Nome.ToUpper().Contains(nome.ToUpper())
                && x.Sobrenome.ToUpper().Contains(sobrenome.ToUpper())
            ).FirstOrDefault();
            if (aluno == null){
                return BadRequest("Aluno não encontrado");
            }
            return Ok(aluno);
        }





        //api/Aluno >> qdo a entidade ta no param, é o body
        [HttpPost()]        
        public IActionResult Post(Aluno aluno)
        {            
            return Ok(aluno);
        }


        //api/Aluno
        [HttpPut("{id}")]        
        public IActionResult Put(int id, Aluno aluno)
        {            
            return Ok(aluno);
        }

        //api/Aluno
        [HttpPatch("{id}")]        
        public IActionResult Patch(int id, Aluno aluno)
        {            
            return Ok(aluno);
        }

        //api/Aluno
        [HttpDelete("{id}")]        
        public IActionResult Delete(int id)
        {            
            return Ok("deltar aluno");
        }










        // [HttpGet("{nome}")]
//         public IActionResult GetByName(string nome)
//         {
//             var aluno = Alunos.Where(x=> x.Nome.Contains(nome)).FirstOrDefault();
//             if (aluno == null){
//                 return BadRequest("Aluno não encontrado");
//             }
//             return Ok(aluno);
//         }



    }
}
