# SmartFactory API Integration

## Description
The SmartFactory API Integration project is designed to facilitate the management of products and orders in a smart factory environment. It implements RESTful APIs to perform CRUD operations on product and order data, allowing seamless integration with other systems and applications.

## Features
- Create, read, update, and delete (CRUD) operations for products and orders.
- API endpoints for product and order management.
- Integration with Swagger for API testing and documentation.
- Automated unit tests to ensure functionality and reliability.

## Folder Structure

SmartFactory_API_Integration
├── Controllers
│   ├── OrderController.cs
│   └── ProductController.cs
├── Models
│   ├── Order.cs
│   └── Product.cs
│
└── Program.cs

SmartFactory_API_Test
└── Tests
      ├── order_tests.cs
      └── product_tests.cs

## Installation
1. Clone the repository:
    git clone https://github.com/sandesh-ks-337/SmartFactoryAutomationProject.git

2. Navigate to the project directory:
    cd SmartFactory_API_Integration

3. Restore the dependencies:
    dotnet restore

4. Run the application:
    dotnet run

5. Access the API using Swagger at 
    https://localhost:7208/swagger 
    http://localhost:5199/swagger

    or test with Postman.

6. Usage
To create a product: POST /api/products with JSON body containing product details.
To retrieve all products: GET /api/products
To create an order: POST /api/orders with JSON body containing order details.
To retrieve all orders: GET /api/orders

7. Running Tests
To run the unit tests, navigate to the test project directory and execute: 
dotnet test

#API Documentation
API endpoints can be tested and documented using Swagger, which is available at: http://localhost:5199/swagger

