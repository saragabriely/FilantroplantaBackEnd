namespace mba_es_25_grupo_02_backend.Models
{
    public class Pessoa
    {
        public long PessoaID { get; set; }
        public string Nome { get; set;}
        public int TipoPessoaID { get; set; }
        public string Documento { get; set; }
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        public const int Restaurante = 1;
        public const int Produtor    = 2;
    }
}