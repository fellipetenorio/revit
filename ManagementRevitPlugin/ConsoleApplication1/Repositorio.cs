using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using ManagementRevitPlugin.Models;

namespace ManagementRevitPlugin.DAO
{
    [Database]
    public class Repositorio : DataContext
    {
        public Repositorio(string connectionString) : base(new SqlConnection(connectionString)) { }
        public Repositorio(SqlConnection cnn) : base(cnn) { }

        public Table<Objeto> Objetos;
        public Table<Medida> Medidas;
        public Table<Material> Materiais;
        public Table<ObjetoMaterial> ObjetoMateriais;
        
        public Objeto FindByReferenciaRevit(int referenciaRevit)
        {
            return (from o in Objetos where o.RevitTypeId == referenciaRevit select o).Single();
        }

        public void CalcularOrcamentoDoObjeto(int referenciaRevit)
        {

        }
    }
}
