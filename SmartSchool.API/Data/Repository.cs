using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Helpers;
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


        public User GetUser(string username, string password)
        {
            var users = new List<User>();
            users.Add(new User { Id = 1, Username = "batman", Password = "batman", Role = "manager" });
            users.Add(new User { Id = 2, Username = "robin", Password = "robin", Role = "employee" });
            users.Add(new User { Id = 3, Username = "11", Password = "11", Role = "employee" });
            return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();
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

        public async Task<PageList<Aluno>> GetAllAlunosAsync(
            PageParams pageParams,
            bool includeProfessor = false)
        {
            IQueryable<Aluno> query = _contexto.Alunos;

            if (includeProfessor)
            {
                query = query.Include(x => x.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Disciplina)
                    .ThenInclude(d => d.Professor);
            }

            query = query.AsNoTracking().OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(pageParams.Nome))
            {
                query = query.Where(aluno => aluno.Nome.ToUpper().Contains(pageParams.Nome.ToUpper())
                                    || aluno.Sobrenome.ToUpper().Contains(pageParams.Nome.ToUpper()));
            }

            if(pageParams.Matricula > 0)
            {
                query = query.Where(aluno => aluno.Matricula == pageParams.Matricula);
            }

            if (pageParams.Ativo != null)
            {
                query = query.Where(aluno => aluno.Ativo == (pageParams.Ativo != 0));
            }


            return await PageList<Aluno>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);
            //return await query.ToListAsync();
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

        public async Task<Aluno[]> GetAllAlunosByDisciplinaIdAsync(int disciplinaId, 
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
            return await query.ToArrayAsync();
        }

        public Aluno GetAllAlunoById(int id, 
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

        public Professor[] GetAllProfessores(bool includeAlunos = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeAlunos){
                query = query.Include(x=> x.Disciplinas)
                    .ThenInclude(d => d.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Aluno)
                    ;
            }

            query = query.AsNoTracking().OrderBy(x=> x.Id);
            return query.ToArray();
        }

        public Professor[] GetAllProfessoresByDisciplinaId(int disciplinaId, 
            bool includeAlunos = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeAlunos){
                query = query.Include(x=> x.Disciplinas)
                    .ThenInclude(d => d.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Aluno)
                    ;
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id)
                .Where(x=> x.Disciplinas
                .Any(d=> d.AlunoDisciplinas.Any(ad => ad.DisciplinaId == disciplinaId)));
            return query.ToArray();
        }

        public Professor GetAllProfessorById(int id,
            bool includeAlunos = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeAlunos){
                query = query.Include(x=> x.Disciplinas)
                    .ThenInclude(d => d.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Aluno)
                    ;
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id).Where(x=> x.Id == id);
            return query.FirstOrDefault();
        }

        public Professor[] GetAllProfessoresByAlunoId(int alunoId,
            bool includeAlunos = false)
        {
            IQueryable<Professor> query = _contexto.Professores;

            if (includeAlunos){
                query = query.Include(x=> x.Disciplinas)
                    .ThenInclude(d => d.AlunoDisciplinas)
                    .ThenInclude(ad => ad.Aluno)
                    ;
            }

            query = query.AsNoTracking()
                .OrderBy(x=> x.Id)
                .Where(aluno => aluno.Disciplinas.Any(
                    d => d.AlunoDisciplinas.Any(ad => ad.Aluno.Id == alunoId)
                ));
            return query.ToArray();
        }
    }
}