﻿using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoOptionController : ControllerBase
    {
        readonly IHistoricoOptionsRepository _historyOptionsRepository;
        public HistoricoOptionController()
        {
            _historyOptionsRepository = new HistoricoOptionsRepository();
        }

        [HttpGet("{idOption}")]
        public IActionResult GetByHistoryOption(int idOption)
        {
            try
            {
                return Ok(new
                {
                    historyList = _historyOptionsRepository.GetHistoryFromOption(idOption).OrderByDescending(H => H.DataHistorico)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GerarOptionHistory(int idOption)
        {
            try
            {
                return Ok(new
                {
                    hitoryOption = _historyOptionsRepository.Equals(idOption)
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
