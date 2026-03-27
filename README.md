BookStoreWCF
A Windows Communication Foundation (WCF) RESTful web service for managing a bookstore inventory, built with C# and Entity Framework connected to MS SQL Server. Includes a Windows Forms client application.

🏗️ Project Structure
BookStoreWCF/
├── BookStoreService/            # WCF Service Library
│   ├── Models/Book.cs           # Data model
│   ├── Data/BookStoreContext.cs # Entity Framework DbContext
│   ├── Migrations/              # EF Database migrations
│   ├── IBookStoreService.cs     # Service contract
│   └── BookStoreService.cs      # Service implementation
├── BookStoreHost/               # Hosts the WCF service
│   └── Program.cs
└── BookStoreClient/             # Windows Forms client
    ├── Book.cs
    └── Form1.cs

✨ Features

Full CRUD operations
Search books by title, author or ISBN
JSON request/response format
Validation and error handling on all operations
Entity Framework 6 with MS SQL Server
Windows Forms client application


🛠️ Technologies
TechnologyUsageC# / .NET Framework 4.8Main languageWCFWeb serviceEntity Framework 6.5.1Database ORMMS SQL Server ExpressDatabaseNewtonsoft.JsonJSON serializationWindows FormsClient application

📡 API Endpoints
MethodURLDescriptionGET/booksGet all booksGET/books/{id}Get book by IDPOST/booksAdd a new bookPUT/books/{id}Update a bookDELETE/books/{id}Delete a bookGET/books/search?query=Search books
Base URL: http://localhost:8733/BookStoreService/

🚀 Getting Started
1. Clone the repository
bashgit clone https://github.com/YOUR_USERNAME/BookStoreWCF.git
2. Update connection string in BookStoreService/App.config
xmlData Source=YOUR_SERVER\SQLEXPRESS;Initial Catalog=BookStoreDB;Integrated Security=True
3. Run BookStoreHost — database will be created automatically
4. Run BookStoreClient — start using the application

📋 Requirements

Visual Studio 2019+
SQL Server Express
.NET Framework 4.8
