using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs.Project;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public ActionResult<List<ProjectDto>> GetAll()
        {
            var projects = _projectService.GetAllProjects();

            var dto = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                TenantId = p.TenantId
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetById(Guid id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null) return NotFound();

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                TenantId = project.TenantId
            };
        }

        [HttpPost]
        public ActionResult<ProjectDto> Create([FromBody] CreateProjectDto dto)
        {
            var tenantIdClaim = User.Claims.FirstOrDefault(c => c.Type == "TenantId")?.Value;

            if (tenantIdClaim == null)
                return Unauthorized("TenantId claim missing in token.");

            var tenantId = Guid.Parse(tenantIdClaim);

            var project = _projectService.CreateProject(dto.Name, tenantId);

            var result = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                TenantId = project.TenantId
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }


        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateProjectDto dto)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null) return NotFound();

            project.Name = dto.Name;
            _projectService.UpdateProject(project);

            return NoContent();
        }
 
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null) return NotFound();

            _projectService.DeleteProject(project);
            return NoContent();
        }
    }
}
