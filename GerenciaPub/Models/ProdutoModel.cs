using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GerenciaPub.Models
{
    public class ProdutoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o código.")]
       // [MaxLength(10, ErrorMessage = "O código pode ter no máximo 10 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
      //  [MaxLength(50, ErrorMessage = "O nome pode ter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o preço de custo.")]
        [Range(0.01, 99999.99,
             ErrorMessage = "O Preço de Custo deve estar entre " +
                            "0,01 e 99999,99.")]
        [DisplayName("Preço de Custo")]
        public decimal? PrecoCusto { get; set; }

        [Required(ErrorMessage = "Preencha o preço de venda.")]
        [Range(0.01, 99999.99,
             ErrorMessage = "O Preço de Venda deve estar entre " +
                            "0,01 e 99999,99.")]
        [DisplayName("Preço de Venda")]
        public decimal? PrecoVenda { get; set; }

        [Required(ErrorMessage = "Preencha a quantidade em estoque.")]
        public int QuantEstoque { get; set; }

        [Required(ErrorMessage = "Preencha a unidade de medida.")]
       // [MaxLength(3, ErrorMessage = "O nome pode ter no máximo 3 caracteres.")]
        public string UnidadeMedida { get; set; }

        [Required(ErrorMessage = "Selecione o grupo.")]
        public int IdGrupo { get; set; }

        [Required(ErrorMessage = "Selecione a marca.")]
        public int IdSubGrupo { get; set; }

        public bool Ativo { get; set; }

        public bool AdicionalAtivo { get; set; }

        public bool GeraEstoque { get; set; }

        public static List<ProdutoModel> RecuperarLista()
        {
            var ret = new List<ProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from produto order by pro_nome";
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ProdutoModel
                        {
                            Id = (int)reader["pro_id"],
                            Codigo = (string)reader["pro_codigo"],
                            Nome = (string)reader["pro_nome"],
                            PrecoCusto = (decimal)reader["pro_valorcusto"],
                            PrecoVenda = (decimal)reader["pro_valorvenda"],
                            QuantEstoque = (int)reader["pro_unidade"],
                            UnidadeMedida = (string)reader["pro_unidademedida"],
                            Ativo = (bool)reader["pro_ativo"],
                            IdGrupo = (int)reader["gru_id"],
                            IdSubGrupo = (int)reader["sub_id"],
                            AdicionalAtivo = (bool)reader["pro_ativoadicional"],
                            GeraEstoque = (bool)reader["pro_geraestoque"]

                        });
                    }
                }
            }

            return ret;
        }

        public static List<ProdutoModel> RecuperarListaAtivos()
        {
            var ret = new List<ProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = string.Format("select * from produto where pro_ativo=1 order by pro_nome");
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ProdutoModel
                        {
                            Id = (int)reader["pro_id"],
                            Codigo = (string)reader["pro_codigo"],
                            Nome = (string)reader["pro_nome"],
                            PrecoCusto = (decimal)reader["pro_valorcusto"],
                            PrecoVenda = (decimal)reader["pro_valorvenda"],
                            QuantEstoque = (int)reader["pro_unidade"],
                            UnidadeMedida = (string)reader["pro_unidademedida"],
                            Ativo = (bool)reader["pro_ativo"],
                            IdGrupo = (int)reader["gru_id"],
                            IdSubGrupo = (int)reader["sub_id"],
                            AdicionalAtivo = (bool)reader["pro_ativoadicional"],
                            GeraEstoque = (bool)reader["pro_geraestoque"]
                        });
                    }
                }
            }

            return ret;
        }

        public static ProdutoModel RecuperarPeloId(int id)
        {
            ProdutoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;
                    comando.CommandText = "select * from produto where (pro_id = @id)";

                    comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    var reader = comando.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new ProdutoModel
                        {
                            Id = (int)reader["pro_id"],
                            Codigo = (string)reader["pro_codigo"],
                            Nome = (string)reader["pro_nome"],
                            PrecoCusto = (decimal)reader["pro_valorcusto"],
                            PrecoVenda = (decimal)reader["pro_valorvenda"],
                            QuantEstoque = (int)reader["pro_unidade"],
                            UnidadeMedida = (string)reader["pro_unidademedida"],
                            Ativo = (bool)reader["pro_ativo"],
                            IdGrupo = (int)reader["gru_id"],
                            IdSubGrupo = (int)reader["sub_id"],
                            AdicionalAtivo = (bool)reader["pro_ativoadicional"],
                            GeraEstoque = (bool)reader["pro_geraestoque"]
                        };
                    }
                }
            }

            return ret;
        }


        public static bool ExcluirPeloId(int id)
        {
            var ret = false;

            if (RecuperarPeloId(id) != null)
            {
                using (var conexao = new SqlConnection())
                {
                    conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                    conexao.Open();
                    using (var comando = new SqlCommand())
                    {
                        comando.Connection = conexao;
                        comando.CommandText = "delete from produto where (pro_id = @id)";

                        comando.Parameters.Add("@id", SqlDbType.Int).Value = id;

                        ret = (comando.ExecuteNonQuery() > 0);
                    }
                }
            }

            return ret;
        }

        public int Salvar()
        {
            var ret = 0;

            var model = RecuperarPeloId(this.Id);

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
                    comando.Connection = conexao;

                    if (model == null)
                    {
                        comando.CommandText = "insert into produto (pro_nome, " +
                            "pro_codigo," +
                            "pro_valorcusto," +
                            "pro_valorvenda," +
                            "pro_unidade," +
                            "pro_unidademedida," +
                            "pro_ativo," +
                            "gru_id," +
                            "sub_id, pro_ativoadicional,pro_geraestoque) " +

                            "values (@nome," +
                            " @codigo," +
                            "@valorcusto," +
                            "@valorvenda," +
                            "@unidade," +
                            "@unidademedida," +
                            "@ativo," +
                            "@gruid," +
                            "@subid,@ativoadicional,@geraestoque);" +
                            " select convert(int, scope_identity())";

                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@codigo", SqlDbType.VarChar).Value = this.Codigo;
                        comando.Parameters.Add("@valorcusto", SqlDbType.Decimal).Value = this.PrecoCusto;
                        comando.Parameters.Add("@valorvenda", SqlDbType.Decimal).Value = this.PrecoVenda;
                        comando.Parameters.Add("@unidade", SqlDbType.Int).Value = this.QuantEstoque;
                        comando.Parameters.Add("@unidademedida", SqlDbType.VarChar).Value = this.UnidadeMedida;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@gruid", SqlDbType.Int).Value = this.IdGrupo;
                        comando.Parameters.Add("@subid", SqlDbType.Int).Value = this.IdSubGrupo;
                        comando.Parameters.Add("@ativoadicional", SqlDbType.VarChar).Value = (this.AdicionalAtivo ? 1 : 0);
                        comando.Parameters.Add("@geraestoque", SqlDbType.VarChar).Value = (this.GeraEstoque ? 1 : 0);
                        

                        ret = (int)comando.ExecuteScalar();                        
                    }
                    else
                    {
                        comando.CommandText = "update produto set pro_nome=@nome, " +
                            "pro_codigo=@codigo," +
                            "pro_valorcusto=@valorcusto," +
                            "pro_valorvenda=@valorvenda," +
                            "pro_unidade=@unidade," +
                            "pro_unidademedida=@unidademedida," +
                            "pro_ativo=@ativo," +
                            "gru_id=@gruid," +
                            "sub_id=@subid, " +
                            "pro_ativoadicional=@ativoadicional,pro_geraestoque@geraestoque " + 
                            " where pro_id = @id";
                        
                        comando.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.Nome;
                        comando.Parameters.Add("@codigo", SqlDbType.VarChar).Value = this.Codigo;
                        comando.Parameters.Add("@valorcusto", SqlDbType.Decimal).Value = this.PrecoCusto;
                        comando.Parameters.Add("@valorvenda", SqlDbType.Decimal).Value = this.PrecoVenda;
                        comando.Parameters.Add("@unidade", SqlDbType.Int).Value = this.QuantEstoque;
                        comando.Parameters.Add("@unidademedida", SqlDbType.VarChar).Value = this.UnidadeMedida;
                        comando.Parameters.Add("@ativo", SqlDbType.VarChar).Value = (this.Ativo ? 1 : 0);
                        comando.Parameters.Add("@ativoadicional", SqlDbType.VarChar).Value = (this.AdicionalAtivo ? 1 : 0);
                        comando.Parameters.Add("@gruid", SqlDbType.Int).Value = this.IdGrupo;
                        comando.Parameters.Add("@subid", SqlDbType.Int).Value = this.IdSubGrupo;
                        comando.Parameters.Add("@ativoadicional", SqlDbType.VarChar).Value = (this.AdicionalAtivo ? 1 : 0);
                        comando.Parameters.Add("@geraestoque", SqlDbType.VarChar).Value = (this.GeraEstoque ? 1 : 0);
                        comando.Parameters.Add("@id", SqlDbType.Int).Value = this.Id;

                        if (comando.ExecuteNonQuery() > 0)
                        {
                            ret = this.Id;
                        }
                    }
                }
            }

            return ret;
        }
    }
}