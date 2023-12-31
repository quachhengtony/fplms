using System.Text;
using FPLMS.Api.Extensions;
using FPLMS.Api.Services;
using FPLMS.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Http;
using Api.Services.Subjects;
using Api.Services.Students;
using Api.Services.Groups;
using Api.Services.Classes;
using Api.Services.Projects;
using Api.Services.Semesters;
using Api.Services.Reports;
using Api.Services.Meetings;
using Repositories.Interfaces;
using Repositories;
using System;
using Repositories.Contracts;
using Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<ICycleReportRepository, CycleReportRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<Repositories.Interfaces.ILecturerRepository, Repositories.LecturerRepository>();
builder.Services.AddScoped<IMeetingRepository, MeetingRepository>();
builder.Services.AddScoped<IProgressReportRepository, ProgressReportRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<Repositories.Interfaces.IStudentRepository, Repositories.StudentRepository>();
builder.Services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
builder.Services.AddScoped<Repositories.Interfaces.ISubjectRepository, Repositories.SubjectRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
});
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token:Secret").Value)),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//     });
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Token:Secret").Value)),
//             ValidateIssuer = false,
//             ValidateAudience = false
//         };
//         // options.Events = new JwtBearerEvents
//         // {
//         //     OnChallenge = context =>
//         //     {
//         //         // Skip the default logic.
//         //         context.HandleResponse();
//         //         return context.Response.WriteAsync(new ErrorBase
//         //         {
//         //             Message = context.ErrorDescription,
//         //             StatusCode = 500,
//         //         }.ToString());
//         //     }
//         // };
//     });
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options =>
// {
//     options.IncludeErrorDetails = true;
//     options.Events = new JwtBearerEvents
//     {
//         OnChallenge = context =>
//         {
//             // Skip the default logic.
//             context.HandleResponse();
//             return context.Response.WriteAsync(new ErrorBase
//             {
//                 Message = context.ErrorDescription,
//                 StatusCode = 500,
//             }.ToString());
//         }
//     };
// });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddScheme<JwtBearerOptions, CustomJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });
builder.Services.AddCors(options => options.AddPolicy(name: "FPLMS-Web",
    policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FPLMS-Web");
app.UseHttpLogging();
// Global exception handler
app.ConfigureExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<TokenInterceptor>();

app.UseAuthorization();

app.MapControllers();

app.Run();
