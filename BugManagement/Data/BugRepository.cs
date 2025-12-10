using System.Data;
using Microsoft.Data.SqlClient;
using BugManagement.Models;

namespace BugManagement.Data
{
    public class BugRepository
    {
        private readonly string _conn;
        private readonly ILogger<BugRepository> _logger;


        public BugRepository(IConfiguration configuration, ILogger<BugRepository> logger)
        {
            _conn = configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException("Missing connection string");
            _logger = logger;
        }

        // Helper to create connection (ADO.NET pooling handled automatically by SqlClient)
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_conn);
        }

        public async Task<List<Bug>> GetAllAsync()
        {
            var list = new List<Bug>();
            const string sql = @"SELECT Id, Title, Description, Status, Severity, AssignedTo, CreatedAt, UpdatedAt, IsDeleted FROM Bugs WHERE IsDeleted = 0 ORDER BY CreatedAt DESC";


            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public async Task<Bug?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT Id, Title, Description, Status, Severity, AssignedTo, CreatedAt, UpdatedAt, IsDeleted FROM Bugs WHERE Id = @id AND IsDeleted = 0";
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync()) return Map(reader);
            return null;
        }

        public async Task<int> CreateAsync(CreateBugDto dto)
        {
            const string sql = @"INSERT INTO Bugs (Title, Description, Status, Severity, AssignedTo, CreatedAt)
OUTPUT INSERTED.Id
VALUES (@Title, @Description, @Status, @Severity, @AssignedTo, SYSUTCDATETIME())";
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)dto.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@Severity", (object?)dto.Severity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)dto.AssignedTo ?? DBNull.Value);


            await conn.OpenAsync();
            var insertedId = (int)await cmd.ExecuteScalarAsync();
            _logger.LogInformation("Created bug {Id} - {Title}", insertedId, dto.Title);
            return insertedId;
        }

        public async Task<bool> UpdateAsync(int id, UpdateBugDto dto)
        {
            const string sql = @"UPDATE Bugs SET Title=@Title, Description=@Description, Status=@Status, Severity=@Severity, AssignedTo=@AssignedTo, UpdatedAt=SYSUTCDATETIME() WHERE Id=@Id AND IsDeleted=0";
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", dto.Title);
            cmd.Parameters.AddWithValue("@Description", (object?)dto.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", dto.Status);
            cmd.Parameters.AddWithValue("@Severity", (object?)dto.Severity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignedTo", (object?)dto.AssignedTo ?? DBNull.Value);


            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = @"UPDATE Bugs SET IsDeleted = 1, UpdatedAt = SYSUTCDATETIME() WHERE Id = @Id AND IsDeleted = 0";
            using var conn = GetConnection();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0;
        }

        private static Bug Map(SqlDataReader r)
        {
            return new Bug
            {
                Id = Convert.ToInt32(r["Id"]),
                Title = r["Title"]?.ToString() ?? "",
                Description = r["Description"] == DBNull.Value ? null : r["Description"].ToString(),
                Status = r["Status"]?.ToString() ?? "Open",
                Severity = r["Severity"] == DBNull.Value ? null : r["Severity"].ToString(),
                AssignedTo = r["AssignedTo"] == DBNull.Value ? null : r["AssignedTo"].ToString(),
                CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt")),
                UpdatedAt = r["UpdatedAt"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(r["UpdatedAt"]),
                IsDeleted = Convert.ToBoolean(r["IsDeleted"])
            };
        }
    }
}
