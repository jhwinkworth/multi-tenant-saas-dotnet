using Application.DTOs.Project;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
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
                Description = p.Description,
                CreatedAt = p.CreatedAt
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectDto> GetById(Guid id)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null) return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public ActionResult<ProjectDto> Create(CreateProjectDto dto)
        {
            var project = _projectService.CreateProject(dto.Name);

            var result = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, result);
        }



        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateProjectDto dto)
        {
            var project = _projectService.GetProjectById(id);
            if (project == null) return NotFound();

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.CreatedAt = dto.CreatedAt;
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
