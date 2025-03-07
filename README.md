
# Auth Payment API

## 📌 Описание проекта
REST API для регистрации, аутентификации и проведения платежей.

##  Функционал
✔ Регистрация пользователя с проверкой логина и пароля.  
✔ Аутентификация через JWT с возможностью выхода (logout).  
✔ Проверка и аннулирование токенов.  
✔ Проведение платежей с учетом баланса.  
✔ Защита от брутфорса и DDoS (Rate Limiting).  
✔ Логирование ошибок (Serilog).  
✔ API-версионность.  
✔ Документация API (Swagger).

## 🛠️ Технологии
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- JWT (Json Web Token)
- FluentValidation
- Serilog
- AspNetCoreRateLimit
- Swagger (Swashbuckle)

## 🔧 Установка и запуск
1️⃣ Клонирование репозитория
```bash
git clone https://github.com/damirovich/InfocomTest.git
cd InfocomTest
```
2️⃣ Настройка базы данных (SQL Server)
 Создай базу данных и настрой appsettings.json:
```
"ConnectionStrings": {
    "MsSQLConnectionString": "Server=localhost;Database=AuthPaymentDb;User Id=sa;Password=your_password;"
}
```
3️⃣ Запуск миграций
```
dotnet ef database update
```
4️⃣ Запуск API
```
dotnet run
```
### Структура проекта
```
InfocomTest/
│── Common/                 # Общие утилиты и BaseResponse
│── Data/                   # Контекст базы данных и сущности
│── Extensions/             # Расширения сервисов (DI)
│── Middleware/             # Глобальные middleware (ошибки, токены)
│── Models/                 # DTO-модели
│── Repositories/           # Репозитории (работа с БД)
│── Services/               # Бизнес-логика (Auth, Payment, Token)
│── Controllers/            # Контроллеры API
│── Program.cs              # Входная точка API
│── README.md               # Документация проекта

