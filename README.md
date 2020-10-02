# This project has been refactored and renamed to Product Management Service
A CRUD product management service to manage products and product options  

# Steps

## To run application:
1. cd src/ProductManagement.API
2. dotnet run

## To run tests:
1. cd RefactorThis
2. dotnet test
3. This will execute API, Core, and Integration tests

## Requirements
- .Net Core 3.1 runtime
- .Net Core 3.1 SDK

## Refactoring changelog
- Upgraded project to .Net Core 3.1
- Organised code into appropriate folder structure for clarity and tidyness.
- Added Serilog logging with log context.
- Added Swagger to document APIs.
- Documented API endpoints with comments.
- Added a HealthController to enable health checks for load balancing.
- Added API versioning.
- Added unit and integration tests.
- Made all calls async to prevent thread pool starvation.
- Added Azure CI pipeline

## Future enhancement
- Deployment pipeline.
- Log shipping to selected log monitoring platform such as Cloudwatch/DataDog/SumoLogic.
- Performance monitoring such as adding a New Relic agent.


# refactor-this   
The attached project is a poorly written products API in C#.

Please evaluate and refactor areas where you think can be improved. 

Consider all aspects of good software engineering and show us how you'll make it #beautiful and make it a production ready code.

## Getting started for applicants   

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**
```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**
```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**
```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```
