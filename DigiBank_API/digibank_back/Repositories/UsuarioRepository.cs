using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly digiBankContext _ctx;
        private readonly IMemoryCache _memoryCache;
        public UsuarioRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
        }

        public object BCrypt { get; private set; }
        //public void AdicionarDigiPoints(int idUsuario, decimal qntDigiPoints)
        //{
        //    Usuario usuario = PorId(idUsuario);
        //    usuario.DigiPoints += qntDigiPoints;

        //    _ctx.Update(usuario);
        //    _ctx.SaveChanges();
        //}

        public bool AdicionarSaldo(int idUsuario, decimal valor)
        {
            Usuario usuarioDesatualizado = PorId(idUsuario);

            if (CanAddSaldo(idUsuario, valor))
            {
                usuarioDesatualizado.Saldo = usuarioDesatualizado.Saldo + valor;
                _ctx.Update(usuarioDesatualizado);
                _ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Update(int idUsuario, Usuario updatedU)
        {
            Usuario usuario = _ctx.Usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

            if (usuario != null)
            {
                usuario.Email = updatedU.Email.ToLower();
                usuario.Telefone = updatedU.Telefone;
                usuario.NomeCompleto = updatedU.NomeCompleto;
                //Adicionar o resto das informações

                if (updatedU.Apelido != null)
                {
                    usuario.Apelido = updatedU.Apelido;
                }

                _ctx.Update(usuario);
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Post(Usuario newUsuario)
        {
            if (VerificarDisponibilidade(newUsuario))
            {
                newUsuario.Saldo = 0;
                newUsuario.DigiPoints = 0;
                newUsuario.Senha = Criptografia.CriptografarSenha(newUsuario.Senha);
                newUsuario.Email = newUsuario.Email.ToLower();

                _ctx.Usuarios.Add(newUsuario);
                _ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CanAddSaldo(int idUsuario, decimal valor)
        {
            Usuario usuarioDesatualizado = PorId(idUsuario);
            if (usuarioDesatualizado.Saldo + valor <= 999999999) //999.999.999,00 porem o limite é de 
            {
                return true;
            }

            return false;
        }

        public bool CanRemoveSaldo(int id, decimal valor)
        {
            Usuario usuario = PorId(id);

            return usuario.Saldo >= valor;
        }

        public bool Delete(int id)
        {
            try
            {
                _ctx.Remove(PorId(id));
                _ctx.SaveChanges();
                //Adicionar removedores

                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        public UsuarioInfos Infos(int id)
        {
            Usuario usuario = PorId(id);
            UsuarioInfos info = new(usuario);
            InvestimentoRepository investimentoRepository = new(_ctx, _memoryCache);
            MetasRepository metaRepository = new(_ctx, _memoryCache);

            info.Investido = investimentoRepository.ExtratoTotalInvestido(id).Total;
            info.MetaDestaque = metaRepository.ListarDestaque(id);

            return info;
        }

        public PublicUsuario PorCpf(string Cpf)
        {
            return _ctx.Usuarios
                .Where(u => u.Cpf == Cpf)
                .Select(u => new PublicUsuario(u))
                .FirstOrDefault();
        }

        public Usuario PorId(int idUsuario)
        {
            return _ctx.Usuarios
                .FirstOrDefault(u => u.IdUsuario == idUsuario);
        }

        public List<Usuario> Todos()
        {
            return _ctx.Usuarios
                .AsNoTracking()
                .ToList();
        }

        public Usuario Login(string cpf, string senha)
        {
            Usuario login = _ctx.Usuarios.FirstOrDefault(u => u.Cpf == cpf);

            return login != null && Criptografia.CompararSenha(senha, login.Senha) ? login : null;
        }

        //public bool RemoverDigiPoints(int id, decimal qntDigiPoints)
        //{
        //    Usuario usuario = PorId(id);

        //    if (usuario.DigiPoints >= qntDigiPoints)
        //    {
        //        usuario.DigiPoints -= qntDigiPoints;
        //        _ctx.Update(usuario);
        //        _ctx.SaveChanges();

        //        return true;
        //    }

        //    return false;
        //}

        public bool RemoverSaldo(short id, decimal valor)
        {
            Usuario usuario = PorId(id);

            if (CanRemoveSaldo(id, valor))
            {
                usuario.Saldo -= valor;
                _ctx.Update(usuario);
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool VerificarDisponibilidade(Usuario newU)
        {
            Usuario tentativa = _ctx.Usuarios.FirstOrDefault(u => u.Cpf == newU.Cpf || u.Email == newU.Email || u.Telefone == newU.Telefone);

            return tentativa == null;
        }
    }
}
