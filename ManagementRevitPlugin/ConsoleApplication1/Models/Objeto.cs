using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ManagementRevitPlugin.Models
{
    [Table(Name ="Objeto")]
    public class Objeto
    {
        private int _IdObjeto;
        private int? _IdMedidaObjeto;
        private EntityRef<Medida> _Medida = new EntityRef<Medida>();
        private EntitySet<ObjetoMaterial> _ObjetoMateriais = new EntitySet<ObjetoMaterial>();
        private decimal _CustoMaoDeObra;
        private string _Nome;
        private int _ReveitTypeId;

        #region Fields
        [Column(IsPrimaryKey = true, Name = "id_objeto", IsDbGenerated = true)]
        public int IdObjeto
        {
            get { return _IdObjeto; }
            set { _IdObjeto = value; }
        }

        [Column(Name = "id_medida_objeto")]
        public int? IdMedidaObjeto
        {
            get { return _IdMedidaObjeto; }
            set { _IdMedidaObjeto = value; }
        }

        [Association(Name = "FK_Objeto_Medida", ThisKey = "IdMedidaObjeto",
            IsForeignKey = true, Storage = "_Medida")]
        public Medida Medida
        {
            get { return _Medida.Entity; }
            set { _Medida.Entity = value; }
        }

        [Association(Name = "FK_Objeto_Material_id_objeto", Storage = "_ObjetoMateriais",
            ThisKey = "IdObjeto", OtherKey = "IdObjeto")]
        private ICollection<ObjetoMaterial> ObjetoMateriais
        {
            get { return _ObjetoMateriais; }
            set { _ObjetoMateriais.Assign(value); }
        }

        public ICollection<Material> Materiais
        {
            get
            {
                return (from om in ObjetoMateriais select om.Material).ToList();
            }
        }

        [Column(Name = "custo_mao_de_obra")]
        public decimal CustoMaoDeObra
        {
            get { return _CustoMaoDeObra; }
            set { _CustoMaoDeObra = value; }
        }

        [Column(Name = "nome")]
        public string Nome {
            get { return _Nome; }
            set { _Nome = value; }
        }
        
        [Column(Name = "referencia_revit")]
        public int RevitTypeId { get { return _ReveitTypeId; } set { _ReveitTypeId = value; } }
        #endregion

    }
}
