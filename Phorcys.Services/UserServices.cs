
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phorcys.Data;
using Phorcys.Domain;
using Telerik.SvgIcons;
public class UserServices
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	
	private readonly PhorcysContext _context;

	public UserServices(IHttpContextAccessor httpContextAccessor, PhorcysContext context)
	{
		_httpContextAccessor = httpContextAccessor;	
		_context = context;
	}

	public string GetLoggedInUserId()
	{
		HttpContext httpContext = _httpContextAccessor.HttpContext;
		ClaimsPrincipal user = httpContext?.User;
		Claim userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier);
		string userId = userIdClaim?.Value;

		return userId;
	}

	public int GetUserId()
	{
		int retVal = 0;
		try
		{
			var loginId = GetLoggedInUserId();
			var User = _context.Users.FirstOrDefault(d => d.AspNetUserId == loginId);
			if (User != null)
			{
				retVal = User.UserId;
			}
		}
		catch (Exception ex)
		{

		}
		return retVal;
	}

	public string GetUserName()
	{
		try
		{
			var loginId = GetLoggedInUserId();
			var User = _context.Users.FirstOrDefault(d => d.AspNetUserId == loginId);
			if (User != null)
			{
				return User.LoginId;
			}
		}
		catch (Exception ex)
		{

		}
		return "";
	}
}
