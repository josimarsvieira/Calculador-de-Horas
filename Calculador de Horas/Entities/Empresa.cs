namespace Calculador_de_Horas.Entities
{
    public class Empresa
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public int DiaFechamento { get; set; }

        public Empresa(string razaoSocial, string cNPJ, int diaFechamento)
        {
            RazaoSocial = razaoSocial;
            CNPJ = cNPJ;
            DiaFechamento = diaFechamento;
        }
    }
}