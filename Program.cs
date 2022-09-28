using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var config = app.Configuration;
ProductRepository.Init(config);

app.MapPost("/Products", (Product p) => {
    ProductRepository.Add(p);
    Results.Created("/products/" + p.Code, p.Code);
});

app.MapGet("/Products/{code}", ([FromRoute] string code) => {
    var prod = ProductRepository.GetBy(code);
    if(prod != null){
        return Results.Ok(prod);
    }
    else{
        return Results.NotFound();
    }
});

app.MapPut("/Products", (Product p) => {
    var productSaved = ProductRepository.GetBy(p.Code);
    productSaved.Name = p.Name;
    return Results.Ok();
});

app.MapDelete("/Products/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
    return Results.Ok();
});

if(app.Environment.IsStaging()){
    app.MapGet("/Configuration/DataBase", (IConfiguration config) => {
        return Results.Ok($"{config["database:Connection"]}/{config["database:Port"]}");
    });
}

app.Run();

public static class ProductRepository{
    public static List<Product> ListaProdutos { get; set; } = ListaProdutos = new List<Product>();

    public static void Init(IConfiguration config){
        var prods = config.GetSection("Products").Get<List<Product>>();
        ListaProdutos = prods;
    }

    public static void Add(Product prod){
        if(ListaProdutos == null){
            ListaProdutos.Add(prod);
        }
        else{
            ListaProdutos.Add(prod);
        }
    }

    public static Product GetBy(string code){
        return ListaProdutos.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product p){
        ListaProdutos.Remove(p);
    }
}

public class Product{
    public string Code { get; set; }
    public string Name { get; set; }
}

// GET - retornar um dado
// POST - inserir um dado
// PUT - atualizar um dado
// DELETE - deleta um dado