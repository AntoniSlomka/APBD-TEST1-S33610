using APBD_TEST_TEMPLATE.DTOs;
using APBD_TEST_TEMPLATE.Exceptions;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace APBD_TEST_TEMPLATE.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("Missing 'Default' connection string");
        }

        public async Task<VendorResponseDTO?> GetVendorAsync(string Code)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string name;

            await using (var customerCommand = new SqlCommand(
                """
                SELECT
                Name 
                FROM Vendors 
                WHERE Code = @Code
                """,
            connection))
            {
                customerCommand.Parameters.AddWithValue("@Code", Code);

                await using var customerReader = await customerCommand.ExecuteReaderAsync();
                if (!await customerReader.ReadAsync())
                {
                    return null;
                }

                name = customerReader.GetString(0);
            }

            var productsById = new Dictionary<int, VendorProductResponseDTO>();

            await using (var productsCommand = new SqlCommand(@"
            SELECT  p.Id,
		            p.Name,
		            p.Description,
		            p.StickerPrice,
		            pt.Id,
		            pt.Name,
		            m.Id,
		            m.Name,
		            vp.Amount,
		            vp.PricePerUnit
            FROM    Products       p
            LEFT JOIN VendorProducts vp ON vp.ProductId = p.Id
            JOIN Makers m ON p.MakerId = m.Id
            JOIN ProductTypes pt ON p.ProductTypeId = pt.Id
            WHERE vp.VendorCode = @Code
            ORDER BY p.Id, p.Name;", connection))
            {
                productsCommand.Parameters.AddWithValue("@Code", Code);

                await using var reader = await productsCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var productId = reader.GetInt32(0);

                    if (!productsById.TryGetValue(productId, out var product))
                    {
                        product = new VendorProductResponseDTO
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            StickerPrice = reader.GetDecimal(3),
                            ProductType = new ProductTypeResponseDTO
                            {
                                Id = reader.GetInt32(4),
                                Name = reader.GetString(5)
                            },
                            ProductMaker = new ProductMakerResponseDTO
                            {
                                Id = reader.GetInt32(6),
                                Name = reader.GetString(7)
                            },
                            ProductVendorOffer = new ProductVendorOfferResponseDTO
                            {
                                Amount = reader.GetInt32(8),
                                PricePetUnit = reader.GetDecimal(9)
                            }

                        };
                        productsById.Add(productId, product);
                    }
                                       
                }
                              
            }
            return new VendorResponseDTO
            {
                Code = Code,
                Name = name,
                Products = productsById.Values.ToList(),
            };
        }
        public async Task CreateVendorAsync(string Code, CreateVendorDTO request)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                await using (var vendorCheck = new SqlCommand(
                    """
                    SELECT 1
                    FROM Vendors 
                    WHERE Code = @Code
                    """,
                connection,
                transaction))
                {
                    vendorCheck.Parameters.AddWithValue("@Code", Code);
                    var exists = await vendorCheck.ExecuteScalarAsync();
                    if (exists is not null)
                    {
                        throw new SqlAlreadyFilledException($"Customer with id {Code} already exists.");
                    }
                }

                foreach (var product in request.Products)
                {
                    await using (var productCheck = new SqlCommand(
                    """
                    SELECT 1
                    FROM Products 
                    WHERE Id = @Id
                    """,
                connection,
                transaction))
                    {
                        productCheck.Parameters.AddWithValue("@Id", product.Id);
                        var exists = await productCheck.ExecuteScalarAsync();
                        if (exists is null)
                        {
                            throw new ($"Customer with id {Code} already exists.");
                        }
                    }
                }




            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

}
