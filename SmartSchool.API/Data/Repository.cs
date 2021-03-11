using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Models;

namespace SmartSchool.API.Data
{
    public class Repository : IRepository
    {
        private readonly SmartContext _contexto;
        public Repository(SmartContext contexto)
        {
            _contexto = contexto;
        }
        public void Add<T>(T entity) where T : class
        {
            _contexto.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _contexto.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _contexto.Update(entity);
        }

        public bool SaveChanges()
        {
            return _contexto.SaveChanges() > 0;
        }

        public Aluno[] GetAllAlunos(bool includeProfessor =false)
        {
            IQueryable<Aluno> query = _contexto.Alunos;

            if (includeProfessor){
                query = query.Include(x=> x.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d=> d.Professor);
            }

            query = query.AsNoTracking().OrderBy(x=> x.Id);
            return query.ToArray();
        }

        public Aluno[] GetAllAlunosByDisciplinaId(int disciplinaId, 
            bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _contexto.Alunos;

            if (includeProfessor){
                query = query.Include(x=> x.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d=> d.Professor);
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id)
                .Where(x=> x.AlunoDisciplinas
                .Any(ad=>ad.DisciplinaId == disciplinaId));
            return query.ToArray();
        }

        public Aluno GetAllAlunosById(int id, 
            bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _contexto.Alunos;

            if (includeProfessor){
                query = query.Include(x=> x.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d=> d.Professor);
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id).Where(x=> x.Id == id);
            return query.FirstOrDefault();
        }

        public Professor[] GetAllProfessores(bool includeDisciplina =false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeDisciplina){
                query = query.Include(x=> x.Disciplinas)
                    ;
            }

            query = query.AsNoTracking().OrderBy(x=> x.Id);
            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, 
            bool includeDisciplina = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeDisciplina){
                query = query.Include(x=> x.Disciplinas)
                    ;
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id)
                .Where(x=> x.Disciplinas
                .Any(ad=>ad.Id == disciplinaId));
            return query.ToArray();
        }

        public Professor GetAllProfessoresById(int id,
            bool includeDisciplina = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeDisciplina){
                query = query.Include(x=> x.Disciplinas)
                    ;
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id).Where(x=> x.Id == id);
            return query.FirstOrDefault();
        }
    }
}