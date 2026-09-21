using FleetControl.Api.Domain.Entities;
using FleetControl.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModelosEquipamentoController : ControllerBase
{
    private readonly FleetControlDbContext _context;

    public ModelosEquipamentoController(FleetControlDbContext context)
    {
        _context = context;
    }
}
