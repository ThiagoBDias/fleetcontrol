
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FleetControl.Api.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var title = "Erro interno do servidor";
        var detail = "Ocorreu um erro inesperado.";

        if (exception is DbUpdateException dbException &&
            dbException.InnerException is PostgresException postgresException)
        {
            switch (postgresException.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Registro duplicado";
                    detail = "Já existe um registro com esses dados.";
                    break;

                case PostgresErrorCodes.ForeignKeyViolation:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Conflito de relacionamento";
                    detail = "A operação viola um relacionamento entre registros.";
                    break;

                case PostgresErrorCodes.CheckViolation:
                    statusCode = StatusCodes.Status400BadRequest;
                    title = "Dados inválidos";
                    detail = "Os dados violam uma restrição do banco.";
                    break;

                case PostgresErrorCodes.ExclusionViolation:
                    statusCode = StatusCodes.Status409Conflict;
                    title = "Conflito de registros";
                    detail = "A operação viola uma restrição de exclusão.";
                    break;
            }
        }

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Erro inesperado na API.");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "Operação rejeitada. HTTP {StatusCode}",
                statusCode);
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: cancellationToken);

        return true;
    }
}
