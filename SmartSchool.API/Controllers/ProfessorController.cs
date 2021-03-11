using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly SmartContext _context;
        private readonly IRepository _repo;

        public ProfessorController(SmartContext context,
            IRepository repo)
        {
            _context = context;
            _repo = repo;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Professores);
        }

        [HttpGet("byId/{id}")]
        public IActionResult GetById(int id)
        {
            var professor = _context.Professores.Where(x=> x.Id == id).FirstOrDefault();
            if (professor == null){
                return BadRequest("Professor não encontrado");
            }
            return Ok(professor);
        }

        //query string >> ex: api/Professor/byName?nome=Laura&sobrenome=Maria
        [HttpGet("byName")]        
        public IActionResult GetByName(string nome)
        {
            var professor = _context.Professores.Where(x=> 
                x.Nome.ToUpper().Contains(nome.ToUpper())
            ).FirstOrDefault();
            if (professor == null){
                return BadRequest("Professor não encontrado");
            }
            return Ok(professor);
        }





        //api/Professor >> qdo a entidade ta no param, é o body
        [HttpPost()]        
        public IActionResult Post(Professor professor)
        {            
            _repo.Add(professor);
            if (_repo.SaveChanges()){
                return Ok(professor);
            }
            return BadRequest("Professor não cadastrado.");
        }


        //api/Professor
        [HttpPut("{id}")]        
        public IActionResult Put(int id, Professor professor)
        {        
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (prof == null){
                return BadRequest("Professor não encontrado.");
            }    

            _repo.Update(prof);
            if (_repo.SaveChanges()){
                return Ok(prof);
            }
            return BadRequest("Professor não cadastrado.");
        }

        //api/Professor
        [HttpPatch("{id}")]        
        public IActionResult Patch(int id, Professor professor)
        {            
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (prof == null){
                return BadRequest("Professor não encontrado.");
            }
            _repo.Update(professor);
            if (_repo.SaveChanges()){
                return Ok(professor);
            }
            return BadRequest("Professor não cadastrado.");
        }

        //api/Professor
        [HttpDelete("{id}")]        
        public IActionResult Delete(int id)
        {           
            var professor = _context.Professores.FirstOrDefault(x=> x.Id == id);
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
