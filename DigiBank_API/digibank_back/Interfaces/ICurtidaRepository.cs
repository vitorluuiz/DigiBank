using digibank_back.Domains;

namespace digibank_back.Interfaces
{
    public interface ICurtidaRepository
    {
        bool Cadastrar(Curtida newReplie);
        bool Deletar(int idCurtida, int idUsuario);
        bool HasSameReplie(Curtida like);
        bool HasSameOrigin(Curtida like);
    }
}
