using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreatTwo.Data;
using DutchTreatTwo.Data.Entities;
using DutchTreatTwo.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo.Controllers
{
   [Route("api/[Controller]")]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   public class OrdersController : ControllerBase
   {
      private readonly IDutchRepository _dutchRepository;
      private readonly ILogger<OrdersController> _logger;
      private readonly UserManager<StoreUser> _userManager;
      private readonly IMapper _mapper;

      public OrdersController(IDutchRepository dutchRepository
         , ILogger<OrdersController> logger
         , UserManager<StoreUser> userManager
         , IMapper mapper)
      {
         _dutchRepository = dutchRepository;
         _logger = logger;
         _userManager = userManager;
         _mapper = mapper;
      }

      [HttpGet]
      public IActionResult GetAllOrders(bool includeItems = true)
      {
         try
         {
            var user = User.Identity.Name;

            var orders = _dutchRepository.GetAllOrderByUser(user, includeItems);
            return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders));
         }
         catch (Exception e)
         {
            _logger.LogError($"Error occurred {e}");
            return BadRequest("End point unavailable please try later");
         }
      }

      [HttpGet("{id:int}")]
      public IActionResult GetOrder(int id)
      {
         try
         {
            var order = _dutchRepository.GetOrderById(User.Identity.Name, id);
            if (order == null)
            {
               return NotFound();
            }

            return Ok(_mapper.Map<Order, OrderViewModel>(order));
         }
         catch (Exception e)
         {
            _logger.LogError($"Error occurred {e}");
            return BadRequest("End point unavailable please try later");
         }
      }

      [HttpPost]
      public async Task<IActionResult> Post([FromBody]OrderViewModel model)
      {
         try
         {
            if (ModelState.IsValid)
            {
               //var newOrder = new Order
               //{
               //   OrderDate = model.OrderDate == default(DateTime) ? DateTime.Now : model.OrderDate,
               //   OrderNumber = model.OrderNumber
               //};
               if (model.OrderDate == default(DateTime))
               {
                  model.OrderDate = DateTime.Now;
               }

               var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
               var newOrder = _mapper.Map<OrderViewModel, Order>(model);
               newOrder.User = currentUser;

               _dutchRepository.AddEntity(newOrder);
               if (_dutchRepository.SaveAll())
               {
                  //var vm = new OrderViewModel
                  //{
                  //   OrderId = newOrder.Id,
                  //   OrderDate = newOrder.OrderDate,
                  //   OrderNumber = newOrder.OrderNumber
                  //};
                  var vm = _mapper.Map<Order, OrderViewModel>(newOrder);
                  return Created($"/api/orders/{vm.OrderId}", vm);
               }
            }
            else
            {
               return BadRequest(ModelState);
            }
         }
         catch (Exception e)
         {
            _logger.LogError($"Failed to save a new order : {e}");
         }

         return BadRequest("Failed to save data");
      }
   }
}
