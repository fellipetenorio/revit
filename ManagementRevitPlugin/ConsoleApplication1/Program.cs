using ManagementRevitPlugin;
using ManagementRevitPlugin.DAO;
using ManagementRevitPlugin.Models;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementRevitPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=FELLIPE-MACWIN;Database=db_LibBim;Integrated Security=true";
            try
            {
                Repositorio rep = new Repositorio(connectionString);

                Console.WriteLine("Objetos Materiais:");
                foreach (ObjetoMaterial om in rep.ObjetoMateriais)
                {
                    Console.Write("Objeto: {0}, Material: {1}, Quantidade: {2}\n",
                        om.Objeto.Nome, om.Material.DescricaoMaterial, om.Quantidade);
                }

                Console.WriteLine("Materiais:");
                foreach(Material m in rep.Materiais)
                {
                    Console.WriteLine("IdMaterial: {0}, IdMedida: {1}, Descricao: {2}, Custo: {3}", 
                        m.IdMaterial, m.IdMedidaMaterial, m.DescricaoMaterial, m.CustoMedida);
                }

                Console.WriteLine("Objetos");
                foreach (Objeto o in rep.Objetos)
                {
                    Console.WriteLine("Quantidade de Materiais deste objeto: {0}", o.Materiais.Count);
                    Console.WriteLine("id_objeto: {0}, id_medida={1}, descricao_medida: {4}, custo_mao_obra={2}, nome={3}", 
                        o.IdObjeto, o.IdMedidaObjeto, o.CustoMaoDeObra, o.Nome, o.Medida.Descricao);
                }

                Console.WriteLine("Medidas:");
                foreach(Medida m in rep.Medidas)
                {
                    Console.WriteLine("id_Medida: {0}, Descricao: {1}", m.IdMedida, m.Descricao);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Console.Write("Fim da Execução. Pressione qualquer tecla para finalizar.");
            Console.Read();
        }
    }
}
