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

        public ProfessorController(SmartContext context)
        {
            _context = context;
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
            _context.Add(professor);
            _context.SaveChanges();
            return Ok(professor);
        }


        //api/Professor
        [HttpPut("{id}")]        
        public IActionResult Put(int id, Professor professor)
        {        
            var alu = _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (alu == null){
                return BadRequest("Professor não encontrado.");
            }    

            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }

        //api/Professor
        [HttpPatch("{id}")]        
        public IActionResult Patch(int id, Professor professor)
        {            
            var prof = _context.Professores.AsNoTracking().FirstOrDefault(x=> x.Id == id);
            if (prof == null){
                return BadRequest("Professor não encontrado.");
            }
            _context.Update(professor);
            _context.SaveChanges();
            return Ok(professor);
        }

        //api/Professor
        [HttpDelete("{id}")]        
        public IActionResult Delete(int id)
        {           
            var professor = _context.Professores.FirstOrDefault(x=> x.Id == id);
            if (professor == null){
                return BadRequest("Professor não encontrado.");
            }
            _context.Remove(professor);
            _context.SaveChanges(); 
            return Ok("Professor deletado com sucesso.");
        }


    }
}
