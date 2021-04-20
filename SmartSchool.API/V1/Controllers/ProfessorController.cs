using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.V1.Dtos;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.V1.Controllers
{
    /// <summary>
    /// Versão 1 do controlador de professores
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        // private readonly SmartContext _context;
        private readonly IRepository _repo;
        private readonly IMapper _mapper;

        public ProfessorController(//SmartContext context,
            IRepository repo, IMapper mapper)
        {
            // _context = context;
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var professores = _repo.GetAllProfessores(true);
            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(professores));
        }

        [HttpGet("getRegister")]
        public IActionResult GetRegister()
        {
            return Ok(new ProfessorRegistrarDto());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var professor = _repo.GetAllProfessorById(id, true);// _context.Professores.Where(x=> x.Id == id).FirstOrDefault();
            if (professor == null){
                return BadRequest("Professor não encontrado");
            }
            return Ok(_mapper.Map<ProfessorDto>(professor));
        }


        [HttpGet("ByAluno/{alunoId}")]
        public IActionResult GetByAluno(int alunoId)
        {
            var professores = _repo.GetAllProfessoresByAlunoId(alunoId, true);// _context.Professores.Where(x=> x.Id == id).FirstOrDefault();
            if (professores == null){
                return BadRequest("Professores não encontrados");
            }
            return Ok(_mapper.Map<IEnumerable<ProfessorDto>>(professores));
        }

        // //query string >> ex: api/Professor/byName?nome=Laura&sobrenome=Maria
        // [HttpGet("byName")]        
        // public IActionResult GetByName(string nome)
        // {
        //     var professor = _context.Professores.Where(x=> 
        //         x.Nome.ToUpper().Contains(nome.ToUpper())
        //     ).FirstOrDefault();
        //     if (professor == null){
        //         return BadRequest("Professor não encontrado");
        //     }
        //     return Ok(professor);
        // }





        //api/Professor >> qdo a entidade ta no param, é o body
        [HttpPost()]        
        public IActionResult Post(ProfessorRegistrarDto model)
        {
            var professor = _mapper.Map<Professor>(model);
            _repo.Add(professor);
            if (_repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(professor));
            }
            return BadRequest("Professor não cadastrado.");
        }


        //api/Professor
        [HttpPut("{id}")]        
        public IActionResult Put(int id, ProfessorRegistrarDto model)
        {        
            var prof = _repo.GetAllProfessorById(id, false);// _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (prof == null){
                return BadRequest("Professor não encontrado.");
            }

            _mapper.Map(model, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof)); ;
            }
            return BadRequest("Professor não cadastrado.");
        }

        //api/Professor
        [HttpPatch("{id}")]        
        public IActionResult Patch(int id, ProfessorRegistrarDto model)
        {            
            var prof = _repo.GetAllProfessorById(id, false);// _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (prof == null){
                return BadRequest("Professor não encontrado.");
            }

            _mapper.Map(model, prof);

            _repo.Update(prof);
            if (_repo.SaveChanges()){
                return Created($"/api/professor/{model.Id}", _mapper.Map<ProfessorDto>(prof)); ;
            }
            return BadRequest("Professor não cadastrado.");
        }

        //api/Professor
        [HttpDelete("{id}")]        
        public IActionResult Delete(int id)
        {           
            var professor = _repo.GetAllProfessorById(id, false);// _context.Professores.FirstOrDefault(x=> x.Id == id);
            if (professor == null){
                return BadRequest("Professor não encontrado.");
            }
            _repo.Delete(professor);
            if (_repo.SaveChanges()){
                return Ok("Professor deletado com sucesso.");
            }
            return BadRequest("Professor não cadastrado.");
            
        }


    }
}
