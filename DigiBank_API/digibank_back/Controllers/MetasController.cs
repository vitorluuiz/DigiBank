using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Meta;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MetasController : ControllerBase
    {
        private readonly IMetasRepository _metasRepository;
        public MetasController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _metasRepository = new MetasRepository(ctx, memoryCache);
        }

        //[Authorize(Roles = "1")]
        //[HttpGet]
        //public IActionResult ListarMetas()
        //{
        //    try
        //    {
        //        return Ok(_metasRepository.GetMetas());
        //    }
        //    catch (Exception error)
        //    {
        //        return BadRequest(error);
        //        throw;
        //    }
        //}

        [HttpGet("Minhas/{idUsuario}")]
        public IActionResult ListarMetas(int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_metasRepository.GetMinhasMetas(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idMeta}")]
        public IActionResult LIstarMeta(int idMeta, [FromHeader] string Authorization)
        {
            try
            {
                Meta meta = _metasRepository.GetMeta(idMeta);

                if (meta == null)
                {
                    return NotFound();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)meta.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_metasRepository.GetMeta(idMeta));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }


        [HttpPost]
        public IActionResult CadastrarMeta(MetaViewModel newMeta, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newMeta.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                Meta meta = new Meta
                {
                    IdUsuario = (short?)newMeta.IdUsuario,
                    Titulo = newMeta.Titulo,
                    ValorMeta = newMeta.ValorMeta

                };

                bool isSucess = _metasRepository.AdicionarMeta(meta);

                if (isSucess)
                {
                    return StatusCode(201, meta);
                }

                return BadRequest("Meta Já existe ou não pode ter valorMeta igual a 0");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("{idMeta}")]
        public IActionResult RemoverMeta(int idMeta, [FromHeader] string Authorization)
        {
            try
            {
                Meta meta = _metasRepository.GetMeta(idMeta);

                if (meta == null)
                {
                    return NotFound();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)meta.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _metasRepository.RemoverMeta(idMeta);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AdicionarSaldo/{idMeta}/{amount}")]
        public IActionResult AdicionarSaldo(int idMeta, decimal amount, [FromHeader] string Authorization)
        {
            try
            {
                Meta meta = _metasRepository.GetMeta(idMeta);

                if (meta == null)
                {
                    return NotFound();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)meta.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucessful = _metasRepository.AdicionarSaldo(idMeta, amount);

                if (isSucessful)
                {
                    return Ok();
                }

                return BadRequest(new
                {
                    Message = "Não foi possível adicionar saldo à meta"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AlterarMeta/{idMeta}/{amount}")]
        public IActionResult AlterarMeta(int idMeta, decimal amount, [FromHeader] string Authorization)
        {
            try
            {
                Meta meta = _metasRepository.GetMeta(idMeta);

                if (meta == null)
                {
                    return NotFound();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)meta.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucessful = _metasRepository.AlterarMeta(idMeta, amount);

                if (isSucessful)
                {
                    return Ok();
                }

                return BadRequest(new
                {
                    Message = "Não foi possível alterar a meta"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
