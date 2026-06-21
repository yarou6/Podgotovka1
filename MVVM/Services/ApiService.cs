using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MVVM.Models.DTO.Auth;
using MVVM.Models.DTO.Categorys;
using MVVM.Models.DTO.Products;

namespace MVVM.Services;

public class ApiService
{
    private readonly HttpClient httpClient;

    public ApiService(string? baseUrl = null)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5202/")
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var response  = await httpClient.PostAsJsonAsync("api/auth/login", dto);
        
        
        return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
    }
    
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var response  = await httpClient.PostAsJsonAsync("api/auth/register", dto);
        
        return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
    }
    
    public async Task<List<ProductReadDto>> GetProductAsync(bool includeInactive = false) => 
        await  httpClient.GetFromJsonAsync<List<ProductReadDto>>("api/products");   
 
    public async Task CreateProductAsync(ProductCreateDto dto) => 
        await httpClient.PostAsJsonAsync("api/products", dto);
    
    public async Task UpdateProductAsync(uint id, ProductUpdateDto dto) =>
        await httpClient.PutAsJsonAsync($"api/products/{id}", dto);
    
    public async Task RemoveProductAsync(uint id) =>
        await httpClient.DeleteAsync($"api/products/{id}");
    
    public async Task<List<CategoryReadDto>> GetCategoriesAsync() => 
        await  httpClient.GetFromJsonAsync<List<CategoryReadDto>>("api/categories");   
    
    public async Task CreateCategoryAsync(CategoryCreateDto dto) => 
        await httpClient.PostAsJsonAsync("api/categories", dto);
    
    public async Task UpdateCategoryAsync(uint id, CategoryUpdateDto dto) =>
        await httpClient.PutAsJsonAsync($"api/categories/{id}", dto);
    
    public async Task RemoveCategoryAsync(uint id) =>
        await httpClient.DeleteAsync($"api/categories/{id}");
    
}