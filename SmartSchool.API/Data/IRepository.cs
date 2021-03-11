using SmartSchool.API.Models;

namespace SmartSchool.API.Data
{
    public interface IRepository
    {
         void Add<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         bool SaveChanges();

         Aluno [] GetAllAlunos(bool includeProfessor =false);
         Aluno [] GetAllAlunosByDisciplinaId(int disciplinaId, 
            bool includeProfessor = false);
         Aluno GetAllAlunosById(int id, 
            bool includeProfessor = false);

         
         Professor[] GetAllProfessores(bool includeDisciplina =false);
         Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, 
            bool includeDisciplina = false);
         Professor GetAllProfessoresById(int id,
            bool includeDisciplina = false);

        //  T [] ObterTodos();
    }
}