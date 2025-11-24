    using Microsoft.AspNetCore.Mvc;
    //using System.Data.Entity;
    using Microsoft.EntityFrameworkCore;
    using TestX.application.Repositories;
    using TestX.domain.Entities.General;
    using TestX.infrastructure.Identity;

    namespace TestX.api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class TypeController : Controller
        {
            private readonly IdentityContext _identityContext;
            private readonly ICacheService _cacheService;
            private readonly TimeSpan _time;
            private readonly ILogger<TypeController> _logger;
            public TypeController(IdentityContext identityContext, ICacheService cacheService,ILogger<TypeController> logger, IConfiguration configuration)
            {
                _identityContext = identityContext;
                _cacheService = cacheService;
                _logger = logger;
                _time = TimeSpan.FromMinutes(configuration.GetValue<int>("CacheSettings:Expiry"));
            
            }
            [HttpGet]
            public async Task<ActionResult> Index()
            {
                try
                {
                    var list = await _identityContext.QuestionTypes.AsNoTracking().ToListAsync();
                    if(list == null)
                        return NotFound();
                    return Ok(list);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                    return BadRequest();
                }
            }
            [HttpGet("getLevelQuestion")]
            public async Task<ActionResult> Question()
            {
                try
                {
                    var key = "list_level";
                var cacheKey = await _cacheService.GetAsync<List<Level>>(key);
                if (cacheKey != null)
                        return Ok(cacheKey); 
                    var list = await _identityContext.Levels.AsNoTracking().ToListAsync();
                    if (list != null)
                        await _cacheService.SetAsync(key, list, _time);
                    _logger.LogInformation("thông tin đã được lưu với cache.");
                    return Ok(list == null ? NotFound() : list);
                }
                catch (Exception ex)
                {
                    _logger.LogError("lỗi: {error}", ex.Message);
                    throw;
                }
            }
        }
    }
