using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs.Subscription;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionService _subscriptionService;

        public SubscriptionsController(SubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet]
        public ActionResult<List<SubscriptionDto>> GetAll()
        {
            var subs = _subscriptionService.GetAllSubscriptions();
            var dto = subs.Select(s => new SubscriptionDto
            {
                Id = s.Id,
                PlanId = s.PlanId,
                TenantId = s.TenantId,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<SubscriptionDto> GetById(Guid id)
        {
            var sub = _subscriptionService.GetSubscriptionById(id);
            if (sub == null) return NotFound();

            return new SubscriptionDto
            {
                Id = sub.Id,
                PlanId = sub.PlanId,
                TenantId = sub.TenantId,
                StartDate = sub.StartDate,
                EndDate = sub.EndDate
            };
        }

        [HttpPost]
        public ActionResult<SubscriptionDto> Create([FromBody] CreateSubscriptionDto dto)
        {
            var sub = _subscriptionService.CreateSubscription(dto.PlanId, dto.StartDate, dto.EndDate);

            return CreatedAtAction(nameof(GetById), new { id = sub.Id }, new SubscriptionDto
            {
                Id = sub.Id,
                PlanId = sub.PlanId,
                TenantId = sub.TenantId,
                StartDate = sub.StartDate,
                EndDate = sub.EndDate
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateSubscriptionDto dto)
        {
            var sub = _subscriptionService.GetSubscriptionById(id);
            if (sub == null) return NotFound();

            sub.PlanId = dto.PlanId;
            sub.StartDate = dto.StartDate;
            sub.EndDate = dto.EndDate;
            _subscriptionService.UpdateSubscription(sub);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var sub = _subscriptionService.GetSubscriptionById(id);
            if (sub == null) return NotFound();

            _subscriptionService.DeleteSubscription(sub);
            return NoContent();
        }
    }
}
