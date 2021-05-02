using SmartSchool.API.Helpers;
using SmartSchool.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartSchool.API.Data
{
    public interface IRepository
    {
        User GetUser(string username, string password);

         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         bool SaveChanges();

         Task<PageList<Aluno>> GetAllAlunosAsync(PageParams pageParams, bool includeProfessor =false);
         Aluno [] GetAllAlunos(bool includeProfessor =false);
         Task<Aluno[]> GetAllAlunosByDisciplinaIdAsync(int disciplinaId, 
            bool includeProfessor = false);
         Aluno GetAllAlunoById(int id, 
            bool includeProfessor = false);

         
         Professor[] GetAllProfessores(bool includeAlunos =false);
         Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, 
            bool includeAlunos = false);
         Professor GetAllProfessorById(int id,
            bool includeAlunos = false);

         Professor[] GetAllProfessoresByAlunoId(int alunoId,
            bool includeAlunos = false);

        //  T [] ObterTodos();
    }
}