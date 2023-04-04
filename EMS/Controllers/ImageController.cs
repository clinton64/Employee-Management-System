using EMS.Data;
using EMS.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Controllers;

public class ImageController : Controller
{
    
    private readonly ILogger<ImageController> _logger;
    private readonly ApplicationDbContext _dbContext;
    
    public ImageController( ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
}