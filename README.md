# PhyGen - AI-powered High School Physics Exam Generator (Backend)

**PhyGen** is a backend application powered by AI that automatically generates high school physics exam questions. Built with .NET using a clean architecture approach, this project is designed for scalability, modularity, and easy integration with frontend applications or mobile apps.

## 🚀 Features

- **Automatic Physics Exam Generation**: Generate physics exam questions based on selected chapters, cognitive levels, and difficulty.
- **Question Categorization**: Classify questions by chapter, topic, or skill level.
- **RESTful API**: Provides endpoints to interact with exam generation, question bank, and related features.
- **Docker Support**: Easy deployment with Docker and Docker Compose.

## 🧱 Architecture Overview

The project follows a clean layered architecture:

- `PhyGen.Api`: Main API layer handling HTTP requests and routing.
- `PhyGen.Application`: Contains business logic and use cases.
- `PhyGen.Domain`: Defines core domain entities and interfaces.
- `PhyGen.Infrastructure`: Implements domain interfaces (e.g., database access, external services like AI model integrations).

## ⚙️ Requirements

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- SQL Server or PostgreSQL (depending on your configuration)

## 🛠️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/HienNguyenDev/PhyGen_BE.git
cd PhyGen_BE
```

### 2. Run with Docker

```bash
docker-compose up --build
```

### 3. Access the API

After launching, the API will be available at:

```
http://localhost:5000
```

You can test the endpoints using Swagger, Postman, or any API client.

## 📁 Project Structure

```bash
PhyGen_BE/
├── PhyGen.Api/              # API layer
├── PhyGen.Application/      # Business logic
├── PhyGen.Domain/           # Core domain entities and interfaces
├── PhyGen.Infrastructure/   # Infrastructure implementations (DB, services)
├── docker-compose.yml       # Docker Compose configuration
├── PhyGen.sln               # .NET Solution file
└── README.md                # Project documentation
```

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🙋 Contact

- **Author**: [HienNguyenDev](https://github.com/HienNguyenDev)
- **Email**: [hiennguyendev@example.com](mailto:hiennguyendev@example.com)
