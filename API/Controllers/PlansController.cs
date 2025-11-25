using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs.Plan;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlansController : ControllerBase
    {
        private readonly PlanService _planService;

        public PlansController(PlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        public ActionResult<List<PlanDto>> GetAll()
        {
            var plans = _planService.GetAllPlans();
            var dto = plans.Select(p => new PlanDto
            {
                Id = p.Id,
                Name = p.Name,
                PricePerMonth = p.PricePerMonth,
                IsActive = p.IsActive
            }).ToList();
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<PlanDto> GetById(Guid id)
        {
            var plan = _planService.GetPlanById(id);
            if (plan == null) return NotFound();

            return new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                PricePerMonth = plan.PricePerMonth,
                IsActive = plan.IsActive
            };
        }

        [HttpPost]
        public ActionResult<PlanDto> Create([FromBody] CreatePlanDto dto)
        {
            var plan = _planService.CreatePlan(dto.Name, dto.PricePerMonth, dto.IsActive);

            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                PricePerMonth = plan.PricePerMonth,
                IsActive = plan.IsActive
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreatePlanDto dto)
        {
            var plan = _planService.GetPlanById(id);
            if (plan == null) return NotFound();

            plan.Name = dto.Name;
            plan.PricePerMonth = dto.PricePerMonth;
            plan.IsActive = dto.IsActive;

            _planService.UpdatePlan(plan);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var plan = _planService.GetPlanById(id);
            if (plan == null) return NotFound();

            _planService.DeletePlan(plan);
            return NoContent();
        }
    }
}
