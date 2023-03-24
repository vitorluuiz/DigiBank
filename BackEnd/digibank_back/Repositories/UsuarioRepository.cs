using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        digiBankContext ctx = new digiBankContext();
        public object BCrypt { get; private set; }
        public void AdicionarDigiPoints(int idUsuario, decimal qntDigiPoints)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.DigiPoints = usuarioDesatualizado.DigiPoints + qntDigiPoints;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AdicionarSaldo(int idUsuario, decimal valor)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.Saldo = usuarioDesatualizado.Saldo + valor;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AlterarApelido(int idUsuario, string newApelido)
        {
            Usuario usuarioDesatualizado =  ListarPorId(idUsuario);
            usuarioDesatualizado.Apelido = newApelido;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AlterarRendaFixa(int idUsuario, decimal newRenda)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.RendaFixa = newRenda;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AlterarSenha(int idUsuario, string newSenha)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.Senha = Criptografia.CriptografarSenha(newSenha);
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public bool Atualizar(int idUsuario, Usuario usuarioAtualizado)
        {
            Usuario usuarioDesatualizado = ctx.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if(usuarioDesatualizado != null)
            {
                usuarioDesatualizado.Email = usuarioAtualizado.Email.ToLower();
                usuarioDesatualizado.Telefone = usuarioAtualizado.Telefone;
                usuarioDesatualizado.NomeCompleto = usuarioAtualizado.NomeCompleto;

                if(usuarioAtualizado.Apelido != null)
                {
                    usuarioDesatualizado.Apelido = usuarioAtualizado.Apelido;
                }

                ctx.Update(usuarioDesatualizado);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Cadastrar(Usuario newUsuario)
        {
            if (VerificarDisponibilidade(newUsuario))
            {
            newUsuario.Saldo = 0;
            newUsuario.DigiPoints = 0;
            newUsuario.Senha = Criptografia.CriptografarSenha(newUsuario.Senha);
            newUsuario.Email.ToLower();
            ctx.Usuarios.Add(newUsuario);
            ctx.SaveChanges();
            return true;
            }
            return false;
        }

        public bool Deletar(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public Usuario ListarPorCpf(string Cpf)
        {
            return ctx.Usuarios
                .Include(u => u.Acos)
                .Include(u => u.Fundos)
                .FirstOrDefault(u => u.Cpf == Cpf);
        }

        public Usuario ListarPorId(int idUsuario)
        {
            return ctx.Usuarios
                .Include(u => u.Acos)
                .Include(u => u.Fundos)
                .FirstOrDefault(u => u.IdUsuario == idUsuario);
        }

        public List<Usuario> ListarTodos()
        {
            return ctx.Usuarios
                .AsNoTracking()
                .ToList();
        }

        public Usuario Login(string cpf, string senha)
        {
            Usuario loginEnviado = ctx.Usuarios.FirstOrDefault(u => u.Cpf == cpf);

            if(loginEnviado != null)
            {
                bool isSenhaValida = Criptografia.CompararSenha(senha, loginEnviado.Senha);

                if (isSenhaValida)
                {
                    return loginEnviado;
                }
            }

            return null;
        }

        public bool RemoverDigiPoints(int idUsuario, decimal qntDigiPoints)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);

            if (usuarioDesatualizado.DigiPoints >= qntDigiPoints)
            {
                usuarioDesatualizado.DigiPoints = usuarioDesatualizado.DigiPoints - qntDigiPoints;
                ctx.Update(usuarioDesatualizado);
                ctx.SaveChanges();
                return true;
            }

                return false;
        }

        public bool RemoverSaldo(short idUsuario, decimal valor)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);

            if (usuarioDesatualizado.Saldo >= valor)
            {
                usuarioDesatualizado.Saldo = usuarioDesatualizado.Saldo - valor;
                ctx.Update(usuarioDesatualizado);
                ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public bool VerificarDisponibilidade(Usuario newUsuario)
        {
            Usuario tentativa = ctx.Usuarios.FirstOrDefault(u => u.Cpf == newUsuario.Cpf || u.Email == newUsuario.Email || u.Telefone == newUsuario.Telefone);
            
            if (tentativa == null)
            {
                return true;
            }

            return false;
        }
    }
}
