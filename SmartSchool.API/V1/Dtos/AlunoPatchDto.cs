using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSchool.API.V1.Dtos
{
    /// <summary>
    /// Objeto para salvar ou alterar alunos novos.
    /// </summary>
    public class AlunoPatchDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }        
        public bool Ativo { get; set; } = true;
    }
}
