using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DutchTreatTwo.Data;
using DutchTreatTwo.Data.Entities;
using DutchTreatTwo.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo.Controllers
{
   [Route("api/orders/{orderid}/items")]
   [Route("api/[Controller]")]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   public class OrderItemsController : Controller
   {
      private readonly IDutchRepository _dutchRepository;
      private readonly ILogger<OrderItemsController> _logger;
      private readonly IMapper _mapper;

      public OrderItemsController(IDutchRepository dutchRepository
              , ILogger<OrderItemsController> logger
              , IMapper mapper)
      {
         _dutchRepository = dutchRepository;
         _logger = logger;
         _mapper = mapper;
      }

      [HttpGet]
      public IActionResult Get(int orderId)
      {
         try
         {
            var order = _dutchRepository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
               var items = order.Items;
               var vm = _mapper.Map<IEnumerable<OrderItem>,
                  IEnumerable<OrderItemViewModel>>(items);

               return Ok(vm.ToList());
            }
         }
         catch (Exception e)
         {
            _logger.LogError($"error occurred while getting order items {e}");
            return NotFound("Unable to get items please try later");
         }

         return BadRequest();
      }

      //Note the parameter in httpGet below if you do not have 
      //this parameter "{orderItemId}" then you will get an error
      [HttpGet("{orderItemId}")]
      public IActionResult Get(int orderId, int orderItemId)
      {
         try
         {
            var order = _dutchRepository.GetOrderById(User.Identity.Name, orderId);
            if (order != null)
            {
               var item = order.Items.SingleOrDefault(orderItem => orderItem.Id == orderItemId);

               var vm = _mapper.Map<OrderItem, OrderItemViewModel>(item);

               return Ok(vm);
            }
         }
         catch (Exception e)
         {
            _logger.LogError($"error occurred while getting order items {e}");
            return NotFound("Unable to get items please try later");
         }

         return BadRequest();
      }
   }
}
