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
            usuarioDesatualizado.PontosVantagem = usuarioDesatualizado.PontosVantagem - qntDigiPoints;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AdicionarSaldo(int idUsuario, decimal valor)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.Saldo = usuarioDesatualizado.Saldo - valor;
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void AlterarApelido(int idUsuario, Usuario newApelido)
        {
            Usuario usuarioDesatualizado =  ListarPorId(idUsuario);
            usuarioDesatualizado.Apelido = newApelido.Apelido;
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

        public void AlterarSenha(int idUsuario, Usuario newSenha)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            usuarioDesatualizado.Senha = Criptografia.CriptografarSenha(newSenha.Senha);
            ctx.Update(usuarioDesatualizado);
            ctx.SaveChanges();
        }

        public void Atualizar(int idUsuario, Usuario usuarioAtualizado)
        {
            throw new System.NotImplementedException();
        }

        public void Cadastrar(Usuario newUsuario)
        {
            newUsuario.Senha = Criptografia.CriptografarSenha(newUsuario.Senha);
            ctx.Usuarios.Add(newUsuario);
            ctx.SaveChanges();
        }

        public bool Deletar(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public Usuario ListarPorCpf(string Cpf)
        {
            return ctx.Usuarios.FirstOrDefault(u => u.Cpf == Cpf);
        }

        public Usuario ListarPorId(int idUsuario)
        {
            return ctx.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
        }

        public List<Usuario> ListarTodos()
        {
            return ctx.Usuarios.AsNoTracking().ToList();
        }

        public Usuario Login(string cpf, string senha)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoverDigiPoints(int idUsuario, decimal qntDigiPoints)
        {
            Usuario usuarioDesatualizado = ListarPorId(idUsuario);
            if (usuarioDesatualizado.PontosVantagem >= qntDigiPoints)
            {
                usuarioDesatualizado.PontosVantagem = usuarioDesatualizado.PontosVantagem - qntDigiPoints;
                ctx.Update(usuarioDesatualizado);
                ctx.SaveChanges();
                return true;
            }
                return false;
        }

        public bool RemoverSaldo(int idUsuario, decimal valor)
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
