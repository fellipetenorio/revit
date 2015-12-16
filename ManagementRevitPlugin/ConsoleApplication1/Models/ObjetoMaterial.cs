using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRevitPlugin.Models
{
    [Table(Name = "Objeto_Materiais")]
    public class ObjetoMaterial
    {
        private int _IdObjeto;
        [Column(IsPrimaryKey = true, Name = "id_objeto")]
        public int IdObjeto { get { return _IdObjeto; } set { _IdObjeto = value; } }
        private EntityRef<Objeto> _Objeto = new EntityRef<Objeto>();
        [Association(Name = "FK_Objeto_Material_id_objeto", IsForeignKey = true, Storage = "_Objeto", ThisKey = "IdObjeto", OtherKey = "IdObjeto")]
        public Objeto Objeto
        {
            get { return _Objeto.Entity; }
            set { _Objeto.Entity = value; }
        }

        private int _IdMaterial;
        [Column(IsPrimaryKey =true, Name ="id_material")]
        public int IdMaterial { get { return _IdMaterial; } set { _IdMaterial = value; } }
        private EntityRef<Material> _Material;
        [Association(Name = "FK_Objeto_Material_id_material", IsForeignKey = true, Storage = "_Material", ThisKey = "IdMaterial", OtherKey = "IdMaterial")]
        public Material Material
        {
            get { return _Material.Entity; }
            set { _Material.Entity = value;  }
        }
        private int _Quantidade;

        [Column(Name = "quantidade")]
        public int Quantidade
        {
            get { return _Quantidade; }
            set { _Quantidade = value; }
        }
    }
}
