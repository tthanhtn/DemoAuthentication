using aspnetcore.Core.Middleware;
using Demo.API.Controllers;
using Demo.API.Interface;
using Demo.API.Services;
using Demo.Business.Entity;
using Demo.Core.DependencyInjection;
using Demo.Core.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var appSettings = builder.Configuration.GetAppSettings();
builder.Services.AddSingleton<AppSettings>(appSettings);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AccountController).Assembly);

builder.Services.AddLogging(logging => logging.AddConsole());

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CookieAndJwt";
    options.DefaultChallengeScheme = "CookieAndJwt";
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "CookieNames";
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logoff";


    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };


}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
 {
     options.Authority = appSettings.IdentityServerAuthority;
     options.RequireHttpsMetadata = false;
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key)),
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero
     };
 }).AddPolicyScheme("CookieAndJwt", "CookieAndJwt", options =>
 {
     options.ForwardDefaultSelector = context =>
     {
         if (context.Request.Headers.ContainsKey("Authorization"))
         {
             string authorization = context.Request.Headers["Authorization"].ToString();
             if (authorization.StartsWith(JwtBearerDefaults.AuthenticationScheme))
                 return JwtBearerDefaults.AuthenticationScheme;
         }

         return CookieAuthenticationDefaults.AuthenticationScheme;
     };
 });

CoreInjection.InjectServices(builder.Services, builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API with JWT Authentication"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field (e.g., 'Bearer {token}')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<HttpExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseStaticFiles();   

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
