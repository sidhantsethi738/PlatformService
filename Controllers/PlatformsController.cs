using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _iPlatformRepo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper  , ICommandDataClient commandDataClient)
        {
            _iPlatformRepo = platformRepo;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [Route("GetPlatforms")]
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            var result = _iPlatformRepo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(result));
        }

        [Route("GetPlatformById")]
        [HttpGet(Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var result = _iPlatformRepo.GePlatformById(id);

            if (result != null)
                return Ok(_mapper.Map<PlatformReadDto>(result));
            else
                return NotFound();

        }

        [Route("CreatePlatform")]
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            if (_iPlatformRepo.NameExists(platformCreateDto.Name.ToLower()))
            {
                return BadRequest("Name of The Book  already Exists");
            }

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _iPlatformRepo.CreatePlatform(platformModel);
            _iPlatformRepo.SaveChanges();

            var PlatformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            try
            {
                await _commandDataClient.SendPlatformCommand(PlatformReadDto);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Could not send Synchronously -->  { ex.Message} ");
            }
            return CreatedAtRoute(nameof(GetPlatformById), new { id = PlatformReadDto.Id }, PlatformReadDto);

        }

    }
}
