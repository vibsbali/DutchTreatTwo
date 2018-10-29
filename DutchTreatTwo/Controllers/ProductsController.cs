using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DutchTreatTwo.Data;
using DutchTreatTwo.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
       private readonly IDutchRepository _dutchRepository;
       private readonly ILogger<ProductsController> _logger;

       public ProductsController(IDutchRepository dutchRepository, ILogger<ProductsController> logger)
       {
          _dutchRepository = dutchRepository;
          _logger = logger;
       }

       [HttpGet]
       [ProducesResponseType(200)]
       [ProducesResponseType(400)]
       public async Task<ActionResult<IEnumerable<Product>>> Get()
       {
          try
          {
             var result = await _dutchRepository.GetAllProducts();
             return Ok(result);
          }
          catch (Exception e)
          {
             _logger.LogError($"Exception occurred {e}");
             return BadRequest();
          }
       }
    }
}
