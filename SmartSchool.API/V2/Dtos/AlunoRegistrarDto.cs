using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.V2.Dtos
{
    /// <summary>
    /// Objeto para salvar ou alterar alunos novos.
    /// </summary>
    public class AlunoRegistrarDto
    {
        /// <summary>
        /// ID do Aluno
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Chave do Aluno para outros negócios na instituição
        /// </summary>
        public int Matricula { get; set; }
        /// <summary>
        /// Nome do Aluno
        /// </summary>
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNasc { get; set; }
        public DateTime DataIni { get; set; } = DateTime.Now;
        public DateTime? DataFim { get; set; } = null;
        public bool Ativo { get; set; } = true;
    }
}
