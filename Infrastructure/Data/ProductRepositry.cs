using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data;

public class ProductRepositry(StoreContext _storeContext) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = _storeContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        query = sort switch
        {
            "priceAsc" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(p => p.Name)
        };

        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _storeContext.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await _storeContext.Products.Select(p => p.Brand)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await _storeContext.Products.Select(p => p.Type)
            .Distinct()
            .ToListAsync();
    }

    public void AddProduct(Product product)
    {
        _storeContext.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        _storeContext.Products.Remove(product);
    }

    public bool ProductExists(int id)
    {
        return _storeContext.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _storeContext.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        _storeContext.Entry(product).State = EntityState.Modified;
    }
}
