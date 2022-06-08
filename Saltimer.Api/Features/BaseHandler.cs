using AutoMapper;
using Saltimer.Api.Services;

namespace Saltimer.Api.Handlers;
public class BaseHandler
{

    protected readonly SaltimerDBContext _context;
    protected readonly IAuthService _authService;
    protected readonly IMapper _mapper;
    public BaseHandler(IMapper mapper, IAuthService authService, SaltimerDBContext context)
    {
        _mapper = mapper;
        _authService = authService;
        _context = context;
    }


}