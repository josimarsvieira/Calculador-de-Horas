namespace Calculador_de_Horas.Entities
{
    /// <summary>
    /// Classe para compartilhamento de informações entre as janelas.
    /// </summary>
    internal static class TranferenciaDados
    {
        /// <summary>
        /// Numero de registro do funcionario.
        /// </summary>
        public static int Registro { get; set; }

        /// <summary>
        /// Boleano responsavel por informar a janela se um funcionario é novo.
        /// </summary>
        public static bool Novo { get; set; } = true;

        /// <summary>
        /// Objeto funcionario recuperado do BD.
        /// </summary>
        public static Funcionario Funcionario { get; set; }

        /// <summary>
        /// Armazena o ultimo registro de horas efetuado.
        /// </summary>
        public static HorasFuncionario UltimaHoraAdicionada { get; set; }

        /// <summary>
        /// Armazena a empresa cadastrada.
        /// </summary>
        public static Empresa Empresa { get; set; }

        public static HorasFuncionario HorasFuncionario { get; set; }
    }
}