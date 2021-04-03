using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.V1.Dtos;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.V1.Controllers
{
    /// <summary>
    /// Versão 1 do controlador de alunos
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mapper"></param>
        public AlunoController(IRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        // public List<Aluno> Alunos = new List<Aluno>(){
        //     new Aluno() {
        //         Id = 1,
        //         Nome = "Marcos",
        //         Sobrenome = "Almeida",
        //         Telefone = "1351132135"
        //     },
        //     new Aluno() {
        //         Id = 2,
        //         Nome = "Marta",
        //         Sobrenome = "Kent",
        //         Telefone = "43567657"
        //     },
        //     new Aluno() {
        //         Id = 3,
        //         Nome = "Laura",
        //         Sobrenome = "Maria",
        //         Telefone = "21343432"
        //     }

        // };

        /// <summary>
        /// Método responsável para retornar todos os alunos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var alunos = _repo.GetAllAlunos(false);
            
            return Ok(_mapper.Map<IEnumerable<AlunoDto>>(alunos));
        }

        /// <summary>
        /// Método responsável por retornar apenas um único AlunoDTO.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new AlunoRegistrarDto());
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
        // /api/Aluno/byId?id=1
        // [HttpGet("byId")]
        // public IActionResult GetById(int id)
        // {
        //     var aluno = Alunos.Where(x=> x.Id == id).FirstOrDefault();
        //     if (aluno == null){
        //         return BadRequest("Aluno não encontrado");
        //     }
        //     return Ok(aluno);
        // }

        /// <summary>
        /// Método responsável por retornar apenas um Aluno por ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // var aluno = _context.Alunos.Where(x => x.Id == id).FirstOrDefault();
            var aluno = _repo.GetAllAlunoById(id, false);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado");
            }

            var alunoDto = _mapper.Map<AlunoDto>(aluno);

            return Ok(alunoDto);
        }

        //query string >> ex: api/Aluno/byName?nome=Laura&sobrenome=Maria
        // [HttpGet("byName")]
        // public IActionResult GetByName(string nome, string sobrenome)
        // {
        //     var aluno = _context.Alunos.Where(x =>
        //         x.Nome.ToUpper().Contains(nome.ToUpper())
        //         && x.Sobrenome.ToUpper().Contains(sobrenome.ToUpper())
        //     ).FirstOrDefault();
        //     if (aluno == null)
        //     {
        //         return BadRequest("Aluno não encontrado");
        //     }
        //     return Ok(aluno);
        // }





        //api/Aluno >> qdo a entidade ta no param, é o body
        [HttpPost()]
        public IActionResult Post(AlunoRegistrarDto model)
        {
            var aluno = _mapper.Map<Aluno>(model);

            _repo.Add(aluno);
            if (_repo.SaveChanges())
            {
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            return BadRequest("Aluno não cadastrado.");
        }


        //api/Aluno
        [HttpPut("{id}")]
        public IActionResult Put(int id, AlunoRegistrarDto model)
        {
            var aluno = _repo.GetAllAlunoById(id);// _context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado.");
            }

            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges()){
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            return BadRequest("Aluno não cadastrado.");
        }

        //api/Aluno
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, AlunoRegistrarDto model)
        {
            var aluno = _repo.GetAllAlunoById(id);//_context.Alunos.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado.");
            }
            _mapper.Map(model, aluno);

            _repo.Update(aluno);
            if (_repo.SaveChanges()){
                return Created($"/api/aluno/{model.Id}", _mapper.Map<AlunoDto>(aluno));
            }
            return BadRequest("Aluno não cadastrado.");
        }

        //api/Aluno
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAllAlunoById(id);//_context.Alunos.FirstOrDefault(x => x.Id == id);
            if (aluno == null)
            {
                return BadRequest("Aluno não encontrado.");
            }
             _repo.Delete(aluno);
            if (_repo.SaveChanges()){
                return Ok("Aluno deletado com sucesso.");
            }
            return BadRequest("Aluno não cadastrado.");
            
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
