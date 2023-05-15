using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.ViewModel.Meta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MetasController : ControllerBase
    {
        private readonly IMetasRepository _metasRepository;
        public MetasController()
        {
            _metasRepository = new MetasRepository();
        }

        [HttpGet]
        public IActionResult ListarMetas()
        {
            try
            {
                return Ok(_metasRepository.GetMetas());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Minhas/{idUsuario}")]
        public IActionResult ListarMetas(int idUsuario)
        {
            try
            {
                return Ok(_metasRepository.GetMinhasMetas(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idMeta}")]
        public IActionResult LIstarMeta(int idMeta)
        {
            try
            {
                return Ok(_metasRepository.GetMeta(idMeta));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }


        [HttpPost]
        public IActionResult CadastrarMeta(MetaViewModel newMeta)
        {
            try
            {
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
        public IActionResult RemoverMeta(int idMeta)
        {
            try
            {
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
        public IActionResult AdicionarSaldo(int idMeta, decimal amount)
        {
            try
            {
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
        public IActionResult AlterarMeta(int idMeta, decimal amount)
        {
            try
            {
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
