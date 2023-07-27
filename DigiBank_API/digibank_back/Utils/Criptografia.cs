namespace digibank_back.Utils
{
    public class Criptografia
    {
        /// <summary>
        /// Criptografa uma senha
        /// </summary>
        /// <param name="senha">senha que será criptografada</param>
        /// <returns>Uma senha criptografada</returns>
        public static string CriptografarSenha(string senha)
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        /// <summary>
        /// Compara uma as senhas recebidas
        /// </summary>
        /// <param name="senhaForm">Senha vinda do front</param>
        /// <param name="senhaBanco">Senha criptografado no banco</param>
        /// <returns>True ou False baseado no resultado da comparação</returns>
        public static bool CompararSenha(string senhaForm, string senhaBanco)
        {
            return BCrypt.Net.BCrypt.Verify(senhaForm, senhaBanco);
        }
    }
}
