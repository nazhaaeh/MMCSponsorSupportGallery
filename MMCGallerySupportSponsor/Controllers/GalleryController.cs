using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastrecture.Repositories;
using Application.Interfaces;
using Application.DTO;
using Microsoft.IdentityModel.Tokens;
using Application.SupportsCQRS.Queries;
using Application.GalleryCQRS.Queries;
using MediatR;
using Application.SupportsCQRS.Commandes;
using Application.GalleryCQRS.Commandes;

namespace MMCGallerySupportSponsor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly IMediator _mediatR;

        public GalleryController( IMediator mediatR)
        {
             _mediatR = mediatR;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllGallery()
        {
            var Query = new GetGalleryQueryRequest();
            var result = await _mediatR.Send(Query);
            return Ok(result);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetGalleryById(Guid id )
        {
            var Query = new GetBydSponsorQueryRequest();
            Query.id = id;
            var result = await _mediatR.Send(Query);
            return Ok(result);

        }

        [HttpPost]
        public async Task<IActionResult> Creategallery([FromForm] GalleryCreateDto createDto)
        {
            var Commandes = new  CreateGalleryCommandeRequest(createDto);

            var result = await _mediatR.Send(Commandes);
            return Ok(result);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> GalleryDelete(Guid id)
        {
            var command = new DeleteGalleryCommandeRequest
            {
                Id = id
            };

            await _mediatR.Send(command);

            return Ok("It's deleted");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGallery(Guid id, [FromForm] GalleryUpdateDto updateDto)
        {
            var command = new UpdateGalleryCommandeRequest
            {
                GalleryId = id,
                GalleryUpdateRequest = updateDto
            };

            var updatedGalleryDto = await _mediatR.Send(command);

            if (updatedGalleryDto == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    } 
}
