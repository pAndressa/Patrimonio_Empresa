﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIPatrimonioEmpresa.Models;
using APIPatrimonioEmpresa.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIPatrimonioEmpresa.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class MarcasController : ControllerBase
    {
        private readonly MarcaRepositorio _marcaRepositorio = new MarcaRepositorio();

        [HttpGet]
        public ActionResult<IEnumerable<Marca>> GetMarca()
        {
            try
            {
                var marcas = _marcaRepositorio.ListarTodasMarcas().ToList();
                return Ok(marcas);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        
        [HttpGet("{id}", Name = "GetMarca")]
        public ActionResult<IEnumerable<Marca>> GetMarca(int id)
        {
            try
            {
                var marca = _marcaRepositorio.FiltrarMarcas(id).ToList();
                return Ok(marca);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        
        [HttpPost]
        public ActionResult Post([FromBody] Marca marca)
        {
            try
            {
                _marcaRepositorio.IncluirMarca(marca);
                return new CreatedAtRouteResult("GetMarca", new { id = marca.MarcaID }, marca);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Marca marca)
        {
            try
            {
                _marcaRepositorio.AtualizarMarca(id, marca);
                return Ok();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _marcaRepositorio.ExcluirMarca(id);
                return Ok();
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
