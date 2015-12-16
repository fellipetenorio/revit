using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRevitPlugin.Models
{
    [Table(Name ="Material")]
    public class Material
    {
        private int _IdMaterial;
        private int _IdMedidaMaterial;
        private EntitySet<ObjetoMaterial> _ObjetosMaterial = new EntitySet<ObjetoMaterial>();
        private string _DescricaoMaterial;
        private decimal _CustoMedida;
        
        [Column(IsPrimaryKey = true, Name = "id_material", IsDbGenerated = true)]
        public int IdMaterial
        {
            get { return _IdMaterial; }
            set { _IdMaterial = value; }
        }

        [Column(IsPrimaryKey = true, Name = "id_medida_material")]
        public int IdMedidaMaterial
        {
            get { return _IdMedidaMaterial; }
            set { _IdMedidaMaterial = value; }
        }

        [Association(Name = "FK_Objeto_Material_id_material", Storage = "_ObjetosMaterial",
            ThisKey = "IdMaterial", OtherKey = "IdMaterial")]
        private ICollection<ObjetoMaterial> ObjetosMaterial
        {
            get { return _ObjetosMaterial; }
            set { _ObjetosMaterial.Assign(value); }
        }

        [Column(IsPrimaryKey = true, Name = "descricao_material")]
        public string DescricaoMaterial
        {
            get { return _DescricaoMaterial; }
            set { _DescricaoMaterial = value; }
        }

        [Column(IsPrimaryKey = true, Name = "custo_medida")]
        public decimal CustoMedida
        {
            get { return _CustoMedida; }
            set { _CustoMedida = value; }
        }
    }
}
