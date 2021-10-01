using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using System.Collections.Generic;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _iPlatformRepo;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepo platformRepo, IMapper mapper)
        {
            _iPlatformRepo = platformRepo;
            _mapper = mapper;
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
        public ActionResult<PlatformReadDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            if (_iPlatformRepo.NameExists(platformCreateDto.Name.ToLower()))
            {
                return BadRequest("Name of The Book  already Exists");
            }

            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _iPlatformRepo.CreatePlatform(platformModel);
            _iPlatformRepo.SaveChanges();

            var PlatformReadDto = _mapper.Map<PlatformReadDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformById), new { id = PlatformReadDto.Id },PlatformReadDto);

        }

    }
}
