using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ManagementRevitPlugin.Models
{
    [Table(Name ="Medida")]
    public class Medida
    {
        private int _IdMedida;
        [Column(IsPrimaryKey = true, Name = "id_medida", IsDbGenerated = true)]
        public int IdMedida
        {
            get { return _IdMedida; }
            set { _IdMedida = value; }
        }

        private string _Descricao;
        [Column(Name = "descricao_medida")]
        public string Descricao
        {
            get { return _Descricao; }
            set { _Descricao = value; }
        }
             
    }
}
